namespace WebApp.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();
    public int Status { get; set; }
}
