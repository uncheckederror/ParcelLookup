using Flurl.Http;
using Microsoft.Extensions.Configuration;
using ParcelLookup.Models;
using System.Collections;
using Xunit.Abstractions;

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
    }
}