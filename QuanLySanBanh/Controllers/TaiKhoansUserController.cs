using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using QuanLySanBanh.Models;
using QuanLySanBanh.Sevices;

namespace QuanLySanBanh.Controllers
{
    public class TaiKhoansUserController : Controller
    {
        private QuanLySanBongEntities db = new QuanLySanBongEntities();


        string LayMaTK()
        {
            var maMax = db.TaiKhoans.ToList().Select(n => n.MaTK).Max();
            int maTK = int.Parse(maMax.Substring(2)) + 1;
            string tk = String.Concat("00", maTK.ToString());
            return "TK" + tk.Substring(maTK.ToString().Length - 1);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // Xóa cookie xác thực
            FormsAuthentication.SignOut();

            // Xóa các cookie khác nếu cần
            CookieSevices.DeleteCookie(HttpContext.ApplicationInstance.Context, "MaTK");
            CookieSevices.DeleteCookie(HttpContext.ApplicationInstance.Context, "Quyen");

            // Chuyển hướng đến trang đăng nhập
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Login(string Email, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var tk = db.TaiKhoans.FirstOrDefault(a => a.Email == Email);
                if (tk != null)
                {
                    // Kiểm tra mật khẩu
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(MatKhau, tk.MatKhau);
                    if (isPasswordValid)
                    {
                        // Đăng nhập thành công, đặt đánh dấu đã xác thực
                        CookieSevices.SetCookie(
                            HttpContext.ApplicationInstance.Context,
                            "MaTK", tk.MaTK.ToString()
                        );

                        // Kiểm tra vai trò và chuyển hướng đến trang tương ứng
                        if (CheckRole(tk))
                        {
                            CookieSevices.SetCookie(
                                HttpContext.ApplicationInstance.Context,"Quyen", "Admin"
                            );
                            return RedirectToAction("Home", "Sans", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Sans");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "MaTK,TenTK,MatKhau,Email,HoTen,SDT,DiaChi,Vip,DiemTich,Quyen")] TaiKhoan taiKhoan)
        {
            /*// Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (db.TaiKhoans.Any(tk => tk.TenTK == taiKhoan.TenTK))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                return View(taiKhoan);
            }
            // Kiểm tra xem email đã tồn tại chưa
            else if (db.TaiKhoans.Any(tk => tk.Email == taiKhoan.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại.");
                return View(taiKhoan);
            }
            else
            {
                
            }*/
            if (ModelState.IsValid)
            {

                try
                {
                    // Mã hóa mật khẩu trước khi lưu vào CSDL
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(taiKhoan.MatKhau);
                    // Gán mật khẩu đã được mã hóa vào đối tượng taiKhoan
                    taiKhoan.MatKhau = hashedPassword;
                    taiKhoan.MaTK = LayMaTK();
                    taiKhoan.Quyen = false;
                    taiKhoan.DiemTich = 0;
                    taiKhoan.Vip = false;

                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();
                    CookieSevices.SetCookie(
                        HttpContext.ApplicationInstance.Context,
                        "MaTK", taiKhoan.MaTK.ToString()
                   );
                    return RedirectToAction("Index", "Sans");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

            }

            return View(taiKhoan);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        // GET: TaiKhoansUser
        public ActionResult Index()
        {
            return View(db.TaiKhoans.ToList());
        }

        // GET: TaiKhoansUser/Details/5
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

        // GET: TaiKhoansUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoansUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TenTK,MatKhau,Email,HoTen,SDT,DiaChi,Vip,DiemTich,Quyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoansUser/Edit/5
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

        // POST: TaiKhoansUser/Edit/5
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
                return RedirectToAction("Details",new { id = taiKhoan.MaTK });
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoansUser/Delete/5
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

        // POST: TaiKhoansUser/Delete/5
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

        public bool CheckRole(TaiKhoan taiKhoan)
        {
            var tk = db.TaiKhoans.FirstOrDefault(a => a.Email == taiKhoan.Email && a.MatKhau == taiKhoan.MatKhau);

            if (tk != null)
            {
                // Xác thực thành công
                if (tk.Quyen == true) // Kiểm tra "Quyen" có giá trị "admin" (hoặc true) hay không
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
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
