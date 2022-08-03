using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ParcelLookup.Models
{
    public class DistrictsReportIdentify
    {
        public Result[] results { get; set; }
        public class Result
        {
            public int layerId { get; set; }
            public string layerName { get; set; }
            public string displayFieldName { get; set; }
            public string value { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Attributes
        {
            public string OBJECTID { get; set; }
            public string MAJOR { get; set; }
            public string MINOR { get; set; }
            public string PIN { get; set; }
            [JsonProperty("Shape.STArea()")]
            [JsonPropertyName("Shape.STArea()")]
            public string ShapeSTArea { get; set; }
            [JsonProperty("Shape.STLength()")]
            [JsonPropertyName("Shape.STLength()")]
            public string ShapeSTLength { get; set; }
            public string ObjectID { get; set; }
            public string shape { get; set; }
            public string ADDR_COMMENTS { get; set; }
            public string ADDR_TYPE { get; set; }
            public string ADDRALIAS1 { get; set; }
            public string ADDRALIAS2 { get; set; }
            public string ESITEID { get; set; }
            public string HouseNum { get; set; }
            public string PreDir { get; set; }
            public string PreType { get; set; }
            public string StreetName { get; set; }
            public string SufType { get; set; }
            public string SufDir { get; set; }
            public string ADDR_NUM { get; set; }
            public string FullAddr { get; set; }
            public string FullName { get; set; }
            public string ZIP5 { get; set; }
            public string PLUS4 { get; set; }
            public string CTYNAME { get; set; }
            public string POSTALCTYNAME { get; set; }
            public string LAT { get; set; }
            public string LON { get; set; }
            public string POINT_X { get; set; }
            public string POINT_Y { get; set; }
            public string COUNTY { get; set; }
            public string KROLL { get; set; }
            public string KCTP_CITY { get; set; }
            public string KCTP_STATE { get; set; }
            public string PLSS { get; set; }
            public string PROP_NAME { get; set; }
            public string PLAT_NAME { get; set; }
            public string PLAT_LOT { get; set; }
            public string PLAT_BLOCK { get; set; }
            public string LOTSQFT { get; set; }
            public string LEVYCODE { get; set; }
            public string LEVY_JURIS { get; set; }
            public string NEW_CONSTR { get; set; }
            public string TAXVAL_RSN { get; set; }
            public string APPRLNDVAL { get; set; }
            public string APPR_IMPR { get; set; }
            public string TAX_LNDVAL { get; set; }
            public string TAX_IMPR { get; set; }
            public string ACCNT_NUM { get; set; }
            public string KCTP_NAME { get; set; }
            public string KCTP_ATTN { get; set; }
            public string KCTP_ADDR { get; set; }
            public string KCTP_CTYST { get; set; }
            public string KCTP_ZIP { get; set; }
            public string KCTP_TAXYR { get; set; }
            public string UNIT_NUM { get; set; }
            public string BLDG_NUM { get; set; }
            public string CONDOSITUS { get; set; }
            public string QTS { get; set; }
            public string SEC { get; set; }
            public string TWP { get; set; }
            public string RNG { get; set; }
            public string PRIMARY_ADDR { get; set; }
            public string legaldesc { get; set; }
            public string ANNEXING_CITY { get; set; }
            public string PAAUNIQUENAME { get; set; }
            public string PROPTYPE { get; set; }
            public string KCA_ZONING { get; set; }
            public string KCA_ACRES { get; set; }
            public string PresentUseCode { get; set; }
            public string PresentUseDescription { get; set; }
            public string JURIS { get; set; }
            public string CITYNAME { get; set; }
            public string ZIP { get; set; }
            public string ZIPCODE { get; set; }
            public string ZIP_TYPE { get; set; }
            public string COUNTY_NAME { get; set; }
            public string PREFERRED_CITY { get; set; }
            public string votdst { get; set; }
            public string NAME { get; set; }
            public string SUM_VOTERS { get; set; }
            public string juddst { get; set; }
            public string kccdst { get; set; }
            public string COUNCILMEM { get; set; }
            public string PHONE { get; set; }
            public string URL { get; set; }
            public string EMAIL { get; set; }
            public string DIRDST { get; set; }
            public string CONGDST { get; set; }
            public string LEGDST { get; set; }
            public string SCHDST { get; set; }
            public string DSTNUM { get; set; }
            public string PRIM_SRC { get; set; }
            public string WRIA { get; set; }
            public string PWS_ID { get; set; }
            public string ADDRESS1 { get; set; }
            public string ADDRESS2 { get; set; }
            public string CITY { get; set; }
            public string AREA_CODE { get; set; }
            public string UGASIDE { get; set; }
            public string EDITDATE { get; set; }
            public string USERID { get; set; }
            public string AREANAME { get; set; }
            public string DISPLAYCLA { get; set; }
            public string PRODUCTARE { get; set; }
            public string FIPS_CO { get; set; }
            public string PAGENUM { get; set; }
            public string PAGE { get; set; }
            public string ROW_NO { get; set; }
            public string COL_LTR { get; set; }
            public string BASIN_NAME { get; set; }
            public string WTRSHD_NAME { get; set; }
            public string WRIA_NO { get; set; }
            public string WRIA_NAME { get; set; }
            public string MILES_FROM { get; set; }
            public string WIND_ROSE { get; set; }
            public string TARGET_FID { get; set; }
            public string GEOID10 { get; set; }
            public string POP2010 { get; set; }
            public string HU2010 { get; set; }
            public string SingleFami { get; set; }
            public string Totalparcels { get; set; }
            public string Numberofparcelsover100ppm { get; set; }
            public string ThresholdAppm { get; set; }
            public string ThresholdBppm { get; set; }
            public string Comments { get; set; }
            public string CURRZONE { get; set; }
            public string POTENTIAL { get; set; }
            public string COND_CODE { get; set; }
            public string CPLU { get; set; }
            public string CSA_NAME { get; set; }
            public string Uninc_Urban_Category { get; set; }
            public string UniqueName { get; set; }
            public string TOWN_NAME { get; set; }
            public string STATUS { get; set; }
            public string SHEDNAME { get; set; }
            public string CAT_CODE { get; set; }
            public string TYPE_ { get; set; }
            public string CURRENCY_ { get; set; }
            public string SOURCE_ { get; set; }
            public string DESIG { get; set; }
            public string RECORDING_ { get; set; }
            public string ComboAddress { get; set; }
            public string HabitatType { get; set; }
            public string WetlandClass { get; set; }
            public string BufferWidth { get; set; }
            public string ShoreEnvir { get; set; }
            public string StreamName { get; set; }
            public string ZoningDescription { get; set; }
            public string Overlay { get; set; }
            public string CompPlanDescription { get; set; }
            public string WaterService { get; set; }
            public string SewerService { get; set; }
            public string StormService { get; set; }
            public string PowerService { get; set; }
            public string PoliceServ { get; set; }
            public string GarbageSer { get; set; }
        }
    }
}