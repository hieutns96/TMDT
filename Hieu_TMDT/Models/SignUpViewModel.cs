using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class SignUpViewModel
    {
        AccountHelper acc = new AccountHelper();
        #region form
        wthEntities db = new wthEntities();
        [Required(ErrorMessage = "Vui lòng điền Username")]
        public string Username { set; get; }
        [Required(ErrorMessage = "Vui lòng điền email")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Vui lòng điền nhập lại địa chỉ email")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        [Compare("Email", ErrorMessage = "Email nhập lại không khớp với Emai đã nhập")]
        public string ReEmail { set; get; }

        [MinLength(6, ErrorMessage = "Vui lòng nhập mật khẩu có độ dài ít nhất là 6")]
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password nhập lại không khớp với password đã nhập")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Vui lòng điền họ tên")]
        public string HO_TEN { get; set; }

        public string DIA_CHI { get; set; }
        #endregion
        //tạo tài khoản mới Sign up
        public bool CreateAcc(NGUOI_DUNG x)
        {
            x.BARCODE = acc.RandomString(10);
            try
            {
                db.NGUOI_DUNG.Add(x);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }   
    }
}