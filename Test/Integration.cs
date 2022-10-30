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
            "3876311580"
        };

        public static IEnumerable<object[]> GetPINsGenerator()
        {
            foreach (var account in _parcelNumbers)
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

        public Integration(ITestOutputHelper output)
        {
            _output = output;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Fact]
        public async Task DistrictsReport()
        {
            var reportFactory = new ParcelLookup.Pages.DistrictsReportModel(_configuration);
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

        [Fact]
        public async Task OnGetAsyncTest()
        {
            var reportFactory = new ParcelLookup.Pages.DistrictsReportModel(_configuration);
            await reportFactory.OnGetAsync("7010700760");
            await reportFactory.OnGetAsync("1924089026");
            await reportFactory.OnGetAsync("0761000160");
            await reportFactory.OnGetAsync("0098300430");
            await reportFactory.OnGetAsync("202350019008");
            await reportFactory.OnGetAsync("3876311580");
        }
    }
}