var HOA_DON = function (mahd, username, idtt, diachi, hoten, dienthoai, ngaydathang, ghichu, tentt) {
    var self = this;
    self.MA_HD = ko.observable(mahd);
    self.USERNAME = ko.observable(username);
    self.IDTT = ko.observable(idtt);
    self.DIA_CHI = ko.observable(diachi);
    self.HO_TEN = ko.observable(hoten);
    self.DIEN_THOAI = ko.observable(dienthoai);
    self.NGAYDATHANG = ko.observable(ngaydathang);
    self.GHI_CHU = ko.observable(ghichu);
    self.listCTHD = ko.observableArray([]);
    self.TENTT = ko.observable(tentt);
    self.TongTien = ko.observable(0);
    self.TongSanPham = ko.observable(0);
    self.btnVisible = ko.observable(true);
    self.HDVisible = ko.observable(true);

    self.TinhTong = function () {
        var tongsoluong = 0;
        var tongtien = 0;
        ko.utils.arrayForEach(self.listCTHD(), function (item) {
            tongtien += parseInt(item.SO_LUONG() * item.DON_GIA());
            self.TongTien(tongtien);
            tongsoluong += parseInt(item.SO_LUONG());
            self.TongSanPham(tongsoluong);

        })
    }

    self.HuyCTHD = function (CTHD) {
        
        $.ajax({
            url: "/Account/XoaCTHD",
            method: "post",
            data: { id: CTHD.ID, mahd: CTHD.MAHD },
            success: function (result) {
                if (result == 1) {
                    alert("Xóa thành công");
                    self.listCTHD.remove(CTHD);
                    self.TinhTong();
                    if (self.listCTHD().length == 0) {
                        self.HDVisible(false); //tự hủy ở server
                    }
                }
                else { alert("Xóa không thành công"); }
            }
        })
    }

}
var CT_HOA_DON= function (id, mahd, tensach, soluong, dongia)
{
    var self=this;
    self.ID =ko.observable(id);
    self.MAHD=ko.observable(mahd);
    self.TENSACH=ko.observable(tensach);
    self.SO_LUONG=ko.observable(soluong);
    self.DON_GIA = ko.observable(dongia);
}

var DSHDViewModel=function()
{
    var self = this;
    self.listhd = ko.observableArray([]);
    self
    self.visiblelist = ko.observable();
    self.Init=function()
    {
        $.ajax({
            url: '/Account/LayDsDonHang',
            method:'post',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                //alert(result);
                if(result.length>0)
                {
                    var tentinhtrang = "";
                    var listItems = ko.utils.arrayMap(result, function (item) {
                        if (item.IDTT == 0) tentinhtrang = "Đơn hàng mới";
                        if (item.IDTT == 1) tentinhtrang = "Đang xử lý";
                        if (item.IDTT == 2) tentinhtrang = "Đã thanh toán";

                        var hd = new HOA_DON(item.MA_HD, item.USERNAME, item.IDTT, item.DIA_CHI, item.HO_TEN, item.DIEN_THOAI, item.NGAYDATHANG, item.GHI_CHU, tentinhtrang);
                        if (item.IDTT == 0) hd.btnVisible(true);
                        if (item.IDTT == 2 || item.IDTT == 1) hd.btnVisible(false);

                        $.ajax({
                            url: "/Account/LayListCTHD",
                            method: "post",
                            data:{mahd:hd.MA_HD()},
                            //contentType: 'application/json; charset=utf-8',
                            success:function(ctresult)
                            {
                                var list = ko.utils.arrayMap(ctresult, function (item) {
                                    //alert(item.MAHD);
                                    return new CT_HOA_DON(item.ID, item.MAHD, item.TENSACH, item.SO_LUONG, item.DON_GIA);
                                });
                                hd.listCTHD.push.apply(hd.listCTHD, list);
                                hd.TinhTong();
                                //alert(hd.listCTHD());
                            }
                        })
                        return hd;
                    });
                    self.listhd.push.apply(self.listhd, listItems);
                }
            },
            error: function (e) {
                alert('Lỗi không thể kết nối đến server.');
            }
        });
    }
    self.HuyHD=function(HD)
    {
        $.ajax({
            url: "/Account/XoaHD",
            method: "post",
            data: { id:HD.MA_HD},
            success: function (result) {
                if (result == 1) {
                    alert("Xóa thành công");
                    self.listhd.remove(HD);
                }
                else { alert("Xóa không thành công"); }
            }
        })
        
    }
    self.SaveHD=function(HD)
    {
        var x = true;
        //alert(HD.listCTHD().length);
        for(var i=0;i<HD.listCTHD().length;i++)
        {
            var json=ko.toJS(HD.listCTHD()[i]);
            //alert(json);
            $.ajax({
                url: "/Account/SaveHD",
                method: "post",
                contenType:'application/json; charset=utf-8',
                data: json ,
                success: function(result)
                {
                    if (result == 0) x = false;
                }
            })
        }
        if (x == true) alert("Cập nhật thành công");
    }
}