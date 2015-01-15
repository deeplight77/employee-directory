using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(EmployeeDirectory.API.Startup))]
namespace EmployeeDirectory.API
{
    /// <summary>
    /// Class that fires on start-up as stated by the assembly attribute, it wires ASP.NET Web API to Owin server pipeline.
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

    }
}