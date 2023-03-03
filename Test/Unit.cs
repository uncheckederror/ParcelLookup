using Microsoft.Extensions.Configuration;
using ParcelLookup.Models;
using Xunit.Abstractions;

namespace Test
{
    public class Unit
    {
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _output;
        private readonly AppConfiguration _appConfiguration;

        public Unit(ITestOutputHelper output)
        {
            _output = output;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AppConfiguration config = new();
            _configuration.Bind(config);
            _appConfiguration = config;
        }

        [Fact]
        public void SchoolDistrictsConversion()
        {
            var empty = ParcelLookup.Pages.DistrictsReportModel.GetSchoolDistrictLinkByNumber(string.Empty, _appConfiguration.SchoolDistrictLinks);
            Assert.NotNull(empty);
            Assert.Equal(string.Empty, empty);

            var bad = ParcelLookup.Pages.DistrictsReportModel.GetSchoolDistrictLinkByNumber("2", _appConfiguration.SchoolDistrictLinks);
            Assert.NotNull(bad);
            Assert.Equal(string.Empty, bad);

            var seattle = ParcelLookup.Pages.DistrictsReportModel.GetSchoolDistrictLinkByNumber("1", _appConfiguration.SchoolDistrictLinks);
            Assert.NotNull(seattle);
            Assert.Equal("https://www.seattleschools.org/", seattle);

            var fife = ParcelLookup.Pages.DistrictsReportModel.GetSchoolDistrictLinkByNumber("888", _appConfiguration.SchoolDistrictLinks);
            Assert.NotNull(fife);
            Assert.Equal("https://www.fifeschools.com/", fife);
        }

        [Fact]
        public void WatershedConversion()
        {
            var empty = ParcelLookup.Pages.DistrictsReportModel.GetWatershedLinkByName(string.Empty, _appConfiguration.WatershedLinks);
            Assert.NotNull(empty);
            Assert.Equal(string.Empty, empty);

            var bad = ParcelLookup.Pages.DistrictsReportModel.GetWatershedLinkByName("Desertshed 0", _appConfiguration.WatershedLinks);
            Assert.NotNull(bad);
            Assert.Equal(string.Empty, bad);

            var puget = ParcelLookup.Pages.DistrictsReportModel.GetWatershedLinkByName("Central Puget Sound", _appConfiguration.WatershedLinks);
            Assert.NotNull(puget);
            Assert.Equal("https://www.kingcounty.gov/environment/watersheds/central-puget-sound/vashon-maury-island.aspx", puget);
        }

        [Fact]
        public void LandUseConversion()
        {
            var empty = ParcelLookup.Pages.DistrictsReportModel.GetLandUseCodeByShortCode(string.Empty);
            Assert.NotNull(empty);
            Assert.Equal(string.Empty, empty);

            var bad = ParcelLookup.Pages.DistrictsReportModel.GetLandUseCodeByShortCode("aa");
            Assert.NotNull(bad);
            Assert.Equal(string.Empty, bad);

            var activityCenter = ParcelLookup.Pages.DistrictsReportModel.GetLandUseCodeByShortCode("ac");
            Assert.NotNull(activityCenter);
            Assert.Equal("Unincorporated Activity Center (ac)", activityCenter);

            var otherPark = ParcelLookup.Pages.DistrictsReportModel.GetLandUseCodeByShortCode("op");
            Assert.NotNull(otherPark);
            Assert.Equal("Other Parks/ Wilderness (op)", otherPark);
        }

        [Fact]
        public void WRIAConversion()
        {
            var empty = ParcelLookup.Pages.DistrictsReportModel.GetWRIALinkByWRIANumber(string.Empty, _appConfiguration.WRIALinks);
            Assert.NotNull(empty);
            Assert.Equal(string.Empty, empty);

            var bad = ParcelLookup.Pages.DistrictsReportModel.GetWRIALinkByWRIANumber("FunkyTown", _appConfiguration.WRIALinks);
            Assert.NotNull(bad);
            Assert.Equal(string.Empty, bad);
        }

        [Fact]
        public void JurisdictionConversion()
        {
            var empty = ParcelLookup.Pages.DistrictsReportModel.GetJuristictionLinkByParcelNumber(string.Empty, string.Empty, _appConfiguration.JurisdictionLinks);
            Assert.NotNull(empty);
            Assert.Equal(string.Empty, empty);

            var bad = ParcelLookup.Pages.DistrictsReportModel.GetJuristictionLinkByParcelNumber("8675309", "FunkyTown", _appConfiguration.JurisdictionLinks);
            Assert.NotNull(bad);
            Assert.Equal(string.Empty, bad);

            var seattle = ParcelLookup.Pages.DistrictsReportModel.GetJuristictionLinkByParcelNumber("1689400860", "Seattle", _appConfiguration.JurisdictionLinks);
            Assert.NotNull(seattle);
            Assert.Equal("https://web6.seattle.gov/dpd/ParcelData/Parcel.aspx?pin=1689400860", seattle);

            var newcastle = ParcelLookup.Pages.DistrictsReportModel.GetJuristictionLinkByParcelNumber("4113800190", "Newcastle", _appConfiguration.JurisdictionLinks);
            Assert.NotNull(newcastle);
            Assert.Equal("http://www.newcastlewa.gov/4113800190", newcastle);
        }
    }
}