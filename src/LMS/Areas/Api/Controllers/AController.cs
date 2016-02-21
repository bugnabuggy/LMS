using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

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

            base.OnActionExecuting(context);
        }
    }
}
