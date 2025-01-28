using AppleAuth;
using Application;
using Application.Common.Interfaces;
using AwsS3;
using Email;
using FileStorage;
using Microsoft.EntityFrameworkCore;
using Moralis;
using NftMetadataLoader;
using Persistence;
using SmartContract;
using ThirdPartyAuthService;
using ThirdPartyAuthService.Services.Facebook;
using ThirdPartyAuthService.Services.Google;
using WebApp.Extensions;
using WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<ContextDb>(opt =>
    opt.UseNpgsql(config.GetConnectionString("Context")));

builder.Services.AddHttpClient<IThirdPartyAuthService, GoogleAuthService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

builder.Services.AddHttpClient<IThirdPartyAuthService, FacebookAuthService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

builder.Services.AddTransient<IThirdPartyAuthFactory, ThirdPartyAuthFactory>();

builder.Services.AddHttpClient<IAppleAuthService, AppleAuthService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

builder.Services.AddHttpClient<INftMetadataLoaderService, NftMetadataLoaderService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = true });

builder.Services.AddHttpClient<IMoralisService, MoralisService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = true });

builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddConfigs(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwt(config);

builder.Services.AddApplication();
builder.Services.AddPersistence();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAwsService, AwsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContextDb>();
    
    if (app.Environment.IsDevelopment())
    {
        //await context.Database.EnsureDeletedAsync();
    }
    
    await context.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true)
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
