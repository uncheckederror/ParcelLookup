namespace ParcelLookup.Models
{
    public class MapServiceDescription
    {
        public float currentVersion { get; set; }
        public string serviceDescription { get; set; } = string.Empty;
        public string mapName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string copyrightText { get; set; } = string.Empty;
        public bool supportsDynamicLayers { get; set; }
        public Layer[] layers { get; set; } = Array.Empty<Layer>();
        public object[] tables { get; set; } = Array.Empty<object>();
        public Spatialreference spatialReference { get; set; } = new();
        public bool singleFusedMapCache { get; set; }
        public Initialextent initialExtent { get; set; } = new();
        public Fullextent fullExtent { get; set; } = new();
        public int minScale { get; set; }
        public int maxScale { get; set; }
        public string units { get; set; } = string.Empty;
        public string supportedImageFormatTypes { get; set; } = string.Empty;
        public Documentinfo documentInfo { get; set; } = new();
        public string capabilities { get; set; } = string.Empty;
        public string supportedQueryFormats { get; set; } = string.Empty;
        public bool exportTilesAllowed { get; set; }
        public int referenceScale { get; set; }
        public Datumtransformation[] datumTransformations { get; set; } = Array.Empty<Datumtransformation>();
        public bool supportsDatumTransformation { get; set; }
        public int maxRecordCount { get; set; }
        public int maxImageHeight { get; set; }
        public int maxImageWidth { get; set; }
        public string supportedExtensions { get; set; } = string.Empty;
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
            public Spatialreference spatialReference { get; set; } = new();
        }

        public class Fullextent
        {
            public float xmin { get; set; }
            public float ymin { get; set; }
            public float xmax { get; set; }
            public float ymax { get; set; }
            public Spatialreference spatialReference { get; set; } = new();
        }

        public class Documentinfo
        {
            public string Title { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public string Comments { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string AntialiasingMode { get; set; } = string.Empty;
            public string TextAntialiasingMode { get; set; } = string.Empty;
            public string Keywords { get; set; } = string.Empty;
        }

        public class Layer
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public int parentLayerId { get; set; }
            public bool defaultVisibility { get; set; }
            public object subLayerIds { get; set; } = string.Empty;
            public int minScale { get; set; }
            public int maxScale { get; set; }
            public string type { get; set; } = string.Empty;
            public string geometryType { get; set; } = string.Empty;
        }

        public class Datumtransformation
        {
            public Geotransform[] geoTransforms { get; set; } = Array.Empty<Geotransform>();
        }

        public class Geotransform
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
            public bool transformForward { get; set; }
            public string name { get; set; } = string.Empty;
        }
    }
}