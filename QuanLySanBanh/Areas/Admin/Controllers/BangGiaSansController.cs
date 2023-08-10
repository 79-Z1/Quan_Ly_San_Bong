using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using QuanLySanBanh.Models;
using QuanLySanBanh.Sevices;

namespace QuanLySanBanh.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [CustomRole(Roles = "Admin")]
    [CustomAuthorize]
    public class BangGiaSansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();
        string LayMaGia()
        {
            var maMax = db.BangGiaSans.ToList().Select(n => n.MaGia).Max();
            int maBG = int.Parse(maMax.Substring(2)) + 1;
            string bg = String.Concat("00000", maBG.ToString());
            return "G" + bg.Substring(maBG.ToString().Length - 1);
        }

        // GET: Admin/BangGiaSans
        public ActionResult Index(string gio = "", string MaSan = "", string gia = "")
        {
            if (MaSan == "")
                MaSan = null;
            if (gio == "")
                gio = null;
            if (gia == "")
                gia = null;
            ViewBag.gio = gio;
            ViewBag.gia = gia;
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            var bangGias = db.BangGiaSans.SqlQuery("BangGiaSan_TimKiem '" + MaSan + "','" + gio + "','" + gia + "'");
            if (bangGias.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            /*var bangGiaSans = db.BangGiaSans.Include(b => b.San);*/
            return View(bangGias.ToList());
        }

        // GET: Admin/BangGiaSans/Details/5
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

        // GET: Admin/BangGiaSans/Create
        public ActionResult Create()
        {
            ViewBag.MaGia = LayMaGia();
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            return View();
        }

        // POST: Admin/BangGiaSans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGia,MaSan,Gio,GiaTheoGio")] BangGiaSan bangGiaSan)
        {
            ViewBag.MaGia = LayMaGia();
            if (ModelState.IsValid)
            {
                bangGiaSan.MaGia = LayMaGia();
                db.BangGiaSans.Add(bangGiaSan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", bangGiaSan.MaSan);
            return View(bangGiaSan);
        }

        // GET: Admin/BangGiaSans/Edit/5
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

        // POST: Admin/BangGiaSans/Edit/5
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

        // GET: Admin/BangGiaSans/Delete/5
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

        // POST: Admin/BangGiaSans/Delete/5
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
