namespace WebApp.Extensions;

public static class UseSwaggerExtension
{
    public static void UseSwagger(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "V1");
            c.RoutePrefix = "api/swagger";
        });
    }
}
