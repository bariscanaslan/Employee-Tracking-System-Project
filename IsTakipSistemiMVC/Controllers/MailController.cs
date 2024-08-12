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
	public class MailController : Controller
	{
		isTakipDBEntities entity = new isTakipDBEntities();
		// GET: Mail
		public ActionResult Index()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var mailler = (from m in entity.TBL_MAILLER
						   where m.mailAktiflik == true &&
						   m.mailAlici == personelID &&
						   m.mailArsiv == false &&
						   m.mailCopKutusu == false
						   orderby m.mailGonderimTarihi descending
						   select m).ToList();

			var personeller = entity.TBL_PERSONELLER.ToList();
			var birimler = entity.TBL_BIRIMLER.ToList();

			if (mailler.Count() == 0)
			{
				ViewBag.mesaj = "Gelen kutunuz boştur!";
			}

			var viewModel = new MailViewModel
			{
				Mailler = mailler,
				Personeller = personeller,
				Birimler = birimler
			};

			return View(viewModel);
		}

		public ActionResult Giden()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var mailler = (from m in entity.TBL_MAILLER
						   where m.mailAktiflik == true &&
						   m.mailGonderen == personelID &&
						   m.mailArsiv == false &&
						   m.mailCopKutusu == false
						   orderby m.mailGonderimTarihi descending
						   select m).ToList();

			var personeller = entity.TBL_PERSONELLER.ToList();
			var birimler = entity.TBL_BIRIMLER.ToList();

			if (mailler.Count() == 0)
			{
				ViewBag.mesaj = "Giden kutunuz boştur!";
			}

			var viewModel = new MailViewModel
			{
				Mailler = mailler,
				Personeller = personeller,
				Birimler = birimler
			};

			return View(viewModel);
		}

		public ActionResult Arsiv()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var mailler = (from m in entity.TBL_MAILLER
						   where m.mailAktiflik == true &&
						   m.mailAlici == personelID &&
						   m.mailArsiv == true &&
						   m.mailCopKutusu == false
						   orderby m.mailGonderimTarihi descending
						   select m).ToList();

			var personeller = entity.TBL_PERSONELLER.ToList();
			var birimler = entity.TBL_BIRIMLER.ToList();

			if (mailler.Count() == 0)
			{
				ViewBag.mesaj = "Arşiv kutunuz boştur!";
			}

			var viewModel = new MailViewModel
			{
				Mailler = mailler,
				Personeller = personeller,
				Birimler = birimler
			};

			return View(viewModel);
		}

		public ActionResult CopKutusu()
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			var mailler = (from m in entity.TBL_MAILLER
						   where m.mailAktiflik == true &&
						   m.mailAlici == personelID &&
						   m.mailCopKutusu == true
						   orderby m.mailGonderimTarihi descending
						   select m).ToList();

			var personeller = entity.TBL_PERSONELLER.ToList();
			var birimler = entity.TBL_BIRIMLER.ToList();

			if (mailler.Count() == 0)
			{
				ViewBag.mesaj = "Çöp kutunuz boştur!";
			}

			var viewModel = new MailViewModel
			{
				Mailler = mailler,
				Personeller = personeller,
				Birimler = birimler
			};

			return View(viewModel);
		}

		public ActionResult Oku(int id)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			MailViewModel mailViewModel = null;

			var mail = (from m in entity.TBL_MAILLER where m.mailID == id && m.mailAktiflik == true select m).FirstOrDefault();
			var personeller = (from p in entity.TBL_PERSONELLER where p.aktiflik == true select p).ToList();
			var birimler = (from b in entity.TBL_BIRIMLER where b.aktiflik == true select b).ToList();

			if (mail == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				if (mail.mailGonderen != personelID)
				{
					mail.mailOkunma = true;
					entity.SaveChanges();
				}

				mailViewModel = new MailViewModel
				{
					Mailler = new List<TBL_MAILLER> { mail }, // Tek bir duyuruyu listeye alıyoruz
					Personeller = personeller,
					Birimler = birimler
				};

				return View(mailViewModel);
			}
		}


		public ActionResult Gonder()
		{
			var personeller = (from p in entity.TBL_PERSONELLER
							   where p.aktiflik == true
							   select p).ToList();

			var model = new PersonelViewModel
			{
				Personeller = personeller
			};

			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Gonder(string personelIDs, string konu, string icerik)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			List<int> personelIdList = new List<int>();

			if (!string.IsNullOrEmpty(personelIDs))
			{
				personelIdList = personelIDs.Split(',')
											 .Select(id => int.TryParse(id, out var result) ? result : (int?)null)
											 .Where(id => id.HasValue)
											 .Select(id => id.Value)
											 .ToList();
			}

			if (personelIdList != null || personelIdList.Count() != 0)
			{
				for (int i = 0; i < personelIdList.Count(); i++)
				{
					TBL_MAILLER yeniMail = new TBL_MAILLER();
					yeniMail.mailAlici = personelIdList[i];
					yeniMail.mailGonderen = personelID;
					yeniMail.mailGonderimTarihi = DateTime.Now;
					yeniMail.mailOkunma = false;
					yeniMail.mailArsiv = false;
					yeniMail.mailAktiflik = true;
					yeniMail.mailCopKutusu = false;
					yeniMail.mailKonu = konu;
					yeniMail.mailIcerik = icerik;
					entity.TBL_MAILLER.Add(yeniMail);
					entity.SaveChanges();
				}
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult TopluIslem(List<int> selectedMails, string islem, string returnUrl)
		{
			if (islem == "gonderAl")
			{
				return RedirectToAction("Index", "Mail");
			}
			else if (islem == "yeniPosta")
			{
				return RedirectToAction("Gonder", "Mail");
			}
			else if (islem == "arsiveGit")
			{
				return RedirectToAction("Arsiv", "Mail");
			}
			else if (islem == "gideneGit")
			{
				return RedirectToAction("Giden", "Mail");
			}
			else if(islem == "copeGit")	
			{
				return RedirectToAction("CopKutusu", "Mail");
			}

			if (selectedMails == null || !selectedMails.Any())
			{
				ViewBag.mesaj = "Hiçbir mail seçilmedi!";
				return RedirectToAction(returnUrl);
			}

			foreach (var mailId in selectedMails)
			{
				var mail = entity.TBL_MAILLER.Find(mailId);

				if (mail != null)
				{
					switch (islem)
					{
						case "okunduOkunmadi":
							if (mail.mailOkunma == true)
							{
								mail.mailOkunma = false;
							}
							else
							{
								mail.mailOkunma = true;
							}
							break;
						case "arsivle":
							mail.mailArsiv = true;
							mail.mailOkunma = true;
							break;

						case "sil":
							mail.mailCopKutusu = true;
							mail.mailArsiv = false;
							mail.mailOkunma = true;
							break;

						case "coptenCikar":
							mail.mailCopKutusu = false;
							mail.mailArsiv = false;
							break;
						
						case "arsivdenCikar":
							mail.mailArsiv = false;
							break;
						
						case "dbdenSil":
							mail.mailAktiflik = false;
							mail.mailDeletionDate = DateTime.Now;
							break;

					}
				}
			}

			if (returnUrl == "/Mail/Arsiv")
			{
				returnUrl = "Arsiv";
			}
			else if (returnUrl == "/Mail/Giden")
			{
				returnUrl = "Giden";
			}
			else if(returnUrl == "/Mail/CopKutusu")
			{
				returnUrl = "CopKutusu";
			}
			else if (returnUrl == "/Mail")
			{
				returnUrl = "";
			}

			entity.SaveChanges();
			return RedirectToAction(returnUrl);
		}

		public ActionResult Yanitla(int? id)
		{
			int personelID = Convert.ToInt32(Session["personelID"]);

			MailViewModel mailViewModel = null;

			var mail = (from m in entity.TBL_MAILLER where m.mailID == id && m.mailAktiflik == true select m).FirstOrDefault();
			var personeller = (from p in entity.TBL_PERSONELLER where p.aktiflik == true select p).ToList();
			var birimler = (from b in entity.TBL_BIRIMLER where b.aktiflik == true select b).ToList();

			if (mail.mailGonderen == personelID)
			{
				return RedirectToAction("Index");
			}
			else
			{
				if (mail == null)
				{
					return RedirectToAction("Index");
				}
				else
				{
					mailViewModel = new MailViewModel
					{
						Mailler = new List<TBL_MAILLER> { mail },
						Personeller = personeller,
						Birimler = birimler
					};
				}

				var gonderen = personeller.FirstOrDefault(p => p.personelID == mail.mailGonderen);
				var gonderenAdSoyad = gonderen != null ? gonderen.personelAdSoyad : "Bilinmiyor";

				var yanitlaViewModel = new YanitlaViewModel
				{
					OkuModel = mailViewModel,
					GonderenAdSoyad = gonderenAdSoyad,
					GonderenID = mail.mailGonderen,
				};

				return View(yanitlaViewModel);
			}
		}
	}
}