namespace ParcelLookup.Models
{
    public class MapServiceDescription
    {
        public float currentVersion { get; set; }
        public string serviceDescription { get; set; }
        public string mapName { get; set; }
        public string description { get; set; }
        public string copyrightText { get; set; }
        public bool supportsDynamicLayers { get; set; }
        public Layer[] layers { get; set; }
        public object[] tables { get; set; }
        public Spatialreference spatialReference { get; set; }
        public bool singleFusedMapCache { get; set; }
        public Initialextent initialExtent { get; set; }
        public Fullextent fullExtent { get; set; }
        public int minScale { get; set; }
        public int maxScale { get; set; }
        public string units { get; set; }
        public string supportedImageFormatTypes { get; set; }
        public Documentinfo documentInfo { get; set; }
        public string capabilities { get; set; }
        public string supportedQueryFormats { get; set; }
        public bool exportTilesAllowed { get; set; }
        public int referenceScale { get; set; }
        public Datumtransformation[] datumTransformations { get; set; }
        public bool supportsDatumTransformation { get; set; }
        public int maxRecordCount { get; set; }
        public int maxImageHeight { get; set; }
        public int maxImageWidth { get; set; }
        public string supportedExtensions { get; set; }
        public bool resampling { get; set; }
        public class Spatialreference
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }

        public class Initialextent
        {
            public float xmin { get; set; }
            public float ymin { get; set; }
            public float xmax { get; set; }
            public float ymax { get; set; }
            public Spatialreference spatialReference { get; set; }
        }

        public class Fullextent
        {
            public float xmin { get; set; }
            public float ymin { get; set; }
            public float xmax { get; set; }
            public float ymax { get; set; }
            public Spatialreference spatialReference { get; set; }
        }

        public class Documentinfo
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Comments { get; set; }
            public string Subject { get; set; }
            public string Category { get; set; }
            public string AntialiasingMode { get; set; }
            public string TextAntialiasingMode { get; set; }
            public string Keywords { get; set; }
        }

        public class Layer
        {
            public int id { get; set; }
            public string name { get; set; }
            public int parentLayerId { get; set; }
            public bool defaultVisibility { get; set; }
            public object subLayerIds { get; set; }
            public int minScale { get; set; }
            public int maxScale { get; set; }
            public string type { get; set; }
            public string geometryType { get; set; }
        }

        public class Datumtransformation
        {
            public Geotransform[] geoTransforms { get; set; }
        }

        public class Geotransform
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
            public bool transformForward { get; set; }
            public string name { get; set; }
        }
    }
}