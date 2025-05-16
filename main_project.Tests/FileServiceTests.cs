using Xunit;
using main_project.Services;
using main_project.Model;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FileServiceTests
{
    [Fact]
    public void GetAllFile_ShouldReturnListOfMetadata()
    {
        var service = new FileService();

        var result = service.getAllFile();

        Assert.NotNull(result);
        Assert.IsType<List<FileMetaData>>(result);
    }

    [Fact]
    public async Task UploadFile_ShouldReturnFileMetaData()
    {
        var content = "Dummy content";
        var fileName = "test.txt";
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileMock = new Mock<IFormFile>();

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(memoryStream.Length);
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns((Stream stream, System.Threading.CancellationToken token) =>
        {
            return memoryStream.CopyToAsync(stream);
        });

        var service = new FileService();

        var result = await service.uploadFIle(fileMock.Object);

        Assert.NotNull(result);
        Assert.Equal("txt", result.file_type);
        Assert.Equal("test", result.filename);
    }
}
