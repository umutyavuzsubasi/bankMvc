using BankaOtomasyon.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankaOtomasyon.ViewModels
{
    public class KullaniciFormViewModel
    {
        public IEnumerable<KullaniciTipleri> KullaniciTipleri { get; set; }
        
        public Kullanici Kullanici { get; set; }

        public IEnumerable<HesapHareketleri> HesapHareketleri { get; set; }
        
        public IEnumerable<HareketTipleri> HareketTipleri { get; set; }

        public IEnumerable<Bakiye> Bakiye { get; set; }

        public Hesap Hesap { get; set; }

    }
}