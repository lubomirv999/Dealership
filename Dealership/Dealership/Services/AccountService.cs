namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models;
    using Dealership.Models.AccountViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public class AccountService : IAccountService
    {
        private readonly DealershipDbContext _db;

        public AccountService(DealershipDbContext db)
        {
            this._db = db;
        }

        public AdminListingUsersModel All(int pageSize, int page = 1)
        {
            AdminListingUsersModel model = new AdminListingUsersModel();

            model.Users = this._db.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            model.Roles = AllRoles();

            return model;
        }

        public IEnumerable<SelectListItem> AllRoles()
        {
            return this._db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();
        }


        public void Delete(ApplicationUser user)
        {
            this._db.Remove(user);
            this._db.SaveChanges();
        }

        public int Count()
        {
            return _db.Users.Count();
        }

        public ApplicationUser FindById(string id)
            => this._db
            .Users
            .Where(u => u.Id == id)
            .FirstOrDefault();
    }
}
