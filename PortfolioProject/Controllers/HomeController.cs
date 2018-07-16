using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using PortfolioProject.Models;
using PortfolioProject.ViewModel;
namespace PortfolioProject.Controllers
{
   
    public class HomeController : Controller
    {
        HttpCookie cookie = new HttpCookie("Language");

        PortfolioDbEntities db = new PortfolioDbEntities();
        IndexModel im = new IndexModel();
        AboutModel am = new AboutModel();
        ContactModel cm = new ContactModel();
        ProjectModel pm = new ProjectModel();
        WorkshopModel wm = new WorkshopModel();
        PressModel psm = new PressModel();
        SinglePressModel sprm = new SinglePressModel();
        SingleProjectModel spm = new SingleProjectModel();
        SingleWorkshopModel swm = new SingleWorkshopModel();

        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l=>l.language1==cookie.Value).First().id; 
            im._slider = db.Sliders.ToList();
            im._first_img = db.Sliders.Take(1).FirstOrDefault();
            im._anime = db.AnimeImages.ToList();
            im._project = db.Projects.ToList();
            im._workshop = db.Workshops.ToList();
            im._about = db.Abouts.ToList();
            im._press = db.Presses.ToList();
            im._contact = db.Contacts.ToList();
            foreach (var item in im._slider)
            {
                item.list = db.FindWords(lang_id, item.slider_keyName).FirstOrDefault();
            }
            foreach (var item in im._project)
            {
                item.list = db.FindWords(lang_id, item.project_keyName).FirstOrDefault();
            }
            foreach (var item in im._workshop)
            {
                item.list = db.FindWords(lang_id, item.workshop_keyName).FirstOrDefault();
            }
            foreach (var item in im._about)
            {
                item.list = db.FindWords(lang_id, item.about_keyName).FirstOrDefault();
            }
            foreach (var item in im._press)
            {
                item.list = db.FindWords(lang_id, item.press_keyName).FirstOrDefault();
            }
            foreach (var item in im._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(im);
            
        }

        public ActionResult About()
        {

            HttpCookie cookie = Request.Cookies["Language"];

            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            am._about = db.Abouts.ToList();
            am._about_img = db.AboutImages.ToList();
            am._contact = db.Contacts.ToList();
            foreach (var item in am._about)
            {
                item.list = db.FindWords(lang_id, item.about_keyName).FirstOrDefault();
            }
            foreach (var item in am._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(am);
        }

        public ActionResult Contact()
        {
            HttpCookie cookie = Request.Cookies["Language"];
             int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            cm._contact = db.Contacts.ToList();
            foreach (var item in cm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }

            return View(cm);
        }
        [HttpPost]
        public ActionResult Contact(string name,string email,string subject,string message)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    string receiver = "narminrn@code.edu.az";
                    MailMessage mailmessage = new MailMessage();
                    SmtpClient connect = new SmtpClient("smpt.gmail.com", 587);
                    mailmessage.From = new MailAddress("nasirovanarmin96@gmail.com", name, System.Text.Encoding.UTF8);
                    mailmessage.To.Add(receiver);
                    mailmessage.Subject = subject;
                    Random random = new Random();
                    mailmessage.Body = message+"\r\n"+email;
                    connect.Credentials = new NetworkCredential("nasirovanarmin96@gmail.com", "yataqxana123");
                    connect.EnableSsl = true;
                    connect.Host = "smtp.gmail.com";
                    connect.Send(mailmessage);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Message(string user_name,string user_message)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string receiver = "narminrn@code.edu.az";
                    MailMessage mailmessage = new MailMessage();
                    SmtpClient connect = new SmtpClient("smpt.gmail.com", 587);
                    mailmessage.From = new MailAddress("nasirovanarmin96@gmail.com", user_name, System.Text.Encoding.UTF8);
                    mailmessage.To.Add(receiver);
                    mailmessage.Subject = "Teklif ve ya irad";
                    Random random = new Random();
                    mailmessage.Body = user_message;
                    connect.Credentials = new NetworkCredential("nasirovanarmin96@gmail.com", "yataqxana123");
                    connect.EnableSsl = true;
                    connect.Host = "smtp.gmail.com";
                    connect.Send(mailmessage);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("Index");
        }
        public ActionResult Projects()
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            pm._project = db.Projects.ToList();
            pm._contact = db.Contacts.ToList();
            foreach (var item in pm._project)
            {
                item.list = db.FindWords(lang_id, item.project_keyName).FirstOrDefault();
            }
            foreach (var item in pm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(pm);
        }

        public ActionResult Workshops()
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            wm._workshop = db.Workshops.ToList();
            wm._contact = db.Contacts.ToList();
            foreach (var item in wm._workshop)
            {
                item.list = db.FindWords(lang_id, item.workshop_keyName).FirstOrDefault();
            }
            foreach (var item in wm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(wm);
        }
        public ActionResult Press()
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            psm._press = db.Presses.ToList();
            psm._contact = db.Contacts.ToList();
            foreach (var item in psm._press)
            {
                item.list = db.FindWords(lang_id, item.press_keyName).FirstOrDefault();
            }
            foreach (var item in psm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(psm);
        }
        public ActionResult SinglePress(int id)
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            sprm._single_press = db.Presses.Where(s => s.id == id).FirstOrDefault();
            sprm._press = db.Presses.ToList();
            sprm._contact = db.Contacts.ToList();
            foreach (var item in sprm._press)
            {
                item.list = db.FindWords(lang_id, item.press_keyName).FirstOrDefault();
            }
            foreach (var item in sprm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(sprm);
        }
        public ActionResult SingleProject(int id)
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            spm._single_project = db.Projects.Where(p => p.id == id).FirstOrDefault();
            spm._project = db.Projects.ToList();
            spm._project_img = db.ProjectImages.Where(i => i.project_id == id).ToList();
            spm._contact = db.Contacts.ToList();
            foreach (var item in spm._project)
            {
                item.list = db.FindWords(lang_id, item.project_keyName).FirstOrDefault();
            }
            foreach (var item in spm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(spm);
        }
        public ActionResult SingleWorkshop(int id)
        {
            HttpCookie cookie = Request.Cookies["Language"];
            int lang_id = db.Languages.Where(l => l.language1 == cookie.Value).First().id;
            swm._single_workshop = db.Workshops.Where(w => w.id == id).FirstOrDefault();
            swm._workshop = db.Workshops.ToList();
            swm._workshop_img = db.WorkshopImages.Where(i => i.workshop_id == id).ToList();
            swm._contact = db.Contacts.ToList();
            foreach (var item in swm._workshop)
            {
                item.list = db.FindWords(lang_id, item.workshop_keyName).FirstOrDefault();
            }
            foreach (var item in swm._contact)
            {
                item.list = db.FindWords(lang_id, item.contact_keyName).FirstOrDefault();
            }
            return View(swm);
        }
        public ActionResult Change(string LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            string url = this.Request.UrlReferrer.AbsolutePath;

            return Redirect(url);
        }
    }
}