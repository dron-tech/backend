namespace Application.Common.Interfaces;

public interface IAppleAuthService
{
    public Task<string> GetEmailByCodeOrFail(string code);
}
