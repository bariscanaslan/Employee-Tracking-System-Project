using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace IsTakipSistemiMVC.Controllers
{
	[LoginFilter, AuthFilter(1), LayoutActionFilter]
	public class YoneticiController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		// YEMEK TABLOSU İÇİN OLUŞTURULAN FONKSİYON
		public void SetBugunYemekInViewBag()
		{
			DateTime bugun = DateTime.Today;
			var bugunYemek = entity.YemekTablo
								   .Where(y => DbFunctions.TruncateTime(y.Tarih) == bugun)
								   .FirstOrDefault();

			ViewBag.BugunYemek = bugunYemek;
		}

		public void GetPersonelForDatePickerChart(DateTime date)
		{
			int birimID = Convert.ToInt32(Session["personelBirimID"]);
			DateTime selectedDate = date;
			DateTime startDate = selectedDate.Date;
			DateTime endDate = startDate.AddDays(1).AddTicks(-1);
			ViewBag.selectedDate = selectedDate.ToString("yyyy-MM-ddTHH:mm:ss");

			// Tek bir sorgu ile gerekli verileri çekiyoruz
			var personelData = from p in entity.TBL_PERSONELLER
							   join i in entity.TBL_ISLER on p.personelID equals i.isPersonelID into isGroup
							   where p.personelBirimID == birimID &&
									 p.aktiflik == true &&
									 p.personelYetkiTurID == 2
							   select new
							   {
								   p.personelAdSoyad,
								   p.personelFoto,
								   ToplamIs = isGroup.Count(isler => isler.isDurumID == 2 &&
																	  isler.yapilanTarih >= startDate &&
																	  isler.yapilanTarih <= endDate)
							   };

			var personelList = personelData.ToList();
			ViewBag.personelIsimleri = $"[{string.Join(",", personelList.Select(p => $"'{p.personelAdSoyad}'"))}]";
			ViewBag.toplamIsStr = $"[{string.Join(",", personelList.Select(p => p.ToplamIs))}]";
		}

		// PIE CHART İÇİN PERSONEL İŞ SAYISI
		public void GetPersonelIsSayisiForPieChart()
		{
			int birimID = Convert.ToInt32(Session["personelBirimID"]);
			// Veritabanından gerekli verileri tek bir sorguda çekiyoruz
			var calisanlarListesi = from c in entity.TBL_PERSONELLER
									where c.aktiflik == true &&
										  c.personelYetkiTurID == 2 &&
										  c.personelBirimID == birimID
									join i in entity.TBL_ISLER on c.personelID equals i.isPersonelID into isGroup
									select new
									{
										c.personelAdSoyad,
										IsSayisi = isGroup.Count()
									};
			var calisanlarListesiData = calisanlarListesi.ToList();
			// Çalışanların isimlerini ve iş sayılarını JSON formatında hazırlıyoruz
			var calisanlarListesiStr = $"[{string.Join(",", calisanlarListesiData.Select(c => $"'{c.personelAdSoyad}'"))}]";
			var isSayilariStr = $"[{string.Join(",", calisanlarListesiData.Select(c => c.IsSayisi))}]";

			// PIE CHART İÇİN GEREKEN VERİLERİ VIEWBAG'E ATAYORUZ
			ViewBag.calisanlarListesiStr = calisanlarListesiStr;
			ViewBag.isSayilariStr = isSayilariStr;
		}

		private ToplamIs GetAyinElemaniWithDate(int ay, int yil)
		{
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			DateTime baslangicTarih = new DateTime(yil, ay, 1);
			DateTime bitisTarih = baslangicTarih.AddMonths(1).AddDays(-1);

			// Personelleri ve işleri veritabanından çekiyoruz
			var personeller = entity.TBL_PERSONELLER
				.Where(p => p.personelBirimID == birimID && p.personelYetkiTurID == 2)
				.ToList();

			var isler = entity.TBL_ISLER
				.Where(i => i.yapilanTarih >= baslangicTarih && i.yapilanTarih <= bitisTarih)
				.ToList();

			// Personelleri ve işleri gruplama
			var topEleman = personeller
				.GroupJoin(isler, p => p.personelID, i => i.isPersonelID, (p, group) => new ToplamIs
				{
					personelAdSoyad = p.personelAdSoyad,
					personelFotosu = p.personelFoto,
					toplamIs = group.Count(i => i.isDurumID == 2)
				})
				.OrderByDescending(p => p.toplamIs)
				.FirstOrDefault();

			// topEleman'ı döndürme
			return topEleman;
		}

		public ActionResult Index()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			// BİRİM ADI ÇEKME
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();
			ViewBag.birimAd = birim.birimAd;

			// YEMEK TABLOSU İÇİN OLUŞTURULAN FONKSİYON
			SetBugunYemekInViewBag();
			// AYIN ELEMANI CARD'INI DOLDURMA
			int ay = DateTime.Now.Month;
			int yil = DateTime.Now.Year;
			ViewBag.ayinElemaniIndex = GetAyinElemaniWithDate(ay, yil);
			// DATE PICKER & CHART İÇİN PERSONEL ÇEKİMİ
			GetPersonelForDatePickerChart(DateTime.Today);
			// PIE CHART İÇİN PERSONEL İŞ SAYISI
			GetPersonelIsSayisiForPieChart();

			// BİRİM YÖNETİCİSİNE PERSONELLERİ GÖSTEREN DIV İÇİN OLUŞTURULAN MODELİ YARATAN FONKSİYON
			var personelListesi = (from p in entity.TBL_PERSONELLER
								   where p.aktiflik == true &&
								   p.personelYetkiTurID == 2 &&
								   p.personelBirimID == birimID
								   select new Personel
								   {
									   ModelPersonelAd = p.personelAdSoyad,
									   ModelPersonelFoto = p.personelFoto,
									   ModelPersonelID = p.personelID,
								   }).ToList();
			var viewModel = new YoneticiHomeViewModel
			{
				Personeller = personelListesi
			};

			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Index(DateTime date)
		{
			// SESSION ID ÇEKİMİ
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			// BİRİM ADI ÇEKME
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();
			ViewBag.birimAd = birim.birimAd;
			// AYIN ELEMANI CARD'INI DOLDURMA
			int ay = DateTime.Now.Month;
			int yil = DateTime.Now.Year;
			ViewBag.ayinElemaniIndex = GetAyinElemaniWithDate(ay, yil);

			// BİRİM YÖNETİCİSİNE PERSONELLERİ GÖSTEREN DIV İÇİN OLUŞTURULAN MODEL
			var personelListesi = (from p in entity.TBL_PERSONELLER
								   where p.aktiflik == true &&
								   p.personelYetkiTurID == 2 &&
								   p.personelBirimID == birimID
								   select new Personel
								   {
									   ModelPersonelAd = p.personelAdSoyad,
									   ModelPersonelFoto = p.personelFoto,
									   ModelPersonelID = p.personelID,
								   }).ToList();
			var viewModel = new YoneticiHomeViewModel
			{
				Personeller = personelListesi
			};

			// DATE PICKER & CHART İÇİN PERSONEL ÇEKİMİ
			GetPersonelForDatePickerChart(date);
			// PIE CHART İÇİN PERSONEL İŞ SAYISI
			GetPersonelIsSayisiForPieChart();
			return View(viewModel);
		}

		[HttpGet]
		public ActionResult AyinElemani()
		{
			int simdikiYil = DateTime.Now.Year;
			int simdikiAy = DateTime.Now.Month;

			List<int> yillar = new List<int>();
			for (int i = simdikiYil; i >= 2023; i--)
			{
				yillar.Add(i);
			}
			ViewBag.yillar = yillar;

			// Formun varsayılan ay ve yılını ViewBag'e ekleyin
			ViewBag.SelectedAy = simdikiAy.ToString("D2"); // İki basamaklı ay
			ViewBag.SelectedYil = simdikiYil;
			// İlk açılışta ayın elemanını otomatik getirme
			return AyinElemani(simdikiAy, simdikiYil);
		}

		[HttpPost]
		public ActionResult AyinElemani(int aylar, int yillar)
		{
			ViewBag.ayinElemani = GetAyinElemaniWithDate(aylar, yillar);

			// Seçilen ay ve yıl için ViewBag'e değer ekleyin
			ViewBag.SelectedAy = aylar.ToString("D2"); // İki basamaklı ay
			ViewBag.SelectedYil = yillar;

			if (ViewBag.ayinElemani.toplamIs == 0)
			{
				ViewBag.isYapilmamis = "Bu ay hiç iş yapılmamıştır!";
			}

			int simdikiYil = DateTime.Now.Year;
			List<int> sonucYillar = new List<int>();
			for (int i = simdikiYil; i >= 2023; i--)
			{
				sonucYillar.Add(i);
			}
			ViewBag.yillar = sonucYillar;
			return View();
		}

		public void GetPersonelVeBirim()
		{
			// Yetki türü ve birim türü session'dan çekilir
			int birimID = Convert.ToInt32(Session["personelBirimID"]);
			int personelID = Convert.ToInt32(Session["personelID"]);
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);

			var calisanlar = (from p in entity.TBL_PERSONELLER
							  where p.personelBirimID == birimID &&
							  p.personelYetkiTurID == 2 &&
							  p.aktiflik == true
							  select p).ToList();
			ViewBag.personeller = calisanlar;
			var birim = (from b in entity.TBL_BIRIMLER where b.birimID == birimID select b).FirstOrDefault();
			ViewBag.birimAd = birim.birimAd;
		}

		public ActionResult Ata()
		{
			GetPersonelVeBirim();


			/*
				1 - YAPILIYOR
				2 - YAPILDI
				3 - ÇALIŞAN CEVABI BEKLENİYOR
				4 - ÇALIŞAN TARAFINDAN REDDEDİLDİ
				5 - İŞ İPTAL EDİLDİ
			*/

			int birimID = Convert.ToInt32(Session["personelBirimID"]);
			var kabulEdilmemisIsler = (from i in entity.TBL_ISLER
									   join p in entity.TBL_PERSONELLER on i.isPersonelID equals p.personelID
									   where p.personelBirimID == birimID && i.isDurumID == 3 && i.isAktiflik == true
									   select new IsViewModel
									   {
										   Is = i,
										   PersonelAdSoyad = p.personelAdSoyad
									   }).ToList();

			// kei = kabul edilmemiş işler
			int kei = 0;
			kei = kabulEdilmemisIsler.Count();
			ViewBag.kei = kei;
			return View(kabulEdilmemisIsler);
		}

		[HttpPost, ActFilter("İş Atandı.")]
		public ActionResult Ata(FormCollection formCollect)
		{
			string isBaslik = formCollect["isBaslik"];
			string isAciklama = formCollect["isAciklama"];
			int secilenPersonelID = Convert.ToInt32(formCollect["selectPer"]);
			var personel = (from p in entity.TBL_PERSONELLER where p.personelID == secilenPersonelID && p.aktiflik == true select p).FirstOrDefault();


			TBL_ISLER yeniIs = new TBL_ISLER();
			yeniIs.isBaslik = isBaslik;
			yeniIs.isAciklama = isAciklama;
			yeniIs.isPersonelID = secilenPersonelID;
			yeniIs.iletilenTarih = DateTime.Now;
			yeniIs.isDurumID = 3;
			yeniIs.isCreationDate = DateTime.Now;
			yeniIs.isAktiflik = true;

			entity.TBL_ISLER.Add(yeniIs);
			entity.SaveChanges();

			TempData["bilgi"] = isBaslik + " - " + personel.personelAdSoyad;

			return RedirectToAction("Ata", "Yonetici");
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
			return RedirectToAction("Listele", "Yonetici", new { id = selectPer });
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
					//ViewBag.tumIsler = tumIsler;
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

		[HttpPost, ActFilter("İş Silindi.")]
		public ActionResult IptalEt(int id)
		{
			var isItem = entity.TBL_ISLER.FirstOrDefault(i => i.isID == id);
			if (isItem != null)
			{
				isItem.isAktiflik = false;
				isItem.isDurumID = 5;
				isItem.isDeletionDate = DateTime.Now;
				entity.SaveChanges();
			}

			TempData["bilgi"] = isItem.isBaslik;

			return RedirectToAction("Ata"); // İşlemi bitirdikten sonra geri yönlendirilecek sayfa
		}


		public ActionResult IzinTalepListesi()
		{
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			var izinler = (from i in entity.TBL_IZINLER
						   join p in entity.TBL_PERSONELLER on i.izinPersonelID equals p.personelID
						   where i.izinRed == false &&
						   i.izinOnay == false &&
						   i.izinTalepTarihi < i.izinBaslangicTarihi
						   && p.personelBirimID == birimID
						   orderby i.izinTalepTarihi descending
						   select i).ToList();

			return View(izinler);
		}

		public ActionResult IzniKabulEt(int izinID)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var izinler = (from i in entity.TBL_IZINLER
						   where i.izinID == izinID
						   select i).FirstOrDefault();

			izinler.izinOnay = true;
			izinler.izinDegerlendirmeTarihi = DateTime.Now;

			var personel = (from p in entity.TBL_PERSONELLER
							where p.personelID == izinler.izinPersonelID
							where p.aktiflik == true select p).FirstOrDefault();

			personel.personelIzinGun = personel.personelIzinGun - izinler.izinGunuSayisi;

			TBL_MAILLER izinMaili = new TBL_MAILLER();
			izinMaili.mailGonderen = personelID;
			izinMaili.mailAlici = personel.personelID;
			izinMaili.mailKonu = personel.personelAdSoyad + " İzin Talebi";
			izinMaili.mailIcerik = "<br>" +
				izinler.izinBaslangicTarihi?.ToString("dd/MM/yyyy") + " - " + izinler.izinBitisTarihi?.ToString("dd/MM/yyyy") + " tarihleri için" + "<br>" + "oluşturduğunuz izin talebiniz " + "<br>" + "<b>ONAYLANMIŞTIR.</b>" + 
				"<br>"+ "Kalan izin günü sayınız: " + personel.personelIzinGun
				+ "<br>" + "<br>" + "<hr>" + "Bu mail otomatik olarak sistem tarafından oluşturulmuştur."
				+ "<br>" + "<br>";
			izinMaili.mailGonderimTarihi = DateTime.Now;
			izinMaili.mailArsiv = false;
			izinMaili.mailAktiflik = true;
			izinMaili.mailOkunma = false;
			entity.TBL_MAILLER.Add(izinMaili);

			entity.SaveChanges();

			return RedirectToAction("IzinTalepListesi");
		}

		public ActionResult IzniReddet(int izinID)
		{

			var izinler = (from i in entity.TBL_IZINLER
						   where i.izinID == izinID
						   select i).FirstOrDefault();
			izinler.izinRed = true;
			izinler.izinDegerlendirmeTarihi = DateTime.Now;

			entity.SaveChanges();
			return RedirectToAction("IzinTalepListesi");
		}

		public ActionResult Izinler()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var yonetici = (from y in entity.TBL_PERSONELLER
							where y.personelID  == personelID
							select y).FirstOrDefault();

			var izinler = (from i in entity.TBL_IZINLER
						   join p in entity.TBL_PERSONELLER on
						   i.izinPersonelID equals p.personelID
						   where yonetici.personelBirimID == p.personelBirimID
						   select i).ToList();

			return View(izinler);
		}

		public ActionResult PartialTakvim()
		{
			return PartialView("_partialTakvim");
		}

		public ActionResult GetApprovedLeaves()
		{
			int birimID = Convert.ToInt32(Session["personelBirimID"]);
			var approvedLeaves = (from i in entity.TBL_IZINLER
								  join p in entity.TBL_PERSONELLER on 
								  i.izinPersonelID equals p.personelID
								  where i.izinOnay == true &&
								  p.personelBirimID == birimID
								  select new
								  {
									  title = p.personelAdSoyad,
									  start = i.izinBaslangicTarihi,
									  end = i.izinBitisTarihi

								  }).ToList();
			var formattedLeaves = approvedLeaves.Select(leave => new
			{
				title = leave.title,
				start = leave.start?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
				end = leave.end?.ToString("yyyy-MM-ddTHH:mm:ssZ")
			}).ToList();
			return Json(formattedLeaves, JsonRequestBehavior.AllowGet);
		}

	}
}