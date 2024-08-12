using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	[LoginFilter, LayoutActionFilter]
	public class DuyurularController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		[HttpGet]
		public ActionResult Index()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);

			DuyuruTableModel viewModel = null;

			if (yetkiTurID == 1)
			{
				var personelYonetici = (from p in entity.TBL_PERSONELLER
										where
										p.personelID == personelID
										select p).FirstOrDefault();
				int birimID = Convert.ToInt32(personelYonetici.personelBirimID);
				var duyurular = (from d in entity.TBL_DUYURULAR
								 where (d.duyuruOlusturanBirim == birimID || d.duyuruOlusturanBirim == null)
								 && d.duyuruAktiflik == true
								 orderby d.duyuruTarih descending
								 select d).ToList();

				var personeller = entity.TBL_PERSONELLER.ToList();
				var birimler = entity.TBL_BIRIMLER.ToList();
				viewModel = new DuyuruTableModel
				{
					Duyurular = duyurular,
					Personeller = personeller,
					Birimler = birimler
				};
			}
			else if (yetkiTurID == 2)
			{
				var personelCalisan = (from p in entity.TBL_PERSONELLER
									   where
									   p.personelID == personelID
									   select p).FirstOrDefault();
				int birimID = Convert.ToInt32(personelCalisan.personelBirimID);
				var duyurular = (from d in entity.TBL_DUYURULAR
								 where (d.duyuruOlusturanBirim == birimID || d.duyuruOlusturanBirim == null) &&
								 d.duyuruAktiflik == true
								 orderby d.duyuruTarih descending
								 select d).ToList();
				var personeller = entity.TBL_PERSONELLER.ToList();
				var birimler = entity.TBL_BIRIMLER.ToList();
				viewModel = new DuyuruTableModel
				{
					Duyurular = duyurular,
					Personeller = personeller,
					Birimler = birimler
				};
			}
			else
			{
				var personelAdmin = (from p in entity.TBL_PERSONELLER
									 where
									 p.personelID == personelID
									 select p).FirstOrDefault();
				var duyurular = (from d in entity.TBL_DUYURULAR
								 where d.duyuruAktiflik == true
								 orderby d.duyuruTarih descending
								 select d).ToList();
				var personeller = entity.TBL_PERSONELLER.ToList();
				var birimler = entity.TBL_BIRIMLER.ToList();
				viewModel = new DuyuruTableModel
				{
					Duyurular = duyurular,
					Personeller = personeller,
					Birimler = birimler
				};
			}
			return View(viewModel);
		}

		[HttpGet]
		public ActionResult DuyuruEkle()
		{
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			if (yetkiTurID == 2)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpPost, ActFilter("Duyuru Eklendi.")]
		public ActionResult DuyuruEkle(string duyuruBaslik, string duyuruIcerik)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);

			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID select p).FirstOrDefault();
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == personel.personelBirimID select b).FirstOrDefault();

			if (string.IsNullOrWhiteSpace(duyuruBaslik) || string.IsNullOrWhiteSpace(duyuruIcerik))
			{
				ViewBag.mesaj = "Duyuru başlığı ve açıklaması boş olamaz.";
				return View();
			}

			TBL_DUYURULAR duyuru = null;

			if (yetkiTurID == 2)
			{
				return RedirectToAction("Index");
			}
			else
			{
				if (birim != null)
				{
					duyuru = new TBL_DUYURULAR
					{
						duyuruBaslik = duyuruBaslik,
						duyuruIcerik = duyuruIcerik,
						duyuruAktiflik = true,
						duyuruCreationDate = DateTime.Now,
						duyuruDeletionDate = null,
						duyuruOlusturan = personelID,
						duyuruOlusturanBirim = birim.birimID,
						duyuruTarih = DateTime.Now,
					};
				}
				else
				{
					duyuru = new TBL_DUYURULAR
					{
						duyuruBaslik = duyuruBaslik,
						duyuruIcerik = duyuruIcerik,
						duyuruAktiflik = true,
						duyuruCreationDate = DateTime.Now,
						duyuruDeletionDate = null,
						duyuruOlusturan = personelID,
						duyuruOlusturanBirim = null,
						duyuruTarih = DateTime.Now,
					};
				}

				entity.TBL_DUYURULAR.Add(duyuru);
				entity.SaveChanges();
				TempData["bilgi"] = duyuru.duyuruBaslik;
				ViewBag.mesaj = "Duyuru başarıyla yayınlandı!";
				return View();
			}
		}


		public ActionResult DuyuruGuncelle(int id)
		{
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			var duyuru = (from d in entity.TBL_DUYURULAR where d.duyuruID == id select d).FirstOrDefault();
			return View(duyuru);
		}

		[HttpPost, ActFilter("Duyuru Güncellendi.")]
		public ActionResult DuyuruGuncelle(int id, string duyuruBaslik, string duyuruIcerik)
		{
			TBL_DUYURULAR duyuru = (from d in entity.TBL_DUYURULAR where d.duyuruID == id select d).FirstOrDefault();
			int personelID = Convert.ToInt32(Session["personelID"]);
			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID select p).FirstOrDefault();
			int birimID = 0;
			if (personel.personelYetkiTurID != 3)
			{
				var birim = (from b in entity.TBL_BIRIMLER where b.birimID == personel.personelBirimID select b).FirstOrDefault();
				birimID = birim.birimID;
			}
			if (string.IsNullOrWhiteSpace(duyuruBaslik) || string.IsNullOrWhiteSpace(duyuruIcerik))
			{
				ViewBag.mesaj = "Duyuru başlığı ve açıklaması boş olamaz.";
				return View();
			}
			duyuru.duyuruAktiflik = false;
			duyuru.duyuruDeletionDate = DateTime.Now;
			var yeniDuyuru = new TBL_DUYURULAR
			{
				duyuruBaslik = duyuruBaslik,
				duyuruIcerik = duyuruIcerik,
				duyuruAktiflik = true,
				duyuruCreationDate = DateTime.Now,
				duyuruDeletionDate = null,
				duyuruOlusturan = personelID,
				duyuruOlusturanBirim = birimID,
				duyuruTarih = DateTime.Now,
			};
			entity.TBL_DUYURULAR.Add(yeniDuyuru);
			entity.SaveChanges();
			ViewBag.mesaj = "Duyuru başarıyla güncellendi.";
			TempData["bilgi"] = duyuru.duyuruBaslik;
			return View(yeniDuyuru);
		}

		public ActionResult DuyuruSil(int id)
		{
			var duyuru = (from d in entity.TBL_DUYURULAR where d.duyuruID == id select d).FirstOrDefault();
			return View(duyuru);
		}

		[HttpPost, ActFilter("Duyuru Silindi.")]
		public ActionResult DuyuruSil(FormCollection fc)
		{
			int duyuruID = Convert.ToInt32(fc["duyuruID"]);
			var duyuru = (from d in entity.TBL_DUYURULAR where d.duyuruID == duyuruID select d).FirstOrDefault();
			duyuru.duyuruAktiflik = false;
			duyuru.duyuruDeletionDate = DateTime.Now;
			entity.SaveChanges();
			TempData["bilgi"] = duyuru.duyuruBaslik;
			return RedirectToAction("Index");
		}

		public ActionResult DuyuruInfo(int id)
		{
			DuyuruTableModel viewModel = null;
			var duyuru = (from d in entity.TBL_DUYURULAR
						  where d.duyuruID == id
						  select d).FirstOrDefault();


			if (duyuru == null)
			{
				// Eğer duyuru bulunamazsa Index action'ına yönlendirme yapılır
				return RedirectToAction("Index");
			}

			var personeller = entity.TBL_PERSONELLER.ToList();
			var birimler = entity.TBL_BIRIMLER.ToList();

			viewModel = new DuyuruTableModel
			{
				Duyurular = new List<TBL_DUYURULAR> { duyuru }, // Tek bir duyuruyu listeye alıyoruz
				Personeller = personeller,
				Birimler = birimler
			};

			return View(viewModel);
		}
	}
}