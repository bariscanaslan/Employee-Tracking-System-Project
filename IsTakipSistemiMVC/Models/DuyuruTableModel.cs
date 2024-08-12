using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class DuyuruTableModel
	{
		public IEnumerable<TBL_DUYURULAR> Duyurular { get; set; }
		public IEnumerable<TBL_PERSONELLER> Personeller { get; set; }
		public IEnumerable<TBL_BIRIMLER> Birimler { get; set; }

	}
}