using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RentApp.Providers
{
    //[EnableCors("*","*","*")]
    public class CustomOAuthProvider : Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST", "GET", "PUT", "DELETE", "OPTIONS" });

            ApplicationUserManager userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            RAIdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.!!!!");
                return;
            }

            //if (!user.EmailConfirmed)
            //{
            //    context.SetError("invalid_grant", "AppUser did not confirm email.");
            //    return;
            //}
            

            if (await userManager.IsInRoleAsync(user.UserName, "Admin"))
            {
                context.OwinContext.Response.Headers.Add("Role", new[] { "Admin" });
            }
            else
            {
               
                if (await userManager.IsInRoleAsync(user.UserName, "Manager"))
                {
                    context.OwinContext.Response.Headers.Add("Role", new[] { "Manager" });
                }
                else
                {
                    context.OwinContext.Response.Headers.Add("Role", new[] { "AppUser" });
                }
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Expose-Headers", new[] { "Role" });


            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);

        }
    }
}