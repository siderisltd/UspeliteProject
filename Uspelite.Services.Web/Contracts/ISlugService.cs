namespace Uspelite.Services.Web.Contracts
{
    public interface ISlugService
    {
        string GetSlug(string title, int length = 50);
    }
}
