using BankaOtomasyon.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankaOtomasyon.ViewModels;
using System.Data.Entity;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;

namespace BankaOtomasyon.Controllers
{
    public class HesapController : Controller
    {
        [Authorize]
        // GET: Hesap
        public ActionResult Index()
        {
            using (BankaEntities db = new BankaEntities())
            {
                Kullanici k = db.Kullanicis.Where(x => x.TcKimlik == this.HttpContext.User.Identity.Name).FirstOrDefault();

                var model = db.Hesaps.Include(x => x.Bakiyes).Where(m => m.KullaniciId == k.Id).ToList();
                return View("Hesap", model);
            }
        }

        [Authorize(Roles = "1,2,3")]
        public ActionResult Hesap()
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = db.Hesaps.Include(x => x.Bakiyes).Where(x => x.KullaniciId == Convert.ToInt32(this.HttpContext.User.Identity.Name)).ToList();
                return View(model);
            }
        }
        public ActionResult HesapHareket(int id)
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = db.Hesaps.Include(x => x.HesapHareketleris).ToList();
                return View(model);
            }
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult Yeni()
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = new HesapKontrolViewModel()
                {
                    Kullanici = db.Kullanicis.ToList()
                };
                return View("Yeni", model);
            }
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult Kayit(Hesap Hesap)
        {
            using (BankaEntities db = new BankaEntities())
            {
                if (Hesap.Id == 0)//ekleme
                {
                    db.Hesaps.Add(Hesap);
                }
                else //güncelle
                {
                    db.Entry(Hesap).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "1,2")]
        public ActionResult Guncelle(int id)
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = new HesapKontrolViewModel()
                {
                    Kullanici = db.Kullanicis.ToList(),
                    Hesap = db.Hesaps.Find(id)
                };
                return View("Guncelle", model);
            }
        }
        [HttpGet]
        public ActionResult Gonder()
        {
            using (BankaEntities db = new BankaEntities())
            {
                HesapKontrolViewModel Gonderilicek = new HesapKontrolViewModel();

                Gonderilicek.HareketTipleri = db.HareketTipleris.ToList();
               
                return View("Gonder", Gonderilicek);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1,2,3")]
        public ActionResult Gonder(HesapKontrolViewModel model)
        {
            using (BankaEntities db = new BankaEntities())
            {

                #region kaynak hesaptan para çekiliyor
                decimal YeniBakiye;
                Hesap YeniHesap = new Hesap();
                YeniHesap = db.Hesaps.Find(model.Hesap.Id);
                YeniBakiye = Convert.ToDecimal(YeniHesap.Bakiye) - model.Hesap.GonderilicekPara;
                if (YeniBakiye < 0)
                {
                    return Content("Paran Yok Hacıı");
                }
                YeniHesap.Bakiye = YeniBakiye;
                #endregion
                #region Hedef Hesaba para transfer ediliyor
                Hesap GonderilicekHesap = db.Hesaps.Find(model.Hesap.GonderilecekId);
                GonderilicekHesap.Bakiye += model.Hesap.GonderilicekPara;
                #endregion
                #region hesap hareket işlemi kayıt ediliyor
                HesapHareketleri YeniHesapHareketi = new HesapHareketleri();

                YeniHesapHareketi.Tarih = DateTime.Now;
                YeniHesapHareketi.KaynakID = (model.Hesap.GonderilecekId);
                YeniHesapHareketi.Tutari = model.Hesap.GonderilicekPara;
                YeniHesapHareketi.TipiId = model.HareketTipId;
                db.HesapHareketleris.Add(YeniHesapHareketi);
                YeniHesapHareketi.KaynakID = (model.Hesap.Id);
                YeniHesapHareketi.HedefHesapId = model.Hesap.GonderilecekId;
                #endregion

                var KullaniciEmail = db.Kullanicis.Find(db.Hesaps.Find(model.Hesap.Id).KullaniciId);

                if (Convert.ToInt32(model.Hesap.GonderilicekPara) >= 1000)
                {
                    #region e mail gönderiliyor
                    var fromAddress = new MailAddress("shufflethemilk@gmail.com", "Çiftlik Bank");
                    var toAddress = new MailAddress(KullaniciEmail.Email,KullaniciEmail.AdSoyad );
                    const string fromPassword = "31634468140umut";
                    const string subject = "Hesap Harekeleri";
                    string body = "Sayın "+ KullaniciEmail.AdSoyad +" "+YeniHesapHareketi.KaynakID +" nolu hesabınızdan " + model.Hesap.GonderilicekPara +" Türk Lirası banka hesabınızdan çekilmiştir." ;
                    
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                        Timeout = 20000
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    #endregion
                }


                db.SaveChanges();
                HesapKontrolViewModel Gonderilicek = new HesapKontrolViewModel();

                Gonderilicek.HareketTipleri = db.HareketTipleris.ToList();
                return RedirectToAction("Index","Hesap");

            }
        }
        [Authorize(Roles = "1,2")]
        public ActionResult Yonetici()
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = db.Hesaps.Include(x => x.HesapHareketleris).ToList();
                return View(model);
            }
        }

        [Authorize(Roles = "1,2,3")]
        public ActionResult Hareketler (int id)
        {
            using (BankaEntities db = new BankaEntities())
            {
                List<HesapHareketleri> h = db.HesapHareketleris.Where(x => x.KaynakID == id).ToList();
                return View(h);
            }

        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult HareketMiktari (int id)
        {
            using (BankaEntities db = new BankaEntities())
            {
                var hesap = db.Hesaps.Where(x => x.KullaniciId==id).Select(x=>x.Id).ToList();
                List<HesapHareketleri> model = db.HesapHareketleris.Where(m=> hesap.Contains(m.KaynakID)).ToList();

                return View("HareketMiktari",model);
            }

        }
    }
}