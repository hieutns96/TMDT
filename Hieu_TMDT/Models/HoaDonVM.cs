using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class HoaDonVM
    {
        [StringLength(200)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DIA_CHI { get; set; }

        [Required(ErrorMessage = "Vui lòng điền họ tên người nhận")]
        [StringLength(100)]
        public string HO_TEN { get; set; }

        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        public long DIEN_THOAI { get; set; }

        public string GHI_CHU { get; set; }
    }
}