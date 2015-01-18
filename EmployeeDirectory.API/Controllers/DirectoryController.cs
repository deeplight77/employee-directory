using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EmployeeDirectory.API;
using EmployeeDirectory.API.Models;

namespace EmployeeDirectory.API.Controllers
{
    public class DirectoryController : ApiController
    {
        private AuthContext db = new AuthContext();

        // GET: api/Directory
        [Authorize]
        public IQueryable<DirectoryEntryModel> GetDirectoryEntryModels()
        {
            return db.DirectoryEntryModels;
        }

        // GET: api/Directory/{#}
        [Authorize]
        public IQueryable<DirectoryEntryModel> GetDirectoryEntryModels(int from, int howMany, string filter)
        {
            IQueryable<DirectoryEntryModel> dbSet = filter == null
                ? db.DirectoryEntryModels.OrderBy(d => d.UserName).Skip(from).Take(howMany)
                : db.DirectoryEntryModels.Where(d => d.UserName.Contains(filter) || d.FullName.Contains(filter)).OrderBy(d => d.UserName).Skip(from).Take(howMany);

            return dbSet;
        }

        // GET: api/Directory/5
        [Authorize]
        [ResponseType(typeof(DirectoryEntryModel))]
        public async Task<IHttpActionResult> GetDirectoryEntryModel(string id)
        {
            DirectoryEntryModel directoryEntryModel = await db.DirectoryEntryModels.FindAsync(id);
            if (directoryEntryModel == null)
            {
                return NotFound();
            }

            return Ok(directoryEntryModel);
        }

        // PUT: api/Directory/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDirectoryEntryModel(string id, DirectoryEntryModel directoryEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != directoryEntryModel.UserName)
            {
                return BadRequest();
            }

            db.Entry(directoryEntryModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectoryEntryModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        
        // POST: api/Directory
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(DirectoryEntryModel))]
        public async Task<IHttpActionResult> PostDirectoryEntryModel(DirectoryEntryModel directoryEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DirectoryEntryModels.Add(directoryEntryModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DirectoryEntryModelExists(directoryEntryModel.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = directoryEntryModel.UserName }, directoryEntryModel);
        }

        // DELETE: api/Directory/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(DirectoryEntryModel))]
        public async Task<IHttpActionResult> DeleteDirectoryEntryModel(string id)
        {
            DirectoryEntryModel directoryEntryModel = await db.DirectoryEntryModels.FindAsync(id);
            if (directoryEntryModel == null)
            {
                return NotFound();
            }

            db.DirectoryEntryModels.Remove(directoryEntryModel);
            await db.SaveChangesAsync();

            return Ok(directoryEntryModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DirectoryEntryModelExists(string id)
        {
            return db.DirectoryEntryModels.Count(e => e.UserName == id) > 0;
        }
    }
}