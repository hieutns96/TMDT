using Hieu_TMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hieu_TMDT.Controllers
{
    public class SearchADVController : Controller
    {
        // GET: SearchADV
        public ActionResult SearchADV()
        {

            var CatalogInfor = new InfoView();
            CatalogInfor.tongSpTrongGio = 0;
            if (Session["cart"] != null)
            {
                CatalogInfor.tongSpTrongGio = ((GioHang)Session["cart"]).TongSoluong();
            }
            return View(CatalogInfor);
        }
        [HttpPost]
        //public ActionResult Search(string tensach,long madm,long nxb,long min,long max)
        public ActionResult Search(string tensach, long madm, long nxb, long min, long max)
        {
            wthEntities db = new wthEntities();
            IEnumerable<SACH> list = db.SACHes.ToList();
            if (tensach != "")
            {
                list = list.Where(p => p.TEN_SACH.ToLower().Contains(tensach.ToLower())).Select(p => p);
            }
            if (madm != -1)
            {
                list = list.Where(p => p.MA_DM == madm).Select(p => p);
            }
            if (nxb != -1)
            {
                list = list.Where(p => p.MA_NXB == nxb).Select(p => p);
            }
            list = list.Where(p => p.GIA_BAN >= min && p.GIA_BAN <= max).Select(p => p);
            NhaSachRepository ns = new NhaSachRepository();
            list = list.ToList();
            return new JsonResult { Data = ns.ConvertSaches(list), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //
        // GET: /SearchADV/
        public ActionResult Index()
        {
            return View();
        }
	}
}