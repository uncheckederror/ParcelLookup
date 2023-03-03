namespace ParcelLookup.Models
{
    public class DistrictsReport
    {
        public string ParcelNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ParcelJurisdiction[] Jurisdictions { get; set; } = Array.Empty<ParcelJurisdiction>();
        public string ZipCode { get; set; } = string.Empty;
        public string KrollMapPage { get; set; } = string.Empty;
        public string ThomasGuidePage { get; set; } = string.Empty;
        public string DrainageBasin { get; set; } = string.Empty;
        public string Watershed { get; set; } = string.Empty;
        public string WatershedLink { get; set; } = string.Empty;
        public string WRIAName { get; set; } = string.Empty;
        public string WRIANumber { get; set; } = string.Empty;
        public string WRIALink { get; set; } = string.Empty;
        public string PLSS { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string VotingDistrict { get; set; } = string.Empty;
        public string CountyCouncilDistrict { get; set; } = string.Empty;
        public string CountyCouncilMemberName { get; set; } = string.Empty;
        public string CountyCouncilMemberPhoneNumber { get; set; } = string.Empty;
        public string CountyCouncilMemberEmail { get; set; } = string.Empty;
        public string CountyCouncilMemberLink { get; set; } = string.Empty;
        public string CongressionalDistrict { get; set; } = string.Empty;
        public string LegislativeDistrict { get; set; } = string.Empty;
        public string SchoolDistrict { get; set; } = string.Empty;
        public string SchoolDistrictLink { get; set; } = string.Empty;
        public string SchoolBoardDistrict { get; set; } = string.Empty;
        public string DistrictCourtElectoralDistrict { get; set; } = string.Empty;
        public string RegionalFireDistrict { get; set; } = string.Empty;
        public string FireDistrict { get; set; } = string.Empty;
        public string WaterDistrict { get; set; } = string.Empty;
        public string SewerDistrict { get; set; } = string.Empty;
        public string WaterAndSewerDistrict { get; set; } = string.Empty;
        public string ParksAndRecreationDistrict { get; set; } = string.Empty;
        public string HospitalDistrict { get; set; } = string.Empty;
        public string RuralLibraryDistrict { get; set; } = string.Empty;
        public string TribalLand { get; set; } = string.Empty;
        public string Zoning { get; set; } = string.Empty;
        public DevelopmentCondition[] DevelopmentConditions { get; set; } = Array.Empty<DevelopmentCondition>();
        public string CompPlanLandUseDesignation { get; set; } = string.Empty;
        public string UrbanGrowthArea { get; set; } = string.Empty;
        public string CommunityServiceArea { get; set; } = string.Empty;
        public string CommunityPlanningArea { get; set; } = string.Empty;
        public string CoalMineHazard { get; set; } = string.Empty;
        public string ErosionHazard { get; set; } = string.Empty;
        public string LandslideHazard { get; set; } = string.Empty;
        public string SeismicHazard { get; set; } = string.Empty;
        public string FloodPlain { get; set; } = string.Empty;
        public string SeaLevelRise { get; set; } = string.Empty;
        public string UrbanUnincorporatedStatus { get; set; } = string.Empty;
        public string RuralTown { get; set; } = string.Empty;
        public string WaterServicePlanningArea { get; set; } = string.Empty;
        public string TransportationConcurrencyManagment { get; set; } = string.Empty;
        public string ForestProductDistrict { get; set; } = string.Empty;
        public string AgriculturalProductionDistrict { get; set; } = string.Empty;
        public string SnoqualmieValleyWatershedImpDistrict { get; set; } = string.Empty;
        public string AquiferRechargeArea { get; set; } = string.Empty;
        public string Wetlands { get; set; } = string.Empty;
        public string TacomaSmelterPlume { get; set; } = string.Empty;
        public string ShorelineManagementDesignation { get; set; } = string.Empty;
        public string RealPropertyLink { get; set; } = string.Empty;
        public string QuarterSectionLink { get; set; } = string.Empty;
        public string PermittingLink { get; set; } = string.Empty;
        public string TreasuryLink { get; set; } = string.Empty;
        public string PlatLink { get; set; } = string.Empty;
        public string SurveysLink { get; set; } = string.Empty;
        public string SepticLink { get; set; } = string.Empty;
        public string iMapLink { get; set; } = string.Empty;
        public string ParcelViewerLink { get; set; } = string.Empty;
        public SensitiveAreaNotice[] SensitiveAreaNotices { get; set; } = Array.Empty<SensitiveAreaNotice>();
        public ParcelTukwila Tukwila { get; set; } = new();

        public class DevelopmentCondition
        {
            public string Condition { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
        }

        public class ParcelJurisdiction
        {
            public string Name { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
        }
        public class SensitiveAreaNotice
        {
            public string RecordingId { get; set; } = string.Empty;
            public string Link { get; set; } = string.Empty;
        }
        public class ParcelTukwila
        {
            public string SiteAddress { get; set; } = string.Empty;
            public string ZoningDistrict { get; set; } = string.Empty;
            public string ZoningOverlay { get; set; } = string.Empty;
            public string CompPlanLandUseDesignation { get; set; } = string.Empty;
            public string Wetlands { get; set; } = string.Empty;
            public string WetlandsBufferWidth { get; set; } = string.Empty;
            public string Stream { get; set; } = string.Empty;
            public string StreamBufferWidth { get; set; } = string.Empty;
            public string FishAndWildlife { get; set; } = string.Empty;
            public string ShorelineJurisdiction { get; set; } = string.Empty;
            public string Landslide { get; set; } = string.Empty;
            public string Seismic { get; set; } = string.Empty;
            public string Coalmine { get; set; } = string.Empty;
            public string WaterServiceDistrict { get; set; } = string.Empty;
            public string SewerServiceDistrict { get; set; } = string.Empty;
            public string StormServiceDistrict { get; set; } = string.Empty;
            public string PowerServiceDistrict { get; set; } = string.Empty;
            public string GarbageServiceDistrict { get; set; } = string.Empty;
            public string PoliceServiceDistrict { get; set; } = string.Empty;
            public string TukwilaMapLink { get; set; } = string.Empty;
        }
    }
}
