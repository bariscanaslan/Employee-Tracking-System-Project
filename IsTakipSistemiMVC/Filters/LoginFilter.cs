using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Filters
{
	public class LoginFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var session = filterContext.HttpContext.Session;
			if (session["personelID"] == null)
			{
				filterContext.Result = new RedirectResult("~/Login/Index");
			}
			base.OnActionExecuting(filterContext);
		}
	}
}