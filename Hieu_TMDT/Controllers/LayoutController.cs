using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hieu_TMDT.Models;

namespace Hieu_TMDT.Controllers
{
    public class LayoutController : Controller
    {
        //
        // GET: /Layout/
        public ActionResult Index()
        {
            return View();
        }
        NhaSachRepository ns = new NhaSachRepository();
        wthEntities db = new wthEntities();
        // GET: Layout
        [HttpGet]
        public ActionResult GetSliders()
        {
            List<SLIDER> sliders = db.SLIDERs.ToList();
            return new JsonResult() { Data = db.SLIDERs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult GetListDanhMuc()
        {
            return new JsonResult { Data = ns.ConvertToMyDanhMucs(db.DANH_MUC), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult GetListNXB()
        {
            return new JsonResult { Data = ns.ConvertToMyNXBs(db.NHA_XUAT_BAN), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // 1=tên,2 mã dm, 3 mã NXB
        [HttpPost]
        public string EncodeName(string s)
        {
            return ns.Encode("TENSACH=" + s) + "1";
        }
        [HttpPost]
        public string EncodeDM(string s)
        {
            return ns.Encode("MADM=" + s) + "2";
        }
        [HttpPost]
        public string EncodeNXB(string s)
        {
            return ns.Encode("MANXB=" + s) + "3";
        }
        
	}
}