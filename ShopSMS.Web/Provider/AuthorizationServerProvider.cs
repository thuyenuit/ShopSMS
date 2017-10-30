using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ShopSMS.Common.Common;
using ShopSMS.DAL;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ShopSMS.Web.Provider
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {}

        // đăng nhập -> gửi 1 requet lên server sẽ Validate tất cả request
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null)
                allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            UserManager<ApplicationUser> userManager = context.OwinContext.GetUserManager<UserManager<ApplicationUser>>();
            ApplicationUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                context.SetError("server-error");
                context.Rejected();
                return;
            }

            if (user != null)
            {
                /*ShopSMSDbcontext DbContext = new ShopSMSDbcontext();
                List<MenuGroup> lstResultMenu = DbContext.MenuGroup.ToList();
                List<Menu> lstMenu = DbContext.Menu.Where(x=>x.MenuStatus == true).ToList();

                List<ListGroupMenu> LstGroupMenu = new List<ListGroupMenu>();             
                ListGroupMenu objListGroupMenu = null;

                List<ListMenu> LstListMenu = null;
                ListMenu objListMenu = null;
                             
                foreach (var item in lstResultMenu)
                {
                    objListGroupMenu = new ListGroupMenu();                    
                    objListGroupMenu.MenuGroupName = item.MenuGroupName;

                    LstListMenu = new List<ListMenu>();
                    
                    var lstMenuDB = lstMenu.Where(x => x.MenuGroupID == item.MenuGroupID).ToList();

                    if (lstMenuDB.Count > 0)
                    {
                        foreach(var itemMe in lstMenuDB)
                        {
                            objListMenu = new ListMenu();
                            objListMenu.MenuName = itemMe.MenuName;
                            objListMenu.OrderBy = itemMe.MenuOrderBy;
                            objListMenu.Icon = itemMe.ImageURL;
                            LstListMenu.Add(objListMenu);
                        }
                    }
                    objListGroupMenu.ListMenu = LstListMenu;
                    LstGroupMenu.Add(objListGroupMenu);
                }*/

                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);

                string userCode = !string.IsNullOrEmpty(user.UserCode) ? user.UserCode : string.Empty;
                string email = !string.IsNullOrEmpty(user.Email) ? user.Email : string.Empty;
                string phoneNumber = !string.IsNullOrEmpty(user.PhoneNumber) ? user.PhoneNumber : string.Empty;

                identity.AddClaim(new Claim("userId", user.Id));
                identity.AddClaim(new Claim("fullName", user.FullName));
                identity.AddClaim(new Claim("userCode", userCode));
                identity.AddClaim(new Claim("email", email));
                identity.AddClaim(new Claim("phoneNumber", phoneNumber));

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                        {"userId", user.Id },
                        {"fullName", user.FullName },
                        {"userCode", userCode },
                        {"email", email },
                        {"phoneNumber",phoneNumber }
                    });

                UserInfoInstance.EmailInstance = email;
                UserInfoInstance.FullNameInstance = user.FullName;
                UserInfoInstance.PhoneInstance = phoneNumber;
                UserInfoInstance.UserCodeInstance = userCode;
                UserInfoInstance.UserNameInstance = user.UserName;
               
                List<ListStatus> listStatus = new List<ListStatus> {
                    //new ListStatus() { StatusID = SystemParameter.StatusID_0, StatusName = SystemParameter.StatusName_0},
                    new ListStatus() { StatusID = SystemParameter.StatusID_1, StatusName = SystemParameter.StatusName_1},
                    new ListStatus() { StatusID = SystemParameter.StatusID_2, StatusName = SystemParameter.StatusName_2}
                };
                UserInfoInstance.ListStatus = listStatus;
               // UserInfoInstance.ListGroupMenu = LstGroupMenu;

                context.Validated(new AuthenticationTicket(identity, props));
            }
            else
            {
                context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không hợp lệ");
                context.Rejected();
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}