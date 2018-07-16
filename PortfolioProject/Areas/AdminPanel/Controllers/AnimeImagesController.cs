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
    public class AnimeImagesController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/AnimeImages
        public ActionResult Index()
        {
            return View(db.AnimeImages.ToList());
        }

        // GET: AdminPanel/AnimeImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimeImage animeImage = db.AnimeImages.Find(id);
            if (animeImage == null)
            {
                return HttpNotFound();
            }
            return View(animeImage);
        }

        // GET: AdminPanel/AnimeImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/AnimeImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,anime_img")] AnimeImage animeImage,HttpPostedFileBase anime_img)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100000);
                var date = DateTime.Now;
                var filename = Path.GetFileName(anime_img.FileName);
                var src = Path.Combine(Server.MapPath("~/Uploads/"), randomNumber + filename);
                anime_img.SaveAs(src);
                animeImage.anime_img = "/Uploads/" + randomNumber + filename;
                db.AnimeImages.Add(animeImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(animeImage);
        }

        // GET: AdminPanel/AnimeImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimeImage animeImage = db.AnimeImages.Find(id);
            if (animeImage == null)
            {
                return HttpNotFound();
            }
            return View(animeImage);
        }

        // POST: AdminPanel/AnimeImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,anime_img")] AnimeImage animeImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animeImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(animeImage);
        }

        // GET: AdminPanel/AnimeImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimeImage animeImage = db.AnimeImages.Find(id);
            if (animeImage == null)
            {
                return HttpNotFound();
            }
            return View(animeImage);
        }

        // POST: AdminPanel/AnimeImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnimeImage animeImage = db.AnimeImages.Find(id);
            db.AnimeImages.Remove(animeImage);
            db.SaveChanges();
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
