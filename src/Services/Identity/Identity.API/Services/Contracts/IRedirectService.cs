namespace AnteyaSidOnContainers.Services.Identity.API.Services.Contracts
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
