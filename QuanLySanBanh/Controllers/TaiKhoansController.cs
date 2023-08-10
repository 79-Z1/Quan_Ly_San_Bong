using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using QuanLySanBanh.Models;

namespace QuanLySanBong_Web.Controllers
{   
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

        // GET: TaiKhoans
        public ActionResult Index()
        {
            return View(db.TaiKhoans.ToList());
        }
        
        // GET: TaiKhoans/Details/TK005
        [Route("TaiKhoans/Details/{maTK}")]
        public ActionResult Details(string maTK)
        {
            if (maTK == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(maTK);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }


        // GET: TaiKhoans/Edit/5
        [Route("TaiKhoans/Edit/{id}")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TenTK,Email,HoTen,SDT,DiaChi,Vip,DiemTich")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = taiKhoan.MaTK });

            }
            return View(taiKhoan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void CheckRole(TaiKhoan taikhoan)
        {
            var tk = db.TaiKhoans.Where(a => a.Email.Equals(taikhoan.Email) && a.MatKhau.Equals(taikhoan.MatKhau)).FirstOrDefault();

            if (tk != null)
            {
                // Xác thực thành công

                if (tk.Quyen == true) // Kiểm tra "Quyen" có giá trị "admin" (hoặc 1) hay không
                {
                    // Đặt vai trò "admin" cho người dùng
                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, tk.TenTK),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, "ApplicationCookie");

                    // Đăng nhập người dùng
                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);
                }
            }
        }
    }
}
