using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class ConfirmSignUpVMD
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã xác nhận")]
        public string Barcode { get; set; }
    }
}