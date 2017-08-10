using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class ResetPassVMD
    {
        [Required(ErrorMessage = "Vui lòng điền đầy đủ Email")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Vui lòng điền đầy đủ Username")]
        public string Username { set; get; }
        
    }
}