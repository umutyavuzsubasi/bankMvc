using BankaOtomasyon.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankaOtomasyon.ViewModels
{
    public class HesapKontrolViewModel
    {
        public Hesap Hesap { get; set; }
        public IEnumerable<Kullanici> Kullanici { get; set; }
        public Bakiye Bakiye { get; set; }

        public decimal GonderilecekPara { get; set; }
        public int GonderilecekId { get; set; }
        public HesapHareketleri HesapHareketleri { get; set; }
        public IEnumerable<HareketTipleri> HareketTipleri { get; set; }
        public int HareketTipId { get; set; }



    }
}