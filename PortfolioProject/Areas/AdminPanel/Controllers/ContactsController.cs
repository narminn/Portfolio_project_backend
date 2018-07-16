using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortfolioProject.Models;

namespace PortfolioProject.Areas.AdminPanel.Controllers
{
    [FilterAdmin]
    public class ContactsController : Controller
    {
        private PortfolioDbEntities db = new PortfolioDbEntities();

        // GET: AdminPanel/Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: AdminPanel/Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: AdminPanel/Contacts/Create
        public ActionResult Create()
        {
            ViewBag.langlist = db.Languages.ToList();

            return View();
        }

        // POST: AdminPanel/Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contact_keyName,contact_main_phone,contact_secod_phone,contact_email")] Contact contact, List<string> content, List<string> langlist)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int randomKey = random.Next(0, 1000000);
                contact.contact_keyName = randomKey.ToString();
                db.Contacts.Add(contact);
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
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: AdminPanel/Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            ViewBag.KeyWords = db.Keystrings.Where(s => s.keyName == contact.contact_keyName).ToList();

            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: AdminPanel/Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contact_keyName,contact_main_phone,contact_secod_phone,contact_email")] Contact contact, List<string> content, List<int> langlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                List<Keystring> editKey = db.Keystrings.Where(s => s.keyName == contact.contact_keyName).ToList();
                int i = 0;
                foreach (var item in editKey)
                {
                    item.language_id = langlist[i];
                    item.content = content[i];
                    db.SaveChanges();
                    i++;

                }
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: AdminPanel/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: AdminPanel/Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            List<Keystring> deletkey = db.Keystrings.Where(d => d.keyName == contact.contact_keyName).ToList();
            foreach (var item in deletkey)
            {
                db.Keystrings.Remove(item);
                db.SaveChanges();
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
