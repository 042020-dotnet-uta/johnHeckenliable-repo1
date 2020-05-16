using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TeamProject_p1.IntegrationTests
{
    public class DailyTasksTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public DailyTasksTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Privacy")]
        [InlineData("/DailyTasks")]
        [InlineData("/DailyTasks/Index")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // var content = await response.Content.ReadAsStringAsync();

            // Assert
            // we got a success response
            //  that is an HTML page
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetDailyTasksHasDailyTaskOnPage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("DailyTasks");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            // that the page contains the description of the task in the test DB.
            Assert.Contains(_factory.TasksInDb[0].Description, content);
        }

        // example of getting a form and sending it back
        // (using AngleSharp 3rd-party package)
        // https://github.com/dotnet/AspNetCore.Docs/blob/master/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/IntegrationTests/IndexPageTests.cs#L38
    }
}
