using IsTakipSistemiMVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	public class LogoutController : Controller
	{
		// GET: Logout
		[ActFilter("Çıkış Yapıldı.")]
		public ActionResult Index()
		{
			TempData["bilgi"] = "";
			// Session değerleri serbest bırakılır
			Session.Abandon();
			// Login Controller'ının Index Action'ına dönülür
			return RedirectToAction("Index", "Login");
		}
	}
}