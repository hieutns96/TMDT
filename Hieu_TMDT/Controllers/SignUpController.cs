using Hieu_TMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hieu_TMDT.Controllers
{
    public class SignUpController : Controller
    {
        //
        // GET: /SignUp/
        public ActionResult Index()
        {
            return View();
        }
        AccountHelper acc = new AccountHelper();
        public ActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }
        //sau khi đăng ký , form confirm 
        public ActionResult AfterSignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AfterSignUp(ConfirmSignUpVMD model)
        {
            try
            {
                wthEntities db = new wthEntities();
                string str = model.Barcode;
                NGUOI_DUNG user = db.NGUOI_DUNG.Where(p => p.USERNAME == model.Username && p.EMAIL == model.Email).SingleOrDefault();
                if (acc.CheckBarCode(model.Username, model.Email, model.Barcode) == true && user != null)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin vừa điền không hợp lệ");
                    return View(model);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Thông tin vừa điền không hợp lệ");
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                string password = acc.md5(model.Password);
                //chưa  tạo mã ngẫu nhiên
                NGUOI_DUNG user = new NGUOI_DUNG { USERNAME = model.Username, PASSWORD = password, EMAIL = model.Email, IDQH = 1, XACTHUC = 1, HO_TEN = model.HO_TEN, DIA_CHI = model.DIA_CHI };

                if (model.CreateAcc(user) == true)//đã tiến hành tạo
                {
                    //gửi thông tin đăng ký cho user
                    AccountHelper a = new AccountHelper();
                    string subject = "Hi Hoàng " + user.USERNAME;
                    string body = String.Format("Tài khoản của bạn có Username là {0}, Password là {1}, Mã xác nhận là {2}, Link xác nhận {3}"
                        , user.USERNAME, model.Password, user.BARCODE, "localhost/SignUp/AfterSignUp");
                    a.SendMail(user.EMAIL, subject, body);
                    //chuyển hướng tới trang xác nhận
                    return RedirectToAction("AfterSignUp", "SignUp");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Thông tin đăng ký không hợp lệ");
                return View(model);
            }
        }
	}
}