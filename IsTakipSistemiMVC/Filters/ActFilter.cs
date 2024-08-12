using IsTakipSistemiMVC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Filters
{
	public class ActFilter : FilterAttribute, IActionFilter
	{
		public int Order { get; set; } = 0;
		private readonly isTakipDBEntities entity = new isTakipDBEntities();
		private readonly string aciklama;

		public ActFilter(string actAciklama)
		{
			this.aciklama = actAciklama;
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			string aciklama = string.Empty;
			if (filterContext.Result is RedirectToRouteResult)
			{
				if (filterContext.Controller.TempData["bilgi"] != null)
				{
					aciklama = filterContext.Controller.TempData["bilgi"].ToString();
					filterContext.Controller.TempData["bilgi"] = null; // TempData'yı sıfırla
				}

				var log = new TBL_LOGLAR
				{
					logAciklama = this.aciklama + " (" + aciklama + ")",
					actionAd = filterContext.ActionDescriptor.ActionName,
					controllerAd = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
					tarih = DateTime.Now,
					personelID = Convert.ToInt32(filterContext.HttpContext.Session["personelID"])
				};

				entity.TBL_LOGLAR.Add(log);
				entity.SaveChanges();
			}
		}


		public void OnActionExecuting(ActionExecutingContext filterContext)
		{

		}
	}
}
