using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConsoleProject.Model;
using ConsoleProject.Repository;
using Xunit;

namespace main_project.Tests
{
    public class FileRepositoryTests
    {
        [Fact]
        public async Task GetData_ReturnsEmptyList_WhenApiUnavailable()
        {

            var sw = new StringWriter();
            Console.SetOut(sw);

            var result = await FileRepository.GetData();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteFileById_PrintsFailureMessage_WhenApiUnavailable()
        {
            string fakeId = "nonexistent-id";

            var sw = new StringWriter();
            Console.SetOut(sw);

            await FileRepository.DeleteFileById(fakeId);

            var output = sw.ToString();
            Assert.Contains("Exception occurred while deleting:", output);
        }

        [Fact]
        public async Task SearchData_PrintsNoDataAvailable_WhenGetDataEmpty()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            await FileRepository.SearchData("anything");

            var output = sw.ToString();
            Assert.Contains("No data available.", output);
        }
    }
}
