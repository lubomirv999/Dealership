namespace Dealership.Models.AccountViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class UserAndRolesModel
    {
        public ApplicationUser User { get; set; }

        public IList<string> Roles { get; set; }

        public IEnumerable<SelectListItem> AllRoles { get; set; }
    }
}
