namespace ParcelLookup.Models
{
    public class AddressLookup
    {
        public string County { get; set; } = string.Empty;
        public string PostalCity { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string Minor { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Street_Type { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public object Plus4 { get; set; } = new();
        public float Lat { get; set; }
        public float Lon { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Juris { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
    }
}
