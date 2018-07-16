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
    public class SlidersController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();
        

        // GET: AdminPanel/Sliders
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        // GET: AdminPanel/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: AdminPanel/Sliders/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();
            return View();
        }

        // POST: AdminPanel/Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,slider_img")] Slider slider,HttpPostedFileBase slider_img, List<string> content, List<string> langlist)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                int randomKey = random.Next(0, 1000000);
                var filename = Path.GetFileName(slider_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                slider_img.SaveAs(src);
                slider.slider_img = "/Uploads/" + randomNumber + filename;
                slider.slider_keyName = randomKey.ToString() ;
                db.Sliders.Add(slider);
                db.SaveChanges();
                int i = 0;
                foreach(string item in content)
                {
                    Keystring newkey = new Keystring();
                    newkey.keyName = randomKey.ToString();
                    newkey.content = item;
                    int lang =Convert.ToInt16(langlist[i]);
                    newkey.language_id = lang;
                    db.Keystrings.Add(newkey);
                    db.SaveChanges();
                    i++;
                }
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: AdminPanel/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);

            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == slider.slider_keyName).ToList();

            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminPanel/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,slider_img,slider_keyName")] Slider slider, List<string> content, List<int> langlist, HttpPostedFileBase slider_img)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();


                 List<Keystring> editKey = db.Keystrings.Where(s=>s.keyName==slider.slider_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.content = content[i];
                    db.SaveChanges();
                    i++;

                }

                var newslider = db.Sliders.Find(slider.id);
                string oldfilepath = newslider.slider_img;
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                var filename = Path.GetFileName(slider_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                slider_img.SaveAs(src);
                newslider.slider_img = "/Uploads/" + randomNumber + filename;
                string fullPath = Request.MapPath("~" + oldfilepath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                newslider.slider_img = slider.slider_img;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: AdminPanel/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminPanel/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            db.Sliders.Remove(slider);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == slider.slider_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
            }


            string fullPath = Request.MapPath("~/Uploads/" + slider.slider_img);
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
