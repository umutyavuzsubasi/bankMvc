using BankaOtomasyon.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BankaOtomasyon.ViewModels;
using System.Web.Security;

namespace BankaOtomasyon.Controllers
{
    [Authorize]
    public class OtomasyonController : Controller
    {

        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = db.Kullanicis.Include(x => x.KullaniciTipleri).ToList();
                return View(model);
            }
        }
        [Authorize(Roles ="1,2")]
        public ActionResult Yeni()
        {
            using(BankaEntities db = new BankaEntities())
            {
                var model = new KullaniciFormViewModel()
                {
                    KullaniciTipleri = db.KullaniciTipleris.ToList()
                };
                return View("Kayit", model);
            }
        }
        [Authorize(Roles = "1,2")]

        public ActionResult Kayit(Kullanici Kullanici)
        {
            using (BankaEntities db = new BankaEntities())
            {
                if (Kullanici.Id == 0)//ekleme
                {
                    db.Kullanicis.Add(Kullanici);
                }
                else //güncelle
                {
                    db.Entry(Kullanici).State = System.Data.Entity.EntityState.Modified;
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
                var model = new KullaniciFormViewModel()
                {
                    KullaniciTipleri = db.KullaniciTipleris.ToList(),
                    Kullanici = db.Kullanicis.Find(id)
                };
                return View("Kayit", model);
            }    
        }
        [Authorize(Roles ="1")]
        public ActionResult Sil(int id)
        {
            using(BankaEntities db = new BankaEntities())
            {
                    var SilinecekKullanici = db.Kullanicis.Find(id);
                    if (SilinecekKullanici == null)
                    {
                        return HttpNotFound();
                    }
                    db.Kullanicis.Remove(SilinecekKullanici);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}
