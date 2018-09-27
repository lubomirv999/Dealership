namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.AccountViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;

    public class AccountService : IAccountService
    {
        private readonly DealershipDbContext db;

        public AccountService(DealershipDbContext db)
        {
            this.db = db;
        }

        public AdminListingUsersModel All()
        {
            AdminListingUsersModel model = new AdminListingUsersModel();

            model.Users = this.db.Users.ToList();
            model.Roles = this.db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            })
                .ToList();

            return model;
        }
    }
}
