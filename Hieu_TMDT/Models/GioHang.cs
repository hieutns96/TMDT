using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class GioHang
    {
        wthEntities db = new wthEntities();
        public List<Item> List { set; get; } //list đựng hàng
        public GioHang() { List = new List<Item>(); }

        public void DropItem(long BookID)
        {
            Item dropbook = this.List.Where(p => p.MaSach == BookID).SingleOrDefault();
            this.List.Remove(dropbook);
        }

        public void AddItem(MyBook mb)
        {
            NhaSachRepository ns = new NhaSachRepository();
            bool check = CheckItemExist(mb.MaSach);
            if (check == false)//chưa có hàng này cho vào số lượng =1
            {
                Item item = ns.ConvertToItem(mb, 1);
                this.List.Add(item);
            }
            else //đã có
                this.List.Where(p => p.MaSach == mb.MaSach).SingleOrDefault().SoLuongTrongGio += 1;
            //tăng số lượt mua lên 1
            db.SACHes.Where(p => p.MA_SACH == mb.MaSach).SingleOrDefault().SOLUOTMUA++;
            db.SaveChanges();
        }
        public bool CheckItemExist(long BookID)
        {
            Item checkitem = this.List.Where(p => p.MaSach == BookID).SingleOrDefault();
            if (checkitem == null) return false;
            return true;
        }
        public int TongSoluong()
        {
            int soluong = 0;
            foreach (Item item in this.List)
            {
                soluong += item.SoLuongTrongGio;
            }
            return soluong;
        }
        public long TongTien()
        {
            long tongTien = 0;
            foreach (Item item in this.List)
            {
                tongTien += item.GiaBan * item.SoLuongTrongGio;
            }
            return tongTien;
        }
        public string GenerateResult()
        {
            return this.TongSoluong().ToString() + " " + this.TongTien().ToString();
        }
        //public bool DatHang(NGUOI_DUNG user)
        //{
        //    //tạo hóa đơn

        //}
    }
}