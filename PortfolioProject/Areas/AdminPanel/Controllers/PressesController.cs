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
    public class PressesController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/Presses
        public ActionResult Index()
        {
            return View(db.Presses.ToList());
        }

        // GET: AdminPanel/Presses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = db.Presses.Find(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // GET: AdminPanel/Presses/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();

            return View();
        }

        // POST: AdminPanel/Presses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,press_title,press_keyName,press_img")] Press press,HttpPostedFileBase press_img, List<string> content, List<string> langlist,List<string> title)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                int randomKey = random.Next(0, 1000000);
                var filename = Path.GetFileName(press_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                press_img.SaveAs(src);
                press.press_img = "/Uploads/" + randomNumber + filename;
                press.press_keyName = randomKey.ToString();
                db.Presses.Add(press);
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
                return RedirectToAction("Index");
            }

            return View(press);
        }

        // GET: AdminPanel/Presses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = db.Presses.Find(id);
            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == press.press_keyName).ToList();

            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // POST: AdminPanel/Presses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,press_title,press_keyName,press_img")] Press press, List<string> title, List<string> content, List<int> langlist,HttpPostedFileBase press_img)
        {
            if (ModelState.IsValid)
            {
                db.Entry(press).State = EntityState.Modified;
                db.SaveChanges();
                List<Keystring> editKey = db.Keystrings.Where(s => s.keyName == press.press_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.title = title[i];
                    item.content = content[i];
                    db.SaveChanges();
                    i++;

                }
                var newpress = db.Presses.Find(press.id);
                string oldfilepath = newpress.press_img;
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                var filename = Path.GetFileName(press_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                press_img.SaveAs(src);
                newpress.press_img = "/Uploads/" + randomNumber + filename;
                string fullPath = Request.MapPath("~" + oldfilepath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                newpress.press_img = press.press_img;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(press);
        }

        // GET: AdminPanel/Presses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = db.Presses.Find(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // POST: AdminPanel/Presses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Press press = db.Presses.Find(id);
            db.Presses.Remove(press);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == press.press_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
            }
            string fullPath = Request.MapPath("~/Uploads/" + press.press_img);
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
