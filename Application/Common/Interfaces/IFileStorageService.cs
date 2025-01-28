using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileStorageService
{
    public Task Save(IFormFile file, string fileName);
    public void Delete(string fileName);
}
