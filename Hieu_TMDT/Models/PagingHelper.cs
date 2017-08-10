using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hieu_TMDT.Models
{

    public class PageItem
    {
        public int First { get; set; }
        public int Last { get; set; }
        public bool IsLast { get; set; }
    }
    public class ProductIndex
    {
        public int FirstProduct { get; set; }
        public int LastProduct { get; set; }
    }
    public class ViewInfo
    {
        public int SoSPHienThi;
        public int SoTrangHienThi;
        public ViewInfo()
        {
            SoSPHienThi = 8;
            SoTrangHienThi = 5;
        }
    }
    public class PagingHelper
    {
        NhaSachRepository ns = new NhaSachRepository();
        wthEntities db = new wthEntities();
        public int TongSoTrang(int sosp, int sosphienthi)
        {
            int sotrang = sosp / sosphienthi;
            if (sosp % sosphienthi > 0) sotrang++;
            return sotrang;
        }
        public ProductIndex TinhDotProductCuoi(int sosp, int sosphienthi)
        {
            int sodu = sosp % sosphienthi;
            return new ProductIndex { FirstProduct = sosp - sodu, LastProduct = sosp - 1 };
        }
        public PageItem NhanPrev(int sotranghienthi, PageItem cur)
        {
            int newlast = cur.First - 1;
            int newfirst = newlast - sotranghienthi + 1;
            return new PageItem { First = newfirst, Last = newlast };
        }
        public PageItem NhanNext(int sosp, int sosphienthi, int sotranghienthi, PageItem cur)
        {
            int curlast = 0, newfirst = 0, newlast = 0;
            //tính đợt cuối
            PageItem dotcuoi = TinhDotCuoi(sosp, sosphienthi, sotranghienthi);
            if (sosp > sosphienthi)
            {
                if (cur.Last + 1 == dotcuoi.First)//đã là đợt cuối
                {
                    return new PageItem { First = cur.Last + 1, Last = dotcuoi.Last, IsLast = true };
                }
                else //chưa là đợt cuối
                {
                    curlast = cur.Last;
                    newfirst = curlast += 1;
                    newlast = newfirst + sotranghienthi - 1;
                    return new PageItem { First = newfirst, Last = newlast, IsLast = false };
                }
            }
            else
            {
                return new PageItem { First = dotcuoi.First, Last = dotcuoi.Last, IsLast = true }; //đây là đợt cuối
            }
        }
        public PageItem TinhDotCuoi(int sosp, int sosphienthi, int sotranghienthi)
        {
            PageItem result = new PageItem();
            //tính số dư
            int tongsotrang = TongSoTrang(sosp, sosphienthi);
            int sotrangdu = tongsotrang % sotranghienthi;
            result.Last = tongsotrang;
            if (sotrangdu > 0)
                result.First = tongsotrang - sotrangdu + 1;
            else //số trang dư ==0
                result.First = tongsotrang - sotranghienthi + 1;
            return result;
        }
        public ProductIndex GetProductsIndex(int pagenum, int sosp, int sosphienthi)
        {
            ProductIndex pdi = new ProductIndex();
            ProductIndex dotcuoi = TinhDotProductCuoi(sosp, sosphienthi);
            //trường hợp bình thường
            pdi.FirstProduct = (pagenum - 1) * sosphienthi;
            if (pdi.FirstProduct == dotcuoi.FirstProduct)//đây là đợt cuối
            {
                pdi.LastProduct = dotcuoi.LastProduct;
            }
            else { pdi.LastProduct = pdi.FirstProduct + sosphienthi - 1; }
            return pdi;
        }

        public PageItem InitPageItem(int sosp, int sosphienthi, int sotranghienthi)
        {
            int tongsotrang = TongSoTrang(sosp, sosphienthi);
            if (tongsotrang < sotranghienthi) //đây là đợt cuối
            {
                return new PageItem { First = 1, Last = tongsotrang, IsLast = true };
            }
            return new PageItem { First = 1, Last = sotranghienthi };
        }
        public List<MyBook> GetSearchList(string ten, long manxb, long madm, string searchby)
        {
            IEnumerable<SACH> list = db.SACHes;
            if (ten == "" && manxb == -1 && madm == -1 && searchby == "all")
            {
                return ns.ConvertSaches(list);
            }
            if (ten != "" && manxb == -1 && madm == -1 && searchby == "ten")
            {
                return ns.ConvertSaches(list.Where(p => p.TEN_SACH.ToLower().Contains(ten)).Select(p => p));
            }
            if (ten == "" && manxb == -1 && madm != -1 && searchby == "dm")
            {
                return ns.ConvertSaches(list = list.Where(p => p.MA_DM == madm).Select(p => p));
            }
            if (ten == "" && manxb != -1 && madm == -1 && searchby == "nxb")
            {
                return ns.ConvertSaches(list.Where(p => p.MA_NXB == manxb).Select(p => p));
            }

            else //search by ==advance
            {
                if (ten != "")
                {
                    list = list.Where(p => p.TEN_SACH.Contains(ten)).Select(p => p);
                }
                if (manxb != -1)
                {
                    list = list.Where(p => p.MA_NXB == manxb).Select(p => p);
                }
                if (madm != -1)
                {
                    list = list.Where(p => p.MA_DM == madm).Select(p => p);
                }
                return ns.ConvertSaches(list);
            }
        }
    }
}
