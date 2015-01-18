using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeDirectory.API.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //This will allways return "validated successfullly" for the purpose of this excercise, if there were more than one client each should be validated accordingly.
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Enable CORS
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            IdentityUser user = null;
            List<String> roles = null;

            //Validate if username and pass stored in the Database are valid.
            using (AuthRepository _repo = new AuthRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                roles = await _repo.GetRoles(context.UserName);
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            context.Validated(identity);
        }
    }
}