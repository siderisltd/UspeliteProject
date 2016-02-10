namespace Uspelite.Services.Contracts
{
    using Data.Models;

    public interface IUsersService
    {
        User GetById(string id);

        User GetByUsername(string username);
    }
}
