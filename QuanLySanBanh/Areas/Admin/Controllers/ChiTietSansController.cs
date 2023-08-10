using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySanBanh.Models;
using QuanLySanBanh.Sevices;

namespace QuanLySanBanh.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [CustomRole(Roles = "Admin")]
    [CustomAuthorize]
    public class ChiTietSansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();
        string LayMaChiTietSan()
        {
            var maMax = db.ChiTietSans.ToList().Select(n => n.MaCTS).Max();
            int maCTS = int.Parse(maMax.Substring(2)) + 1;
            string cts = String.Concat("000", maCTS.ToString());
            return "S" + cts.Substring(maCTS.ToString().Length - 1);
        }

        // GET: Admin/ChiTietSans
        public ActionResult Index(string MaSan = "", string viTri = "")
        {
            if (MaSan == "")
                MaSan = null;
            if (viTri == "")
                viTri = null;
            ViewBag.viTri = viTri;
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            var chiTietSans = db.ChiTietSans.SqlQuery("ChiTietSan_TimKiem '" + MaSan + "','" + viTri + "'");
            if (chiTietSans.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
/*            var chiTietSans = db.ChiTietSans.Include(c => c.San);
*/            return View(chiTietSans.ToList());
        }

        // GET: Admin/ChiTietSans/Details/5
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

        // GET: Admin/ChiTietSans/Create
        public ActionResult Create()
        {
            ViewBag.MaCTS = LayMaChiTietSan();
            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan");
            return View();
        }
        // POST: Admin/ChiTietSans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCTS,MaSan,SoSan")] ChiTietSan chiTietSan)
        {
            if (ModelState.IsValid)
            {
                chiTietSan.MaCTS = LayMaChiTietSan();
                db.ChiTietSans.Add(chiTietSan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSan = new SelectList(db.Sans, "MaSan", "TenSan", chiTietSan.MaSan);
            return View(chiTietSan);
        }

        // GET: Admin/ChiTietSans/Edit/5
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

        // POST: Admin/ChiTietSans/Edit/5
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

        // GET: Admin/ChiTietSans/Delete/5
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

        // POST: Admin/ChiTietSans/Delete/5
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
