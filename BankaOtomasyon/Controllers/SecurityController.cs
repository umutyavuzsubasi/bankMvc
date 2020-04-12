using BankaOtomasyon.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankaOtomasyon.Controllers
{

    public class SecurityController : Controller
    {
        BankaEntities db = new BankaEntities();
        // GET: Security
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Kullanici Kullanici)
        {
            var KullaniciDb = db.Kullanicis.FirstOrDefault(x=>x.TcKimlik == Kullanici.TcKimlik && x.Sifre == Kullanici.Sifre);
            if (KullaniciDb != null)
            {
                FormsAuthentication.SetAuthCookie(KullaniciDb.TcKimlik,false);
                return RedirectToAction("Index","Hesap");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz Tc Kimlik veya Geçersiz Şifre";
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}