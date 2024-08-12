using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
	public class DuyuruDropdownController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();
		// GET: DuyuruDropdown
		public ActionResult PartialDuyurularDropdown()
		{
			int yetkiTurID = Convert.ToInt32(Session["personelYetkiTurID"]);

			int personelID = Convert.ToInt32(Session["personelID"]);

			var log = (from l in entity.TBL_LOGLAR where l.personelID == personelID select l).ToList();
			
			if(yetkiTurID != 3)
			{
				int birimID = Convert.ToInt32(Session["personelBirimID"]);

				var duyurular = (from d in entity.TBL_DUYURULAR
							 where d.duyuruAktiflik == true &&
							 (d.duyuruOlusturanBirim == birimID || d.duyuruOlusturanBirim == null)
							 orderby d.duyuruID descending
							 select new DuyuruDropdownModel
							 {
								 DuyuruID = d.duyuruID,
								 DuyuruBaslik = d.duyuruBaslik,
								 YayinlanmaTarihi = d.duyuruTarih
							 })
							 .Take(3)
							 .ToList();

				return PartialView("_partialDuyurularDropdown", duyurular);
			}
			else
			{
				var duyurular = (from d in entity.TBL_DUYURULAR
								 where d.duyuruAktiflik == true
								 orderby d.duyuruID descending
								 select new DuyuruDropdownModel
								 {
									 DuyuruID = d.duyuruID,
									 DuyuruBaslik = d.duyuruBaslik,
									 YayinlanmaTarihi = d.duyuruTarih
								 })
							 .Take(3)
							 .ToList();

				return PartialView("_partialDuyurularDropdown", duyurular);
			}
		}
	}
}