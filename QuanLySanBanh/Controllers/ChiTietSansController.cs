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
    public class ChiTietSansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();

        // GET: ChiTietSans
        public ActionResult Index()
        {
            var chiTietSans = db.ChiTietSans.Include(c => c.San);
            return View(chiTietSans.ToList());
        }

        // GET: ChiTietSans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietSan chiTietSan = db.ChiTietSans.Find(id);
            if (chiTietSan == null)
            {
                return HttpNotFound();
            }
            return View(chiTietSan);
        }

        // GET: ChiTietSans/Create
        public ActionResult Create()
        {
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            return View();
        }

        // POST: ChiTietSans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCTS,MaSan,SoSan")] ChiTietSan chiTietSan)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietSans.Add(chiTietSan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", chiTietSan.MaSan);
            return View(chiTietSan);
        }

        // GET: ChiTietSans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietSan chiTietSan = db.ChiTietSans.Find(id);
            if (chiTietSan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", chiTietSan.MaSan);
            return View(chiTietSan);
        }

        // POST: ChiTietSans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCTS,MaSan,SoSan")] ChiTietSan chiTietSan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietSan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", chiTietSan.MaSan);
            return View(chiTietSan);
        }

        // GET: ChiTietSans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietSan chiTietSan = db.ChiTietSans.Find(id);
            if (chiTietSan == null)
            {
                return HttpNotFound();
            }
            return View(chiTietSan);
        }

        // POST: ChiTietSans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ChiTietSan chiTietSan = db.ChiTietSans.Find(id);
            db.ChiTietSans.Remove(chiTietSan);
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
