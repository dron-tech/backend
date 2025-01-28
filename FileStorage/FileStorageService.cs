using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public FileStorageService(IHostingEnvironment env)
    {
        _rootPath = env.WebRootPath;
    }
    
    public async Task Save(IFormFile file, string fileName)
    {
        var fullPath = Path.Combine(_rootPath, fileName);
        
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public void Delete(string fileName)
    {
        var fullPath = Path.Combine(_rootPath, fileName);
        if (!File.Exists(fullPath))
        {
            return;
        }

        File.Delete(fullPath);
    }
}
