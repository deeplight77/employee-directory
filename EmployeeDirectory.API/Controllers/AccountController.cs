using EmployeeDirectory.API.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EmployeeDirectory.API.Controllers
{
    /// <summary>
    /// Account controller found under api/Account url
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthContext db = new AuthContext();
        private AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            /* Add user to role if available */

            if (!string.IsNullOrEmpty(userModel.RoleName))
            {
                if (await _repo.IsValidRole(userModel.RoleName))
                {
                    IdentityResult roleResult = await _repo.AddUserToRole(userModel, userModel.RoleName);

                    errorResult = GetErrorResult(roleResult);

                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
            }

            /* Save a Directory Entry for this user as well */
            db.DirectoryEntryModels.Add(new DirectoryEntryModel() { UserName = userModel.UserName, JobTitle = userModel.RoleName });

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok();
        }

        // POST api/Account/Remove
        [Authorize(Roles = "Admin, HR")]
        [Route("Remove")]
        public async Task<IHttpActionResult> Remove(UserModel userModel)
        {
            if (!ModelState.IsValidField(userModel.UserName))
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RemoveUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            /* Remove a Directory Entry for this user as well */
            DirectoryEntryModel directoryEntryModel = await db.DirectoryEntryModels.FindAsync(userModel.UserName);
            if (directoryEntryModel == null)
            {
                return NotFound();
            }

            db.DirectoryEntryModels.Remove(directoryEntryModel);
            await db.SaveChangesAsync();

            return Ok();
        }

        // GET api/Account/GetRoles
        [Authorize(Roles = "Admin, HR")]
        [Route("GetRoles")]
        public IHttpActionResult GetRoles()
        {
            return Ok(_repo.GetRoles());
        }

        // GET api/Account/GetRoles
        [Authorize]
        [Route("GetRoles")]
        public async Task<IHttpActionResult> GetRoles(string userName)
        {
            return Ok(await _repo.GetRoles(userName));
        }

        // POST api/Account/AddRole
        [Authorize(Roles = "Admin, HR")]
        [Route("AddRole")]
        public async Task<IHttpActionResult> AddRole(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.AddUserToRole(userModel, userModel.RoleName);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
