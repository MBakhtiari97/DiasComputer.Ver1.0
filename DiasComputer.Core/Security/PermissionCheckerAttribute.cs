using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.Services;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace DiasComputer.Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        //Declaring a permissionId variable
        private int _permissionId = 0;

        //Creating an instance of permission service
        private IPermissionService _permissionService;

        //Requiring filling permission id by using it on constructor
        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        //Implementing OnAuthorization Method
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Getting IPermissionService from Context
            _permissionService =
                    (IPermissionService)context.HttpContext
                    .RequestServices
                    .GetService(typeof(IPermissionService));

            //Checking if the user is authenticated or not
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                //Getting User EmailAddress (in this case)
                string userName = context.HttpContext
                    .User
                    .Identity
                    .Name;
                
                //Checking if the user has required role then giving access to requested page otherwise redirecting user to sign in page
                if (!_permissionService.CheckPermission(_permissionId, userName))
                {
                    context.Result = new RedirectResult("/SignIn");
                }
            }
            else
            {
                context.Result = new RedirectResult("/SignIn");
            }
        }
    }
}
