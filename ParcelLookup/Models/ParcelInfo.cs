namespace ParcelLookup.Models
{
    public class ParcelInfo
    {
        public string Parcel { get; set; } = string.Empty;
        public string AddressFull { get; set; } = string.Empty;
        public string AddressZip { get; set; } = string.Empty;
        public string AddressZip4 { get; set; } = string.Empty;
        public string AddressCounty { get; set; } = string.Empty;
        public ParcelJurisdiction Jurisdiction { get; set; } = new();
        public float Lat { get; set; }
        public float Lng { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsCondo { get; set; }
        public string Plat_Book { get; set; } = string.Empty;
        public string Plat_Page { get; set; } = string.Empty;
        public string PresentUse { get; set; } = string.Empty;
        public string PLSS { get; set; } = string.Empty;
        public string PLSS_QuarterSection { get; set; } = string.Empty;
        public string PLSS_Section { get; set; } = string.Empty;
        public string PLSS_Township { get; set; } = string.Empty;
        public string PLSS_Range { get; set; } = string.Empty;
        public class ParcelJurisdiction
        {
            public string Value { get; set; } = string.Empty;
            public string URL { get; set; } = string.Empty;
        }
    }
}
