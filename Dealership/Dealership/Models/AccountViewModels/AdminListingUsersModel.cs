namespace Dealership.Models.AccountViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class AdminListingUsersModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PreviousPage => this.CurrentPage == 1
            ? 1
            : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages
            ? this.TotalPages
            : this.CurrentPage + 1;
    }
}
