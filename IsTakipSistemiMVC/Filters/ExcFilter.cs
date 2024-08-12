using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Filters
{
	public class ExcFilter : FilterAttribute, IExceptionFilter
	{
		// Uygulamada meydana gelen hata & istisnaları yakalar
		public void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;
			filterContext.Controller.TempData["error"] = filterContext.Exception;
			filterContext.Result = new RedirectResult("/Error/Index");
		}
	}
}