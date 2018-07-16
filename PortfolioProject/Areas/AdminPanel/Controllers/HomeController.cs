using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PortfolioProject.Models;

namespace PortfolioProject.Areas.AdminPanel.Controllers
{
    [FilterAdmin]
    public class HomeController : Controller
    {
        // GET: AdminPanel/Home

        PortfolioDbEntities db = new PortfolioDbEntities();

        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            string username = frm["a_username"];
            string password = frm["a_password"];
            var ad = db.Admins.Where(e => e.admin_username == username && e.admin_password == password).FirstOrDefault();
            if (ad != null)
            {
                var admin = db.Admins.First(p => p.admin_username == username && p.admin_password == password);
                Session["username"] = ad.admin_username;
                Session["id"] = ad.id;

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Yanlış username və ya şifrə!";
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection frm)
        {
            Admin admn = db.Admins.Find(1);

            string pass = admn.admin_password;
            string oldPass = frm["old_password"];
            string newPass = frm["new_password"];
            string confPass = frm["conf_password"];
            if (oldPass == pass && confPass == newPass)
            {
                admn.admin_password = newPass;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Yanlis şifrə!";
                return View();
            }

        }
        [AllowAnonymous]
        public ActionResult Forgot()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Forgot(string a_email)
        {

            if (a_email == "narminrn@code.edu.az")
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Admin admn = db.Admins.Find(1);

                        string passw = admn.admin_password;
                        MailMessage mailmessage = new MailMessage();
                        SmtpClient connect = new SmtpClient("smpt.gmail.com", 587);
                        mailmessage.From = new MailAddress("nasirovanarmin96@gmail.com", "Admin", System.Text.Encoding.UTF8);
                        mailmessage.To.Add(a_email);
                        mailmessage.Subject = "Şifrəniz";
                        Random random = new Random();
                        mailmessage.Body = passw;
                        connect.Credentials = new NetworkCredential("nasirovanarmin96@gmail.com", "yataqxana123");
                        connect.EnableSsl = true;
                        connect.Host = "smtp.gmail.com";
                        connect.Send(mailmessage);
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Some Error";
                }
                return View();
            }
            else
            {
                ViewBag.Error = "Some Error";
                return View();
            }

        }
    }
}