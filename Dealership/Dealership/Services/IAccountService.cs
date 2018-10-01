namespace Dealership.Services
{
    using Dealership.Models;
    using Dealership.Models.AccountViewModels;

    public interface IAccountService
    {
        AdminListingUsersModel All(int pageSize, int page = 1);
        int Count();
    }
}
