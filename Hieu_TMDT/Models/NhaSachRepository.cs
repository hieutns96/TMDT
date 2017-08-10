using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hieu_TMDT.Models;

namespace Hieu_TMDT.Models
{
    public class NhaSachRepository
    {
        private wthEntities db = new wthEntities();
        #region GetBook
        public List<MyBook> BestSeller()
        {
            return ConvertSaches(db.SACHes.OrderByDescending(p => p.SOLUOTMUA).Select(p => p).Take(9));
        }
        //Lấy hết sách
        public List<MyBook> GetAllBooks()
        {
            return ConvertSaches(db.SACHes); ;
        }
        //lấy sách mới
        public List<MyBook> GetNewBooks()
        {
            return ConvertSaches(db.SACHes.OrderByDescending(p => p.NGAY_NHAP).Select(p => p).Take(9));
        }
        #endregion
        #region GioHang
        //Convert sang danh sách mybook
        public List<MyBook> ConvertSaches(IEnumerable<SACH> list)
        {
            List<MyBook> result = new List<MyBook>();
            foreach (SACH item in list)
            {
                result.Add(ConvertToMyBook(item));
            }
            return result;
        }
        //Convert sang mybook
        public MyBook ConvertToMyBook(SACH sach)
        {
            MyBook x = new MyBook();
            x.MaNXB = sach.MA_NXB;
            x.MaSach = sach.MA_SACH;
            x.MaDanhMuc = sach.MA_DM;
            x.TenSach = sach.TEN_SACH;
            x.GiaBan = (long)sach.GIA_BAN;
            x.GiaBia = (int)sach.GIA_BIA;
            x.TinhTrang = (int)sach.TINHTRANG;
            x.LuotXem = (int)sach.LUOT_XEM;
            x.AnhBia = sach.ANHBIA;
            x.NgayNhap = sach.NGAY_NHAP;
            return x;
        }
        //Convert sang Item (cho vào giỏ hàng)
        public Item ConvertToItem(MyBook sach, int soluong)
        {
            Item x = new Item();
            x.MaNXB = sach.MaNXB;
            x.MaSach = sach.MaSach;
            x.MaDanhMuc = sach.MaDanhMuc;
            x.TenSach = sach.TenSach;
            x.GiaBan = (long)sach.GiaBan;
            x.GiaBia = (int)sach.GiaBia;
            x.TinhTrang = (int)sach.TinhTrang;
            x.LuotXem = (int)sach.TinhTrang;
            x.AnhBia = sach.AnhBia;
            x.NgayNhap = sach.NgayNhap;
            x.SoLuongTrongGio = soluong;
            return x;
        }
        #endregion
        #region Product
        public List<MyHinhAnh> GetListHinh(long BookID)
        {
            //List<MyHinhAnh> listResult = db.HINH_ANH.Where(p => p.MA_SACH == BookID).ToList();
            List<MyHinhAnh> result = ConvertToListMyHinh(db.HINH_ANH.Where(p => p.MA_SACH == BookID).Select(p => p).ToList());
            return result;
        }

        public MyHinhAnh ConvertToMyHinh(HINH_ANH h)
        {
            MyHinhAnh hinh = new MyHinhAnh
            {
                MaHinh = h.ID,
                TenHinh = h.HINH,
                MaSach = h.MA_SACH
            };
            return hinh;
        }
        public List<MyHinhAnh> ConvertToListMyHinh(List<HINH_ANH> list)
        {
            List<MyHinhAnh> result = new List<MyHinhAnh>();
            foreach (HINH_ANH item in list)
            {
                result.Add(ConvertToMyHinh(item));
            }
            return result;
        }
        public string Encode(string s)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string Decode(string s)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(s);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public string GetID(string s)
        {
            string ID = s.Split('=')[1];
            return ID;
        }
        public bool CheckBookExistInDB(long id)
        {
            SACH s = db.SACHes.Where(p => p.MA_SACH == id).SingleOrDefault();
            if (s == null) return false;
            return true;
        }
        public CTBook GetCTBook(long id)
        {
            //lấy mã danh mục
            var x = (from sach in db.SACHes
                     join dm in db.DANH_MUC on sach.MA_DM equals dm.MA_DM
                     join nxb in db.NHA_XUAT_BAN on sach.MA_NXB equals nxb.MA_NXB
                     select new { sach, dm, nxb }).Where(p => p.sach.MA_SACH == id).SingleOrDefault();

            CTBook ct = new CTBook
            {
                MaNXB = x.sach.MA_NXB,
                MaDanhMuc = x.sach.MA_DM,
                MaSach = x.sach.MA_SACH,
                TenSach = x.sach.TEN_SACH,
                GiaBia = (long)x.sach.GIA_BIA,
                GiaBan = (long)x.sach.GIA_BAN,
                NgayNhap = x.sach.NGAY_NHAP,
                SoLuotMua = (long)x.sach.SOLUOTMUA,
                AnhBia = x.sach.ANHBIA,
                TinhTrang = (int)x.sach.TINHTRANG,
                LuotXem = (long)x.sach.LUOT_XEM,
                TenNXB = x.nxb.TEN_NXB,
                TenDanhMuc = x.dm.TEN_DM
            };
            return ct;
        }
        #endregion
        #region Layout
        public MyNXB ConvertToMyNXB(NHA_XUAT_BAN nxb)
        {
            return new MyNXB { MaNXB = nxb.MA_NXB, TenNXB = nxb.TEN_NXB };
        }
        public List<MyNXB> ConvertToMyNXBs(IEnumerable<NHA_XUAT_BAN> list)
        {
            List<MyNXB> result = new List<MyNXB>();
            foreach (var item in list)
            {
                result.Add(ConvertToMyNXB(item));
            }
            return result;
        }
        public MyDanhMuc ConvertToMyDanhMuc(DANH_MUC dm)
        {
            return new MyDanhMuc { TenDM = dm.TEN_DM, MaDM = dm.MA_DM };
        }
        public List<MyDanhMuc> ConvertToMyDanhMucs(IEnumerable<DANH_MUC> list)
        {
            List<MyDanhMuc> result = new List<MyDanhMuc>();
            foreach (var item in list)
            {
                result.Add(ConvertToMyDanhMuc(item));
            }
            return result;
        }
        #endregion
    }
}