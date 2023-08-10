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

namespace QuanLySanBong.Controllers
{
    public class SansController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();

        // GET: Sans
        public ActionResult Index()
        {
            String Ngay;
            int Gio;
            Ngay = DateTime.Now.ToString("yyyy-MM-dd");
            Gio = DateTime.Now.Hour;

            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("select * from fn_DanhSachSanVaTinhTrangTheoNgayTheoGio('" + Ngay + "'," + Gio + ")", connection))
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
            return View(rows);
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
