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
    [CustomAuthorize]
    [CustomRole(Roles = "Admin")]
    public class SansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();
        /*string LayMaSan()
        {
            var maMax = db.Sans.ToList().Select(n => n.MaSan).Max();
            int maSan = int.Parse(maMax.Substring(2)) + 1;
            string San = String.Concat("000", maSan.ToString());
            return "S" + San.Substring(maSan.ToString().Length - 1);
        }*/

        // GET: Admin/Sans
        public ActionResult Index(string maSan="", string tenSan = "")
        {
            if (maSan == "")
                maSan = null;
            if(tenSan == "")
                tenSan= null;
            ViewBag.maSan = maSan;
            ViewBag.tenSan = tenSan;
            var sans = db.Sans.SqlQuery("San_TimKiem '" + maSan + "',N'" + tenSan +"'");
            if (sans.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(sans.ToList());
        }
        public ActionResult Home()
        {
            
            return View();
        }

        // GET: Admin/Sans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // GET: Admin/Sans/Create
        public ActionResult Create()
        {
            /*ViewBag.MaSan = LayMaSan();*/
            return View();
        }

        // POST: Admin/Sans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSan,TenSan,DiaChi,SoLuongSan,AnhSan")] San san)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var AnhSan = Request.Files["AnhSan"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(AnhSan.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            AnhSan.SaveAs(path);
            if (ModelState.IsValid)
            {
                /*san.MaSan = LayMaSan();*/
                san.AnhSan = postedFileName;
                db.Sans.Add(san);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(san);
        }

        // GET: Admin/Sans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // POST: Admin/Sans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSan,TenSan,DiaChi,SoLuongSan,AnhSan")] San san)
        {
            try
            {
                var imgSan = Request.Files["Avatar"];
                string postedFileName = System.IO.Path.GetFileName(imgSan.FileName);
                var path = Server.MapPath("/Images/" + postedFileName);
                imgSan.SaveAs(path);
            }
            catch { }
            if (ModelState.IsValid)
            {
                db.Entry(san).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(san);
            var AnhSan = Request.Files["Avatar"];
            if (ModelState.IsValid)
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(AnhSan.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                AnhSan.SaveAs(path);
                san.AnhSan = postedFileName;
                db.Entry(san).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(san);
        }

        // GET: Admin/Sans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // POST: Admin/Sans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            San san = db.Sans.Find(id);
            db.Sans.Remove(san);
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
