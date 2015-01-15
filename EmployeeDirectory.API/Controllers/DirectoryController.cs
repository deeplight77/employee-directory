using EmployeeDirectory.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeDirectory.API.Controllers
{

    [RoutePrefix("api/Directory")]
    public class DirectoryController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(DirectoryEntry.CreateEntries());
        }
    }
}
