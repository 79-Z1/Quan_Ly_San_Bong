using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySanBanh.Models;
using QuanLySanBanh.Sevices;
using SelectPdf;

namespace QuanLySanBanh.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [CustomRole(Roles = "Admin")]
    [CustomAuthorize]
    public class DatSansController : Controller
    {   
        private QuanLySanBongEntities db = new QuanLySanBongEntities();
        string LayMaDatSan()
        {
            var maMax = db.DatSans.ToList().Select(n => n.MaDS).Max();
            int maDS = int.Parse(maMax.Substring(2)) + 1;
            string datSan = String.Concat("000", maDS.ToString());
            return "DS" + datSan.Substring(maDS.ToString().Length - 1);
        }

        public ActionResult BaoCaoThongKe(string thang="")
        {
            ViewBag.thang = thang;
            decimal tongTien;
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select * from dbo.fn_BaoCaoThongKe('" + thang + "')", connection))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select dbo.fn_TongDoanhThu('" + thang + "')", connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        tongTien = (decimal)result;
                    }
                    else
                    {
                        tongTien = 0;
                    }
                }
            }
            /*if (dataTable.Rows.Count == 0)
            {
                ViewBag.TB = "Tháng này chưa có các dữ liệu về doanh thu";
            }*/
            ViewBag.tongTien = tongTien;
            List<DataRow> rows = dataTable.AsEnumerable().ToList();
            ViewBag.Rows = dataTable;
            return View();
        }
        // GET: Admin/DatSans
        public ActionResult Index(string gioBatDau = "", string gioKetThuc = "", string MaTK = "", string ngay = "")
        {
            if (MaTK == "")
                MaTK = null;
            if (gioBatDau == "")
                gioBatDau = null;
            if (gioKetThuc == "")
                gioKetThuc = null;
            if (ngay == "")
                ngay = null;
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK");
            ViewBag.gioBatDau = gioBatDau;
            ViewBag.gioKetThuc = gioKetThuc;
            ViewBag.ngay = ngay;
            var datSans = db.DatSans.SqlQuery("DatSan_TimKiem '" + MaTK + "','" + gioBatDau +  "','" + gioKetThuc +  "','" + ngay + "'");
            if (datSans.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
           /* var datSans = db.DatSans.Include(d => d.TaiKhoan);*/
            return View(datSans.ToList());
        }
        // GET: Admin/DatSans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatSan datSan = db.DatSans.Find(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }
            return View(datSan);
        }

        // GET: Admin/DatSans/Create
        public ActionResult Create()
        {
            ViewBag.MaDS = LayMaDatSan();
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK");
            ViewBag.MaCTS = new SelectList(db.ChiTietSans, "MaCTS", "SoSan");
            return View();
        }

        // POST: Admin/DatSans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDS,MaCTS,MaTK,NgayDenSan,GioBatDau,GioKetThuc")] DatSan datSan)
        {
            if (ModelState.IsValid)
            {
                datSan.MaDS = LayMaDatSan();
                db.DatSans.Add(datSan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", datSan.MaTK);
            ViewBag.MaCTS = new SelectList(db.ChiTietSans, "MaCTS", "SoSan", datSan.MaCTS);
            return View(datSan);
        }

        // GET: Admin/DatSans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatSan datSan = db.DatSans.Find(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", datSan.MaTK);
            ViewBag.MaCTS = new SelectList(db.ChiTietSans, "MaCTS", "SoSan", datSan.MaCTS);
            return View(datSan);
        }

        // POST: Admin/DatSans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDS,MaCTS,MaTK,NgayDenSan,GioBatDau,GioKetThuc")] DatSan datSan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datSan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "TenTK", datSan.MaTK);
            ViewBag.MaCTS = new SelectList(db.ChiTietSans, "MaCTS", "SoSan", datSan.MaCTS);
            return View(datSan);
        }

        // GET: Admin/DatSans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatSan datSan = db.DatSans.Find(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }
            return View(datSan);
        }

        // POST: Admin/DatSans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DatSan datSan = db.DatSans.Find(id);
            db.DatSans.Remove(datSan);
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
