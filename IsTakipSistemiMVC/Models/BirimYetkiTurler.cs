using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakipSistemiMVC.Models
{
	public class BirimYetkiTurler
	{
        public List<TBL_BIRIMLER> birimlerList { get; set; }
        public List<TBL_YETKITURLER> yetkiTurlerList { get; set; }
    }
}