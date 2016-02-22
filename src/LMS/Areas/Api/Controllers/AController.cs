using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.CodeGeneration.CommandLine;
using AppContext = LMS.Services.AppContext;

namespace LMS.Areas.Api.Controllers
{
    [Authorize]
    public class AController : Controller
    {
        protected IAppContext AppContext { get; }

        public AController(IAppContext appContext)
        {
            AppContext = appContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = ActionContext.HttpContext.User;
            ((AppContext)AppContext).UserId = user.GetUserId();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ItemNotFountException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ArgumentException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                if (context.Exception.Message != null)
                {
                    ModelState.AddModelError("", context.Exception.Message);
                }
                context.Result = HttpBadRequest(ModelState);
                context.ExceptionHandled = true;
            }
        }
    }
}
