using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Accommodation.Interfaces;
using Accommodation.Interfaces.Maps;
using Accommodation.Models;
using Accommodation.Models.Context;
using Accommodation.ViewModels;
using Accommodation.Web.API.Models;
using Accommodation.Web.API.Providers;
using Accommodation.Web.API.Results;
using Accommodation.Web.API.Result_Handlers;
using System.Web.Security;
using System.Text.RegularExpressions;
using Accommodation.Web.API.Attributes;
using System.Configuration;
using Accommodation.Interfaces.Services;

namespace Accommodation.Web.API.Controllers
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;

        IEmailService _emailService;

        public AccountController(IEmailService emailService)
            : base("AccountController")
        {
            _emailService = emailService;
            AccessTokenFormat = Startup.OAuthOptions.AccessTokenFormat;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public IResponseMessage<UserInfoViewModel> GetUserInfo()
        {
            IResponseMessage<UserInfoViewModel> response = new ResponseMessage<UserInfoViewModel>();
            try
            {

                AccommodationContext _context = new AccommodationContext();
                using (UserManager<User> userManager = new UserManager<User>(new UserStore<User>(_context)))
                {
                    var user = userManager.FindByEmail(User.Identity.Name);
                    UserViewModel result = null; // _userMap.GetByCompanyEmail(User.Identity.Name);
                    var model = new UserInfoViewModel
                    {
                        Email = user != null ? user.Email : null,
                        Name = user != null ? user.UserShowName : null,
                        HasRegistered = user != null,
                        User = result
                    };
                    response.IsValid = true;
                    response.ResponseData = model;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                response.IsValid = false;
                response.Exception = ex;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.Email.IndexOf("@santexgroup.com") == -1)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return Redirect(Url.Content("~/").Replace("api/", "login") + "?message=The only domain allowed is santexgroup.com");
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            AccommodationContext _context = new AccommodationContext();
            using (UserManager<User> userManager = new UserManager<User>(new UserStore<User>(_context)))
            {
                var user = await userManager.FindByEmailAsync(externalLogin.Email);

                bool hasRegistered = user != null;

                if (!hasRegistered)
                {
                    user = new User();
                    user.UserShowName = externalLogin.UserName;
                    user.UserName = externalLogin.Email;
                    user.Email = externalLogin.Email;
                    user.EmailConfirmed = true;
                    IdentityResult result = await userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    //IEnumerable<Claim> claims = externalLogin.GetClaims();
                    //ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                    //Authentication.SignIn(identity);
                }

                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, user.Email);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }

            return Ok();
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST api/Account/Logout
        [Route("Verification")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Verification(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new Exception("Error");
            }
            code = code.Replace("/1/", "+");
            var result = UserManager.ConfirmEmail(userId, code);

            var response = Request.CreateResponse(HttpStatusCode.Found);
            response.Headers.Location = new Uri("http://turismoalojamiento.com.ar");
            return response;
        }

        // POST api/Account/Register
        [Route("Register")]
        [CustomAuthorize(Rights = "CanCreate", Module = "ThirdParty")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AccommodationContext _context = new AccommodationContext();
            using (UserManager<User> userManager = new UserManager<User>(new UserStore<User>(_context)))
            {
                var user = await userManager.FindByEmailAsync(model.UserName);

                bool hasRegistered = user != null;

                if (!hasRegistered)
                {
                    user = new User();
                    user.UserShowName = model.Name + " " + model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.UserName;
                    user.EmailConfirmed = false;
                    IdentityResult result = await userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    //IEnumerable<Claim> claims = externalLogin.GetClaims();
                    //ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                    //Authentication.SignIn(identity);


                    //var user = new ApplicationUser() { UserName = model.UserName, Email = model.UserName };

                    //string newPassword = Membership.GeneratePassword(15, 1);
                    //newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => "9") + "%2";
                    //user.PhoneNumber = newPassword;
                    //var result = await UserManager.CreateAsync(user, model.Password);

                    //if (!result.Succeeded)
                    //{
                    //    return GetErrorResult(result);
                    //}

                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    code = code.Replace("+", "/1/");
                    var callbackUrl = "turismoalojamiento.com.ar/api/Account/Verification?userId=" + user.Id + "&code=" + code;

                    var emailContent = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.w3.org/1999/xhtml\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\"> <head>     <meta name=\"viewport\" content=\"width=device-width\" />     <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />     <title>Email Confirmation</title> </head> <body bgcolor=\"#f6f6f6\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; -webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; margin: 0; padding: 0;\">      <!-- body -->     <table class=\"body-wrap\" bgcolor=\"#f6f6f6\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; width: 100%; margin: 0; padding: 20px;\">         <tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\">             <td style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\"></td>             <td class=\"container\" bgcolor=\"#FFFFFF\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto; padding: 20px; border: 1px solid #f0f0f0;\">                  <!-- content -->                 <div class=\"content\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; max-width: 600px; display: block; margin: 0 auto; padding: 0;\">                     <table style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; width: 100%; margin: 0; padding: 0;\">                         <tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\">                             <td style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\">                                 <p style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6; font-weight: normal; margin: 0 0 10px; padding: 0;\">Hola!,</p>                                 <h1 style=\"font-family: 'Helvetica Neue', Helvetica, Arial, 'Lucida Grande', sans-serif; font-size: 36px; line-height: 1.2; color: #000; font-weight: 200; margin: 20px 0 10px; padding: 0;\">Activación de Cuenta</h1>                                 <p style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6; font-weight: normal; margin: 0 0 10px; padding: 0;\">Para finalizar su activacion de cuenta en <strong>turismoalojamiento.com.ar</strong> haga click en el siguiente enlace..</p>                                 <table style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; width: 100%; margin: 0; padding: 0;\">                                     <tr style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\">                                         <td class=\"padding\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 10px 0;\">                                             <p style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6; font-weight: normal; margin: 0 0 10px; padding: 0;\">" +
                        "<a href=\"" + callbackUrl + "\" class=\"btn-primary\" style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 2; color: #FFF; text-decoration: none; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 25px; background: #348eda; margin: 0 10px 0 0; padding: 0; border-color: #348eda; border-style: solid; border-width: 10px 20px;\">" +
                        "Activar " + user.Email + "</a>" +
                        "</p>                                         </td>                                     </tr>                                 </table>                                  <p style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6; font-weight: normal; margin: 0 0 10px; padding: 0;\">Gracias,<br />turismoalojamiento.com.ar</p>                              </td>                         </tr>                     </table>                 </div>                 <!-- /content -->              </td>             <td style=\"font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif; font-size: 100%; line-height: 1.6; margin: 0; padding: 0;\"></td>         </tr> </body> </html>";

                    _emailService.Send(model.UserName, "Activacion de Cuenta", emailContent, true);
                }
                else
                {
                    throw new Exception("Email ya registrado.");
                }
            }

            return new GetListHandler<string>(model.Password, Request);
        }

        // POST api/Account/Register
        [Route("Users")]
        [HttpGet]
        [CustomAuthorize(Rights = "CanView", Module = "ThirdParty")]
        public IHttpActionResult Users()
        {
            var users = new List<ApplicationUser>();
            foreach (var item in UserManager.Users)
            {
                if (item.PasswordHash != null)
                {
                    users.Add(item);
                }
            }
            return new GetListHandler<List<ApplicationUser>>(users, Request);
        }


        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                if (Email != null)
                {
                    claims.Add(new Claim(ClaimTypes.Email, Email, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    Email = identity.FindFirstValue(ClaimTypes.Email),
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
