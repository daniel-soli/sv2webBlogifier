using Blogifier.Core.Data;
using Blogifier.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Core
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        
        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string visitorId = context.Request.Cookies["VisitorId"];
            string visitorIdCreated = context.Request.Cookies["VisitorIdCreated"];
            //DateTime VisitorIdCreatedDT = DateTime.Parse(visitorIdCreated);
            
            if (visitorId == null)
            {
                //don the necessary staffs here to save the count by one
                context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
                context.Response.Cookies.Append("VisitorIdCreated", DateTime.Now.ToString("dd-MM-yyyy"), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
            }
            else if (visitorId != null && visitorIdCreated != null && (DateTime.Now - DateTime.Parse(visitorIdCreated)).TotalDays > 2)
            {
                context.Response.Cookies.Delete("VisitorIdCreated");
                context.Response.Cookies.Delete("VisitorId");
                context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
                context.Response.Cookies.Append("VisitorIdCreated", DateTime.Now.ToString("dd-MM-yyyy"), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
            }

            await _requestDelegate(context);
        }
    }
}
