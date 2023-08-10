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

namespace QuanLySanBong.Controllers
{
    public class DatSansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();


        public ActionResult BangGia(string maSan = "")
        {
            if (maSan == "")
                maSan = null;
            var bangGias = db.BangGiaSans.SqlQuery("BangGiaSan_TheoMaSan '" + maSan + "'");
            if (bangGias.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            /*var bangGiaSans = db.BangGiaSans.Include(b => b.San);*/
            return View(bangGias.ToList());
        }
      
        string LayMaDatSan()
        {
            var maMax = db.DatSans.ToList().Select(n => n.MaDS).Max();
            int maDS = int.Parse(maMax.Substring(2)) + 1;
            string datSan = String.Concat("000", maDS.ToString());
            return "DS" + datSan.Substring(maDS.ToString().Length - 1);
        }


        [Route("DatSans/PhieuDatSan/{maSan}")]
        [CustomAuthorize]
        public ActionResult PhieuDatSan(string maSan)
        {
            String maTK = CookieSevices.GetCookie(HttpContext.ApplicationInstance.Context,"MaTK");
            String ngayDat = Request.QueryString["ngayDat"];
            String giobatDau = Request.QueryString["giobatDau"];
            String viTriSan = Request.QueryString["viTriSan"];
            String tenSan = Request.QueryString["tenSan"];
            String maCTS = Request.QueryString["maCTS"];
            decimal tongTien;
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select dbo.fn_TinhTienSan (@maSan,@maTK,@gioBatDau,@gioKetThuc)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@maSan", maSan);
                    command.Parameters.AddWithValue("@maTK", maTK);
                    command.Parameters.AddWithValue("@gioBatDau", giobatDau);
                    command.Parameters.AddWithValue("@gioKetThuc", int.Parse(giobatDau) + 1);
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

            ViewBag.tongTien = tongTien;
            ViewBag.maCTS = maCTS;
            ViewBag.ngayDat = ngayDat;
            ViewBag.giobatDau = giobatDau;
            ViewBag.viTriSan = viTriSan;
            ViewBag.maSan = maSan;
            ViewBag.tenSan = tenSan;
            return View();
        }

        [HttpPost]
        [Route("DatSans/TinhTien")]
        [CustomAuthorize]
        public ActionResult TinhTien(string maSan, string maTK, int gioBatDau, int gioKetThuc)
        {
            decimal tongTien;
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select dbo.fn_TinhTienSan (@maSan,@maTK,@gioBatDau,@gioKetThuc)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@maSan", maSan);
                    command.Parameters.AddWithValue("@maTK", maTK);
                    command.Parameters.AddWithValue("@gioBatDau", gioBatDau);
                    command.Parameters.AddWithValue("@gioKetThuc", gioKetThuc);
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
            return Json(new { tongTien = tongTien });
        }

        [Route("DatSans/DatSan/{maSan}")]
        public ActionResult DatSan(string maSan)
        {
            String ngayDat = Request.QueryString["ngayDat"];
            String gio = Request.QueryString["gio"];
            String Ngay = ngayDat == null ? DateTime.Now.ToString("yyyy-MM-dd") : ngayDat;
            String Gio = gio == null ? (DateTime.Now.Hour + 1).ToString() : gio;

            if (maSan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select * from fn_tinhTrangTheoNgayTheoGio ('" + maSan + "'," + "'" + Ngay + "'," + Gio + ")", connection))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            if (dataTable.Rows.Count == 0)
            {
                ViewBag.TB = "Không có kết quả";
            }
            List<DataRow> rows = dataTable.AsEnumerable().ToList();

            ViewBag.ngayDat = Ngay;
            ViewBag.gio = Gio;
            ViewBag.maSan = maSan;

            return View(rows);
        }

        [Route("DatSans/DatSanJson/{maSan}")]
        public ActionResult DatSanJson(string maSan)
        {
            String ngayDat = Request.QueryString["ngayDat"];
            String gio = Request.QueryString["gio"];
            String Ngay = ngayDat == null ? DateTime.Now.ToString("yyyy-MM-dd") : ngayDat;
            String Gio = gio == null ? (DateTime.Now.Hour + 1).ToString() : gio;

            if (maSan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select * from fn_tinhTrangTheoNgayTheoGio ('" + maSan + "'," + "'" + Ngay + "'," + Gio + ")", connection))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            if (dataTable.Rows.Count == 0)
            {
                ViewBag.TB = "Không có kết quả";
            }
            List<DataRow> rows = dataTable.AsEnumerable().ToList();


            ViewBag.ngayDat = Ngay;
            ViewBag.gio = Gio;
            ViewBag.maSan = maSan;

            return View(rows);
        }

        // GET: DatSans/LichSu/:maTK
        [Route("DatSans/LichSu/{maTK}")]
        [CustomAuthorize]
        public ActionResult LichSu(string maTK)
        {
            if (maTK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select * from dbo.fn_lichSuDatSanTheoMaTK('" + maTK + "')", connection))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            if (dataTable.Rows.Count == 0)
            {
                ViewBag.TB = "Bạn chưa đặt sân nào";
            }
            List<DataRow> rows = dataTable.AsEnumerable().ToList();
            return View(rows);
        }
        
        // POST: DatSans/Create
        [HttpPost]
        [Route("DatSans/Create")]
        [CustomAuthorize]
        public ActionResult Create([Bind(Include = "MaDS,MaCTS,MaTK,NgayDenSan,GioBatDau,GioKetThuc")] DatSan datSan)
        {
            if (ModelState.IsValid)
            {
                for (int? i = datSan.GioBatDau; i < datSan.GioKetThuc; i++)
                {
                    var newDatSan = new DatSan
                    {
                        MaDS = LayMaDatSan(),
                        MaCTS = datSan.MaCTS,
                        MaTK = "Hide",
                        NgayDenSan = datSan.NgayDenSan,
                        GioBatDau = i,
                        GioKetThuc = i + 1
                    };

                    db.DatSans.Add(newDatSan);
                    db.SaveChanges();
                }
                datSan.MaDS = LayMaDatSan();
                db.DatSans.Add(datSan);
                db.SaveChanges();

                return Json(new { message = "oke" });
            }

            return Json(new { message = "có lỗi" });
        }

        // GET: /DatSans/Delete/5
        [CustomAuthorize]
        public ActionResult Delete(string id)
        {
            String viTriSan = Request.QueryString["viTriSan"];
            String tenSan = Request.QueryString["tenSan"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatSan datSan = db.DatSans.Find(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }

            ViewBag.viTriSan = viTriSan;
            ViewBag.tenSan = tenSan;
            return View(datSan);
        }

        //
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize]
        public ActionResult DeleteConfirmed(string id)
        {
            String maTK = CookieSevices.GetCookie(HttpContext.ApplicationInstance.Context, "MaTK");
            DatSan datSan = db.DatSans.Find(id);
            db.DatSans.Remove(datSan);
            db.SaveChanges();
            return RedirectToAction("LichSu", new { maTk = maTK });
        }

        // GET: Admin/DatSans/Details/5
        [CustomAuthorize]
        public ActionResult Details(string id)
        {
            String viTriSan = Request.QueryString["viTriSan"];
            String tenSan = Request.QueryString["tenSan"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatSan datSan = db.DatSans.Find(id);
            if (datSan == null)
            {
                return HttpNotFound();
            }
            ViewBag.viTriSan = viTriSan;
            ViewBag.tenSan = tenSan;
            return View(datSan);
        }

    }
}
