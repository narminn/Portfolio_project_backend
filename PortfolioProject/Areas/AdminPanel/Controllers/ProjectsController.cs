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
    public class ProjectsController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: AdminPanel/Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: AdminPanel/Projects/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();
            

            return View();
        }

        // POST: AdminPanel/Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,project_title,project_keyName,project_main_img,project_draft_img,project_video")] Project project,HttpPostedFileBase project_main_img,HttpPostedFileBase project_draft_img,HttpPostedFileBase project_video, List<string> content, List<string> langlist,List<HttpPostedFileBase> proj_image,List<string>title)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                
                int randomNumbermain = random.Next(0, 10000000);
                int randomNumberdraft = random.Next(0, 10000000);
                int randomvideo = random.Next(0, 10000000);
                int randomKey = random.Next(0, 1000000);
                var filenamemain = Path.GetFileName(project_main_img.FileName);
                var filenamedraft = Path.GetFileName(project_draft_img.FileName);
                var videoname = Path.GetFileName(project_video.FileName);
                var srcmain = Path.Combine(Server.MapPath("~/Uploads/"), randomNumbermain + filenamemain);
                var srcdraft = Path.Combine(Server.MapPath("~/Uploads/"), randomNumberdraft + filenamedraft);
                var srcvideo = Path.Combine(Server.MapPath("~/Uploads/"), randomvideo + videoname);
                project_main_img.SaveAs(srcmain);
                project_draft_img.SaveAs(srcdraft);
                project_video.SaveAs(srcvideo);
                project.project_main_img = "/Uploads/" + randomNumbermain + filenamemain;
                project.project_draft_img = "/Uploads/" + randomNumberdraft + filenamedraft;
                project.project_video = "/Uploads/" + randomvideo + videoname;
                project.project_keyName = randomKey.ToString();
                
                db.Projects.Add(project);
                db.SaveChanges();
                var titleandcontent = title.Zip(content, (t, c) => new { Title = t, Content = c });
                int i = 0;
                foreach (var item in titleandcontent)
                {
                    Keystring newkey = new Keystring();
                    newkey.keyName = randomKey.ToString();
                    newkey.content = item.Content;
                    newkey.title = item.Title;
                    int lang = Convert.ToInt32(langlist[i]);
                    newkey.language_id = lang;
                    db.Keystrings.Add(newkey);
                    db.SaveChanges();
                    i++;
                }
                foreach (var item in proj_image)
                {
                    if (item !=null)
                    {
                        ProjectImage projimg = new ProjectImage();
                        var imgname = Path.GetFileName(item.FileName);
                        var imgsrc = Path.Combine(Server.MapPath("~/Uploads/"), randomKey + imgname);
                        item.SaveAs(imgsrc);
                        projimg.project_img = "/Uploads/" + randomKey + imgname;
                        projimg.project_id = project.id;
                        db.ProjectImages.Add(projimg);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("error");
                    }
                  
                }

                
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: AdminPanel/Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);

            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == project.project_keyName).ToList();

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: AdminPanel/Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,project_title,project_keyName,project_main_img,project_draft_img")] Project project, List<string> content, List<int> langlist, List<string> title,HttpPostedFileBase project_main_img,HttpPostedFileBase project_draft_img)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                List<Keystring> editKey = db.Keystrings.Where(s => s.keyName == project.project_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.content = content[i];
                    item.title = title[i];
                    db.SaveChanges();
                    i++;

                }
                var newproject = db.Projects.Find(project.id);
                string oldfilepath1 = newproject.project_main_img;
                string oldfilepath2 = newproject.project_draft_img;
                Random random = new Random();

                int randomNumbermain = random.Next(0, 100000);
                int randomNumberdraft = random.Next(0, 100000);
                var filenamemain = Path.GetFileName(project_main_img.FileName);
                var filenamedraft = Path.GetFileName(project_draft_img.FileName);
                var srcmain = Path.Combine(Server.MapPath("~/Uploads/"), randomNumbermain + filenamemain);
                var srcdraft = Path.Combine(Server.MapPath("~/Uploads/"), randomNumberdraft + filenamedraft);
                project_main_img.SaveAs(srcmain);
                project_draft_img.SaveAs(srcdraft);
                project.project_main_img = "/Uploads/" + randomNumbermain + filenamemain;
                project.project_draft_img = "/Uploads/" + randomNumberdraft + filenamedraft;
                string fullPath1 = Request.MapPath("~" + oldfilepath1);
                string fullPath2 = Request.MapPath("~" + oldfilepath2);
                if (System.IO.File.Exists(fullPath1))
                {
                    System.IO.File.Delete(fullPath1);
                }
                if (System.IO.File.Exists(fullPath2))
                {
                    System.IO.File.Delete(fullPath2);
                }

                newproject.project_main_img = project.project_main_img;
                newproject.project_draft_img = project.project_draft_img;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: AdminPanel/Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: AdminPanel/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == project.project_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
            }
            string fullPath1 = Request.MapPath("~/Uploads/" + project.project_main_img);
            string fullPath2 = Request.MapPath("~/Uploads/" + project.project_draft_img);
            if (System.IO.File.Exists(fullPath1))
            {
                System.IO.File.Delete(fullPath1);
            }
            if (System.IO.File.Exists(fullPath2))
            {
                System.IO.File.Delete(fullPath2);
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
