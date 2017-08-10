using Hieu_TMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hieu_TMDT.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        NhaSachRepository ns = new NhaSachRepository();
        wthEntities db = new wthEntities();
        public ActionResult ViewCart()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCart()
        {
            //giả sử đã có session
            List<Item> list = ((GioHang)(Session["cart"])).List;
            return new JsonResult() { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public int AddBookFromID(string id)//từ trang chi tiết sản phẩm
        {
            try
            {
                long BookID = long.Parse(ns.GetID(ns.Decode(id)));
                //kiểm tra tồn tại 
                bool check = ns.CheckBookExistInDB(BookID);
                if (check == true)
                {
                    MyBook mb = ns.ConvertToMyBook(db.SACHes.Where(p => p.MA_SACH == BookID).SingleOrDefault());
                    if (mb.TinhTrang == 0)//còn hàng
                    {
                        if (Session["cart"] == null) Session["cart"] = new GioHang();
                        ((GioHang)(Session["cart"])).AddItem(mb);
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // GET: Cart
        [HttpPost]
        public string AddBook(MyBook s) // trả về tổng số lượng
        {
            try
            {
                if (s.TinhTrang == 0)//sách này còn bán
                {
                    if (Session["cart"] == null) //chưa có giỏ hàng
                    {
                        Session["cart"] = new GioHang();
                        ((GioHang)Session["cart"]).AddItem(s);
                        return ((GioHang)(Session["cart"])).GenerateResult();
                    }
                    else //đã có giỏ hàng
                    {
                        ((GioHang)Session["cart"]).AddItem(s);
                        return ((GioHang)(Session["cart"])).GenerateResult();
                    }
                }
                else return ((GioHang)(Session["cart"])).GenerateResult();  //trả về số sách hiện tại trong giỏ 0 tăng thêm gì
            }
            catch (Exception) { return "0/0"; }
        }
        [HttpPost]
        public int DelBook(int id)
        {
            try
            {
                ((GioHang)Session["cart"]).DropItem(id);
                if (((GioHang)(Session["cart"])).List.Count == 0)
                {
                    Session.Remove("cart");
                    return 0;//hết hàng
                }
                return 1; //còn hàng
            }
            catch (Exception)
            {
                return -1; //lỗi}
            }
        }
        [HttpPost]
        public int DelCart()
        {
            try
            {
                Session.Remove("cart");
                return 1;
            }
            catch (Exception) { return 0; }
        }
        [HttpPost]
        public int SaveCart(List<Item> list)
        {
            try { ((GioHang)(Session["cart"])).List = list; return 1; }
            catch (Exception) { return 0; }
        }

        public ActionResult GoToOrder()
        {
            var currentUser = Session["nguoidung"] as NGUOI_DUNG;
            NGUOI_DUNG nguoidungHienTai = db.NGUOI_DUNG.FirstOrDefault(x => x.USERNAME == currentUser.USERNAME);
            HoaDonVM temp = new HoaDonVM();
            temp.HO_TEN = nguoidungHienTai.HO_TEN;
            temp.DIEN_THOAI = 0907625361;
            temp.DIA_CHI = nguoidungHienTai.DIA_CHI;
            return View(temp);
        }

        public ActionResult ThankYou()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Order(HoaDonVM model)
        {
            try
            {
                //lấy giỏ hàng
                List<Item> orders = ((GioHang)(Session["cart"])).List;
                //lấy người dùng
                NGUOI_DUNG user = (NGUOI_DUNG)(Session["nguoidung"]);
                //tạo hóa đơn
                HOA_DON hd = new HOA_DON
                {
                    USERNAME = user.USERNAME,
                    IDTT = 0,
                    NGAYDATHANG = DateTime.Now.Date,
                    DIA_CHI = model.DIA_CHI,
                    DIEN_THOAI = model.DIEN_THOAI,
                    GHI_CHU = model.GHI_CHU,
                    HO_TEN = model.HO_TEN,
                    TONG_TIEN = 0
                };
                db.HOA_DON.Add(hd);
                db.SaveChanges();
                //tạo chi tiết hóa đơn
                long MaHD = hd.MA_HD;
                long tongtien = 0;
                foreach (Item item in orders)
                {
                    CT_HOA_DON cthd = new CT_HOA_DON
                    {
                        MAHD = MaHD,
                        DON_GIA = item.GiaBan,
                        SO_LUONG = item.SoLuongTrongGio,
                        TENSACH = item.TenSach
                    };
                    tongtien += (item.SoLuongTrongGio * item.GiaBan);
                    
                    db.CT_HOA_DON.Add(cthd);
                }
                var updatedHd = db.HOA_DON.FirstOrDefault(x => x.MA_HD == hd.MA_HD);
                updatedHd.TONG_TIEN = tongtien;
                db.SaveChanges();
                
                Session.Remove("cart");
                
                return RedirectToAction("ThankYou", "Cart");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi");
                return View(model);
            }
        }

        public ActionResult Index()
        {
            
            
            return View();
        }
	}
}