namespace ParcelLookup.Models
{
    public class ParcelInfo
    {
        public string Parcel { get; set; }
        public string AddressFull { get; set; }
        public string AddressZip { get; set; }
        public string AddressZip4 { get; set; }
        public string AddressCounty { get; set; }
        public ParcelJurisdiction Jurisdiction { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsCondo { get; set; }
        public string Plat_Book { get; set; }
        public string Plat_Page { get; set; }
        public string PresentUse { get; set; }
        public string PLSS { get; set; }
        public string PLSS_QuarterSection { get; set; }
        public string PLSS_Section { get; set; }
        public string PLSS_Township { get; set; }
        public string PLSS_Range { get; set; }
        public class ParcelJurisdiction
        {
            public string Value { get; set; }
            public string URL { get; set; }
        }
    }
}
