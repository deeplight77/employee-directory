using EmployeeDirectory.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeDirectory.API
{
    public class AuthRepository : IDisposable
    {
        //Entity Framework Database Context
        private AuthContext _ctx;

        //User manager provides logic for membership
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            //User manager must be initialized with a User Store, which uses Entity Framework for persistance
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));

            //Initialize role manager with db store
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityResult> RemoveUser(UserModel userModel)
        {
            IdentityUser user = _userManager.FindByName(userModel.UserName);

            IList<string> rolesForUser = await _userManager.GetRolesAsync(user.Id);
            
            foreach (string role in rolesForUser)
            {
                IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user.Id, role);
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> AddUserToRole(UserModel userModel, string roleName)
        {
            IdentityUser user = _ctx.Users.FirstOrDefault(u => u.UserName.Equals(userModel.UserName, StringComparison.CurrentCultureIgnoreCase));
            IdentityRole role = _roleManager.FindByName(roleName);

            if (role == null)
            {
                throw new ArgumentException(String.Format("Role {0} not available", roleName));
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user.Id, role.Name);

            return result;
        }

        public async Task<Boolean> IsValidRole(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public List<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles = _ctx.Roles.OrderBy(r => r.Name).ToList();

            return roles;
        }

        public async Task<List<String>> GetRoles(string userName)
        {
            IdentityUser user   = await _userManager.FindByNameAsync(userName);
            IList<String> roles = await _userManager.GetRolesAsync(user.Id);

            return roles.ToList();
        }

        public void InitializeUsersAndRoles()
        {
            const string userName = "admin";
            const string password = "adminpass";
            const string roleName = "Admin";

            //Create admin user if not exists
            IdentityUser user = _userManager.FindByName(userName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName
                };
                IdentityResult result = _userManager.Create(user, password);
            }

            //Create Role Admin if it does not exist
            IdentityRole role = _roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                IdentityResult roleresult = _roleManager.Create(role);
            }

            // Add user admin to Role Admin if not already added
            IList<string> rolesForUser = _userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                IdentityResult result = _userManager.AddToRole(user.Id, role.Name);
            }
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}