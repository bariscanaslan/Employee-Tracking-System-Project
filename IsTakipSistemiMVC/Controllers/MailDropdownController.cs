using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	public class MailDropdownController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();
		public ActionResult PartialMailDropdown()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);
			int birimID = Convert.ToInt32(Session["personelBirimID"]);

			var personeller = (from p in entity.TBL_PERSONELLER where p.aktiflik == true select p).ToList();

			var mailler = (from m in entity.TBL_MAILLER
						   join p in entity.TBL_PERSONELLER on m.mailGonderen equals p.personelID
						   where m.mailAktiflik == true &&
						   m.mailAlici == personelID &&
						   m.mailOkunma == false
						   orderby m.mailGonderimTarihi descending
						   select new MailDropdownModel
						   {
							   MailID = m.mailID,
							   Konu = m.mailKonu,
							   GonderimTarihi = m.mailGonderimTarihi,
							   PersonelAdSoyad = p.personelAdSoyad,
							   PersonelFoto = p.personelFoto,
						   }
						   
						   ).Take(3).ToList();

			if (mailler == null)
			{
				ViewBag.mesaj = "Okunmamış mailiniz bulunmamaktadır!";
			}

			return PartialView("_partialMailDropdown", mailler);
		}
	}
}