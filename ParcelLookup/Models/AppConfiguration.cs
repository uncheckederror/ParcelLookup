namespace ParcelLookup.Models
{
    public class AppConfiguration
    {
        public Logging Logging { get; set; } = new();
        public APIs KingCountyAPIs { get; set; } = new();
        public Jurisdiction JurisdictionLinks { get; set; } = new();
        public WRIAs WRIALinks { get; set; } = new();
        public Watersheds WatershedLinks { get; set; } = new();
        public SchoolDistricts SchoolDistrictLinks { get; set; } = new();
        public RelatedServices RelatedServices { get; set; } = new();
        public string AllowedHosts { get; set; } = string.Empty;
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; } = new();
    }

    public class Loglevel
    {
        public string Default { get; set; } = string.Empty;
        public string MicrosoftAspNetCore { get; set; } = string.Empty;
    }

    public class APIs
    {
        public string ParcelInfo { get; set; } = string.Empty;
        public string DistrictReport { get; set; } = string.Empty;
    }

    public class Jurisdiction
    {
        public string SeattleProperyURL { get; set; } = string.Empty;
        public string NewcastleURL { get; set; } = string.Empty;
    }

    public class WRIAs
    {
        public string WRIA7 { get; set; } = string.Empty;
        public string WRIA8 { get; set; } = string.Empty;
        public string WRIA9 { get; set; } = string.Empty;
        public string WRIA10 { get; set; } = string.Empty;
    }

    public class Watersheds
    {
        public string PugetSound { get; set; } = string.Empty;
        public string Cedar { get; set; } = string.Empty;
        public string Sammamish { get; set; } = string.Empty;
        public string Snoqualmie { get; set; } = string.Empty;
        public string Green { get; set; } = string.Empty;
        public string White { get; set; } = string.Empty;
    }

    public class SchoolDistricts
    {
        public string Highline { get; set; } = string.Empty;
        public string Seattle { get; set; } = string.Empty;
        public string LkWA { get; set; } = string.Empty;
        public string Mercer { get; set; } = string.Empty;
        public string Northshore { get; set; } = string.Empty;
        public string Shoreline { get; set; } = string.Empty;
        public string Tahoma { get; set; } = string.Empty;
        public string FedWay { get; set; } = string.Empty;
        public string Issaquah { get; set; } = string.Empty;
        public string Riverview { get; set; } = string.Empty;
        public string Tuckwila { get; set; } = string.Empty;
        public string Snoqualmie { get; set; } = string.Empty;
        public string Kent { get; set; } = string.Empty;
        public string Auburn { get; set; } = string.Empty;
        public string Renton { get; set; } = string.Empty;
        public string Bellevue { get; set; } = string.Empty;
        public string Enumclaw { get; set; } = string.Empty;
        public string Vashon { get; set; } = string.Empty;
        public string Skykomish { get; set; } = string.Empty;
        public string Fife { get; set; } = string.Empty;
    }

    public class RelatedServices
    {
        public string Septic { get; set; } = string.Empty;
        public string DD { get; set; } = string.Empty;
        public string Tax { get; set; } = string.Empty;
        public string Excise { get; set; } = string.Empty;
        public string EReal { get; set; } = string.Empty;
        public string Correction { get; set; } = string.Empty;
        public string EMap { get; set; } = string.Empty;
        public string Permit { get; set; } = string.Empty;
        public string Instrument { get; set; } = string.Empty;
        public string iMAP { get; set; } = string.Empty;
        public string ParcelViewer { get; set; } = string.Empty;
        public string BookPage { get; set; } = string.Empty;
        public string PlatSurvey { get; set; } = string.Empty;
        public string TukwilaMap { get; set; } = string.Empty;
    }
}