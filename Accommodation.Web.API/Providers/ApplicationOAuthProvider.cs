﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Accommodation.Web.API.Models;
using Accommodation.Models.Context;
using Accommodation.Models;

namespace Accommodation.Web.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                AccommodationContext _context = new AccommodationContext();
                using (UserManager<User> userManager = new UserManager<User>(new UserStore<User>(_context)))
                {
                    //var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>(_context);

                    var user = await userManager.FindAsync(context.UserName, context.Password);
                    //var user = await userManager.FindByEmailAsync(context.UserName);

                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }

                    if (!user.EmailConfirmed)
                    {
                        context.SetError("not_activated", "The user account was not authenticated.");
                        return;
                    }

                    ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                       OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                        CookieAuthenticationDefaults.AuthenticationType);

                    AuthenticationProperties properties = CreateProperties(user.UserName, user.Email);
                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            try
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                // Resource owner password credentials does not provide a client ID.
                if (context.ClientId == null)
                {
                    context.Validated();
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            try
            {
                if (context.ClientId == _publicClientId)
                {

                    //TODO: Uncomment this when define finaly url
                    /*Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                    if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                    {*/
                    context.Validated();
                    //}
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static AuthenticationProperties CreateProperties(string userName, string email)
        {
            try
            {
                IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }, { "Email", email }
            };
                return new AuthenticationProperties(data);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}