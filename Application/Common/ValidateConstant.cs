namespace Application.Common;

public static class ValidateConstant
{
    public const int MinDefaultStrLength = 5;
    public const int MaxDefaultStrLength = 30;
    
    public const int MinPswLength = 8;
    
    public const string PswRegPattern = "[a-zA-Z0-9!@#$%^&*)(+=._-]+$";
    public const string PswRegFailMessage = "Password contains not allowed symbols";

    public const int MaxResetCodeValue = 1_000_000;
    
    public const int MaxImageSize = 10_194304;
    public const string MaxImageSizeFailMessage = "The image cannot weigh more than 10 MB";
    public const string ImageExtensionFailMessage = "Provides image with unsupported extension";

    public const int MaxNameLength = 16;
    public const int MaxDescLength = 160;

    public const string NotValidLinkMsg = "Invalid site";

    public const int MaxVideoSize = 300_194304;
    public const string MaxVideoSizeFailMsg = "The video cannot weigh more than 300 MB";
    public const string VideoExtensionFailMsg = "Provides video with unsupported extension";

    public const int MaxNftDesc = 250;
    public const string NftLinkFailMsg = "Invalid NFT url";

    public const int MaxLocationLength = 128;
    public const int MaxCommentLength = 1_000;

    public const int EthAddressLength = 42;
    public const string EthAddressLengthFailMsg = "Eth address must be 42 length";

    public const int MaxTokensUriLinks = 100;
    public const string MaxTokensUriLinksFailMsg = "Max links per request is 100";
}
