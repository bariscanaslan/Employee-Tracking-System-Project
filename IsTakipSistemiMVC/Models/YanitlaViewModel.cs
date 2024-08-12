using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class YanitlaViewModel
	{
		public MailViewModel OkuModel { get; set; }
		public string GonderenAdSoyad { get; set; }
		public int? GonderenID { get; set; }


	}
}