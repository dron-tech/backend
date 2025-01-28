namespace Application.Common.Interfaces;

public interface IAwsService
{
    public Task UploadFile(string fileName, Stream stream, string contentType);
    public Task UploadFile(string fileName, string path, string contentType);
    public Task DeleteFile(string fileName);
}
