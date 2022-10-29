using Flurl.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite.Geometries;
using ParcelLookup.Models;
using System.Text.Json;

namespace ParcelLookup.Pages
{
    public class DistrictsReportModel : PageModel
    {
        private string ParcelNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ParcelInfo? ParcelInfo { get; set; } = null;
        public DistrictsReport? Report { get; set; } = null;
        private readonly int BufferDistance = -1;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Get configuration data (magic strings)
        /// </summary>
        /// <param name="configuration"></param>
        public DistrictsReportModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get the Districts Report for a Parcel in King County.
        /// </summary>
        /// <param name="PIN">A valid King County Parcel Number (PIN)</param>
        /// <returns>An HTML formatted Districts Report.</returns>
        /// <exception cref="Exception"></exception>
        public async Task OnGetAsync(string PIN)
        {
            if (!string.IsNullOrWhiteSpace(PIN))
            {
                // Drop all non-digit characters.
                var digitsFromPIN = new string(PIN.Where(x => char.IsDigit(x)).ToArray());

                if (!string.IsNullOrWhiteSpace(digitsFromPIN))
                {
                    /*
                        Account Numbers are 12 digits long. The first 6 digits are the Major, the next 4 are the Minor, and the finally 2 are the Account Id.
                        Parcel Numbers or PINs are 10 digits long. The first 6 digits are the Major and the finally 4 are the Minor.
                        The last two digits of an tax Account Number change everytime ownership of the Parcel changes. 
                        If the last 2 digits of the Account Number can't be matched, you can drop them to find the active account using the prior 10 digits as the Parcel Number.
                     */
                    ParcelNumber = digitsFromPIN.Length switch
                    {
                        // Major only
                        12 => digitsFromPIN[..10],
                        10 => digitsFromPIN[..10],
                        _ => string.Empty
                    };
                }
                else
                {
                    Message = "Please enter a parcel number.";
                }
            }

            // Treat it as an address.
            if (!string.IsNullOrWhiteSpace(PIN) && string.IsNullOrWhiteSpace(ParcelNumber))
            {
                try
                {
                    var response = await $"https://www5.kingcounty.gov/kcgislookup/api/address/{PIN.Trim()}".GetJsonAsync<AddressLookup[]>();
                    if (response is not null && response.Any())
                    {
                        ParcelNumber = response.FirstOrDefault()?.PIN ?? string.Empty;
                    }
                    else
                    {
                        Message = $"Could not match {PIN} to a parcel in King County. Please enter a parcel number or address.";
                    }
                }
                catch
                {
                    Message = $"Could not match {PIN} to a parcel in King County. Please enter a parcel number or address.";
                }
            }

            if (!string.IsNullOrWhiteSpace(ParcelNumber))
            {
                // Query the backend api for the parcel number to find the current account number.
                var parcelInfoUrl = $"{_configuration["KingCountyAPIs:ParcelInfo"]}{ParcelNumber}";
                // Could this API call be replaced with a direct database call?
                ParcelInfo = await parcelInfoUrl.GetJsonAsync<ParcelInfo>();

                if (ParcelInfo is not null && !string.IsNullOrWhiteSpace(ParcelInfo?.Parcel))
                {
                    // PIN is for a condo unit; so convert to condocomplex PIN.
                    if (ParcelInfo.IsCondo)
                    {
                        Message = $"{ParcelNumber} is a condo unit within parcel number {ParcelInfo.Parcel[..6]}.";
                    }

                    // Run the districts report on this parcel number.
                    Report = await DistrictsReport(ParcelInfo);
                }
            }

            // If the PIN cannot be parsed the page defaults back to the empty search bar and example parcels.
        }

        /// <summary>
        /// Generate the Districts Report based on the parcel info.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public async Task<DistrictsReport> DistrictsReport(ParcelInfo parcel)
        {
            // Get the layer definitions for the Districts Report map service.
            var districtServicesUrl = $"{_configuration["KingCountyAPIs:DistrictReport"]}?f=pjson";
            var districts = await districtServicesUrl.GetJsonAsync<MapServiceDescription>();

            // Reuse the extent from the map service definition because we don't want to calculate it or query for it.
            var extent = $"{districts?.initialExtent?.xmax},{districts?.initialExtent?.ymax},{districts?.initialExtent?.xmin},{districts?.initialExtent?.ymin}";
            var parcelAddressAreaLayer = districts?.layers?.FirstOrDefault(x => x.name == "parcel_address_area")?.id;
            var layerServiceUrl = $"{_configuration["KingCountyAPIs:DistrictReport"]}{parcelAddressAreaLayer}/query";
            var pinQueryResponse = await layerServiceUrl
                                        .PostUrlEncodedAsync(new
                                        {
                                            outFields = "*",
                                            returnGeometry = "true",
                                            where = $"PIN='{parcel?.Parcel}'",
                                            f = "json"
                                        });
            var pinQuery = await pinQueryResponse.GetJsonAsync<MapServiceLayerQuery>();

            // Apply the buffer to the geometry of our parcel number query result.
            if (pinQuery.features is not null && pinQuery.features.Any() && BufferDistance is not 0)
            {
                var buffered = BufferParcel(pinQuery, districts!.spatialReference!.wkid);

                var existingRing = pinQuery.features.FirstOrDefault()?.geometry?.rings;
                var bufferedCoordinates = buffered!.Coordinates;
                var ringFromCoordinates = new List<float[]>();
                foreach (var coord in bufferedCoordinates)
                {
                    ringFromCoordinates.Add(new float[] { Convert.ToSingle(coord.X), Convert.ToSingle(coord.Y) });
                }
                pinQuery.features.FirstOrDefault()!.geometry!.rings = new float[][][] { ringFromCoordinates.ToArray() };
            }

            // Run the identify query based on the buffered parcel geometry.
            var identifyQueryUrl = $"{_configuration["KingCountyAPIs:DistrictReport"]}Identify";
            var matchingDistrictsResponse = await identifyQueryUrl.PostUrlEncodedAsync(new
            {
                geometryType = "esriGeometryPolygon",
                geometry = JsonSerializer.Serialize(pinQuery?.features?.FirstOrDefault()?.geometry),
                mapExtent = extent,
                tolerance = 0,
                imageDisplay = "1024,768,96",
                returnGeometry = false,
                sr = pinQuery?.spatialReference?.wkid,
                f = "json",
                layers = "all"
            });
            var stringCheck = await matchingDistrictsResponse.GetStringAsync();
            var matchingDistricts = await matchingDistrictsResponse.GetJsonAsync<DistrictsReportIdentify>();

            // Map the data into the final Districts Report.
            var districtsReport = GetFormattedDistrictsReport(parcel, pinQuery, matchingDistricts);

            // Handle the Parcel's with Tukwila specific data.
            var tukwila = districtsReport.Jurisdictions.Where(x => x.Name.ToLower() is "tukwila").FirstOrDefault();
            var tukwilaReport = await GetTukwilaReportByParcelNumberAsync(parcel?.Parcel ?? string.Empty, extent, tukwila, pinQuery);

            if (tukwilaReport is not null)
            {
                districtsReport.Tukwila = tukwilaReport;
            }

            return districtsReport;
        }

        /// <summary>
        /// Parse the data from the Map and API Querys into data model that we can easily render into HTML.
        /// 
        /// This process would be easy were it not for the way that ArcGIS MapServices describe their data.
        /// To find the values we want we first must loop through the data from every layer returned by our Identify query call here "results".
        /// Once we've found the specific layer were interested in by checking for a specific layer name, because the objects in the "results" array are generic.
        /// Then we can check the attributes object to see if there is a value in the specific field we want.
        /// Attributes of a layer are key, value pairs because they can be added and removed at will in ArcGIS.
        /// For our purposes this means we must create an Attributes object that models all the possible fields returned by this query
        /// so that we can deserialize the JSON response and get the data we want.
        /// Therefore, the Attributes data model has 160 fields when we are really only interested in one or two values for each layer from that Attributes object.
        /// I don't love this, but it does work.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="pinQuery"></param>
        /// <param name="matchingDistricts"></param>
        /// <returns></returns>
        public DistrictsReport GetFormattedDistrictsReport(ParcelInfo? parcel, MapServiceLayerQuery? pinQuery, DistrictsReportIdentify? matchingDistricts)
        {
            // Force all the layernames to be lowercase to regularize them and prevent mismatching.
            if (matchingDistricts?.results is not null)
            {
                foreach (var layer in matchingDistricts.results)
                {
                    layer.layerName = layer?.layerName?.ToLower(System.Globalization.CultureInfo.CurrentCulture);
                }
            }

            var parcelNumber = pinQuery?.features?.FirstOrDefault()?.attributes?.PIN ?? parcel!.Parcel;
            var address = matchingDistricts?.results?.Where(x => x.layerName is "parcel_address_area").FirstOrDefault()?.attributes?.FullAddr;

            var jurisdictionResults = matchingDistricts?.results?.Where(x => x.layerName is "city").ToArray();
            var jurisdictions = new List<DistrictsReport.ParcelJurisdiction>();
            if (jurisdictionResults?.Length > 1)
            {
                foreach (var city in jurisdictionResults)
                {
                    var name = city?.attributes?.CITYNAME ?? "King County";
                    var link = GetJuristictionLinkByParcelNumber(parcel!.Parcel, name, _configuration);
                    jurisdictions.Add(new DistrictsReport.ParcelJurisdiction { Name = name, Url = link });
                }
            }
            else
            {
                var name = jurisdictionResults?.FirstOrDefault()?.attributes?.CITYNAME ?? "King County";
                var link = GetJuristictionLinkByParcelNumber(parcel!.Parcel, name, _configuration);
                jurisdictions.Add(new DistrictsReport.ParcelJurisdiction { Name = name, Url = link });
            }

            var zipcode = matchingDistricts?.results?.Where(x => x.layerName is "zipcode").FirstOrDefault()?.attributes?.ZIPCODE;
            var krollPageResults = matchingDistricts?.results?.Where(x => x.layerName is "krollidx").Select(x => x?.attributes?.KROLL).ToArray();
            var krollPage = krollPageResults?.Length > 1 ? string.Join(" and ", krollPageResults) : krollPageResults?.FirstOrDefault();
            var thomasPageResults = matchingDistricts?.results?.Where(x => x.layerName is "thom_bros").Select(x => x?.attributes?.PAGENUM).ToArray();
            var thomasPage = thomasPageResults?.Length > 1 ? string.Join(" and ", thomasPageResults) : thomasPageResults?.FirstOrDefault();

            // Handle when there are multiple results that apply to a parcel.
            var drainageBasinResults = matchingDistricts?.results?.Where(x => x.layerName is "topo_basin").Select(x => x?.attributes?.BASIN_NAME).ToArray();
            var drainageBasin = drainageBasinResults?.Length > 1 ? string.Join(" and ", drainageBasinResults) : drainageBasinResults?.FirstOrDefault();

            var watershedResults = matchingDistricts?.results?.Where(x => x.layerName is "topo_basin").Select(x => x?.attributes?.WTRSHD_NAME).ToArray();
            var watershed = watershedResults?.Length > 1 ? string.Join(" and ", watershedResults) : watershedResults?.FirstOrDefault();
            var watershedLink = GetWatershedLinkByName(watershed ?? string.Empty, _configuration);

            var wriaNameResults = matchingDistricts?.results?.Where(x => x.layerName is "topo_basin").Select(x => x?.attributes?.WRIA_NAME).ToArray();
            var wriaName = wriaNameResults?.Length > 1 ? string.Join(" and ", wriaNameResults) : wriaNameResults?.FirstOrDefault();

            var wriaNumberResults = matchingDistricts?.results?.Where(x => x.layerName is "topo_basin").Select(x => x?.attributes?.WRIA_NO).ToArray();
            var wriaNumber = wriaNumberResults?.Length > 1 ? string.Join(" and ", wriaNumberResults) : wriaNumberResults?.FirstOrDefault();
            var wriaLink = GetWRIALinkByWRIANumber(wriaNumber ?? string.Empty, _configuration);

            var votingDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "votdst").Select(x => x?.attributes?.NAME).ToArray();
            var votingDistrict = votingDistrictResults?.Length > 1 ? string.Join(" and ", votingDistrictResults) : votingDistrictResults?.FirstOrDefault();

            var councilDistrict = matchingDistricts?.results?.Where(x => x.layerName is "kccdst").FirstOrDefault()?.attributes?.kccdst;
            var councilMemberName = matchingDistricts?.results?.Where(x => x.layerName is "kccdst").FirstOrDefault()?.attributes?.COUNCILMEM;
            var councilMemberPhone = matchingDistricts?.results?.Where(x => x.layerName is "kccdst").FirstOrDefault()?.attributes?.PHONE;
            var councilMemberEmail = matchingDistricts?.results?.Where(x => x.layerName is "kccdst").FirstOrDefault()?.attributes?.EMAIL;
            var councilMemberLink = matchingDistricts?.results?.Where(x => x.layerName is "kccdst").FirstOrDefault()?.attributes?.URL;

            var congessionalDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "congdst").Select(x => x?.attributes?.CONGDST).ToArray();
            var congessionalDistrict = congessionalDistrictResults?.Length > 1 ? string.Join(" and ", congessionalDistrictResults) : congessionalDistrictResults?.FirstOrDefault();

            var legislativeDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "legdst").Select(x => x?.attributes?.LEGDST).ToArray();
            var legislativeDistrict = legislativeDistrictResults?.Length > 1 ? string.Join(" and ", legislativeDistrictResults) : legislativeDistrictResults?.FirstOrDefault();

            var schoolDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "schdst").ToArray();
            var schoolDistrictPrimary = schoolDistrictResults?.Length > 1 ? GetSchoolDistrictByArea(schoolDistrictResults, pinQuery!) : schoolDistrictResults?.FirstOrDefault();
            var schoolDistrict = $"{schoolDistrictPrimary?.attributes?.NAME} {schoolDistrictPrimary?.attributes?.SCHDST}";
            var schoolDistrictLink = GetSchoolDistrictLinkByNumber(schoolDistrictPrimary?.attributes?.SCHDST ?? string.Empty, _configuration);

            var schoolDistrictBoardResults = matchingDistricts?.results?.Where(x => x.layerName is "dirdst").Select(x => x?.attributes?.NAME).ToArray();
            var schoolDistrictBoard = schoolDistrictBoardResults?.Length > 1 ? string.Join(" and ", schoolDistrictBoardResults) : schoolDistrictBoardResults?.FirstOrDefault();

            var districtCourtElectoralDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "juddst").Select(x => x?.attributes?.NAME).ToArray();
            var districtCourtElectoralDistrict = districtCourtElectoralDistrictResults?.Length > 1 ? string.Join(" and ", districtCourtElectoralDistrictResults) : districtCourtElectoralDistrictResults?.FirstOrDefault();

            var regionalFireDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "rfadst").Select(x => x?.attributes?.NAME).ToArray();
            var regionalFireDistrict = regionalFireDistrictResults?.Length > 1 ? string.Join(" and ", regionalFireDistrictResults) : regionalFireDistrictResults?.FirstOrDefault();

            var fireDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "firdst").Select(x => x?.attributes?.NAME).ToArray();
            var fireDistrict = fireDistrictResults?.Length > 1 ? string.Join(" and ", fireDistrictResults) : fireDistrictResults?.FirstOrDefault();

            var waterDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "wtrdst").Select(x => x?.attributes?.NAME).ToArray();
            var waterDistrict = waterDistrictResults?.Length > 1 ? string.Join(" and ", waterDistrictResults) : waterDistrictResults?.FirstOrDefault();

            var sewerDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "swrdst").Select(x => x?.attributes?.NAME).ToArray();
            var sewerDistrict = sewerDistrictResults?.Length > 1 ? string.Join(" and ", sewerDistrictResults) : sewerDistrictResults?.FirstOrDefault();

            var waterSewerDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "wsdst").Select(x => x?.attributes?.NAME).ToArray();
            var waterSewerDistrict = waterSewerDistrictResults?.Length > 1 ? string.Join(" and ", waterSewerDistrictResults) : waterSewerDistrictResults?.FirstOrDefault();

            var parkDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "prkdst").Select(x => x?.attributes?.NAME).ToArray();
            var parkDistrict = parkDistrictResults?.Length > 1 ? string.Join(" and ", parkDistrictResults) : parkDistrictResults?.FirstOrDefault();

            var hospitalDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "hspdst").Select(x => x?.attributes?.NAME).ToArray();
            var hospitalDistrict = hospitalDistrictResults?.Length > 1 ? string.Join(" and ", hospitalDistrictResults) : hospitalDistrictResults?.FirstOrDefault();

            var libraryDistrictResults = matchingDistricts?.results?.Where(x => x.layerName is "libdst").Select(x => x?.attributes?.NAME).ToArray();
            var libraryDistrict = libraryDistrictResults?.Length > 1 ? string.Join(" and ", libraryDistrictResults) : libraryDistrictResults?.FirstOrDefault();

            var tribalLand = matchingDistricts?.results?.Where(x => x.layerName is "tribal_lands").FirstOrDefault();

            // Handle zoning
            var zoningResults = matchingDistricts?.results?.Where(x => x.layerName is "zoning").ToArray();
            var zones = new List<string>();
            if (zoningResults is not null && zoningResults.Any())
            {
                foreach (var zone in zoningResults)
                {
                    var currentZone = zone.attributes?.CURRZONE;
                    var potential = zone.attributes?.POTENTIAL;
                    if (string.IsNullOrWhiteSpace(potential) && !string.IsNullOrWhiteSpace(currentZone))
                    {
                        zones.Add(currentZone);
                    }
                    else
                    {
                        zones.Add($"{currentZone}, {potential} (Potential)");
                    }
                }
            }

            var zoning = zones.Count > 1 ? string.Join(" and ", zones) : zones.FirstOrDefault();

            // Handle development conditions
            var conditionResults = matchingDistricts?.results?.Where(x => x.layerName is "development_condition").ToArray();
            var conditions = new List<DistrictsReport.DevelopmentCondition>();
            if (conditionResults is not null && conditionResults.Any())
            {
                foreach (var condition in conditionResults)
                {
                    conditions.Add(new DistrictsReport.DevelopmentCondition { Condition = condition.attributes?.COND_CODE ?? string.Empty, Url = condition.attributes?.URL ?? string.Empty });
                }
            }

            var compPlanResults = matchingDistricts?.results?.Where(x => x.layerName is "complu").Select(x => GetLandUseCodeByShortCode(x?.attributes?.CPLU ?? string.Empty)).ToArray();
            var compPlan = compPlanResults?.Length > 1 ? string.Join(" and ", compPlanResults) : compPlanResults?.FirstOrDefault();

            var ugaResults = matchingDistricts?.results?.Where(x => x.layerName is "urban_growth").Select(x => x?.attributes?.UGASIDE is "u" ? "Urban" : "Rural").ToArray();
            var uga = ugaResults?.Length > 1 ? string.Join(" and ", ugaResults) : ugaResults?.FirstOrDefault();

            var communityServiceResults = matchingDistricts?.results?.Where(x => x.layerName is "community_service_area").Select(x => x?.attributes?.CSA_NAME).ToArray();
            var communityService = communityServiceResults?.Length > 1 ? string.Join(" and ", communityServiceResults) : communityServiceResults?.FirstOrDefault();

            var communityPlanResults = matchingDistricts?.results?.Where(x => x.layerName is "community_plan").Select(x => x?.attributes?.AREANAME).ToArray();
            var communityPlan = communityPlanResults?.Length > 1 ? string.Join(" and ", communityPlanResults) : communityPlanResults?.FirstOrDefault();

            var coalMine = matchingDistricts?.results?.Where(x => x.layerName is "coalmine").FirstOrDefault();
            var erosion = matchingDistricts?.results?.Where(x => x.layerName is "erode").FirstOrDefault();
            var landslide = matchingDistricts?.results?.Where(x => x.layerName is "slide_sao").FirstOrDefault();
            var seismic = matchingDistricts?.results?.Where(x => x.layerName is "seism").FirstOrDefault();
            var floodplain = matchingDistricts?.results?.Where(x => x.layerName is "fldplain").FirstOrDefault();
            var sealevelrise = matchingDistricts?.results?.Where(x => x.layerName is "sea_level_rise").FirstOrDefault();

            var potentialAnnexationResults = matchingDistricts?.results?.Where(x => x.layerName is "paa").Select(x => $"{x?.attributes?.Uninc_Urban_Category} : {x?.attributes?.UniqueName}").ToArray();
            var potentialAnnexations = potentialAnnexationResults?.Length > 1 ? string.Join(" and ", potentialAnnexationResults) : potentialAnnexationResults?.FirstOrDefault();

            var ruralTownResults = matchingDistricts?.results?.Where(x => x.layerName is "rural_town").Select(x => x?.attributes?.TOWN_NAME).ToArray();
            var ruralTown = ruralTownResults?.Length > 1 ? string.Join(" and ", ruralTownResults) : ruralTownResults?.FirstOrDefault();

            var waterServiceAreaResults = matchingDistricts?.results?.Where(x => x.layerName is "wtr_serv").Select(x => x?.attributes?.NAME).ToArray();
            var waterServiceArea = waterServiceAreaResults?.Length > 1 ? string.Join(" and ", waterServiceAreaResults) : waterServiceAreaResults?.FirstOrDefault();

            var travelshedResults = matchingDistricts?.results?.Where(x => x.layerName is "travelshed").Select(x => $"{x?.attributes?.STATUS} - {x?.attributes?.SHEDNAME} Travelshed").ToArray();
            var travelshed = travelshedResults?.Length > 1 ? string.Join(" and ", travelshedResults) : travelshedResults?.FirstOrDefault();

            var forestProductionDistrict = matchingDistricts?.results?.Where(x => x.layerName is "forpddst").FirstOrDefault();
            var agProductionDistrict = matchingDistricts?.results?.Where(x => x.layerName is "agrpddst").FirstOrDefault();

            var smeltPlumeDistrict = matchingDistricts?.results?.Where(x => x.layerName is "tacomasmelterplume").Select(x => x?.attributes?.NAME).ToArray();
            var smeltPlume = smeltPlumeDistrict?.Length > 1 ? string.Join(" and ", smeltPlumeDistrict) : smeltPlumeDistrict?.FirstOrDefault();

            var snoqualmieValleyWatershedResults = matchingDistricts?.results?.Where(x => x.layerName is "snoqvaly_wshedimprovdist").FirstOrDefault();

            var criticalAquafierResult = matchingDistricts?.results?.Where(x => x.layerName is "cara").Select(x => x?.attributes?.CAT_CODE).ToArray();
            var criticalAquafier = criticalAquafierResult?.Length > 1 ? string.Join(" and ", criticalAquafierResult) : criticalAquafierResult?.FirstOrDefault();

            var wetlandResults = matchingDistricts?.results?.Where(x => x.layerName is "sao_wetland").ToArray();
            var wetlands = new List<string>();
            if (wetlandResults is not null && wetlandResults.Any())
            {
                foreach (var wetland in wetlandResults)
                {
                    var type = wetland?.attributes?.TYPE_ ?? "Not Designated";
                    var currency = wetland?.attributes?.CURRENCY_;
                    var source = wetland?.attributes?.SOURCE_;
                    wetlands.Add($"Rating = {type} Currency = {type} Source = {type}");
                }
            }

            var wetlanding = wetlands.Count > 1 ? string.Join(" and ", wetlands) : wetlands.FirstOrDefault();

            var shorelineManagementResults = matchingDistricts?.results?.Where(x => x.layerName is "shorelinemanagement").Select(x => x?.attributes?.DESIG).ToArray();
            var shorelineManagement = shorelineManagementResults?.Length > 1 ? string.Join(" and ", shorelineManagementResults) : shorelineManagementResults?.FirstOrDefault();

            var sensitiveAreaNoticeResults = matchingDistricts?.results?.Where(x => x.layerName is "sant").Select(x => x?.attributes?.RECORDING_).ToArray();
            var sensitiveAreaNotices = new List<DistrictsReport.SensitiveAreaNotice>();
            if (sensitiveAreaNoticeResults is not null && sensitiveAreaNoticeResults.Any())
            {
                foreach (var notice in sensitiveAreaNoticeResults)
                {
                    if (!string.IsNullOrWhiteSpace(notice))
                    {
                        sensitiveAreaNotices.Add(new DistrictsReport.SensitiveAreaNotice
                        {
                            RecordingId = notice,
                            Link = $"{_configuration["RelatedServices:Instrument"]}{notice}",
                        });
                    }
                }
            }

            return new DistrictsReport
            {
                ParcelNumber = parcelNumber,
                Address = address ?? string.Empty,
                Jurisdictions = jurisdictions.ToArray(),
                ZipCode = zipcode ?? string.Empty,
                KrollMapPage = krollPage ?? string.Empty,
                ThomasGuidePage = thomasPage ?? string.Empty,
                DrainageBasin = drainageBasin ?? string.Empty,
                Watershed = watershed ?? string.Empty,
                WatershedLink = watershedLink ?? string.Empty,
                WRIAName = wriaName ?? string.Empty,
                WRIANumber = wriaNumber ?? string.Empty,
                WRIALink = wriaLink ?? string.Empty,
                PLSS = $"{parcel?.PLSS_QuarterSection} - {parcel?.PLSS_Section} - {parcel?.PLSS_Township} - {parcel?.PLSS_Range}",
                Latitude = parcel?.Lat.ToString() ?? string.Empty,
                Longitude = parcel?.Lng.ToString() ?? string.Empty,
                VotingDistrict = votingDistrict ?? string.Empty,
                CountyCouncilDistrict = councilDistrict ?? string.Empty,
                CountyCouncilMemberEmail = councilMemberEmail ?? string.Empty,
                CountyCouncilMemberLink = councilMemberLink ?? string.Empty,
                CountyCouncilMemberName = councilMemberName ?? string.Empty,
                CountyCouncilMemberPhoneNumber = councilMemberPhone ?? string.Empty,
                CongressionalDistrict = congessionalDistrict ?? string.Empty,
                LegislativeDistrict = legislativeDistrict ?? string.Empty,
                SchoolDistrict = schoolDistrict ?? string.Empty,
                SchoolDistrictLink = schoolDistrictLink ?? string.Empty,
                SchoolBoardDistrict = schoolDistrictBoard ?? "N/A",
                DistrictCourtElectoralDistrict = districtCourtElectoralDistrict ?? string.Empty,
                RegionalFireDistrict = regionalFireDistrict ?? "N/A",
                FireDistrict = fireDistrict ?? "N/A",
                WaterDistrict = waterDistrict ?? "N/A",
                SewerDistrict = sewerDistrict ?? "N/A",
                WaterAndSewerDistrict = waterSewerDistrict ?? "N/A",
                ParksAndRecreationDistrict = parkDistrict ?? "N/A",
                HospitalDistrict = hospitalDistrict ?? "N/A",
                RuralLibraryDistrict = libraryDistrict ?? "N/A",
                TribalLand = tribalLand is not null ? "Yes" : "No",
                Zoning = zoning ?? jurisdictions.FirstOrDefault()?.Name ?? "N/A",
                DevelopmentConditions = conditions.ToArray(),
                CompPlanLandUseDesignation = compPlan ?? "N/A",
                UrbanGrowthArea = uga ?? string.Empty,
                CommunityServiceArea = communityService ?? "N/A",
                CommunityPlanningArea = communityPlan ?? string.Empty,
                CoalMineHazard = coalMine is not null ? "Yes" : "No",
                ErosionHazard = erosion is not null ? "Yes" : "No",
                LandslideHazard = landslide is not null ? "Yes" : "No",
                SeismicHazard = seismic is not null ? "Yes" : "No",
                FloodPlain = floodplain is not null ? "Yes" : "No",
                SeaLevelRise = sealevelrise is not null ? "Yes" : "No",
                UrbanUnincorporatedStatus = potentialAnnexations ?? "N/A",
                RuralTown = ruralTown ?? "No",
                WaterServicePlanningArea = waterServiceArea ?? string.Empty,
                TransportationConcurrencyManagment = travelshed ?? "N/A",
                ForestProductDistrict = forestProductionDistrict is not null ? "Yes" : "No",
                AgriculturalProductionDistrict = agProductionDistrict is not null ? "Yes" : "No",
                TacomaSmelterPlume = smeltPlume ?? "No",
                SnoqualmieValleyWatershedImpDistrict = snoqualmieValleyWatershedResults is not null ? "Yes" : "No",
                AquiferRechargeArea = criticalAquafier ?? "No",
                Wetlands = wetlanding ?? "No",
                ShorelineManagementDesignation = shorelineManagement ?? "No",
                RealPropertyLink = $"{_configuration["RelatedServices:EReal"]}{parcel?.Parcel}",
                TreasuryLink = $"{_configuration["RelatedServices:Tax"]}{parcel?.Parcel}",
                QuarterSectionLink = $"{_configuration["RelatedServices:EMap"]}{parcel?.Parcel}",
                PermittingLink = $"{_configuration["RelatedServices:Permit"]}{parcel?.Parcel}&tab=2",
                SepticLink = $"{_configuration["RelatedServices:Septic"]}{parcel?.Parcel}",
                PlatLink = $"{_configuration["RelatedServices:BookPage"]}booknumber={parcel?.Plat_Book}&bookpage={parcel?.Plat_Page}",
                SurveysLink = $"{_configuration["RelatedServices:PlatSurvey"]}section={parcel?.PLSS_Section}&township={parcel?.PLSS_Township}&range={parcel?.PLSS_Range}&quarter={parcel?.PLSS_QuarterSection}",
                iMapLink = $"{_configuration["RelatedServices:iMap"]}{parcel?.Parcel}",
                ParcelViewerLink = $"{_configuration["RelatedServices:ParcelViewer"]}{parcel?.Parcel}",
                SensitiveAreaNotices = sensitiveAreaNotices.ToArray(),
            };
        }

        /// <summary>
        /// Find the primary school district for the parcel based on the area of the parcel overlayed by each district.
        /// </summary>
        /// <param name="schoolDistrictResults"></param>
        /// <param name="pinQuery"></param>
        /// <returns></returns>
        public DistrictsReportIdentify.Result? GetSchoolDistrictByArea(DistrictsReportIdentify.Result[] schoolDistrictResults, MapServiceLayerQuery pinQuery)
        {
            var checkParse = double.TryParse(pinQuery?.features?.FirstOrDefault()?.attributes?.ShapeSTArea, out var featureArea);
            var highestOverlayPercent = 0.0;
            DistrictsReportIdentify.Result? highestDistrict = null;

            if (checkParse && schoolDistrictResults is not null && schoolDistrictResults.Any())
            {
                foreach (var school in schoolDistrictResults)
                {
                    checkParse = double.TryParse(school?.attributes?.ShapeSTArea, out var overlayArea);
                    if (checkParse && school is not null)
                    {
                        var percentOverlay = (featureArea / overlayArea) * 100;
                        if (highestOverlayPercent < percentOverlay)
                        {
                            highestOverlayPercent = percentOverlay;
                            highestDistrict = school;
                        }
                    }
                }

                return highestDistrict;
            }
            else
            {
                return schoolDistrictResults?.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the districts report specific to Tukwila.
        /// </summary>
        /// <param name="parcelNumber"></param>
        /// <param name="extent"></param>
        /// <param name="tukwila"></param>
        /// <param name="pinQuery"></param>
        /// <returns></returns>
        public async Task<DistrictsReport.ParcelTukwila?> GetTukwilaReportByParcelNumberAsync(string parcelNumber, string extent, DistrictsReport.ParcelJurisdiction? tukwila, MapServiceLayerQuery? pinQuery)
        {
            if (tukwila is not null && pinQuery is not null && !string.IsNullOrWhiteSpace(parcelNumber) && !string.IsNullOrWhiteSpace(extent))
            {
                // Magic
                var tukwilaServicesUrl = "https://maps.tukwilawa.gov/arcgis/rest/services/iMap/DistrictsReport/MapServer/?f=pjson";
                var tukwilaLayers = await tukwilaServicesUrl.GetJsonAsync<MapServiceDescription>();

                var tukwilaIdentifyQueryUrl = $"https://maps.tukwilawa.gov/arcgis/rest/services/iMap/DistrictsReport/MapServer/Identify";
                var tukwilaMatchingDistrictsResponse = await tukwilaIdentifyQueryUrl.PostUrlEncodedAsync(new
                {
                    geometryType = "esriGeometryPolygon",
                    geometry = JsonSerializer.Serialize(pinQuery?.features?.FirstOrDefault()?.geometry),
                    mapExtent = extent,
                    tolerance = 0,
                    imageDisplay = "1024,768,96",
                    returnGeometry = false,
                    sr = pinQuery?.spatialReference?.wkid,
                    f = "json",
                    layers = "all"
                });
                var tukwilastringCheck = await tukwilaMatchingDistrictsResponse.GetStringAsync();
                var tukwilaMatchingDistricts = await tukwilaMatchingDistrictsResponse.GetJsonAsync<DistrictsReportIdentify>();

                var tukwilaAddressResults = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "AddressPoints").Select(x => x?.attributes?.ComboAddress).ToArray();
                var tukwilaAddress = tukwilaAddressResults?.Length > 1 ? string.Join(" and ", tukwilaAddressResults) : tukwilaAddressResults?.FirstOrDefault();

                var tukwilaFishResults = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "FishAndWildlifeHabitat").Select(x => x?.attributes?.HabitatType).ToArray();
                var tukwilaFish = tukwilaFishResults?.Length > 1 ? string.Join(" and ", tukwilaFishResults) : tukwilaFishResults?.FirstOrDefault();

                var tukwilaWetlandsResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "Wetlands").Select(x => x?.attributes?.WetlandClass).ToArray();
                var tukwilaWetlands = tukwilaWetlandsResult?.Length > 1 ? string.Join(" and ", tukwilaWetlandsResult) : tukwilaWetlandsResult?.FirstOrDefault();

                var tukwilaWetlandsBufferResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "WetlandsBuffer").Select(x => x?.attributes?.BufferWidth).ToArray();
                var tukwilaWetlandsBuffer = tukwilaWetlandsBufferResult?.Length > 1 ? string.Join(" and ", tukwilaWetlandsBufferResult) : tukwilaWetlandsBufferResult?.FirstOrDefault();

                var tukwilaShorelineResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "ShorelineJurisdiction").Select(x => x?.attributes?.ShoreEnvir).ToArray();
                var tukwilaShoreline = tukwilaShorelineResult?.Length > 1 ? string.Join(" and ", tukwilaShorelineResult) : tukwilaShorelineResult?.FirstOrDefault();

                var tukwilaLandslide = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "Landslide").FirstOrDefault();
                var tukwilaSeismic = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "SeismicHazardAreas").FirstOrDefault();
                var tukwilaMinehazard = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "MineHazardAreas").FirstOrDefault();

                var tukwilaStreamResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "Streams").Select(x => x?.attributes?.StreamName).ToArray();
                var tukwilaStream = tukwilaStreamResult?.Length > 1 ? string.Join(" and ", tukwilaStreamResult) : tukwilaStreamResult?.FirstOrDefault();

                var tukwilaStreamBufferResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "StreamBuffers").Select(x => x?.attributes?.BufferWidth).ToArray();
                var tukwilaStreamBuffer = tukwilaStreamBufferResult?.Length > 1 ? string.Join(" and ", tukwilaStreamBufferResult) : tukwilaStreamBufferResult?.FirstOrDefault();

                var tukwilaZoningResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "Zoning").Select(x => x?.attributes?.ZoningDescription)?.ToArray();
                var tukwilaZoning = tukwilaStreamBufferResult?.Length > 1 ? string.Join(" and ", tukwilaZoningResult ?? Array.Empty<string>()) : tukwilaZoningResult?.FirstOrDefault();

                var tukwilaZoningOverlayResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "Zoning_Overlays").Select(x => x?.attributes?.Overlay).ToArray();
                var tukwilaZoningOverlay = tukwilaZoningOverlayResult?.Length > 1 ? string.Join(" and ", tukwilaZoningOverlayResult) : tukwilaZoningOverlayResult?.FirstOrDefault();

                var tukwilaCompPlanResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "CompPlan").Select(x => x?.attributes?.CompPlanDescription).ToArray();
                var tukwilaCompPlan = tukwilaCompPlanResult?.Length > 1 ? string.Join(" and ", tukwilaCompPlanResult) : tukwilaCompPlanResult?.FirstOrDefault();

                var tukwilaWaterServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "WaterService").Select(x => x?.attributes?.WaterService).ToArray();
                var tukwilaWaterService = tukwilaWaterServiceResult?.Length > 1 ? string.Join(" and ", tukwilaWaterServiceResult) : tukwilaWaterServiceResult?.FirstOrDefault();

                var tukwilaSewerServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "SewerService").Select(x => x?.attributes?.SewerService).ToArray();
                var tukwilaSewerService = tukwilaSewerServiceResult?.Length > 1 ? string.Join(" and ", tukwilaSewerServiceResult) : tukwilaSewerServiceResult?.FirstOrDefault();

                var tukwilaStormServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "StormService").Select(x => x?.attributes?.StormService).ToArray();
                var tukwilaStormService = tukwilaStormServiceResult?.Length > 1 ? string.Join(" and ", tukwilaStormServiceResult) : tukwilaStormServiceResult?.FirstOrDefault();

                var tukwilaPowerServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "PowerService").Select(x => x?.attributes?.PowerService).ToArray();
                var tukwilaPowerService = tukwilaPowerServiceResult?.Length > 1 ? string.Join(" and ", tukwilaPowerServiceResult) : tukwilaPowerServiceResult?.FirstOrDefault();

                var tukwilaPoliceServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "PoliceService").Select(x => x?.attributes?.PoliceServ).ToArray();
                var tukwilaPoliceService = tukwilaPoliceServiceResult?.Length > 1 ? string.Join(" and ", tukwilaPoliceServiceResult) : tukwilaPoliceServiceResult?.FirstOrDefault();

                var tukwilaGarbageServiceResult = tukwilaMatchingDistricts?.results?.Where(x => x?.layerName is "GarbageService").Select(x => x?.attributes?.GarbageSer).ToArray();
                var tukwilaGarbageService = tukwilaGarbageServiceResult?.Length > 1 ? string.Join(" and ", tukwilaGarbageServiceResult) : tukwilaGarbageServiceResult?.FirstOrDefault();

                return new DistrictsReport.ParcelTukwila
                {
                    SiteAddress = tukwilaAddress ?? string.Empty,
                    FishAndWildlife = tukwilaFish ?? "No",
                    Wetlands = tukwilaWetlands ?? "No",
                    WetlandsBufferWidth = tukwilaWetlandsBuffer ?? "N/A",
                    ShorelineJurisdiction = tukwilaShoreline ?? "N/A",
                    Landslide = tukwilaLandslide is not null ? "Yes" : "No",
                    Seismic = tukwilaSeismic is not null ? "Yes" : "No",
                    Coalmine = tukwilaMinehazard is not null ? "Yes" : "No",
                    Stream = tukwilaStream ?? "No",
                    StreamBufferWidth = tukwilaStreamBuffer ?? "N/A",
                    ZoningDistrict = tukwilaZoning ?? string.Empty,
                    ZoningOverlay = tukwilaZoningOverlay ?? "N/A",
                    CompPlanLandUseDesignation = tukwilaCompPlan ?? string.Empty,
                    WaterServiceDistrict = tukwilaWaterService ?? string.Empty,
                    SewerServiceDistrict = tukwilaSewerService ?? string.Empty,
                    StormServiceDistrict = tukwilaStormService ?? string.Empty,
                    PowerServiceDistrict = tukwilaPowerService ?? string.Empty,
                    PoliceServiceDistrict = tukwilaPoliceService ?? string.Empty,
                    GarbageServiceDistrict = tukwilaGarbageService ?? string.Empty,
                    TukwilaMapLink = $"{_configuration["RelatedServices:TukwilaMap"]}{parcelNumber}"
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Build a link based on the district number.
        /// </summary>
        /// <param name="schoolDistrictNumber"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetSchoolDistrictLinkByNumber(string schoolDistrictNumber, IConfiguration configuration)
        {
            return schoolDistrictNumber switch
            {
                "1" => configuration["SchoolDistrictLinks:Seattle"],
                "210" => configuration["SchoolDistrictLinks:FedWay"],
                "216" => configuration["SchoolDistrictLinks:Enumclaw"],
                "400" => configuration["SchoolDistrictLinks:Mercer"],
                "401" => configuration["SchoolDistrictLinks:Highline"],
                "402" => configuration["SchoolDistrictLinks:Vashon"],
                "403" => configuration["SchoolDistrictLinks:Renton"],
                "404" => configuration["SchoolDistrictLinks:Skykomish"],
                "405" => configuration["SchoolDistrictLinks:Bellevue"],
                "406" => configuration["SchoolDistrictLinks:Tuckwila"],
                "407" => configuration["SchoolDistrictLinks:Riverview"],
                "408" => configuration["SchoolDistrictLinks:Auburn"],
                "409" => configuration["SchoolDistrictLinks:Tahoma"],
                "410" => configuration["SchoolDistrictLinks:Snoqualmie"],
                "411" => configuration["SchoolDistrictLinks:Issaquah"],
                "412" => configuration["SchoolDistrictLinks:Shoreline"],
                "414" => configuration["SchoolDistrictLinks:LkWA"],
                "415" => configuration["SchoolDistrictLinks:Kent"],
                "417" => configuration["SchoolDistrictLinks:Northshore"],
                "888" => configuration["SchoolDistrictLinks:Fife"],
                _ => string.Empty
            };
        }

        /// <summary>
        /// Build a link based on the watershed name.
        /// </summary>
        /// <param name="watershed"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetWatershedLinkByName(string watershed, IConfiguration configuration)
        {
            return watershed switch
            {
                "Central Puget Sound" => configuration["WatershedLinks:PugetSound"],
                "Cedar River / Lake Washington" => configuration["WatershedLinks:Cedar"],
                "Sammamish River" => configuration["WatershedLinks:Sammamish"],
                "Snoqualmie River" => configuration["WatershedLinks:Snoqualmie"],
                "Duwamish - Green River" => configuration["WatershedLinks:Green"],
                "White River" => configuration["WatershedLinks:White"],
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Build a link based on the WRIA number.
        /// </summary>
        /// <param name="wriaNumber"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetWRIALinkByWRIANumber(string wriaNumber, IConfiguration configuration)
        {
            return wriaNumber switch
            {
                "7" => configuration["WRIALinks:7"],
                "8" => configuration["WRIALinks:8"],
                "9" => configuration["WRIALinks:9"],
                "10" => configuration["WRIALinks:10"],
                _ => string.Empty,
            };
        }

        public static string GetLandUseCodeByShortCode(string landUseCode)
        {
            return landUseCode switch
            {
                "ac" => "Unincorporated Activity Center (ac)",
                "ag" => "Agriculture (ag)",
                "cb" => "Community Business Center (cb)",
                "co" => "Commercial Outside of Centers (co)",
                "f" => "Forestry (f)",
                "gb" => "Greenbelt / Urban Separator (gb)",
                "i" => "Industrial (i)",
                "m" => "Mining (m)",
                "nb" => "Neighborhood Business Center (nb)",
                "os" => "King County Open Space System (os)",
                "rn" => "Rural Neighborhood Commercial Center (rn)",
                "ra" => "Rural Area (1 dwelling unit / 2.5 - 10acres) (ra)",
                "rt" => "Rural Town (rt)",
                "rx" => "Rural City Urban Growth Area (rx)",
                "uh" => "Urban Residential, High (> 12 dwelling unit / acre) (uh)",
                "ul" => "Urban Residential, Low (1 dwelling unit / acre) (ul)",
                "um" => "Urban Residential, Medium (4 - 12 dwelling unit / acre) (um)",
                "upd" => "Urban Planned Development (upd)",
                "op" => "Other Parks/ Wilderness (op)",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Build a link to the parcel based on the Jurisdiction.
        /// </summary>
        /// <param name="parcelNumber"></param>
        /// <param name="jurisdiction"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetJuristictionLinkByParcelNumber(string parcelNumber, string jurisdiction, IConfiguration configuration)
        {
            return jurisdiction switch
            {
                "Seattle" => $"{configuration["JurisdictionLinks:SeattleProperyURL"]}{parcelNumber}",
                "Newcastle" => $"{configuration["JurisdictionLinks:NewcastleURL"]}{parcelNumber}",
                // Should we include Tukwila in here rather than having it happen separately in the Tukwila-only districts report?
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Shrink the geometry of a parcel by a meter to reduce accidental overlays.
        /// </summary>
        /// <param name="pinQuery"></param>
        /// <param name="wkid"></param>
        /// <returns></returns>
        public Geometry? BufferParcel(MapServiceLayerQuery pinQuery, int wkid)
        {
            var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(wkid);

            var bufferedGeometries = new List<Geometry>();

            if (pinQuery?.features is not null && pinQuery.features.Any())
            {
                foreach (var feature in pinQuery.features)
                {
                    if (feature.geometry is not null)
                    {
                        var polygons = new List<Polygon>();
                        Polygon? polygon;

                        if (feature?.geometry?.rings is not null && feature.geometry.rings.Any())
                        {
                            foreach (var ring in feature.geometry.rings)
                            {
                                var coords = new List<Coordinate>();

                                foreach (var coord in ring)
                                {
                                    coords.Add(new Coordinate(Convert.ToDouble(coord[0]), Convert.ToDouble(coord[1])));
                                }

                                if (coords.Count > 1)
                                {
                                    var start = coords.FirstOrDefault();
                                    var end = coords.LastOrDefault();
                                    if (start is not null && end is not null && start.X != end.X && start.Y != end.Y)
                                    {
                                        coords.Add(start);
                                    }
                                }

                                var polyFromCoordinates = geometryFactory.CreatePolygon(coords.ToArray());

                                // If we've created a valid polygon add it to the list; Otherwise fix it, and then add it to the list.
                                if (polyFromCoordinates is not null && polyFromCoordinates.IsValid)
                                {
                                    polygons.Add(polyFromCoordinates);
                                }
                                else
                                {
                                    // Magic to fix invalid polygons. Check the NetTopologySuite for an explaintion.
                                    polygons.Add(geometryFactory.CreatePolygon(NetTopologySuite.Geometries.Utilities.GeometryFixer.Fix(polyFromCoordinates).Coordinates));
                                }
                            }

                            if (polygons.Count > 1)
                            {
                                var union = polygons[0].Union(polygons[1]);

                                if (polygons.Count > 2)
                                {
                                    foreach (var poly in polygons.Skip(2).ToArray())
                                    {
                                        union = union.Union(poly);
                                    }
                                }

                                var unioned = geometryFactory.CreatePolygon(union.Coordinates);

                                // Magic to fix invalid polygons. Check the NetTopologySuite for an explaintion.
                                if (unioned.IsValid)
                                {
                                    polygon = geometryFactory.CreatePolygon(union.Coordinates);
                                }
                                else
                                {
                                    polygon = geometryFactory.CreatePolygon(NetTopologySuite.Geometries.Utilities.GeometryFixer.Fix(unioned).Coordinates);
                                }
                            }
                            else
                            {
                                polygon = polygons.FirstOrDefault();
                            }

                            var buffered = polygon?.Buffer(BufferDistance);

                            if (buffered is not null && buffered.IsValid)
                            {
                                bufferedGeometries.Add(buffered);
                            }
                        }
                        else
                        {
                            throw new Exception($"The geometry rings are null or empty: {feature?.geometry?.rings} for {pinQuery}");
                        }
                    }
                }
            }
            else
            {
                throw new Exception($"The pinQuery is null or has no features: {pinQuery}");
            }

            if (bufferedGeometries.Count > 1)
            {
                var unioned = bufferedGeometries[0].Union(bufferedGeometries[1]);
                if (bufferedGeometries.Count > 2)
                {
                    foreach (var buff in bufferedGeometries.Skip(2).ToArray())
                    {
                        unioned = unioned.Union(buff);
                    }
                }

                return unioned;
            }
            else
            {
                return bufferedGeometries.FirstOrDefault();
            }
        }
    }
}
