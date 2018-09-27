namespace Dealership.Services
{
    using Dealership.Models.AccountViewModels;

    public interface IAccountService
    {
        AdminListingUsersModel All();
    }
}
