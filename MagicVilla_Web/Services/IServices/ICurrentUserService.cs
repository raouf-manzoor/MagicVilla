namespace MagicVilla_Web.Services.IServices
{
    public interface ICurrentUserService
    {
        string? Token { get; }
        bool IsLoggedIn();
    }
}
