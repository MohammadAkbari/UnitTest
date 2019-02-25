using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Pose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using WebApplication3.Controllers;
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

        [Fact]
        public void  Upload_document_should_upload_document_and_return_dto()
        {
            //https://stackoverflow.com/questions/40860305/unit-testing-fileupload-with-moq-net-core
            //https://stackoverflow.com/questions/8308899/unit-test-a-file-upload-how

            var controller = new HomeController();

            var goalId = Guid.NewGuid();

            controller.ControllerContext = RequestWithFile();
            var result = controller.UploadFile();

            Assert.Equal(1, 1);
        }

        private ControllerContext RequestWithFile()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 20, "Data", "dummy.jpg");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            return new ControllerContext(actx);
        }
    }
}
