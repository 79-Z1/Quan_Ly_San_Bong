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
    public class TaiKhoansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();
        string LayMaTK()
        {
            var maMax = db.TaiKhoans.ToList().Select(n => n.MaTK).Max();
            int maTK = int.Parse(maMax.Substring(2)) + 1;
            string tk = String.Concat("00", maTK.ToString());
            return "TK" + tk.Substring(maTK.ToString().Length - 1);
        }

        // GET: Admin/TaiKhoans
        public ActionResult Index(string maTK = "", string tenTK = "",string sdt="", string vip = "", string quyen = "")
        {
            if (maTK == "")
                maTK = null;
            if (tenTK == "")
                tenTK = null;
            if (sdt == "")
                sdt = null;
            if (vip == "")
                vip = null;
            if (quyen == "")
                quyen = null;
            ViewBag.maTK = maTK;
            ViewBag.tenTK = tenTK;
            ViewBag.sdt = sdt;
            ViewBag.vip = vip;
            ViewBag.quyen = quyen;
            var taiKhoans = db.TaiKhoans.SqlQuery("TaiKhoan_TimKiem '" + maTK + "','" + tenTK + "','" + sdt + "','" + vip + "','" + quyen + "'");
            if (taiKhoans.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(taiKhoans.ToList());
        }

        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Create
        public ActionResult Create()
        {
            ViewBag.MaTK = LayMaTK();
            return View();
        }

        // POST: Admin/TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TenTK,MatKhau,Email,HoTen,SDT,DiaChi,Vip,DiemTich,Quyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                taiKhoan.MaTK = LayMaTK();
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TenTK,MatKhau,Email,HoTen,SDT,DiaChi,Vip,DiemTich,Quyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
