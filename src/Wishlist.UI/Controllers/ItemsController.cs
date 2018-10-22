using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wishlist.DATA;

namespace Wishlist.UI.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private WishlistDBEntities db = new WishlistDBEntities();

        // GET: Items
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var items = db.Items.Include(i => i.AspNetUser);
            if (User.IsInRole("Family Admin, Member"))
            {
                items = db.Items.Where(x => x.MemberId == userId);
            }

            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.AspNetUsers, "Id", "FirstName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,ItemName,ItemDescription,ItemLink,ItemImage,MemberId,Purchased")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (User.IsInRole("Member"))
            {
                ViewBag.MemberId = User.Identity.GetUserId();
            }
            if (User.IsInRole("Family Admin"))
            {
                var userId = User.Identity.GetUserId();
                var user = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                var famMembers = db.AspNetUsers.Where(x => x.FamilyID == user.FamilyID);
                ViewBag.MemberId = new SelectList(famMembers, "Id", "Name", item.MemberId);
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.MemberId = new SelectList(db.AspNetUsers, "Id", "Name", item.MemberId);
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.AspNetUsers, "Id", "FirstName", item.MemberId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,ItemDescription,ItemLink,ItemImage,MemberId,Purchased")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.AspNetUsers, "Id", "FirstName", item.MemberId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
