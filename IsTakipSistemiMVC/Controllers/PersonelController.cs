using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace IsTakipSistemiMVC.Controllers
{
	[AuthFilter(3)]
	public class PersonelController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		public BirimYetkiTurler BirimYetkiTurlerDoldur()
		{
			BirimYetkiTurler by = new BirimYetkiTurler();

			by.birimlerList = (from b in entity.TBL_BIRIMLER where b.aktiflik == true select b).ToList();
			by.yetkiTurlerList = (from y in entity.TBL_YETKITURLER where y.yetkiTurID != 3 select y).ToList();

			return by;
		}

		// GET: Personel
		public ActionResult Index()
		{
			var personeller = (from p in entity.TBL_PERSONELLER where p.aktiflik == true select p).ToList();
			return View(personeller);
		}

		[HttpGet]
		public ActionResult Search(string searchTerm)
		{
			var result = (from p in entity.TBL_PERSONELLER
						  where p.personelAdSoyad.Contains(searchTerm)
						  select p)
				.Include(p => p.TBL_BIRIMLER)
				.Include(p => p.TBL_YETKITURLER)
				.ToList();

			return View("Index", result);
		}

		public ActionResult Olustur()
		{
			BirimYetkiTurler by = BirimYetkiTurlerDoldur();
			ViewBag.mesaj = null;
			return View(by);
		}

		[HttpPost, ActFilter("Yeni Personel Eklendi.")]
		public ActionResult Olustur(FormCollection fc)
		{
			string personelKullaniciAd = fc["kullaniciAd"];
			var personel = (from p in entity.TBL_PERSONELLER
							where p.personelKullaniciAdi == personelKullaniciAd
							select p).FirstOrDefault();
			int birimID = Convert.ToInt32(fc["birim"]);

			if (Convert.ToInt32(fc["yetkiTur"]) == 1)
			{
				var birimYoneticiKontrol = (from p in entity.TBL_PERSONELLER
											where p.personelBirimID == birimID
											&& p.personelYetkiTurID == 1
											select p).FirstOrDefault();

				if (birimYoneticiKontrol != null)
				{
					BirimYetkiTurler by = BirimYetkiTurlerDoldur();
					ViewBag.mesaj = "Bir birimin sadece 1 yöneticisi olabilir!";
					TempData["bilgi"] = null;
					return View(by);
				}
			}

			if (personel == null)
			{
				TBL_PERSONELLER yeniPersonel = new TBL_PERSONELLER();

				yeniPersonel.personelAdSoyad = fc["adSoyad"];
				yeniPersonel.personelKullaniciAdi = personelKullaniciAd;
				yeniPersonel.personelParola = fc["sifre"];
				yeniPersonel.personelBirimID = Convert.ToInt32(fc["birim"]);
				yeniPersonel.personelYetkiTurID = Convert.ToInt32(fc["yetkiTur"]);
				yeniPersonel.personelCreationDate = DateTime.Now;
				if (fc["dogumTarihi"] != "")
				{
					yeniPersonel.personelDogumTarihi = Convert.ToDateTime(fc["dogumTarihi"]);
				}
				yeniPersonel.yeniPersonel = true;
				yeniPersonel.mailAdresi = fc["mailAdresi"];
				yeniPersonel.telefonNumarasi = fc["telefon"];
				if (fc["foto"] == null)
				{
					yeniPersonel.personelFoto = "https://i.hizliresim.com/lf71mhf.jpg";
				}
				else
				{
					yeniPersonel.personelFoto = fc["foto"];
				}
				yeniPersonel.aktiflik = true;
				yeniPersonel.personelIzinGun = 26;

				entity.TBL_PERSONELLER.Add(yeniPersonel);
				entity.SaveChanges();

				TempData["bilgi"] = yeniPersonel.personelKullaniciAdi;
				return RedirectToAction("Index");
			}
			else
			{
				BirimYetkiTurler by = BirimYetkiTurlerDoldur();
				TempData["bilgi"] = null;
				return View(by);
			}
		}

		public ActionResult Guncelle(int id)
		{
			var personel = (from p in entity.TBL_PERSONELLER
							where p.personelID == id
							select p).FirstOrDefault();
			var personeller = (from p in entity.TBL_PERSONELLER where p.personelYetkiTurID != 3 && p.aktiflik == true select p).ToList();
			
			return View(personel);
		}

		[HttpPost, ActFilter("Personel Güncellendi.")]
		public ActionResult Guncelle(int id, string adSoyad, string kullaniciAd, string sifre, string foto, DateTime dogumTarihi, string mailAdresi, string telefonNumarasi, string izinGun)
		{
			TBL_PERSONELLER personel = (from p in entity.TBL_PERSONELLER where p.personelID == id select p).FirstOrDefault();
			TempData["bilgi"] = "Güncelleme öncesi izin gün: " + personel.personelIzinGun;
			personel.personelAdSoyad = adSoyad;
			personel.personelKullaniciAdi = kullaniciAd;
			personel.personelParola = sifre;
			personel.personelFoto = foto;
			personel.personelDogumTarihi = dogumTarihi;
			personel.mailAdresi = mailAdresi;
			personel.telefonNumarasi = telefonNumarasi;
			personel.personelIzinGun = Convert.ToInt32(izinGun);
			entity.SaveChanges();
			TempData["bilgi"] += ", Güncelleme sonrası izin gün: " + personel.personelIzinGun + " - " +  personel.personelKullaniciAdi;
			return RedirectToAction("Index");
		}

		public ActionResult Sil(int id)
		{
			TBL_PERSONELLER personel = (from p in entity.TBL_PERSONELLER where p.personelID == id select p).FirstOrDefault();
			if(personel.personelYetkiTurID == 3)
			{
				ViewBag.mesaj = "Başka bir admini / kendi personel profilinizi silemezsiniz!";
			}
			return View(personel);
		}

		[HttpPost, ActFilter("Personel Silindi.")]
		public ActionResult Sil(FormCollection fc)
		{
			int personelID = Convert.ToInt32(fc["personelID"]);
			var personel = (from p in entity.TBL_PERSONELLER
							where p.personelID == personelID &&
							p.personelYetkiTurID != 3
							select p).FirstOrDefault();
			personel.aktiflik = false;
			personel.personelDeletionDate = DateTime.Now;
			entity.SaveChanges();
			TempData["bilgi"] = personel.personelKullaniciAdi;
			return RedirectToAction("Index");

		}
	}

}