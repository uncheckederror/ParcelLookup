using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ParcelLookup.Models
{
    public class DistrictsReportIdentify
    {
        public Result[] results { get; set; } = Array.Empty<Result>();
        public class Result
        {
            public int layerId { get; set; }
            public string layerName { get; set; } = string.Empty;
            public string displayFieldName { get; set; } = string.Empty;
            public string value { get; set; } = string.Empty;
            public Attributes attributes { get; set; } = new();
        }

        public class Attributes
        {
            public string OBJECTID { get; set; } = string.Empty;
            public string MAJOR { get; set; } = string.Empty;
            public string MINOR { get; set; } = string.Empty;
            public string PIN { get; set; } = string.Empty;
            [JsonProperty("Shape.STArea()")]
            [JsonPropertyName("Shape.STArea()")]
            public string ShapeSTArea { get; set; } = string.Empty;
            [JsonProperty("Shape.STLength()")]
            [JsonPropertyName("Shape.STLength()")]
            public string ShapeSTLength { get; set; } = string.Empty;
            public string ObjectID { get; set; } = string.Empty;
            public string shape { get; set; } = string.Empty;
            public string ADDR_COMMENTS { get; set; } = string.Empty;
            public string ADDR_TYPE { get; set; } = string.Empty;
            public string ADDRALIAS1 { get; set; } = string.Empty;
            public string ADDRALIAS2 { get; set; } = string.Empty;
            public string ESITEID { get; set; } = string.Empty;
            public string HouseNum { get; set; } = string.Empty;
            public string PreDir { get; set; } = string.Empty;
            public string PreType { get; set; } = string.Empty;
            public string StreetName { get; set; } = string.Empty;
            public string SufType { get; set; } = string.Empty;
            public string SufDir { get; set; } = string.Empty;
            public string ADDR_NUM { get; set; } = string.Empty;
            public string FullAddr { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
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
            public string legaldesc { get; set; } = string.Empty;
            public string ANNEXING_CITY { get; set; } = string.Empty;
            public string PAAUNIQUENAME { get; set; } = string.Empty;
            public string PROPTYPE { get; set; } = string.Empty;
            public string KCA_ZONING { get; set; } = string.Empty;
            public string KCA_ACRES { get; set; } = string.Empty;
            public string PresentUseCode { get; set; } = string.Empty;
            public string PresentUseDescription { get; set; } = string.Empty;
            public string JURIS { get; set; } = string.Empty;
            public string CITYNAME { get; set; } = string.Empty;
            public string ZIP { get; set; } = string.Empty;
            public string ZIPCODE { get; set; } = string.Empty;
            public string ZIP_TYPE { get; set; } = string.Empty;
            public string COUNTY_NAME { get; set; } = string.Empty;
            public string PREFERRED_CITY { get; set; } = string.Empty;
            public string votdst { get; set; } = string.Empty;
            public string NAME { get; set; } = string.Empty;
            public string SUM_VOTERS { get; set; } = string.Empty;
            public string juddst { get; set; } = string.Empty;
            public string kccdst { get; set; } = string.Empty;
            public string COUNCILMEM { get; set; } = string.Empty;
            public string PHONE { get; set; } = string.Empty;
            public string URL { get; set; } = string.Empty;
            public string EMAIL { get; set; } = string.Empty;
            public string DIRDST { get; set; } = string.Empty;
            public string CONGDST { get; set; } = string.Empty;
            public string LEGDST { get; set; } = string.Empty;
            public string SCHDST { get; set; } = string.Empty;
            public string DSTNUM { get; set; } = string.Empty;
            public string PRIM_SRC { get; set; } = string.Empty;
            public string WRIA { get; set; } = string.Empty;
            public string PWS_ID { get; set; } = string.Empty;
            public string ADDRESS1 { get; set; } = string.Empty;
            public string ADDRESS2 { get; set; } = string.Empty;
            public string CITY { get; set; } = string.Empty;
            public string AREA_CODE { get; set; } = string.Empty;
            public string UGASIDE { get; set; } = string.Empty;
            public string EDITDATE { get; set; } = string.Empty;
            public string USERID { get; set; } = string.Empty;
            public string AREANAME { get; set; } = string.Empty;
            public string DISPLAYCLA { get; set; } = string.Empty;
            public string PRODUCTARE { get; set; } = string.Empty;
            public string FIPS_CO { get; set; } = string.Empty;
            public string PAGENUM { get; set; } = string.Empty;
            public string PAGE { get; set; } = string.Empty;
            public string ROW_NO { get; set; } = string.Empty;
            public string COL_LTR { get; set; } = string.Empty;
            public string BASIN_NAME { get; set; } = string.Empty;
            public string WTRSHD_NAME { get; set; } = string.Empty;
            public string WRIA_NO { get; set; } = string.Empty;
            public string WRIA_NAME { get; set; } = string.Empty;
            public string MILES_FROM { get; set; } = string.Empty;
            public string WIND_ROSE { get; set; } = string.Empty;
            public string TARGET_FID { get; set; } = string.Empty;
            public string GEOID10 { get; set; } = string.Empty;
            public string POP2010 { get; set; } = string.Empty;
            public string HU2010 { get; set; } = string.Empty;
            public string SingleFami { get; set; } = string.Empty;
            public string Totalparcels { get; set; } = string.Empty;
            public string Numberofparcelsover100ppm { get; set; } = string.Empty;
            public string ThresholdAppm { get; set; } = string.Empty;
            public string ThresholdBppm { get; set; } = string.Empty;
            public string Comments { get; set; } = string.Empty;
            public string CURRZONE { get; set; } = string.Empty;
            public string POTENTIAL { get; set; } = string.Empty;
            public string COND_CODE { get; set; } = string.Empty;
            public string CPLU { get; set; } = string.Empty;
            public string CSA_NAME { get; set; } = string.Empty;
            public string Uninc_Urban_Category { get; set; } = string.Empty;
            public string UniqueName { get; set; } = string.Empty;
            public string TOWN_NAME { get; set; } = string.Empty;
            public string STATUS { get; set; } = string.Empty;
            public string TSHED_STAT { get; set; } = string.Empty;
            public string SHEDNAME { get; set; } = string.Empty;
            public string CAT_CODE { get; set; } = string.Empty;
            [JsonProperty("_TYPE_")]
            [JsonPropertyName("_TYPE_")]
            public string TYPE_ { get; set; } = string.Empty;
            [JsonProperty("_CURRENCY_")]
            [JsonPropertyName("_CURRENCY_")]
            public string CURRENCY_ { get; set; } = string.Empty;
            [JsonProperty("_SOURCE_")]
            [JsonPropertyName("_SOURCE_")]
            public string SOURCE_ { get; set; } = string.Empty;
            public string DESIG { get; set; } = string.Empty;
            public string RECORDING_ { get; set; } = string.Empty;
            public string ComboAddress { get; set; } = string.Empty;
            public string HabitatType { get; set; } = string.Empty;
            public string WetlandClass { get; set; } = string.Empty;
            public string BufferWidth { get; set; } = string.Empty;
            public string ShoreEnvir { get; set; } = string.Empty;
            public string StreamName { get; set; } = string.Empty;
            public string ZoningDescription { get; set; } = string.Empty;
            public string Overlay { get; set; } = string.Empty;
            public string CompPlanDescription { get; set; } = string.Empty;
            public string WaterService { get; set; } = string.Empty;
            public string SewerService { get; set; } = string.Empty;
            public string StormService { get; set; } = string.Empty;
            public string PowerService { get; set; } = string.Empty;
            public string PoliceServ { get; set; } = string.Empty;
            public string GarbageSer { get; set; } = string.Empty;
        }
    }
}