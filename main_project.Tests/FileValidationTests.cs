using FileValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main_project.Tests
{
    public class FileValidationTest
    {
        private readonly DefaultFileValidation _validation = new DefaultFileValidation();

        [Fact]
        public void Validate_Throws_WhenFileIsEmpty()
        {
   
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

       
            var ex = Assert.Throws<ArgumentException>(() => _validation.validate(mockFile.Object));
            Assert.Equal("File cannot be empty.", ex.Message);
        }

        [Fact]
        public void Validate_Throws_WhenFileIsTooLarge()
        {
            
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(15 * 1024 * 1024); // 15 MB

            
            var ex = Assert.Throws<ArgumentException>(() => _validation.validate(mockFile.Object));
            Assert.Equal("File size exceeds the maximum allowed size of 5 MB.", ex.Message); // 10 MB
        }

        [Fact]
        public void Validate_Throws_WhenExtensionIsNotAllowed()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1000);
            mockFile.Setup(f => f.FileName).Returns("test.exe");

            var ex = Assert.Throws<ArgumentException>(() => _validation.validate(mockFile.Object));
            Assert.Equal("File type .exe is not supported.", ex.Message);
        }

        [Fact]
        public void Validate_Passes_WhenFileIsValid()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024 * 1024); // 1 MB
            mockFile.Setup(f => f.FileName).Returns("file.pdf");

            var exception = Record.Exception(() => _validation.validate(mockFile.Object));
            Assert.Null(exception); // Tidak ada exception = valid
        }
    }
}
