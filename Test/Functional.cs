using Microsoft.Extensions.Configuration;
using ParcelLookup.Models;
using ParcelLookup.Pages;
using Xunit.Abstractions;

namespace Test
{
    public class Functional
    {
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _output;
        private readonly AppConfiguration _appConfiguration;

        public Functional(ITestOutputHelper output)
        {
            _output = output;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AppConfiguration config = new();
            _configuration.Bind(config);
            _appConfiguration = config;
        }

        [Theory]
        [MemberData(nameof(TestData.GetPINsGenerator), MemberType = typeof(TestData))]
        public async Task ByPIN(string PIN)
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync(PIN);
            Assert.Equal(string.Empty, indexPage.Message);
            Assert.Equal(PIN, indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
        }

        [Theory]
        [MemberData(nameof(TestData.GetAddressesGenerator), MemberType = typeof(TestData))]
        public async Task ByAddress(string address)
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync(address);
            Assert.Equal(string.Empty, indexPage.Message);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.ParcelInfo);
        }

        [Theory]
        [MemberData(nameof(TestData.GetCondosGenerator), MemberType = typeof(TestData))]
        public async Task CondoMessage(string PIN)
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync(PIN);
            Assert.Equal($"{PIN[..10]} is a condo unit within parcel number {PIN[..6]}.", indexPage.Message);
            Assert.Equal(PIN[..10], indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.Report);
        }

        [Fact]
        public async Task IsCondoMessageByAddress()
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync("2145 Dexter Ave N");
            Assert.Equal($"2023500000 is a condo unit within parcel number 202350.", indexPage.Message);
            Assert.Equal("2023500000", indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.Report);
        }

        [Fact]
        public async Task IsNotCondoMessage()
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync("7010700760");
            Assert.NotEqual($"7010700760 is a condo unit within parcel number 701070.", indexPage.Message);
            Assert.Equal("", indexPage.Message);
            Assert.Equal("7010700760", indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.Report);
        }

        [Fact]
        public async Task IsHydroMessage()
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync("302504HYDR");
            Assert.Equal($"This parcel number 302504HYDR ends with HYDR, which represents a water body within the plat.", indexPage.Message);
            Assert.Equal("302504HYDR", indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.Report);
        }

        [Fact]
        public async Task IsNotHydroMessage()
        {
            var indexPage = new DistrictsReportModel(_appConfiguration);
            await indexPage.OnGetAsync("7010700760");
            Assert.NotEqual($"This parcel number 302504HYDR ends with HYDR, which represents a water body within the plat.", indexPage.Message);
            Assert.Equal("7010700760", indexPage.ParcelNumber);
            Assert.NotNull(indexPage.ParcelInfo);
            Assert.NotNull(indexPage.Report);
        }
    }
}