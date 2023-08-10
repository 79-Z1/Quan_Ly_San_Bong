using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySanBanh.Models;

namespace QuanLySanBanh.Controllers
{
    public class BangGiaSansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();

        // GET: BangGiaSans
        public ActionResult Index()
        {
            var bangGiaSans = db.BangGiaSans.Include(b => b.San);
            return View(bangGiaSans.ToList());
        }

        // GET: BangGiaSans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangGiaSan bangGiaSan = db.BangGiaSans.Find(id);
            if (bangGiaSan == null)
            {
                return HttpNotFound();
            }
            return View(bangGiaSan);
        }

        // GET: BangGiaSans/Create
        public ActionResult Create()
        {
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            return View();
        }

        // POST: BangGiaSans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGia,MaSan,Gio,GiaTheoGio")] BangGiaSan bangGiaSan)
        {
            if (ModelState.IsValid)
            {
                db.BangGiaSans.Add(bangGiaSan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", bangGiaSan.MaSan);
            return View(bangGiaSan);
        }

        // GET: BangGiaSans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangGiaSan bangGiaSan = db.BangGiaSans.Find(id);
            if (bangGiaSan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", bangGiaSan.MaSan);
            return View(bangGiaSan);
        }

        // POST: BangGiaSans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGia,MaSan,Gio,GiaTheoGio")] BangGiaSan bangGiaSan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bangGiaSan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", bangGiaSan.MaSan);
            return View(bangGiaSan);
        }

        // GET: BangGiaSans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangGiaSan bangGiaSan = db.BangGiaSans.Find(id);
            if (bangGiaSan == null)
            {
                return HttpNotFound();
            }
            return View(bangGiaSan);
        }

        // POST: BangGiaSans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BangGiaSan bangGiaSan = db.BangGiaSans.Find(id);
            db.BangGiaSans.Remove(bangGiaSan);
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
