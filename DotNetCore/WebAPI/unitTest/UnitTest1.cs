using WebAPI.Controllers;
using Xunit.Abstractions;

namespace unitTest
{
    public class UnitTest1
    {
        private ITestOutputHelper _logger;

        // DI로 logger 받아들여서 필드에 추가
        public UnitTest1(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public void TestGetCount()
        {
            var controller = new WeatherForecastController(null);

            var datas = controller.Get();

            int expected = 5;
            int actual = datas.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGet()
        {
            var controller = new WeatherForecastController(null);

            var datas = controller.Get();

            foreach (var data in datas)
            {
                _logger.WriteLine($"{data.Date}: {data.TemperatureC},{data.Summary}");
            }


            int expected = 5;
            int actual = datas.Count();

            Assert.Equal(expected, actual);
        }
    }
}