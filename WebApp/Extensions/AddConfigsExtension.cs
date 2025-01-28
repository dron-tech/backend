using AppleAuth.Common.Configs;
using Application.Common.Configs;
using AwsS3.Common;
using Email.Common;
using Moralis.Common.Configs;
using SmartContract.Common;
using ThirdPartyAuthService.Services.Facebook.Common;
using ThirdPartyAuthService.Services.Google.Common;

namespace WebApp.Extensions;

public static class AddConfigsExtension
{
    public static void AddConfigs(this IServiceCollection collection, IConfiguration cfg)
    {
        collection.Configure<JwtCfg>(cfg.GetSection("JWT"));
        collection.Configure<ConfirmCodeCfg>(cfg.GetSection("ConfirmCode"));
        collection.Configure<EmailCfg>(cfg.GetSection("Email"));
        collection.Configure<FacebookCfg>(cfg.GetSection("Facebook"));
        collection.Configure<GoogleCfg>(cfg.GetSection("Google"));
        collection.Configure<AppleCfg>(cfg.GetSection("Apple"));
        collection.Configure<ContractCfg>(cfg.GetSection("Contract"));
        collection.Configure<AwsCfg>(cfg.GetSection("Aws"));
        collection.Configure<MoralisCfg>(cfg.GetSection("Moralis"));
    }
}
