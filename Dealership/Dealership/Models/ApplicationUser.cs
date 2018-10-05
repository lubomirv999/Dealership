using Dealership.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Dealership.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<Comment> Comments { get; set; }
    }
}
