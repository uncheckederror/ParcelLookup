using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Test
{
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

            var empty = await reportFactory.DistrictsReport(new ParcelLookup.Models.ParcelInfo { });
            Assert.NotNull(empty);
        }
    }
}