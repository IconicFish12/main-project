using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using main_project.Services;

namespace main_project.Tests
{
    public class FileHelperTests
    {
        [Theory]
        [InlineData(".jpg", "image")]
        [InlineData(".pdf", "document/pdf")]
        [InlineData(".unknown", "others")]
        public void GetFolderExtension_ReturnsCorrectFolder(string ext, string expected)
        {
            var result = FileHelper.GetFolderExtension(ext);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteJson_And_ReadJson_WorksCorrectly()
        {
            var data = new List<string> { "a", "b", "c" };
            var path = Path.GetTempFileName();

            FileHelper.WriteJson(data, path);
            var result = FileHelper.ReadJson<string>(path);

            Assert.Equal(data, result);
            File.Delete(path);
        }
    }
}
