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


namespace BankaOtomasyon.Controllers
{
    [Authorize]
    public class ProfilimController : Controller
    {
        // GET: Profilim
        public ActionResult Index()
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = db.Kullanicis.Include(x => x.KullaniciTipleri).Where( x => x.TcKimlik  == this.HttpContext.User.Identity.Name).ToList();
                return View(model);
            }
        }
        public ActionResult Profilim(Kullanici Kullanici)
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
        public ActionResult Guncelle(int id)
        {
            using (BankaEntities db = new BankaEntities())
            {
                var model = new KullaniciFormViewModel()
                {
                    KullaniciTipleri = db.KullaniciTipleris.ToList(),
                    Kullanici = db.Kullanicis.Find(id)
                };
                return View("Guncelle", model);
            }

        }
    }
}