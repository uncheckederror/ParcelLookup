namespace ParcelLookup.Models
{
    public class DistrictsReport
    {
        public string ParcelNumber { get; set; }
        public string Address { get; set; }
        public ParcelJurisdiction[] Jurisdictions { get; set; }
        public string ZipCode { get; set; }
        public string KrollMapPage { get; set; }
        public string ThomasGuidePage { get; set; }
        public string DrainageBasin { get; set; }
        public string Watershed { get; set; }
        public string WatershedLink { get; set; }
        public string WRIAName { get; set; }
        public string WRIANumber { get; set; }
        public string WRIALink { get; set; }
        public string PLSS { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string VotingDistrict { get; set; }
        public string CountyCouncilDistrict { get; set; }
        public string CountyCouncilMemberName { get; set; }
        public string CountyCouncilMemberPhoneNumber { get; set; }
        public string CountyCouncilMemberEmail { get; set; }
        public string CountyCouncilMemberLink { get; set; }
        public string CongressionalDistrict { get; set; }
        public string LegislativeDistrict { get; set; }
        public string SchoolDistrict { get; set; }
        public string SchoolDistrictLink { get; set; }
        public string SchoolBoardDistrict { get; set; }
        public string DistrictCourtElectoralDistrict { get; set; }
        public string RegionalFireDistrict { get; set; }
        public string FireDistrict { get; set; }
        public string WaterDistrict { get; set; }
        public string SewerDistrict { get; set; }
        public string WaterAndSewerDistrict { get; set; }
        public string ParksAndRecreationDistrict { get; set; }
        public string HospitalDistrict { get; set; }
        public string RuralLibraryDistrict { get; set; }
        public string TribalLand { get; set; }
        public string Zoning { get; set; }
        public DevelopmentCondition[] DevelopmentConditions { get; set; }
        public string CompPlanLandUseDesignation { get; set; }
        public string UrbanGrowthArea { get; set; }
        public string CommunityServiceArea { get; set; }
        public string CommunityPlanningArea { get; set; }
        public string CoalMineHazard { get; set; }
        public string ErosionHazard { get; set; }
        public string LandslideHazard { get; set; }
        public string SeismicHazard { get; set; }
        public string FloodPlain { get; set; }
        public string SeaLevelRise { get; set; }
        public string UrbanUnincorporatedStatus { get; set; }
        public string RuralTown { get; set; }
        public string WaterServicePlanningArea { get; set; }
        public string TransportationConcurrencyManagment { get; set; }
        public string ForestProductDistrict { get; set; }
        public string AgriculturalProductionDistrict { get; set; }
        public string SnoqualmieValleyWatershedImpDistrict { get; set; }
        public string AquiferRechargeArea { get; set; }
        public string Wetlands { get; set; }
        public string TacomaSmelterPlume { get; set; }
        public string ShorelineManagementDesignation { get; set; }
        public string RealPropertyLink { get; set; }
        public string QuarterSectionLink { get; set; }
        public string PermittingLink { get; set; }
        public string TreasuryLink { get; set; }
        public string PlatLink { get; set; }
        public string SurveysLink { get; set; }
        public string SepticLink { get; set; }
        public string iMapLink { get; set; }
        public string ParcelViewerLink { get; set; }
        public SensitiveAreaNotice[] SensitiveAreaNotices { get; set; }
        public ParcelTukwila Tukwila { get; set; }

        public class DevelopmentCondition
        {
            public string Condition { get; set; }
            public string Url { get; set; }
        }

        public class ParcelJurisdiction
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        public class SensitiveAreaNotice
        {
            public string RecordingId { get; set; }
            public string Link { get; set; }
        }
        public class ParcelTukwila
        {
            public string SiteAddress { get; set; }
            public string ZoningDistrict { get; set; }
            public string ZoningOverlay { get; set; }
            public string CompPlanLandUseDesignation { get; set; }
            public string Wetlands { get; set; }
            public string WetlandsBufferWidth { get; set; }
            public string Stream { get; set; }
            public string StreamBufferWidth { get; set; }
            public string FishAndWildlife { get; set; }
            public string ShorelineJurisdiction { get; set; }
            public string Landslide { get; set; }
            public string Seismic { get; set; }
            public string Coalmine { get; set; }
            public string WaterServiceDistrict { get; set; }
            public string SewerServiceDistrict { get; set; }
            public string StormServiceDistrict { get; set; }
            public string PowerServiceDistrict { get; set; }
            public string GarbageServiceDistrict { get; set; }
            public string PoliceServiceDistrict { get; set; }
            public string TukwilaMapLink { get; set; }
        }
    }
}
