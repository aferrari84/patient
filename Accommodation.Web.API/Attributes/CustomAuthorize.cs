using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Properties;
using Accommodation.Interfaces.Maps;
using Accommodation.Interfaces.Services;
using Accommodation.Models;
using Accommodation.Unity.Injection;
using Accommodation.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace Accommodation.Web.API.Attributes
{
    [ExcludeFromCodeCoverage]
    public class CustomAuthorize : System.Web.Http.AuthorizeAttribute
    {
        private Object thisLock = new Object();
        private static readonly string[] _emptyArray = new string[0];

        private readonly object _typeId = new object();

        private string _module;
        private string _rights;
        private string[] _rightsSplit = _emptyArray;

        //private ILodgingMap _lodgingMap;

        public CustomAuthorize()
        {
            //_employeeMap = UnityConfig.ResolveDependency<ILodgingMap>();
        }

        /// <summary>
        /// Gets or sets the authorized module.
        /// </summary>
        /// <value>
        /// The module string.
        /// </value>
        public string Module
        {
            get { return _module ?? String.Empty; }
            set
            {
                _module = value;
            }
        }

        /// <summary>
        /// Gets or sets the authorized rights.
        /// </summary>
        /// <value>
        /// The rights string.
        /// </value>
        /// <remarks>Multiple rights names can be specified using the comma character as a separator.</remarks>
        public string Rights
        {
            get { return _rights ?? String.Empty; }
            set
            {
                _rights = value;
                _rightsSplit = SplitString(value);
            }
        }


        /// <summary>
        /// Determines whether access for this particular request is authorized. This method uses the user <see cref="IPrincipal"/>
        /// returned via <see cref="Thread.CurrentPrincipal"/>. Authorization is denied if the user is not authenticated,
        /// the user is not in the authorized group of <see cref="Users"/> (if defined), or if the user is not in any of the authorized 
        /// <see cref="Module"/> (if defined).
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <returns><c>true</c> if access is authorized; otherwise <c>false</c>.</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext == null)
                {
                    throw new ArgumentNullException("actionContext");
                }

                IPrincipal user = Thread.CurrentPrincipal;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    return false;
                }

                if (user.IsInRole(Roles))
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(_module) && _rightsSplit.Length != 0)
                {
                    //EmployeeViewModel employee = _employeeMap.GetByCompanyEmail(user.Identity.Name);

                    //if (employee != null)
                    //{
                        return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Called when an action is being authorized. This method uses the user <see cref="IPrincipal"/>
        /// returned via <see cref="Thread.CurrentPrincipal"/>. Authorization is denied if
        /// - the request is not associated with any user.
        /// - the user is not authenticated,
        /// - the user is authenticated but is not in the authorized group of <see cref="Users"/> (if defined), or if the user
        /// is not in any of the authorized <see cref="Module"/> (if defined).
        /// 
        /// If authorization is denied then this method will invoke <see cref="HandleUnauthorizedRequest(HttpActionContext)"/> to process the unauthorized request.
        /// </summary>
        /// <remarks>You can use <see cref="AllowAnonymousAttribute"/> to cause authorization checks to be skipped for a particular
        /// action or controller.</remarks>
        /// <seealso cref="IsAuthorized(HttpActionContext)" />
        /// <param name="actionContext">The context.</param>
        /// <exception cref="ArgumentNullException">The context parameter is null.</exception>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext == null)
                {
                    throw new ArgumentNullException("actionContext");
                }

                if (SkipAuthorization(actionContext))
                {
                    return;
                }

                lock (thisLock)
                {
                    if (!IsAuthorized(actionContext))
                    {
                        HandleUnauthorizedRequest(actionContext);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            try
            {
                return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Splits the string on commas and removes any leading/trailing whitespace from each result item.
        /// </summary>
        /// <param name="original">The input string.</param>
        /// <returns>An array of strings parsed from the input <paramref name="original"/> string.</returns>
        internal static string[] SplitString(string original)
        {
            try
            {
                if (String.IsNullOrEmpty(original))
                {
                    return _emptyArray;
                }

                var split = from piece in original.Split(',')
                            let trimmed = piece.Trim()
                            where !String.IsNullOrEmpty(trimmed)
                            select trimmed;
                return split.ToArray();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}