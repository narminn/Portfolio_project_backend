using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortfolioProject.Models;

namespace PortfolioProject.Areas.AdminPanel.Controllers
{
    [FilterAdmin]
    public class AboutsController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/Abouts
        public ActionResult Index()
        {
            return View(db.Abouts.ToList());
        }

        // GET: AdminPanel/Abouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // GET: AdminPanel/Abouts/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();

            return View();
        }

        // POST: AdminPanel/Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,about_keyName,about_main_img")] About about,HttpPostedFileBase about_main_img, List<string> content, List<string> langlist, List<HttpPostedFileBase> proj_image)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                int randomKey = random.Next(0, 1000000);
                var filename = Path.GetFileName(about_main_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                about_main_img.SaveAs(src);
                about.about_main_img = "/Uploads/" + randomNumber + filename;
                about.about_keyName = randomKey.ToString();
                db.Abouts.Add(about);
                db.SaveChanges();
                int i = 0;
                foreach (string item in content)
                {
                    Keystring newkey = new Keystring();
                    newkey.keyName = randomKey.ToString();
                    newkey.content = item;
                    int lang = Convert.ToInt16(langlist[i]);
                    newkey.language_id = lang;
                    db.Keystrings.Add(newkey);
                    db.SaveChanges();
                    i++;
                }
                foreach (var item in proj_image)
                {
                    if (item != null)
                    {
                        AboutImage aboutimg = new AboutImage();
                        var imgname = Path.GetFileName(item.FileName);
                        var imgsrc = Path.Combine(Server.MapPath("~/Uploads/"), randomKey + imgname);
                        item.SaveAs(imgsrc);
                        aboutimg.about_img = "/Uploads/" + randomKey + imgname;
                        
                        db.AboutImages.Add(aboutimg);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("error");
                    }

                }
                return RedirectToAction("Index");
            }

            return View(about);
        }

        // GET: AdminPanel/Abouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == about.about_keyName).ToList();

            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: AdminPanel/Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,about_keyName,about_main_img")] About about, List<string> content, List<int> langlist, List<string> title,HttpPostedFileBase about_main_img)
        {
            if (ModelState.IsValid)
            {
                db.Entry(about).State = EntityState.Modified;
                db.SaveChanges();
                List<Keystring> editKey = db.Keystrings.Where(s => s.keyName == about.about_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.content = content[i];
                    item.title = title[i];
                    db.SaveChanges();
                    i++;

                }
                var newabout = db.Abouts.Find(about.id);
                string oldfilepath = newabout.about_main_img;
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                var filename = Path.GetFileName(about_main_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                about_main_img.SaveAs(src);
                newabout.about_main_img = "/Uploads/" + randomNumber + filename;
                string fullPath = Request.MapPath("~" + oldfilepath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                newabout.about_main_img = about.about_main_img;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: AdminPanel/Abouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: AdminPanel/Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            About about = db.Abouts.Find(id);
            db.Abouts.Remove(about);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == about.about_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
            }
            string fullPath = Request.MapPath("~/Uploads/" + about.about_main_img);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
