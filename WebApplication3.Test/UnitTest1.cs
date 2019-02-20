using Pose;
using System;
using WebApplication3.Services;
using Xunit;

namespace WebApplication3.Test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("2/20/2019 11:10:15 AM")]
        [InlineData("2/20/2019 4:10:15 AM")]
        [InlineData("2/20/2019 12:10:15 AM")]
        public void GreetService_GoodMorning(string time)
        {
            var dateTime = Convert.ToDateTime(time);

            Shim shim = Shim.Replace(() => DateTime.Now).With(() => dateTime);

            var greetService = new GreetService();

            var response = string.Empty;

            PoseContext.Isolate(() =>
            {
                response = greetService.Greet();
            }, shim);

            Assert.Equal("Good morning!", response);
        }

        [Fact]
        public void GreetService_HaveGreatDay()
        {
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => new DateTime(2019, 2, 20, 20, 50, 0));

            var greetService = new GreetService();

            var response = string.Empty;

            PoseContext.Isolate(() =>
            {
                response = greetService.Greet();
            }, shim);

            Assert.Equal("Have a great day!", response);
        }
    }
}
