using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using OfficeOpenXml.Table.PivotTable;

namespace IsTakipSistemiMVC.Controllers
{
	public class LoginController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();
		// GET: Login
		public ActionResult Index()
		{
			ViewBag.mesaj = null;
			return View();
		}

		[HttpPost, ExcFilter,
			ActFilter("Giriş Yapıldı.")]
		public ActionResult Index(string kullaniciAd, string sifre)
		{

			var personel = (from p in entity.TBL_PERSONELLER
							where p.personelKullaniciAdi == kullaniciAd
							&& p.personelParola == sifre
							&& p.aktiflik == true
							select p).FirstOrDefault();

			if (personel != null)
			{
				var yetki = (from y in entity.TBL_YETKITURLER
							 where y.yetkiTurID == personel.personelYetkiTurID
							 select y).FirstOrDefault();

				if (yetki.yetkiTurAktiflik == true)
				{
					var birim = (from b in entity.TBL_BIRIMLER
								 where b.birimID == personel.personelBirimID
								 select b).FirstOrDefault();

					Session["personelAdSoyad"] = personel.personelAdSoyad;
					Session["personelID"] = personel.personelID;
					Session["personelBirimID"] = personel.personelBirimID;
					Session["personelYetkiTurID"] = personel.personelYetkiTurID;
					Session["personelFoto"] = personel.personelFoto;
					if (yetki.yetkiTurID != 3)
					{
						Session["personelBirimAd"] = birim.birimAd;
					}
					Session["personelYetkiTurAd"] = yetki.yetkiTurAd;

					if (birim == null)
					{
						return RedirectToAction("Index", "Admin");
					}
					if (birim.aktiflik == true)
					{
						DateTime bugun = DateTime.Today;
						if(bugun.Day == 1 && bugun.Month == 1)
						{
							var personeller = (from p in entity.TBL_PERSONELLER where p.aktiflik == true && p.personelYetkiTurID == 2 select p).ToList();
							foreach (var p in personeller)
							{
								p.personelIzinGun = 26;
							}
							entity.SaveChanges();
						}


						// Eğer personel yeni eklenmişse
						if (personel.yeniPersonel == true)
						{
							return RedirectToAction("Index", "SifreKontrol");
						}

						switch (personel.personelYetkiTurID)
						{
							// personelYetkiTurID 1 ise YÖNETİCİ
							// personelYetkiTurID 2 ise ÇALIŞAN
							// personelYetkiTurID 3 ise ADMIN
							// Sadece ADMIN'in birim ID'si olmadığı için üstte Admin Controller'ına yönlendirildi
							case 1:
								return RedirectToAction("Index", "Yonetici");
							case 2:
								return RedirectToAction("Index", "Calisan");
							default:
								return View();
						}
					}
					// Eğer birim artık kullanılmıyorsa
					else
					{
						ViewBag.mesaj = "Bu birim artık kullanılmamaktadır! " +
							"Hata olduğunu düşünüyorsanız lütfen sistem yöneticinize danışın.";
						return View();
					}
				}

				else
				{
					ViewBag.mesaj = "Bu yetki türü artık kullanılmamaktadır! " +
							"Hata olduğunu düşünüyorsanız lütfen sistem yöneticinize danışın.";
					return View();
				}

			}

			// personelKullaniciAdi ve personelParola değerleriyle bir personel yoksa
			else
			{
				ViewBag.mesaj = "Kullanıcı adı ya da şifre hatalı!";
				return View();
			}
		}


		public ActionResult SifremiUnuttum()
		{
			ViewBag.mesaj = null;
			return View();
		}

		[HttpPost, ExcFilter]
		public ActionResult SifremiUnuttum(string kullaniciAd)
		{
			var sifreUnutan = (from p in entity.TBL_PERSONELLER 
							   where p.personelKullaniciAdi == kullaniciAd
							   && p.aktiflik == true select p).FirstOrDefault();

			if(sifreUnutan == null)
			{
				ViewBag.mesaj1 = "Girdiğiniz kullanıcı adı sistemde bulunmamaktadır!";
			}

			else
			{
				TBL_MAILLER sifreMaili = new TBL_MAILLER();

				var admin = (from p in entity.TBL_PERSONELLER where p.personelYetkiTurID == 3 && p.aktiflik == true select p).FirstOrDefault();
				// yetkiTurID'si 3 olan admindir

				sifreMaili.mailGonderen = sifreUnutan.personelID;
				sifreMaili.mailAlici = admin.personelID;
				sifreMaili.mailKonu = sifreUnutan.personelAdSoyad + " Şifre Talebi";
				
				sifreMaili.mailIcerik = "<br>" +
				sifreUnutan.personelAdSoyad + " isimli personel şifre talebi göndermiştir."
				+ "<br> Personelin şifresi: " + sifreUnutan.personelParola + "<br>" +
				"Kontrolü sağlayıp personele dönüş yapmanız rica olunur."
				+ "<br>" + "<br>" + "<hr>" + "Bu mail otomatik olarak sistem tarafından oluşturulmuştur."
				+ "<br>" + "<br>";
				sifreMaili.mailOkunma = false;
				sifreMaili.mailGonderimTarihi = DateTime.Now;
				sifreMaili.mailArsiv = false;
				sifreMaili.mailAktiflik = true;
				sifreMaili.mailCopKutusu = false;

				entity.TBL_MAILLER.Add(sifreMaili);

				entity.SaveChanges();

				TempData["bilgi"] = kullaniciAd;

				ViewBag.mesaj2 = "Talebiniz gönderilmiştir. Sistem yöneticisi en kısa zamanda size dönüş sağlayacaktır." +
					"Giriş sayfasına yönlendiriliyorsunuz...";
				ViewBag.redirect = true;
			}

			return View();
		}

	}
}