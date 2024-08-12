using IsTakipSistemiMVC.Models;
using IsTakipSistemiMVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using System.Data.Entity;
using PagedList;

namespace IsTakipSistemiMVC.Controllers
{
	[LoginFilter, AuthFilter(3), LayoutActionFilter]
	public class AdminController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		// BİRİM BAŞINA YAPILAN TOPLAM İŞ SAYISI CHARTINI DOLDURAN FONKSİYON
		public (string labelBirim, string dataIs) BirimlerinIsleri()
		{
			var birimler = (from b in entity.TBL_BIRIMLER where b.aktiflik == true select b).ToList();
			string labelBirim = "[";
			foreach (var birim in birimler)
			{
				labelBirim += "'" + birim.birimAd + "',";
			}
			labelBirim = labelBirim.TrimEnd(',') + "]";

			List<int> islerToplam = (from birim in birimler
									 let personeller = (from p in entity.TBL_PERSONELLER
														where p.personelBirimID == birim.birimID &&
														p.aktiflik == true
														select p.personelID)
									 let isSayisi = (from i in entity.TBL_ISLER
													 where personeller.Any(pid => pid == i.isPersonelID)
													 select i).Count()
									 select isSayisi).ToList();
			string dataIs = "[";
			foreach (var isler in islerToplam)
			{
				dataIs += "'" + isler + "',";
			}
			dataIs = dataIs.TrimEnd(',') + "]";
			return (labelBirim, dataIs);
		}
		// BİRİM BAŞINA DÜŞEN PERSONEL SAYISI CHARTINI DOLDURAN FONKSİYON
		public string PersonelBirimSayimi()
		{
			int muhasebePer = 0;
			int bilgiIslemPer = 0;
			int sahaPer = 0;

			var personellerBirimSayimi = (from p in entity.TBL_PERSONELLER
										  where p.aktiflik == true
										  select p).ToList();

			foreach (var per in personellerBirimSayimi)
			{
				if (per.personelBirimID == 1)
				{
					muhasebePer += 1;
				}
				else if (per.personelBirimID == 2)
				{
					bilgiIslemPer += 1;
				}
				else if (per.personelBirimID == 3)
				{
					sahaPer += 1;
				}
			}

			List<int> personelSayiList = new List<int>
			{
				muhasebePer,
				bilgiIslemPer,
				sahaPer
			};

			string personelSayiListesi = "[";
			foreach (var i in personelSayiList)
			{
				personelSayiListesi += "'" + i + "',";
			}
			// Son virgülü kaldır
			if (personelSayiListesi.EndsWith(","))
			{
				personelSayiListesi = personelSayiListesi.Remove(personelSayiListesi.Length - 1);
			}
			personelSayiListesi += "]";
			return personelSayiListesi;
		}
		// HAFTALIK BİRİM İŞ SAYISINI TAKİP EDEN CHARTI DOLDURAN FONKSİYON
		public void MultipleChartDoldur(ref string gunListesi, List<string> indexlerString)
		{
			DateTime bugun = DateTime.Today;

			string temp = "[";
			for (int i = 6; i >= 0; i--)
			{
				DateTime gun = bugun.AddDays(-i);
				temp += "'" + gun.ToString("yyyy-MM-dd") + "',";
			}
			temp += "]";

			gunListesi = temp;

			//////////////////////////////////////////////////////////////////////////////////

			bugun = DateTime.Today;
			DateTime haftaOnce = bugun.AddDays(-6);

			var birimler = (from b in entity.TBL_BIRIMLER
							where b.aktiflik == true
							select b).ToList();

			var birimIsSayilari = birimler.Select(birim =>
			{
				var birimGecmisIsSayilari = new List<int>();

				for (DateTime date = haftaOnce; date <= bugun; date = date.AddDays(1))
				{
					DateTime dateStart = date.Date; // Günün başlangıcı (00:00)
					DateTime dateEnd = dateStart.AddDays(1).AddTicks(-1); // Günün bitişi (23:59:59.999)

					var toplam = (from p in entity.TBL_PERSONELLER
								  join i in entity.TBL_ISLER on p.personelID equals i.isPersonelID
								  where p.personelBirimID == birim.birimID &&
										p.aktiflik == true &&
										p.personelYetkiTurID == 2 &&
										i.yapilanTarih >= dateStart &&
										i.yapilanTarih <= dateEnd
								  select i).Count();

					birimGecmisIsSayilari.Add(toplam);
				}
				return birimGecmisIsSayilari;
			}).ToList();

			for (int i = 0; i < birimIsSayilari.Count; i++)
			{
				string temp2 = "[";
				foreach (var sayi in birimIsSayilari[i])
				{
					temp2 += "'" + sayi.ToString() + "',";
				}
				temp2 = temp2.TrimEnd(',') + "]";
				indexlerString.Add(temp2);
			}
		}

		public ActionResult Index()
		{
			// BİRİM BAŞINA YAPILAN TOPLAM İŞ SAYISI CHARTINI DOLDURAN FONKSİYON
			var (labelBirim, dataIs) = BirimlerinIsleri();
			string labelBirimStr = labelBirim;
			string dataIsStr = dataIs;
			// BİRİM BAŞINA YAPILAN TOPLAM İŞ SAYISI CHARTINI DOLDURMAK İÇİN OLUŞTURULAN VIEWBAGS
			ViewBag.labelBirim = labelBirimStr;
			ViewBag.dataIs = dataIsStr;

			// BİRİM BAŞINA DÜŞEN PERSONEL SAYISI CHARTINI DOLDURAN FONKSİYON
			string personelSayiListesi = PersonelBirimSayimi();
			ViewBag.personelSayiList = personelSayiListesi;

			// HAFTALIK BİRİM İŞ SAYISINI TAKİP EDEN CHART İÇİN OLUŞTURULAN STRING VE LIST
			string gunListesi = "";
			List<string> indexlerString = new List<string>();

			// HAFTALIK BİRİM İŞ SAYISINI TAKİP EDEN CHARTI DOLDURAN FONKSİYON
			MultipleChartDoldur(ref gunListesi, indexlerString);

			// HAFTALIK BİRİM İŞ SAYISINI TAKİP EDEN CHART İÇİN OLUŞTURULAN VIEWBAGS
			ViewBag.gunListesi = gunListesi;
			ViewBag.muhasebe = indexlerString[0];
			ViewBag.bilgiIslem = indexlerString[1];
			ViewBag.sahaPersoneli = indexlerString[2];

			return View();
		}

		public ActionResult Birim()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			var birimler = (from b in entity.TBL_BIRIMLER where b.aktiflik == true select b).ToList();
			return View(birimler);
		}

		public ActionResult Olustur()
		{
			return View();
		}

		[HttpPost, ActFilter("Yeni Birim Eklendi.")]
		public ActionResult Olustur(string birimAd)
		{
			TBL_BIRIMLER yeniBirim = new TBL_BIRIMLER();
			string yeniAd = birimAd;
			yeniBirim.birimAd = yeniAd;
			yeniBirim.aktiflik = true;
			entity.TBL_BIRIMLER.Add(yeniBirim);
			entity.SaveChanges();

			TempData["bilgi"] = yeniBirim.birimAd;

			return RedirectToAction("Birim");
		}

		public ActionResult Guncelle(int id)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == id select b).FirstOrDefault();
			return View(birim);
		}

		[HttpPost, ActFilter("Birim Güncellendi.")]
		public ActionResult Guncelle(FormCollection fc)
		{
			int birimID = Convert.ToInt32(fc["birimID"]);
			string yeniAd = fc["birimAd"];

			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();

			birim.birimAd = yeniAd;
			entity.SaveChanges();

			TempData["bilgi"] = birim.birimAd;

			return RedirectToAction("Birim");
		}

		public ActionResult Sil(int id)
		{
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == id select b).FirstOrDefault();
			return View(birim);
		}

		[HttpPost, ActFilter("Personel Silindi.")]
		public ActionResult Sil(FormCollection fc)
		{
			int birimID = Convert.ToInt32(fc["birimID"]);
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();
			birim.aktiflik = false;
			entity.SaveChanges();
			TempData["bilgi"] = birim.birimAd;
			return RedirectToAction("Birim");
		}

		public ActionResult Loglar()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			
			// LOG TEMİZLEMEK İÇİN ŞİFRE ÇEKME
			var personelSifresi = (from p in entity.TBL_PERSONELLER
								   where p.personelID == personelID
								   select p).FirstOrDefault();
			string sifre = personelSifresi.personelParola;
			ViewBag.sifre = sifre;

			// LOG LİSTESİ
			var loglar = (from l in entity.TBL_LOGLAR orderby l.tarih descending select l).ToList();
			string displayStyle = (loglar.Count() != 0) ? "none" : "block";
			ViewBag.displayStyle = displayStyle;
			ViewBag.mesaj = "Herhangi bir log bulunmamaktadır!";

			return View(loglar);
		}

		[HttpPost, ActFilter("Excel'e Aktarıldı.")]
		public ActionResult Excel()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var loglar = entity.TBL_LOGLAR.Include(l => l.TBL_PERSONELLER).OrderByDescending(l => l.tarih).ToList();

			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Logs");
				var row = 1;

				// Başlıkları yaz
				worksheet.Cells[row, 1].Value = "Log ID";
				worksheet.Cells[row, 2].Value = "Personel Ad Soyad";
				worksheet.Cells[row, 3].Value = "Log Açıklama";
				worksheet.Cells[row, 4].Value = "Action Ad";
				worksheet.Cells[row, 5].Value = "Controller Ad";
				worksheet.Cells[row, 6].Value = "Tarih";
				row++;

				// Verileri yaz
				foreach (var log in loglar)
				{
					worksheet.Cells[row, 1].Value = log.logID;
					worksheet.Cells[row, 2].Value = log.TBL_PERSONELLER?.personelAdSoyad;
					worksheet.Cells[row, 3].Value = log.logAciklama;
					worksheet.Cells[row, 4].Value = log.actionAd;
					worksheet.Cells[row, 5].Value = log.controllerAd;
					worksheet.Cells[row, 6].Value = log.tarih;
					worksheet.Cells[row, 6].Style.Numberformat.Format = "yyyy-mm-dd HH:mm";
					row++;
				}

				worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
				var stream = new MemoryStream();
				package.SaveAs(stream);
				stream.Position = 0;
				string excelName = $"Logs-{DateTime.Now:yyyyMMddHHmm}.xlsx";
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
			}
		}

		[HttpPost]
		public ActionResult LogTemizle()
		{
			var loglar = (from l in entity.TBL_LOGLAR select l).ToList();
			entity.TBL_LOGLAR.RemoveRange(loglar);
			entity.SaveChanges();
			return RedirectToAction("Loglar");
		}

		public void GetPersonelVeBirim()
		{
			// Yetki türü ve birim türü session'dan çekilir
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);

			var calisanlar = (from p in entity.TBL_PERSONELLER
							  where p.personelYetkiTurID == 2 &&
							  p.aktiflik == true
							  select p).ToList();
			ViewBag.personeller = calisanlar;
		}

		public ActionResult Takip()
		{
			GetPersonelVeBirim();
			return View();
		}

		[HttpPost]
		public ActionResult Takip(int selectPer)
		{
			TempData["secilenPersonelID"] = selectPer;
			return RedirectToAction("Listele", "Admin", new { id = selectPer });
		}

		[HttpGet]
		public ActionResult Listele(int id, int? page)
		{
			int pageSize = 10; // Bir sayfada gösterilecek kart sayısı
			int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1

			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			var secilenPersonel = (from p in entity.TBL_PERSONELLER
								   where p.personelID == id
								   select p).FirstOrDefault();
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == secilenPersonel.personelBirimID select b).FirstOrDefault();

			if (secilenPersonel != null)
			{
				try
				{
					DateTime ayOnce = DateTime.Today.AddDays(-29);
					DateTime ayStart = ayOnce.Date; // Günün başlangıcı (00:00)
					DateTime ayEnd = DateTime.Today.AddDays(1).Date.AddTicks(-1); // Günün bitişi (23:59:59.999)


					DateTime haftaOnce = DateTime.Today.AddDays(-6);
					DateTime haftaStart = haftaOnce.Date; // Günün başlangıcı (00:00)
					DateTime haftaEnd = DateTime.Today.AddDays(1).Date.AddTicks(-1); // Günün bitişi (23:59:59.999)

					var tumIsler = (from i in entity.TBL_ISLER
									where i.isPersonelID == secilenPersonel.personelID &&
									i.isAktiflik == true
									select i).ToList().OrderByDescending(i => i.iletilenTarih);

					var pagedIsler = tumIsler.ToPagedList(pageNumber, pageSize);

					var aylikIsler = (from i in entity.TBL_ISLER
									  where i.isPersonelID == secilenPersonel.personelID &&
									  i.yapilanTarih >= ayStart &&
									  i.yapilanTarih <= ayEnd &&
									  i.isAktiflik == true
									  select i).ToList();
					var haftalikIsler = (from i in entity.TBL_ISLER
										 where i.isPersonelID == secilenPersonel.personelID &&
										 i.yapilanTarih >= haftaStart &&
										 i.yapilanTarih <= haftaEnd &&
										 i.isAktiflik == true
										 select i).ToList();

					var tumIslerC = (from i in entity.TBL_ISLER
									 join p in entity.TBL_PERSONELLER on i.isPersonelID equals p.personelID
									 where p.personelBirimID == birim.birimID && i.isAktiflik == true
									 select i).ToList();

					float tumIslerTumPersonel = tumIslerC.Count();

					var aylikIslerC = (from i in entity.TBL_ISLER
									   join p in entity.TBL_PERSONELLER on i.isPersonelID equals p.personelID
									   where p.personelBirimID == birim.birimID &&
											 i.yapilanTarih >= ayStart &&
											 i.yapilanTarih <= ayEnd &&
											 i.isAktiflik == true
									   select i).ToList();

					float aylikIslerTumPersonel = aylikIslerC.Count();


					var haftalikIslerC = (from i in entity.TBL_ISLER
										  join p in entity.TBL_PERSONELLER on i.isPersonelID equals p.personelID
										  where p.personelBirimID == birim.birimID &&
												i.yapilanTarih >= haftaStart &&
												i.yapilanTarih <= haftaEnd &&
												i.isAktiflik == true
										  select i).ToList();

					float haftalikIslerTumPersonel = haftalikIslerC.Count();

					ViewBag.personel = secilenPersonel;
					ViewBag.tumIsler = pagedIsler;
					ViewBag.isSayisi = pagedIsler.Count; // Toplam iş sayısını güncelle
					ViewBag.tumIslerToplami = tumIsler.Count();
					ViewBag.aylikIslerToplami = aylikIsler.Count();
					ViewBag.haftalikIslerToplami = haftalikIsler.Count();

					ViewBag.tumIslerToplamiC = tumIslerTumPersonel;
					ViewBag.aylikIslerToplamiC = aylikIslerTumPersonel;
					ViewBag.haftalikIslerToplamiC = haftalikIslerTumPersonel;

					float haftalikYuzde = (haftalikIsler.Count() / haftalikIslerTumPersonel) * 100;
					float aylikYuzde = (aylikIsler.Count() / aylikIslerTumPersonel) * 100;
					float tumYuzde = (tumIsler.Count() / tumIslerTumPersonel) * 100;

					ViewBag.haftalikYuzde = haftalikYuzde;
					ViewBag.aylikYuzde = aylikYuzde;
					ViewBag.tumYuzde = tumYuzde;

					ViewBag.pageNumber = pageNumber;
					ViewBag.pageSize = pageSize;
					ViewBag.totalPages = pagedIsler.PageCount;

					return View();
				}
				catch (Exception m)
				{
					ViewBag.mesaj = m.Message;
				}
			}
			return View();
		}

		public ActionResult IzinIslemleri()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID select p).FirstOrDefault();
			ViewBag.sifre = personel.personelParola;
			var izinler = (from i in entity.TBL_IZINLER
						   join p in entity.TBL_PERSONELLER on i.izinPersonelID equals p.personelID
						   where (i.izinOnay == true && i.izinRed == false) ||
						   (i.izinOnay == false && i.izinRed == true)
						   orderby i.izinDegerlendirmeTarihi descending
						   select i).ToList();
			return View(izinler);
		}

		[HttpPost]
		public ActionResult HataliIzinSil(int izinID)
		{
			var izin = (from i in entity.TBL_IZINLER
						where i.izinID == izinID
						select i).FirstOrDefault();
			entity.TBL_IZINLER.Remove(izin);
			entity.SaveChanges();
			return RedirectToAction("IzinIslemleri");
		}
	}
}