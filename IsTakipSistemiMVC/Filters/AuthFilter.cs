using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Filters
{
	public class AuthFilter : FilterAttribute, IAuthorizationFilter
	{
		protected int yetkiTur;

		public AuthFilter(int yetkiTur)
		{
			this.yetkiTur = yetkiTur;
		}
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			int yetkiTurID = Convert.ToInt32(filterContext.HttpContext.Session["personelYetkiTurID"]);

			if (this.yetkiTur != yetkiTurID)
			{
				filterContext.HttpContext.Session.Clear();
				filterContext.HttpContext.Session.Abandon();
				filterContext.Result = new RedirectResult("/Login/Index");

			}
		}
	}
}