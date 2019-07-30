using WxPay2017.API.BLL.Identity;
using WxPay2017.API.DAL.AuthModels;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;
using System.Configuration;

namespace WxPay2017.API.WEBAPI.Core
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            string Permit = ConfigurationManager.AppSettings["Permit"].ToString();
            AccountBLL account = new AccountBLL();
            ApplicationUser user;
            if (!string.IsNullOrEmpty(Permit) && Permit == context.Password)
            {
                user = await account.FindUserByName(context.UserName);
            }
            else
            {
                user = await account.FindUser(context.UserName, context.Password);
            }

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", "user"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            //user.Roles.ForEach(r =>
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, r.RoleId));
            //});
            identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(x => x.RoleId))));

            context.Validated(identity);
            
        }
    }
}
