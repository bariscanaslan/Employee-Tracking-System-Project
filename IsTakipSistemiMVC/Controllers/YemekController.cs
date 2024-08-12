using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	[LoginFilter, LayoutActionFilter]
	public class YemekController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		// GET: Yemek
		public ActionResult Index(DateTime? tarih)
		{
			DateTime secilenTarih = tarih ?? DateTime.Today;
			var yemekler = (from y in entity.YemekTablo where y.Tarih == secilenTarih select y).FirstOrDefault();

			return View(yemekler);
		}


		[AuthFilter(3), HttpGet]
		public ActionResult YemekEkle()
		{
			return View();
		}

		[HttpPost, ActFilter("Menü eklendi.")]
		public ActionResult YemekEkle(FormCollection fc)
		{
			YemekTablo Menu = new YemekTablo();

			Menu.YemekAdi1 = fc["YemekAdi1"];
			Menu.Kalori1 = Convert.ToInt32(fc["Kalori1"]);
			Menu.YemekAdi2 = fc["YemekAdi2"];
			Menu.Kalori2 = Convert.ToInt32(fc["Kalori2"]);
			Menu.YemekAdi3 = fc["YemekAdi3"];
			Menu.Kalori3 = Convert.ToInt32(fc["Kalori3"]);
			Menu.YemekAdi4 = fc["YemekAdi4"];
			Menu.Kalori4 = Convert.ToInt32(fc["Kalori4"]);
			Menu.YemekAdi5 = fc["YemekAdi5"];
			Menu.Kalori5 = Convert.ToInt32(fc["Kalori5"]);
			Menu.YemekAdi6 = fc["YemekAdi6"];
			Menu.Kalori6 = Convert.ToInt32(fc["Kalori6"]);

			if (fc["YemekAdi7"] == "")
			{
				Menu.YemekAdi7 = "-";
				Menu.Kalori7 = 0;
			}
			else
			{
				Menu.YemekAdi7 = fc["YemekAdi7"];
				Menu.Kalori7 = Convert.ToInt32(fc["Kalori7"]);
			}

			if (fc["YemekAdi8"] == "")
			{
				Menu.YemekAdi8 = "-";
				Menu.Kalori8 = 0;
			}
			else
			{
				Menu.YemekAdi8 = fc["YemekAdi8"];
				Menu.Kalori8 = Convert.ToInt32(fc["Kalori8"]);
			}
			Menu.Tarih = DateTime.Parse(fc["Tarih"]); // Tarih bilgisini al 

			var tarihler = (from t in entity.YemekTablo
							where t.Tarih == Menu.Tarih
							select t).ToList();

			if (tarihler.Count() == 0)
			{
				entity.YemekTablo.Add(Menu);
				entity.SaveChanges();

				TempData["yemekEklendi"] = "Yemek menüsü başarıyla eklendi.";
				return RedirectToAction("YemekEkle");
			}

			else
			{
				TempData["yemekEklenemedi"] = "Bu tarihte başka bir yemek menüsü bulunmaktadır.";
				return View();
			}
		}

		[AuthFilter(3), HttpGet]
		public ActionResult YemekDuzenle(string tarih)
		{
			DateTime? selectedDate = null;
			if (!string.IsNullOrEmpty(tarih))
			{
				selectedDate = DateTime.Parse(tarih);
			}

			var yemekler = (from y in entity.YemekTablo where y.Tarih == selectedDate select y).FirstOrDefault();
			ViewBag.SelectedDate = selectedDate;

			return View(yemekler);
		}


		[HttpPost, ActFilter("Menü Güncellendi.")]
		public ActionResult YemekDuzenle(string tarih, string YemekAdi1, int Kalori1, string YemekAdi2, int Kalori2, string YemekAdi3, int Kalori3, string YemekAdi4, int Kalori4,
			string YemekAdi5, int Kalori5, string YemekAdi6, int Kalori6, string YemekAdi7, int Kalori7, string YemekAdi8, int Kalori8)
		{
			DateTime? selectedDate = null;
			if (!string.IsNullOrEmpty(tarih))
			{
				selectedDate = DateTime.Parse(tarih);
			}

			try
			{
				YemekTablo yemekTablo = (from y in entity.YemekTablo where y.Tarih == selectedDate select y).FirstOrDefault();
				yemekTablo.YemekAdi1 = YemekAdi1;
				yemekTablo.Kalori1 = Kalori1;
				yemekTablo.YemekAdi2 = YemekAdi2;
				yemekTablo.Kalori2 = Kalori2;
				yemekTablo.YemekAdi3 = YemekAdi3;
				yemekTablo.Kalori3 = Kalori3;
				yemekTablo.YemekAdi4 = YemekAdi4;
				yemekTablo.Kalori4 = Kalori4;
				yemekTablo.YemekAdi5 = YemekAdi5;
				yemekTablo.Kalori5 = Kalori5;
				yemekTablo.YemekAdi6 = YemekAdi6;
				yemekTablo.Kalori6 = Kalori6;
				yemekTablo.YemekAdi7 = YemekAdi7;
				yemekTablo.Kalori7 = Kalori7;
				yemekTablo.YemekAdi8 = YemekAdi8;
				yemekTablo.Kalori8 = Kalori8;
				entity.SaveChanges();
				TempData["bilgi"] = tarih;

			}

			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
			}

			return RedirectToAction("Index", new { tarih = selectedDate });
		}

		[HttpPost, AuthFilter(3), ActFilter("Menü Silindi.")]
		public ActionResult YemekSil(string tarih)
		{
			DateTime? selectedDate = null;
			if (!string.IsNullOrEmpty(tarih))
			{
				selectedDate = DateTime.Parse(tarih);
			}
			var yemekler = (from y in entity.YemekTablo where y.Tarih == selectedDate select y).FirstOrDefault();

			if (yemekler != null)
			{
				entity.YemekTablo.Remove(yemekler);
				entity.SaveChanges();
				TempData["bilgi"] = tarih;
			}

			return RedirectToAction("Index", new { tarih = selectedDate });
		}

	}
}