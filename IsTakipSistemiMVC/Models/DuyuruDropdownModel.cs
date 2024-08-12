using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class DuyuruDropdownModel
	{
		public int DuyuruID { get; set; }
		public string DuyuruBaslik { get; set; }
		public DateTime? YayinlanmaTarihi { get; set; }

		public string FormattedYayinlanmaTarihi
		{
			get
			{
				return YayinlanmaTarihi?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
			}
		}
	}
}