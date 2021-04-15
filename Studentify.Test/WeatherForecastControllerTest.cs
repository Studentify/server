using NUnit.Framework;

namespace Studentify.Test
{
    [TestFixture]
    public class WeatherForecastControllerTest
    {
        Controllers.WeatherForecastController m_controller;

        [SetUp]
        public void Setup()
        {
            m_controller = new Controllers.WeatherForecastController();
        }

        [Test]
        public void AlwaysTrue()
        {
            Assert.Pass();
        }

        [Test]
        public void GetCommandTest()
        {
            WeatherForecast[] randomTemperatures = (WeatherForecast[]) m_controller.Get();
            int expectedArrayLength = 5;

            Assert.AreEqual(expectedArrayLength, randomTemperatures.Length);
        }
    }
}