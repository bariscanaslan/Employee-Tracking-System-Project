using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	public class IsDurum
	{
		public string isBaslik { get; set; }
		public string isAciklama { get; set; }
		public DateTime? iletilenTarih { get; set; }
		public DateTime? yapilanTarih { get; set; }
		public string durumAd { get; set; }
		public string isYorum { get; set; }

	}

	[LoginFilter, AuthFilter(2), LayoutActionFilter]
	public class CalisanController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		public void SetBugunYemekInViewBag()
		{
			DateTime bugun = DateTime.Today;
			var bugunYemek = entity.YemekTablo
								   .Where(y => DbFunctions.TruncateTime(y.Tarih) == bugun)
								   .FirstOrDefault();

			ViewBag.BugunYemek = bugunYemek;
		}
		public (string gunListesi, string isSayilari) SingleLineChartDoldur()
		{
			DateTime bugun = DateTime.Today;
			DateTime haftaOnce = bugun.AddDays(-6);
			int personelID = Convert.ToInt32(Session["personelID"]);

			// Tarih listesi oluşturma
			var gunListesi = Enumerable.Range(0, 7)
				.Select(i => bugun.AddDays(-i).ToString("yyyy-MM-dd"))
				.Reverse() // Tarihlerin sıralamasını eski tarihlerden yeni tarihlere doğru yapar
				.Aggregate("[", (current, tarih) => current + $"'{tarih}',") + "]";

			// Personelin iş sayısını toplama
			var isSayilari = Enumerable.Range(0, 7)
				.Select(i => haftaOnce.AddDays(i))
				.Select(date => new
				{
					Start = date.Date,
					End = date.Date.AddDays(1).AddTicks(-1)
				})
				.Select(dateRange => entity.TBL_ISLER
					.Count(i => i.isPersonelID == personelID &&
								i.yapilanTarih >= dateRange.Start &&
								i.yapilanTarih <= dateRange.End)
				)
				.Aggregate("[", (current, isSayisi) => current + $"'{isSayisi}',") + "]";

			return (gunListesi, isSayilari);
		}

		public ActionResult Index()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			SetBugunYemekInViewBag();

			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();

			ViewBag.birimAd = birim.birimAd;


			var isler = (from i in entity.TBL_ISLER
						 where i.isPersonelID == personelID
						 && i.isDurumID == 3
						 && i.isAktiflik == true
						 orderby i.iletilenTarih descending
						 select i).ToList();

			string displayStyle = (isler.Count() != 0) ? "none" : "block";

			var yetki = (from y in entity.TBL_YETKITURLER
						 where yetkiTurID == y.yetkiTurID
						 select y).FirstOrDefault();

			ViewBag.yetkiAd = yetki.yetkiTurAd;
			ViewBag.displayStyle = displayStyle;
			ViewBag.mesaj = "Yeni gönderilmiş bir işiniz bulunmamaktadır!";
			ViewBag.isler = isler;

			var (gunListesi, isSayilari) = SingleLineChartDoldur();
			string gunListesiStr = gunListesi;
			string isSayilariStr = isSayilari;
			ViewBag.gunListesi = gunListesiStr;
			ViewBag.isSayilari = isSayilariStr;

			return View();
		}

		public ActionResult IsKabulEt(int isID)
		{
			var tekIs = (from i in entity.TBL_ISLER where i.isID == isID select i).FirstOrDefault();

			/*
				1 - YAPILIYOR
				2 - YAPILDI
				3 - ÇALIŞAN TARAFINDAN REDDEDİLDİ
				4 - YÖNETİCİ TARAFINDAN REDDEDİLDİ
				5 - İŞ İPTAL EDİLDİ
			*/
			tekIs.isDurumID = 1;
			tekIs.isKabulEdilenTarih = DateTime.Now;
			entity.SaveChanges();

			return RedirectToAction("Yap", "Calisan");
		}

		public ActionResult IsReddet(int isID)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			var tekIs = (from i in entity.TBL_ISLER where i.isID == isID select i).FirstOrDefault();

			var calisan = (from p in entity.TBL_PERSONELLER
						   where p.aktiflik == true &&
						   p.personelID == personelID
						   select p).FirstOrDefault();

			var yonetici = (from p in entity.TBL_PERSONELLER
							where p.aktiflik == true &&
							p.personelYetkiTurID == 1 &&
							p.personelBirimID == calisan.personelBirimID
							select p).FirstOrDefault();
			/*
				1 - YAPILIYOR
				2 - YAPILDI
				3 - ÇALIŞAN CEVABI BEKLENİYOR
				4 - ÇALIŞAN TARAFINDAN REDDEDİLDİ
				5 - İŞ İPTAL EDİLDİ
			*/
			tekIs.isReddedilenTarih = DateTime.Now;
			tekIs.isDurumID = 4;

			TBL_MAILLER yeniMail = new TBL_MAILLER();
			yeniMail.mailGonderen = personelID;
			yeniMail.mailAlici = yonetici.personelID;
			yeniMail.mailKonu = calisan.personelAdSoyad + " İşi Reddetti";
			yeniMail.mailIcerik = "<br>" +
				"<b>" + tekIs.isBaslik + "</b>"
				+ "<br> başlıklı, <br>" +
				"<b>" + tekIs.isAciklama + "</b><br> açıklamalı iş, çalışanınız tarafından reddedilmiştir."
				+ "<br>" + "<br>" + "<hr>" + "Bu mail otomatik olarak sistem tarafından oluşturulmuştur."
				+ "<br>" + "<br>";
			yeniMail.mailGonderimTarihi = DateTime.Now;
			yeniMail.mailArsiv = false;
			yeniMail.mailAktiflik = true;
			yeniMail.mailOkunma = false;
			yeniMail.mailCopKutusu = false;
			entity.TBL_MAILLER.Add(yeniMail);

			entity.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Yap()
		{
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			int personelID = Convert.ToInt32(Session["personelID"]);

			var isler = (from i in entity.TBL_ISLER
						 where i.isPersonelID == personelID &&
						 i.isDurumID == 1 && i.isAktiflik == true
						 select i).ToList().OrderByDescending(i => i.iletilenTarih);
			int ic = isler.Count();
			ViewBag.islerCount = ic;
			string displayStyle = (isler.Count() != 0) ? "none" : "block";
			ViewBag.displayStyle = displayStyle;
			ViewBag.isler = isler;


			var sonIs = (from s in entity.TBL_ISLER
						 where s.isPersonelID == personelID &&
							   s.isDurumID == 2 &&
							   s.isAktiflik == true
						 orderby s.yapilanTarih descending
						 select s).FirstOrDefault();

			ViewBag.sonIs = sonIs;

			ViewBag.mesaj = "Tamamlanmamış işiniz bulunmamaktadır!";

			return View();

		}

		[HttpPost, ActFilter("İş tamamlandı.")]
		public ActionResult Yap(int isID, string isYorum)
		{
			var tekIs = (from i in entity.TBL_ISLER where i.isID == isID select i).FirstOrDefault();

			if (isYorum == "")
			{
				isYorum = "-";
			}

			tekIs.yapilanTarih = DateTime.Now;
			tekIs.isDurumID = 2;
			tekIs.isYorum = isYorum;

			entity.SaveChanges();

			return RedirectToAction("Index", "Calisan");
		}

		public ActionResult Takip(int? page)
		{
			int pageSize = 10; // Sayfa başına gösterilecek öğe sayısı
			int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1

			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			var isler = (from i in entity.TBL_ISLER
						 join d in entity.TBL_DURUMLAR on i.isDurumID equals d.durumID
						 where i.isPersonelID == personelID && i.isDurumID == 2
						 select i).ToList().OrderByDescending(i => i.iletilenTarih);

			List<IsDurum> list = new List<IsDurum>();

			foreach (var i in isler)
			{
				IsDurum isDurum = new IsDurum
				{
					isBaslik = i.isBaslik,
					isAciklama = i.isAciklama,
					iletilenTarih = i.iletilenTarih,
					yapilanTarih = i.yapilanTarih,
					durumAd = i.TBL_DURUMLAR.durumAd,
					isYorum = i.isYorum
				};

				list.Add(isDurum);
			}

			isDurumModel model = new isDurumModel
			{
				isDurumlar = list
			};

			ViewBag.PageNumber = pageNumber;
			ViewBag.PageSize = pageSize;
			ViewBag.TotalPages = (int)Math.Ceiling((double)list.Count / pageSize);

			return View(model);
		}

		public ActionResult IzinAl()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			var izinAlanPersonel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID && p.aktiflik == true select p).FirstOrDefault();
			ViewBag.izinHakki = izinAlanPersonel.personelIzinGun;
			return View();
		}

		[HttpPost]
		public ActionResult IzinAl(DateTime baslangic, DateTime bitis)
		{
			bitis = bitis.AddDays(1).AddSeconds(-1);
			int personelID = Convert.ToInt32(Session["personelID"]);
			var izinAlanPersonel = (from p in entity.TBL_PERSONELLER where p.personelID == personelID && p.aktiflik == true select p).FirstOrDefault();
			ViewBag.izinHakki = izinAlanPersonel.personelIzinGun;

			if (izinAlanPersonel.personelYetkiTurID == 2)
			{
				var birim = (from b in entity.TBL_BIRIMLER where b.aktiflik == true && b.birimID == izinAlanPersonel.personelBirimID select b).FirstOrDefault();
				var izinVerenPersonel = (from p in entity.TBL_PERSONELLER where p.aktiflik == true && p.personelYetkiTurID == 1 && p.personelBirimID == birim.birimID select p).FirstOrDefault();

				if (baslangic > bitis)
				{
					ViewBag.tarihHatasi = "Başlangıç tarihi bitiş tarihinden sonra olamaz!";
					return View();
				}
				else
				{
					int toplamGun = 0;
					int tempGun = 0;

					for (DateTime dt = baslangic; dt <= bitis; dt = dt.AddDays(1))
					{
						if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
						{
							tempGun++;
						}
					}

					toplamGun = tempGun;

					if (izinAlanPersonel.personelIzinGun < toplamGun)
					{
						ViewBag.izinGunHatasi = "İzin gün hakkınız talep ettiğiniz gün sayısından daha azdır!";
						return View();
					}
					else
					{
						// Check if there are any overlapping leave requests
						var existingLeave = (from i in entity.TBL_IZINLER
											 where i.izinPersonelID == personelID &&
											 ((i.izinBaslangicTarihi <= bitis && i.izinBitisTarihi >= baslangic) ||
											  (i.izinBaslangicTarihi >= baslangic && i.izinBitisTarihi <= bitis))
											 select i).FirstOrDefault();

						if (existingLeave != null)
						{
							ViewBag.izinTarihHatasi = "Başlangıç ve bitiş tarihleri arasında başka bir izin bulunmaktadır!";
							return View();
						}

						TBL_IZINLER yeniIzin = new TBL_IZINLER();
						yeniIzin.izinPersonelID = personelID;
						yeniIzin.izinBaslangicTarihi = baslangic;
						yeniIzin.izinBitisTarihi = bitis;
						yeniIzin.izinTalepTarihi = DateTime.Now;
						yeniIzin.izinOnay = false;
						yeniIzin.izinRed = false;
						yeniIzin.izinGunuSayisi = toplamGun;
						entity.TBL_IZINLER.Add(yeniIzin);

						TBL_MAILLER izinMaili = new TBL_MAILLER();
						izinMaili.mailGonderen = izinAlanPersonel.personelID;
						izinMaili.mailAlici = izinVerenPersonel.personelID;
						izinMaili.mailKonu = izinAlanPersonel.personelAdSoyad + " İzin Talebi";
						izinMaili.mailIcerik = "<br>" +
							"Biriminize bağlı personel " + izinAlanPersonel.personelAdSoyad + ", <br>" + "<b>" + baslangic.ToString("dd/MM/yyyy") + "</b> ve <b>" + bitis.ToString("dd/MM/yyyy") + "</b> tarihleri arasında <br> toplamda <b>" + toplamGun + "</b> gün izin talebinde bulunmuştur."
							+ "<br>" + "<br>" + "<hr>" + "Bu mail otomatik olarak sistem tarafından oluşturulmuştur."
							+ "<br>" + "<br>";
						izinMaili.mailGonderimTarihi = DateTime.Now;
						izinMaili.mailArsiv = false;
						izinMaili.mailAktiflik = true;
						izinMaili.mailOkunma = false;
						izinMaili.mailCopKutusu = false;
						entity.TBL_MAILLER.Add(izinMaili);

						ViewBag.basarili = "İzin talebiniz başarıyla oluşturulmuştur!";
						entity.SaveChanges();
						return View();
					}
				}
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		public ActionResult IzinTaleplerim()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var izinler = (from i in entity.TBL_IZINLER
						   join p in entity.TBL_PERSONELLER on
						   i.izinPersonelID equals p.personelID
						   where p.personelID == personelID &&
						   i.izinOnay == true &&
						   i.izinRed == false
						   orderby i.izinDegerlendirmeTarihi descending
						   select i).ToList();

			return View(izinler);
		}
	}
}