using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hieu_TMDT.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["nguoidung"] != null)
            {
                return View();
            }
            else
            {
               // var script = String.Format("<script> alert{'Bạn Chưa Đăng Nhập'}");
                //return new JavaScriptResult { Script = "alert('Bạn chưa đăng nhập');" };
               // return Redirect("../Home/Index");
                return View();
            }
        }
    }
}