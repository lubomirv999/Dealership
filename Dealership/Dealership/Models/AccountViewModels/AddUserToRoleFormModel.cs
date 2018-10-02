namespace Dealership.Models.AccountViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddUserToRoleFormModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public IList<string> Roles { get; set; }
    }
}
