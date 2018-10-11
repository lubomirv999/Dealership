namespace Dealership.Services
{
    using Dealership.Models;
    using Dealership.Models.AccountViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public interface IAccountService
    {
        AdminListingUsersModel All(int pageSize, int page = 1);        

        ApplicationUser FindById(string id);

        IEnumerable<SelectListItem> AllRoles();

        int Count();

        void Delete(ApplicationUser user);
    }
}
