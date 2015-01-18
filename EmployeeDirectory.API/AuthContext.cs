using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeDirectory.API
{
    /// <summary>
    /// This class is responsible to communicate with database through Entity Framework
    /// </summary>
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("DBConnection")
        {

        }

        public System.Data.Entity.DbSet<EmployeeDirectory.API.Models.DirectoryEntryModel> DirectoryEntryModels { get; set; }
    }
}