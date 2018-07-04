using System;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreUserIsInRoleTest
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string username) : base(username)
        {
        }

        public ApplicationUser()
        {
        }
    }
}