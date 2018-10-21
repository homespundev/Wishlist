using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wishlist.DATA;
using Wishlist.UI.Models;

namespace Wishlist.UI.Controllers
{
    public class FamiliesController : Controller
    {
        private WishlistDBEntities db = new WishlistDBEntities();

        // GET: Families
        public ActionResult Index()
        {
            return View(db.Families.ToList());
        }

        // GET: Families/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        // GET: Families/Create
        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
            var userFamily = db.Families.Include(y => y.AspNetUsers).Where(z => z.FamilyId == user.FamilyID).FirstOrDefault();
            if (userFamily.FamilyId == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // POST: Families/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FamilyId,FamilyName,FamilyDescription")] Family family)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();
                var user = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                db.Families.Add(family);
                user.FamilyID = family.FamilyId;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                UserManager.AddToRole(userId, "Family Admin");
                UserManager.RemoveFromRole(userId, "Member");
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }

        // GET: Families/Edit/5
        [Authorize(Roles = "Family Admin, Admin" )]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        // POST: Families/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FamilyId,FamilyName,FamilyDescription")] Family family)
        {
            if (ModelState.IsValid)
            {
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family);
        }

        // GET: Families/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        // POST: Families/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Family family = db.Families.Find(id);
            db.Families.Remove(family);
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
