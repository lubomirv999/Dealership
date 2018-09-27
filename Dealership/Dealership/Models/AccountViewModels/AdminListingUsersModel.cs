namespace Dealership.Models.AccountViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class AdminListingUsersModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
