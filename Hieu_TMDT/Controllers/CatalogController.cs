using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hieu_TMDT.Models;




namespace Hieu_TMDT.Controllers
{
    public class CatalogController : Controller
    {
        PagingHelper helper = new PagingHelper();
        wthEntities db = new wthEntities();
        NhaSachRepository ns = new NhaSachRepository();
        #region hiển thị


        //public ActionResult ViewCatalog()
        //{
        //    var CatalogInfor = new InfoView();
        //    CatalogInfor.tongSpTrongGio = 0;
        //    if (Session["cart"] != null)
        //    {
        //        CatalogInfor.tongSpTrongGio = ((GioHang)Session["cart"]).TongSoluong();
        //    }
        //    return View(CatalogInfor);
        //}

        public ActionResult ViewCatalog(string id)
        {
            var CatalogInfor = new InfoView();
            CatalogInfor.Id = id;
            CatalogInfor.tongSpTrongGio = 0;
            if (Session["cart"] != null)
            {
                CatalogInfor.tongSpTrongGio = ((GioHang)Session["cart"]).TongSoluong();
            }
            return View(CatalogInfor);
        }

        //tính toán trang
        [HttpPost]
        public ActionResult InitPageItem(string searchname, string madm, string manxb, string searchby)
        {
            List<MyBook> list = PhanTich(searchname, manxb, madm, searchby);
            int sosp = list.Count();

            ViewInfo info = new ViewInfo();
            PageItem pi = helper.InitPageItem(sosp, info.SoSPHienThi, info.SoTrangHienThi);
            return new JsonResult { Data = pi, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult GetNextPageItem(int curfirst, int curlast, bool islast, string searchname, string madm, string manxb, string searchby)
        {
            PageItem cur = new PageItem { First = curfirst, Last = curlast, IsLast = islast };

            List<MyBook> list = PhanTich(searchname, manxb, madm, searchby);
            int sosp = list.Count();

            ViewInfo info = new ViewInfo();
            //nếu đây đã là trang cuối
            if (cur.IsLast == true)
                return new JsonResult { Data = cur, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            PageItem next = helper.NhanNext(sosp, info.SoSPHienThi, info.SoTrangHienThi, cur);
            return new JsonResult { Data = next, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult GetPrevPageItem(int curfirst, int curlast, bool islast, string searchname, string madm, string manxb, string searchby)
        {
            PageItem cur = new PageItem { First = curfirst, Last = curlast, IsLast = islast };
            List<MyBook> list = PhanTich(searchname, manxb, madm, searchby);
            int sosp = list.Count();

            ViewInfo info = new ViewInfo();
            //nếu đây là trang đầu
            if (cur.First == 1)
                return new JsonResult { Data = cur, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            PageItem prev = helper.NhanPrev(info.SoTrangHienThi, cur);
            return new JsonResult { Data = prev, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        //trả về sản phẩm
        public ActionResult InitListBook(int pagenum, string searchname, string madm, string manxb, string searchby)
        {
            try
            {
                List<MyBook> list = PhanTich(searchname, manxb, madm, searchby);
                int sosp = list.Count();

                ViewInfo info = new ViewInfo();
                //tính toán first last product
                ProductIndex pdi = helper.GetProductsIndex(pagenum, sosp, info.SoSPHienThi);
                list = list.Skip(pdi.FirstProduct).Take(pdi.LastProduct - pdi.FirstProduct + 1).ToList();
                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception) { return new JsonResult { Data = new List<MyBook>(), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
        public List<MyBook> PhanTich(string searchname, string manxb, string madm, string searchby)
        {
            List<MyBook> list = new List<MyBook>();
            if (searchby == "ten")
            {
                searchname = ns.GetID(ns.Decode(searchname));
                list = helper.GetSearchList(searchname, long.Parse(manxb), long.Parse(madm), searchby);
            }
            if (searchby == "dm")
            {
                int mdm = int.Parse(ns.GetID(ns.Decode(madm)));
                list = helper.GetSearchList(searchname, long.Parse(manxb), mdm, searchby);
            }
            if (searchby == "nxb")
            {
                int mnxb = int.Parse(ns.GetID(ns.Decode(manxb)));
                list = helper.GetSearchList(searchname, mnxb, long.Parse(madm), searchby);
            }
            return list;
        }
        #endregion
        
        //
        // GET: /Catalog/
        public ActionResult Index()
        {
            return View();
        }
	}
}