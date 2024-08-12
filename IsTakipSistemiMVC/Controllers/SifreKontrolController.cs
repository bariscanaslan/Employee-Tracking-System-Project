using IsTakipSistemiMVC.Models;
using IsTakipSistemiMVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace IsTakipSistemiMVC.Controllers
{
	[LoginFilter,LayoutActionFilter]
	public class SifreKontrolController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();
		// GET: SifreKontrol
		public ActionResult Index()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			if (personelID == 0)
			{
				return RedirectToAction("Index", "Login");
			}

			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID select p).FirstOrDefault();

			ViewBag.mesaj = null;
			ViewBag.yetkiTurID = null;
			ViewBag.stil = null;

			return View(personel);
		}
		[HttpPost, ActFilter("Parola Değiştirildi.")]
		public ActionResult Index(int personelID, string eskiSifre, string yeniSifre, string yeniSifreTekrar)
		{
			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID select p).FirstOrDefault();

			if (eskiSifre != personel.personelParola)
			{
				ViewBag.mesaj = "Eski şifrenizi yanlış girdiniz!";
				return View(personel);
			}

			personel.personelParola = yeniSifre;
			personel.yeniPersonel = false;
			entity.SaveChanges();

			TempData["bilgi"] = personel.personelKullaniciAdi;
			ViewBag.yetkiTurID = personel.personelYetkiTurID;

			return RedirectToAction("Index", "Logout");
		}

	}
}