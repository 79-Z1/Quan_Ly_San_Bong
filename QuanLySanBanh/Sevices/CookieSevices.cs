using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLySanBanh.Sevices
{
    public class CookieSevices
    {
        public static void SetCookie(HttpContext context, string key, string value)
        {
            var cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.MinValue;
            context.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(HttpContext context, string key)
        {
            string value = string.Empty;

            var cookie = context.Request.Cookies[key];

            if (cookie != null)
            {
                if (string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return value;
                }
                value = cookie.Value;
            }

            return value;
        }

        public static void DeleteCookie(HttpContext context, string key)
        {
            if (context.Request.Cookies[key] != null)
            {
                var cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddDays(-1) // Đặt ngày hết hạn là ngày đã qua
                };
                context.Response.Cookies.Add(cookie);
            }
        }
    }
}