using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using ParcelLookup.Models;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using Xunit.Abstractions;
using System.Net.Http;

namespace Test
{
    public class TestData : IEnumerable<object[]>
    {
        static readonly string[] _parcelNumbers = new[]
{
            "7010700760",
            "1924089026",
            "0761000160",
            "0098300430",
            "3876311580",
            "0822039083",
            "1249700005"
        };

        static readonly string[] _condos = new[]
        {
            "202350019008",
            "8603200060"
        };

        static readonly string[] _addresses = new[]
{
            "4415 31st Ave W",
            "6116 36th Ave NW",
            "8715 28th Ave NW",
            "7012 18th Ave NW",
            "3403 21st Ave W",
            "531 10th Ave E",
            "1760 NW 56TH ST"
        };

        public static IEnumerable<object[]> GetStaticExternalLinks()
        {
            var json = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AppConfiguration config = new();
            json.Bind(config);

            yield return new object[] { config.WRIALinks.WRIA7 };
            yield return new object[] { config.WRIALinks.WRIA8 };
            yield return new object[] { config.WRIALinks.WRIA9 };
            yield return new object[] { config.WRIALinks.WRIA10 };
            yield return new object[] { config.JurisdictionLinks.NewcastleURL };
            yield return new object[] { config.WatershedLinks.PugetSound };
            yield return new object[] { config.WatershedLinks.Cedar };
            yield return new object[] { config.WatershedLinks.Sammamish };
            yield return new object[] { config.WatershedLinks.Snoqualmie };
            yield return new object[] { config.WatershedLinks.Green };
            yield return new object[] { config.WatershedLinks.White };
            yield return new object[] { config.SchoolDistrictLinks.Highline };
            yield return new object[] { config.SchoolDistrictLinks.Seattle };
            yield return new object[] { config.SchoolDistrictLinks.LkWA };
            yield return new object[] { config.SchoolDistrictLinks.Mercer };
            yield return new object[] { config.SchoolDistrictLinks.Northshore };
            yield return new object[] { config.SchoolDistrictLinks.Shoreline };
            yield return new object[] { config.SchoolDistrictLinks.Tahoma };
            yield return new object[] { config.SchoolDistrictLinks.FedWay };
            yield return new object[] { config.SchoolDistrictLinks.Issaquah };
            yield return new object[] { config.SchoolDistrictLinks.Riverview };
            yield return new object[] { config.SchoolDistrictLinks.Tukwila };
            yield return new object[] { config.SchoolDistrictLinks.Snoqualmie };
            yield return new object[] { config.SchoolDistrictLinks.Kent };
            yield return new object[] { config.SchoolDistrictLinks.Auburn };
            yield return new object[] { config.SchoolDistrictLinks.Renton };
            yield return new object[] { config.SchoolDistrictLinks.Bellevue };
            yield return new object[] { config.SchoolDistrictLinks.Enumclaw };
            yield return new object[] { config.SchoolDistrictLinks.Vashon };
            yield return new object[] { config.SchoolDistrictLinks.Skykomish };
            yield return new object[] { config.SchoolDistrictLinks.Fife };
        }

        public static IEnumerable<object[]> GetPINsGenerator()
        {
            foreach (var account in _parcelNumbers)
            {
                yield return new object[] { account };
            }
        }

        public static IEnumerable<object[]> GetCondosGenerator()
        {
            foreach (var account in _condos)
            {
                yield return new object[] { account };
            }
        }

        public static IEnumerable<object[]> GetAddressesGenerator()
        {
            foreach (var account in _addresses)
            {
                yield return new object[] { account };
            }
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class Integration
    {
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _output;
        private readonly AppConfiguration _appConfiguration;

        public Integration(ITestOutputHelper output)
        {
            _output = output;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AppConfiguration config = new();
            _configuration.Bind(config);
            _appConfiguration = config;
        }

        [Fact]
        public async Task DistrictsReport()
        {
            var reportFactory = new ParcelLookup.Pages.DistrictsReportModel(_appConfiguration);
            // This would ideally be a real instance of the ParcelInfo data model.
            var empty = await reportFactory.DistrictsReport(new ParcelLookup.Models.ParcelInfo { });
            Assert.NotNull(empty);
        }

        [Theory]
        [MemberData(nameof(TestData.GetPINsGenerator), MemberType = typeof(TestData))]
        public async Task GetParcelInfoTest(string PIN)
        {
            var parcelInfoUrl = $"{_configuration["KingCountyAPIs:ParcelInfo"]}{PIN}";
            // Could this API call be replaced with a direct database call?
            ParcelInfo info = await parcelInfoUrl.GetJsonAsync<ParcelInfo>();
            Assert.NotNull(info);
            Assert.False(string.IsNullOrWhiteSpace(info.Parcel));
            Assert.False(info.HasError);
            Assert.False(string.IsNullOrWhiteSpace(info.Jurisdiction.Value));
        }

        [Theory]
        [MemberData(nameof(TestData.GetStaticExternalLinks), MemberType = typeof(TestData))]
        public async Task VerifyExternalLink(string externalLink)
        {
            using var client = new HttpClient();
            // If we don't pretend to be a browser some of these webservers will drop or close our request.
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.59");
            var content = await client.GetAsync(externalLink);
            _output.WriteLine($"{content.StatusCode}");
            Assert.True(content.IsSuccessStatusCode);
            _output.WriteLine($"{externalLink} is still a valid link.");
        }
    }
}