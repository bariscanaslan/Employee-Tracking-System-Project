using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class MailDropdownModel
	{
		public int MailID { get; set; }
		public string PersonelAdSoyad { get; set; }
		public string PersonelFoto { get; set; }
		public string Konu { get; set; }
		public DateTime? GonderimTarihi { get; set; }
	}
}