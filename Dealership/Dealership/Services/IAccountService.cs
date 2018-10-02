namespace Dealership.Services
{
    using Dealership.Models;
    using Dealership.Models.AccountViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public interface IAccountService
    {
        AdminListingUsersModel All(int pageSize, int page = 1);

        int Count();

        ApplicationUser FindById(string id);

        IEnumerable<SelectListItem> AllRoles();

        void Delete(string id);
    }
}
