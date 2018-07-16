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
    public class WorkshopsController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/Workshops
        public ActionResult Index()
        {
            return View(db.Workshops.ToList());
        }

        // GET: AdminPanel/Workshops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // GET: AdminPanel/Workshops/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();
            return View();
        }

        // POST: AdminPanel/Workshops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,workshop_title,workshop_keyName,workshop_main_img,workshop_video")] Workshop workshop,HttpPostedFileBase workshop_main_img, List<string> content, List<string> langlist, List<HttpPostedFileBase> proj_image, List<string> title,HttpPostedFileBase workshop_video)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                int randomKey = random.Next(0, 1000000);
                int randomvideo = random.Next(0, 100000);
                var filename = Path.GetFileName(workshop_main_img.FileName);
                var videoname = Path.GetFileName(workshop_video.FileName);

                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                var videosrc = Path.Combine(Server.MapPath("~/Uploads/"), randomvideo + videoname);

                workshop_main_img.SaveAs(src);
                workshop_video.SaveAs(videosrc);
                workshop.workshop_main_img = "/Uploads/" + randomNumber + filename;
                workshop.workshop_video = "/Uploads/" + randomvideo + videoname;
                workshop.workshop_keyName = randomKey.ToString();
                db.Workshops.Add(workshop);
                db.SaveChanges();
                var titleandcontent = title.Zip(content, (t, c) => new { Title = t, Content = c });

                int i = 0;
                foreach (var item in titleandcontent)
                {
                    Keystring newkey = new Keystring();
                    newkey.keyName = randomKey.ToString();
                    newkey.content = item.Content;
                    newkey.title = item.Title;
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
                        WorkshopImage workshopimg = new WorkshopImage();
                        var imgname = Path.GetFileName(item.FileName);
                        var imgsrc = Path.Combine(Server.MapPath("~/Uploads/"), randomKey + imgname);
                        item.SaveAs(imgsrc);
                        workshopimg.workshop_img = "/Uploads/" + randomKey + imgname;
                        workshopimg.workshop_id = workshop.id;
                        db.WorkshopImages.Add(workshopimg);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("error");
                    }

                }
                return RedirectToAction("Index");
            }

            return View(workshop);
        }

        // GET: AdminPanel/Workshops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == workshop.workshop_keyName).ToList();

            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // POST: AdminPanel/Workshops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,workshop_title,workshop_keyName,workshop_main_img,workshop_video")] Workshop workshop, List<string> content, List<int> langlist, List<string> title,HttpPostedFileBase workshop_main_img,HttpPostedFileBase workshop_video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workshop).State = EntityState.Modified;
                db.SaveChanges();
                List<Keystring> editKey = db.Keystrings.Where(s => s.keyName == workshop.workshop_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.content = content[i];
                    item.title = title[i];
                    db.SaveChanges();
                    i++;

                }
                var newworkshop = db.Workshops.Find(workshop.id);
                string oldfilepath = newworkshop.workshop_main_img;
                string oldfilevideo = newworkshop.workshop_video;
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                int randomKey = random.Next(0, 1000000);
                int randomvideo = random.Next(0, 1000000);

                var filename = Path.GetFileName(workshop_main_img.FileName);
                var videoname = Path.GetFileName(workshop_video.FileName);

                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                var videosrc = Path.Combine(Server.MapPath("~/Uploads/"), randomvideo + videoname);

                workshop_main_img.SaveAs(src);
                workshop_video.SaveAs(videosrc);
                workshop.workshop_main_img = "/Uploads/" + randomNumber + filename;
                workshop.workshop_video = "/Uploads/" + randomvideo + videoname;
                string fullPath = Request.MapPath("~" + oldfilepath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                string fullPathvideo = Request.MapPath("~" + oldfilevideo);
                if (System.IO.File.Exists(fullPathvideo))
                {
                    System.IO.File.Delete(fullPathvideo);
                }
                newworkshop.workshop_main_img = workshop.workshop_main_img;
                newworkshop.workshop_video = workshop.workshop_video;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workshop);
        }

        // GET: AdminPanel/Workshops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workshop workshop = db.Workshops.Find(id);
            if (workshop == null)
            {
                return HttpNotFound();
            }
            return View(workshop);
        }

        // POST: AdminPanel/Workshops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Workshop workshop = db.Workshops.Find(id);
            db.Workshops.Remove(workshop);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == workshop.workshop_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
            }
            string fullPath = Request.MapPath("~/Uploads/" + workshop.workshop_main_img);
            string fullPathvideo = Request.MapPath("~/Uploads/" + workshop.workshop_video);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            if (System.IO.File.Exists(fullPathvideo))
            {
                System.IO.File.Delete(fullPathvideo);
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
