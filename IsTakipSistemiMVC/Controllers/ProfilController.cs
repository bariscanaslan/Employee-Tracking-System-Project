using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	[LayoutActionFilter]
	public class ProfilController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();

		public void ProfiliGetir(int personelID, int yetkiTurID)
		{
			var personel = (from p in entity.TBL_PERSONELLER
							where personelID == p.personelID
							select p).FirstOrDefault();
			var birim = (from b in entity.TBL_BIRIMLER
						 where b.birimID == personel.personelBirimID
						 select b).FirstOrDefault();
			var yetki = (from y in entity.TBL_YETKITURLER
						 where y.yetkiTurID == yetkiTurID
						 select y).FirstOrDefault();

			if (birim != null)
			{
				ViewBag.pBirimAd = birim.birimAd;
			}
			else
			{
				ViewBag.pBirimAd = "-";
			}

			string parola = personel.personelParola;
			int parolaUzunluk = parola.Length;
			string parolaYildizStr = "";
			parolaYildizStr += parola[0];
			for (int i = 1; i < parolaUzunluk; i++)
			{
				parolaYildizStr += "*";
			}

			ViewBag.pYetkiTurAd = string.IsNullOrEmpty(yetki.yetkiTurAd) ? "-" : yetki.yetkiTurAd;
			ViewBag.pAdSoyad = string.IsNullOrEmpty(personel.personelAdSoyad) ? "-" : personel.personelAdSoyad;
			ViewBag.pKullaniciAdi = string.IsNullOrEmpty(personel.personelKullaniciAdi) ? "-" : personel.personelKullaniciAdi;
			ViewBag.pParola = string.IsNullOrEmpty(parolaYildizStr) ? "-" : parolaYildizStr;
			ViewBag.pFoto = string.IsNullOrEmpty(personel.personelFoto) ? "-" : personel.personelFoto;
			ViewBag.pDogumTarihi = (personel.personelDogumTarihi)?.ToString("dd.MM.yyyy");
			ViewBag.pTelefonNumara = string.IsNullOrEmpty(personel.telefonNumarasi) ? "-" : personel.telefonNumarasi;
			ViewBag.pMailAdresi = string.IsNullOrEmpty(personel.mailAdresi) ? "-" : personel.mailAdresi;
			ViewBag.pGirisTarihi = personel.personelCreationDate?.ToString("dd.MM.yyyy");

		}

		public ActionResult Index()
		{
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);
			int personelID = Convert.ToInt32(Session["personelID"]);
			ProfiliGetir(personelID, yetkiTurID);
			return View();
		}
	}
}