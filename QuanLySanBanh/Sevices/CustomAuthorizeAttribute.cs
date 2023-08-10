using QuanLySanBanh.Sevices;
using System;
using System.Web;
using System.Web.Mvc;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var maTKCookie = httpContext.Request.Cookies["MaTK"];
        if (maTKCookie != null && !string.IsNullOrEmpty(maTKCookie.Value))
        {
            return true; // Người dùng đã đăng nhập, cho phép truy cập vào tài nguyên
        }

        return false; // Người dùng chưa đăng nhập
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
        filterContext.Result = new RedirectToRouteResult(
            new System.Web.Routing.RouteValueDictionary(new { controller = "TaiKhoansUser", action = "Login" })
        );
    }
}