using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hieu_TMDT;
using Hieu_TMDT.Models;

namespace Hieu_TMDT.Controllers
{
    public class ProductController : Controller
    {
        private wthEntities db = new wthEntities();
        NhaSachRepository ns = new NhaSachRepository();
        public ActionResult Product(string id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }
            long BookID = long.Parse(ns.GetID(ns.Decode(id)));
            //kiểm tra tồn tại
            if (ns.CheckBookExistInDB(BookID) == false)
            {
                return new HttpNotFoundResult();
            }
            var BookInfor = new InfoView();
            BookInfor.Id = id;
            BookInfor.tongSpTrongGio = 0;
            if (Session["cart"] != null)
            {
               // BookInfor.tongSpTrongGio = ((GioHang)Session["cart"]).TongSoluong();
            }
            return View(BookInfor);
        }
         [HttpPost]
        //lấy sách hiển thị
        public ActionResult GetMyBook(string id)// id là chuỗi mã hóa
        {
            try
            {
                //giải mã chuỗi
                long BookID = long.Parse(ns.GetID(ns.Decode(id)));
                //kiểm tra tồn tại
                if (ns.CheckBookExistInDB(BookID) == true)
                    return new JsonResult() { Data = ns.GetCTBook(BookID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                else
                    return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception) { return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
        [HttpPost]
        //lấy danh sách hình của sách 
        public ActionResult GetHinh(string id)
        {
            try
            {
                //giả mã
                long BookID = long.Parse(ns.GetID(ns.Decode(id)));
                List<MyHinhAnh> list = ns.GetListHinh(BookID);
                //kiểm tra tồn tại
                if (ns.CheckBookExistInDB(BookID) == true)
                    return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                else
                    return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            { return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
        [HttpPost]
        public string GoToProDuct(long BookID)
        {
            //kiểm tra sách này tồn tại
            SACH s = db.SACHes.Where(p => p.MA_SACH == BookID).SingleOrDefault();
            if (s != null)
            {
                try
                {
                    //tăng lượt xem
                    db.SACHes.Where(p => p.MA_SACH == BookID).SingleOrDefault().LUOT_XEM += 1;
                    db.SaveChanges();
                    //session
                    string encode = ns.Encode("BookID=" + BookID);
                    return encode;
                }
                catch (Exception) { return ""; }
            }
            return "";
        }
    

        

        
    }
}
