using Hieu_TMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hieu_TMDT.Controllers
{
    public class LoginController : Controller
    {
        AccountHelper acc = new AccountHelper();
        wthEntities db = new wthEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // nếu thỏa các điều kiện về điền đầy đủ form, độ dài vv
            {
                int check = model.CheckAccount(model.user); //NGUOI_DUNG
                if (check == 2) //đã xác thực
                {
                    model.user.EMAIL = db.NGUOI_DUNG.Where(p => p.USERNAME == model.user.USERNAME).SingleOrDefault().EMAIL;
                    Session["nguoidung"] = model.user;
                    return RedirectToAction("Index", "Home");
                }
                else if (check == 1) // chưa xác thực
                {
                    return RedirectToAction("AfterSignUp", "SignUp");
                }
                //chưa có tài khoản
                ModelState.AddModelError("", "Thông tin vừa nhập bị sai");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Thông tin vừa nhập bị sai");
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(ResetPassVMD model)
        {
            if (ModelState.IsValid)
            {
                if (acc.CheckResetPass(model.Username, model.Email) == true)
                {
                    string newpass = db.NGUOI_DUNG.Where(p => p.USERNAME == model.Username).SingleOrDefault().PASSWORD;
                    db.NGUOI_DUNG.Where(p => p.USERNAME == model.Username).SingleOrDefault().PASSWORD = acc.md5(newpass);
                    db.SaveChanges();
                    string subject = "Rosie Million Store: Reset Password";
                    string body = "Chào " + model.Username + "Mật khẩu mới của bạn là: " + newpass;
                    acc.SendMail(model.Email, subject, body);
                    return RedirectToAction("Login", "Login");
                }
                ModelState.AddModelError("", "Thông tin điền vào không hợp lệ");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Email, tên đăng nhập không tồn tại.");
                return View(model);
            }
        }
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }
	}
}