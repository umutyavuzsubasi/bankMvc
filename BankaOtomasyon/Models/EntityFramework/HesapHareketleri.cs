//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankaOtomasyon.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class HesapHareketleri
    {
        public int Id { get; set; }
        public int TipiId { get; set; }
        public decimal Tutari { get; set; }
        public string Aciklama { get; set; }
        public int KaynakID { get; set; }
        public System.DateTime Tarih { get; set; }
        public Nullable<int> HedefHesapId { get; set; }
        public int HareketSayac { get; set; }
    
        public virtual HareketTipleri HareketTipleri { get; set; }
        public virtual Hesap Hesap { get; set; }
    }
}