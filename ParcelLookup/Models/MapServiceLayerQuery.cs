using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ParcelLookup.Models
{
    public class MapServiceLayerQuery
    {
        public string displayFieldName { get; set; } = string.Empty;
        public Fieldaliases fieldAliases { get; set; } = new();
        public string geometryType { get; set; } = string.Empty;
        public Spatialreference spatialReference { get; set; } = new();
        public Field[] fields { get; set; } = Array.Empty<Field>();
        public Feature[] features { get; set; } = Array.Empty<Feature>();
        public class Fieldaliases
        {
            public string PIN { get; set; } = string.Empty;
            public string MINOR { get; set; } = string.Empty;
            public string OBJECTID { get; set; } = string.Empty;
            public string MAJOR { get; set; } = string.Empty;
            public string COMMENTS { get; set; } = string.Empty;
            public string SITETYPE { get; set; } = string.Empty;
            public string ALIAS1 { get; set; } = string.Empty;
            public string ALIAS2 { get; set; } = string.Empty;
            public string SITEID { get; set; } = string.Empty;
            public string ADDR_HN { get; set; } = string.Empty;
            public string ADDR_PD { get; set; } = string.Empty;
            public string ADDR_PT { get; set; } = string.Empty;
            public string ADDR_SN { get; set; } = string.Empty;
            public string ADDR_ST { get; set; } = string.Empty;
            public string ADDR_SD { get; set; } = string.Empty;
            public string ADDR_NUM { get; set; } = string.Empty;
            public string ADDR_FULL { get; set; } = string.Empty;
            public string FULLNAME { get; set; } = string.Empty;
            public string ZIP5 { get; set; } = string.Empty;
            public string PLUS4 { get; set; } = string.Empty;
            public string CTYNAME { get; set; } = string.Empty;
            public string POSTALCTYNAME { get; set; } = string.Empty;
            public string LAT { get; set; } = string.Empty;
            public string LON { get; set; } = string.Empty;
            public string POINT_X { get; set; } = string.Empty;
            public string POINT_Y { get; set; } = string.Empty;
            public string COUNTY { get; set; } = string.Empty;
            public string KROLL { get; set; } = string.Empty;
            public string KCTP_CITY { get; set; } = string.Empty;
            public string KCTP_STATE { get; set; } = string.Empty;
            public string PLSS { get; set; } = string.Empty;
            public string PROP_NAME { get; set; } = string.Empty;
            public string PLAT_NAME { get; set; } = string.Empty;
            public string PLAT_LOT { get; set; } = string.Empty;
            public string PLAT_BLOCK { get; set; } = string.Empty;
            public string LOTSQFT { get; set; } = string.Empty;
            public string LEVYCODE { get; set; } = string.Empty;
            public string LEVY_JURIS { get; set; } = string.Empty;
            public string NEW_CONSTR { get; set; } = string.Empty;
            public string TAXVAL_RSN { get; set; } = string.Empty;
            public string APPRLNDVAL { get; set; } = string.Empty;
            public string APPR_IMPR { get; set; } = string.Empty;
            public string TAX_LNDVAL { get; set; } = string.Empty;
            public string TAX_IMPR { get; set; } = string.Empty;
            public string ACCNT_NUM { get; set; } = string.Empty;
            public string KCTP_NAME { get; set; } = string.Empty;
            public string KCTP_ATTN { get; set; } = string.Empty;
            public string KCTP_ADDR { get; set; } = string.Empty;
            public string KCTP_CTYST { get; set; } = string.Empty;
            public string KCTP_ZIP { get; set; } = string.Empty;
            public string KCTP_TAXYR { get; set; } = string.Empty;
            public string UNIT_NUM { get; set; } = string.Empty;
            public string BLDG_NUM { get; set; } = string.Empty;
            public string CONDOSITUS { get; set; } = string.Empty;
            public string QTS { get; set; } = string.Empty;
            public string SEC { get; set; } = string.Empty;
            public string TWP { get; set; } = string.Empty;
            public string RNG { get; set; } = string.Empty;
            public string PRIMARY_ADDR { get; set; } = string.Empty;
            public string LEGALDESC { get; set; } = string.Empty;
            public string ANNEXING_CITY { get; set; } = string.Empty;
            public string PAAUNIQUENAME { get; set; } = string.Empty;
            public string PROPTYPE { get; set; } = string.Empty;
            public string KCA_ZONING { get; set; } = string.Empty;
            public string KCA_ACRES { get; set; } = string.Empty;
            public string PREUSE_CODE { get; set; } = string.Empty;
            public string PREUSE_DESC { get; set; } = string.Empty;
            public string ShapeSTArea { get; set; } = string.Empty;
            public string ShapeSTLength { get; set; } = string.Empty;
        }

        public class Spatialreference
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }
        public class Field
        {
            public string name { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
            public string alias { get; set; } = string.Empty;
            public int length { get; set; }
        }

        public class Feature
        {
            public Attributes attributes { get; set; } = new();
            public Geometry geometry { get; set; } = new();
        }

        public class Attributes
        {
            public string PIN { get; set; } = string.Empty;
            public string MINOR { get; set; } = string.Empty;
            public int OBJECTID { get; set; }
            public string MAJOR { get; set; } = string.Empty;
            public string COMMENTS { get; set; } = string.Empty;
            public string SITETYPE { get; set; } = string.Empty;
            public string ALIAS1 { get; set; } = string.Empty;
            public string ALIAS2 { get; set; } = string.Empty;
            public string SITEID { get; set; } = string.Empty;
            public string ADDR_HN { get; set; } = string.Empty;
            public string ADDR_PD { get; set; } = string.Empty;
            public string ADDR_PT { get; set; } = string.Empty;
            public string ADDR_SN { get; set; } = string.Empty;
            public string ADDR_ST { get; set; } = string.Empty;
            public string ADDR_SD { get; set; } = string.Empty;
            public string ADDR_NUM { get; set; } = string.Empty;
            public string ADDR_FULL { get; set; } = string.Empty;
            public string FULLNAME { get; set; } = string.Empty;
            public string ZIP5 { get; set; } = string.Empty;
            public string PLUS4 { get; set; } = string.Empty;
            public string CTYNAME { get; set; } = string.Empty;
            public string POSTALCTYNAME { get; set; } = string.Empty;
            public string LAT { get; set; } = string.Empty;
            public string LON { get; set; } = string.Empty;
            public string POINT_X { get; set; } = string.Empty;
            public string POINT_Y { get; set; } = string.Empty;
            public string COUNTY { get; set; } = string.Empty;
            public string KROLL { get; set; } = string.Empty;
            public string KCTP_CITY { get; set; } = string.Empty;
            public string KCTP_STATE { get; set; } = string.Empty;
            public string PLSS { get; set; } = string.Empty;
            public string PROP_NAME { get; set; } = string.Empty;
            public string PLAT_NAME { get; set; } = string.Empty;
            public string PLAT_LOT { get; set; } = string.Empty;
            public string PLAT_BLOCK { get; set; } = string.Empty;
            public int LOTSQFT { get; set; }
            public string LEVYCODE { get; set; } = string.Empty;
            public string LEVY_JURIS { get; set; } = string.Empty;
            public string NEW_CONSTR { get; set; } = string.Empty;
            public string TAXVAL_RSN { get; set; } = string.Empty;
            public float APPRLNDVAL { get; set; }
            public float APPR_IMPR { get; set; }
            public float TAX_LNDVAL { get; set; }
            public float TAX_IMPR { get; set; }
            public string ACCNT_NUM { get; set; } = string.Empty;
            public string KCTP_NAME { get; set; } = string.Empty;
            public string KCTP_ATTN { get; set; } = string.Empty;
            public string KCTP_ADDR { get; set; } = string.Empty;
            public string KCTP_CTYST { get; set; } = string.Empty;
            public string KCTP_ZIP { get; set; } = string.Empty;
            public int KCTP_TAXYR { get; set; }
            public string UNIT_NUM { get; set; } = string.Empty;
            public string BLDG_NUM { get; set; } = string.Empty;
            public string CONDOSITUS { get; set; } = string.Empty;
            public string QTS { get; set; } = string.Empty;
            public string SEC { get; set; } = string.Empty;
            public string TWP { get; set; } = string.Empty;
            public string RNG { get; set; } = string.Empty;
            public int PRIMARY_ADDR { get; set; }
            public string LEGALDESC { get; set; } = string.Empty;
            public string ANNEXING_CITY { get; set; } = string.Empty;
            public string PAAUNIQUENAME { get; set; } = string.Empty;
            public string PROPTYPE { get; set; } = string.Empty;
            public string KCA_ZONING { get; set; } = string.Empty;
            public float KCA_ACRES { get; set; }
            public int PREUSE_CODE { get; set; }
            public string PREUSE_DESC { get; set; } = string.Empty;
            [JsonProperty("Shape.STArea()")]
            [JsonPropertyName("Shape.STArea()")]
            public string ShapeSTArea { get; set; } = string.Empty;
            [JsonProperty("Shape.STLength()")]
            [JsonPropertyName("Shape.STLength()")]
            public string ShapeSTLength { get; set; } = string.Empty;
        }
        public class Geometry
        {
            public float[][][] rings { get; set; } = Array.Empty<float[][]>();
        }
    }
}