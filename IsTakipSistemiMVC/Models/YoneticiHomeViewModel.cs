using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class YoneticiHomeViewModel
	{
		public List<Personel> Personeller { get; set; }
	}

	public class Personel
	{
		public string ModelPersonelAd { get; set; }
		public string ModelPersonelFoto { get; set; }
		public int ModelPersonelID { get; set; }
	}
}