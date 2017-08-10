using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class CTHD
    {
        public long ID { get; set; }
        public long MAHD { get; set; }
        public string TENSACH { get; set; }

        public long SO_LUONG { get; set; }

        public long DON_GIA { get; set; }
    }
}