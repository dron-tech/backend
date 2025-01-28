namespace Application.Common.Utils;

public static class FileHelperUtil
{
    private static readonly string[] ValidVideoExtension = {"video/mp4", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv", "video/x-matroska"};
    public static string GenerateFileName(string contentType)
    {
        var extension = contentType switch
        {
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            "video/mp4" => ".mp4",
            "image/gif" => ".gif",
            "video/quicktime" => ".mov",
            "video/x-msvideo" => ".avi",
            "video/x-ms-wmv" => ".wmv",
            "video/x-matroska" => ".mkv",
            _ => throw new ArgumentException("Unsupported content type")
        };

        return Guid.NewGuid() + extension;
    }
    
    public static bool CheckValidFileExtension(string contentType)
    {
        return contentType is "image/png" or "image/jpeg" or "image/jpg";
    }

    public static bool CheckValidVideoExtension(string contentType)
    {
        return ValidVideoExtension.Contains(contentType);
    }
}
