using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{
    public class MyBook
    {
        public long MaNXB { get; set; }
        public long MaDanhMuc { get; set; }
        public long MaSach { get; set; }
        public string TenSach { get; set; }
        public long GiaBia { get; set; }

        public long GiaBan { get; set; }

        public DateTime? NgayNhap { get; set; }
        public long SoLuotMua { get; set; }
        public string AnhBia { get; set; }
        public int TinhTrang { get; set; }
        public long LuotXem { get; set; }

    }
    public class CTBook : MyBook
    {
        public string TenNXB;
        public string TenDanhMuc;
    }
    public class Item : MyBook
    {
        public int SoLuongTrongGio { set; get; }
    }
    public class MyHinhAnh
    {
        public int MaHinh { get; set; }
        public string TenHinh { get; set; }
        public long MaSach { get; set; }
    }
    public class MyDanhMuc
    {
        public long MaDM { get; set; }
        public string TenDM { get; set; }
    }
    public class MyNXB
    {
        public long MaNXB { get; set; }
        public string TenNXB { get; set; }
    }
}