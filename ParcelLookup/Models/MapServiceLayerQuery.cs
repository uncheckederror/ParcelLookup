using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ParcelLookup.Models
{
    public class MapServiceLayerQuery
    {
        public string? displayFieldName { get; set; }
        public Fieldaliases? fieldAliases { get; set; }
        public string? geometryType { get; set; }
        public Spatialreference? spatialReference { get; set; }
        public Field[]? fields { get; set; }
        public Feature[]? features { get; set; }
        public class Fieldaliases
        {
            public string? PIN { get; set; }
            public string? MINOR { get; set; }
            public string? OBJECTID { get; set; }
            public string? MAJOR { get; set; }
            public string? COMMENTS { get; set; }
            public string? SITETYPE { get; set; }
            public string? ALIAS1 { get; set; }
            public string? ALIAS2 { get; set; }
            public string? SITEID { get; set; }
            public string? ADDR_HN { get; set; }
            public string? ADDR_PD { get; set; }
            public string? ADDR_PT { get; set; }
            public string? ADDR_SN { get; set; }
            public string? ADDR_ST { get; set; }
            public string? ADDR_SD { get; set; }
            public string? ADDR_NUM { get; set; }
            public string? ADDR_FULL { get; set; }
            public string? FULLNAME { get; set; }
            public string? ZIP5 { get; set; }
            public string? PLUS4 { get; set; }
            public string? CTYNAME { get; set; }
            public string? POSTALCTYNAME { get; set; }
            public string? LAT { get; set; }
            public string? LON { get; set; }
            public string? POINT_X { get; set; }
            public string? POINT_Y { get; set; }
            public string? COUNTY { get; set; }
            public string? KROLL { get; set; }
            public string? KCTP_CITY { get; set; }
            public string? KCTP_STATE { get; set; }
            public string? PLSS { get; set; }
            public string? PROP_NAME { get; set; }
            public string? PLAT_NAME { get; set; }
            public string? PLAT_LOT { get; set; }
            public string? PLAT_BLOCK { get; set; }
            public string? LOTSQFT { get; set; }
            public string? LEVYCODE { get; set; }
            public string? LEVY_JURIS { get; set; }
            public string? NEW_CONSTR { get; set; }
            public string? TAXVAL_RSN { get; set; }
            public string? APPRLNDVAL { get; set; }
            public string? APPR_IMPR { get; set; }
            public string? TAX_LNDVAL { get; set; }
            public string? TAX_IMPR { get; set; }
            public string? ACCNT_NUM { get; set; }
            public string? KCTP_NAME { get; set; }
            public string? KCTP_ATTN { get; set; }
            public string? KCTP_ADDR { get; set; }
            public string? KCTP_CTYST { get; set; }
            public string? KCTP_ZIP { get; set; }
            public string? KCTP_TAXYR { get; set; }
            public string? UNIT_NUM { get; set; }
            public string? BLDG_NUM { get; set; }
            public string? CONDOSITUS { get; set; }
            public string? QTS { get; set; }
            public string? SEC { get; set; }
            public string? TWP { get; set; }
            public string? RNG { get; set; }
            public string? PRIMARY_ADDR { get; set; }
            public string? LEGALDESC { get; set; }
            public string? ANNEXING_CITY { get; set; }
            public string? PAAUNIQUENAME { get; set; }
            public string? PROPTYPE { get; set; }
            public string? KCA_ZONING { get; set; }
            public string? KCA_ACRES { get; set; }
            public string? PREUSE_CODE { get; set; }
            public string? PREUSE_DESC { get; set; }
            public string? ShapeSTArea { get; set; }
            public string? ShapeSTLength { get; set; }
        }

        public class Spatialreference
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }
        public class Field
        {
            public string? name { get; set; }
            public string? type { get; set; }
            public string? alias { get; set; }
            public int length { get; set; }
        }

        public class Feature
        {
            public Attributes? attributes { get; set; }
            public Geometry? geometry { get; set; }
        }

        public class Attributes
        {
            public string? PIN { get; set; }
            public string? MINOR { get; set; }
            public int OBJECTID { get; set; }
            public string? MAJOR { get; set; }
            public string? COMMENTS { get; set; }
            public string? SITETYPE { get; set; }
            public string? ALIAS1 { get; set; }
            public string? ALIAS2 { get; set; }
            public string? SITEID { get; set; }
            public string? ADDR_HN { get; set; }
            public string? ADDR_PD { get; set; }
            public string? ADDR_PT { get; set; }
            public string? ADDR_SN { get; set; }
            public string? ADDR_ST { get; set; }
            public string? ADDR_SD { get; set; }
            public string? ADDR_NUM { get; set; }
            public string? ADDR_FULL { get; set; }
            public string? FULLNAME { get; set; }
            public string? ZIP5 { get; set; }
            public string? PLUS4 { get; set; }
            public string? CTYNAME { get; set; }
            public string? POSTALCTYNAME { get; set; }
            public string? LAT { get; set; }
            public string? LON { get; set; }
            public string? POINT_X { get; set; }
            public string? POINT_Y { get; set; }
            public string? COUNTY { get; set; }
            public string? KROLL { get; set; }
            public string? KCTP_CITY { get; set; }
            public string? KCTP_STATE { get; set; }
            public string? PLSS { get; set; }
            public string? PROP_NAME { get; set; }
            public string? PLAT_NAME { get; set; }
            public string? PLAT_LOT { get; set; }
            public string? PLAT_BLOCK { get; set; }
            public int LOTSQFT { get; set; }
            public string? LEVYCODE { get; set; }
            public string? LEVY_JURIS { get; set; }
            public string? NEW_CONSTR { get; set; }
            public string? TAXVAL_RSN { get; set; }
            public float APPRLNDVAL { get; set; }
            public float APPR_IMPR { get; set; }
            public float TAX_LNDVAL { get; set; }
            public float TAX_IMPR { get; set; }
            public string? ACCNT_NUM { get; set; }
            public string? KCTP_NAME { get; set; }
            public string? KCTP_ATTN { get; set; }
            public string? KCTP_ADDR { get; set; }
            public string? KCTP_CTYST { get; set; }
            public string? KCTP_ZIP { get; set; }
            public int KCTP_TAXYR { get; set; }
            public string? UNIT_NUM { get; set; }
            public string? BLDG_NUM { get; set; }
            public string? CONDOSITUS { get; set; }
            public string? QTS { get; set; }
            public string? SEC { get; set; }
            public string? TWP { get; set; }
            public string? RNG { get; set; }
            public int PRIMARY_ADDR { get; set; }
            public string? LEGALDESC { get; set; }
            public string? ANNEXING_CITY { get; set; }
            public string? PAAUNIQUENAME { get; set; }
            public string? PROPTYPE { get; set; }
            public string? KCA_ZONING { get; set; }
            public float KCA_ACRES { get; set; }
            public int PREUSE_CODE { get; set; }
            public string? PREUSE_DESC { get; set; }
            [JsonProperty("Shape.STArea()")]
            [JsonPropertyName("Shape.STArea()")]
            public string? ShapeSTArea { get; set; }
            [JsonProperty("Shape.STLength()")]
            [JsonPropertyName("Shape.STLength()")]
            public string? ShapeSTLength { get; set; }
        }
        public class Geometry
        {
            public float[][][]? rings { get; set; }
        }
    }
}