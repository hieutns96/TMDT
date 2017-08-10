var CTBook = function (manxb, masach, madanhmuc, tensach, giaban, giabia, tinhtrang, luotxem, anhbia, ngaynhap, soluotmua, tennxb, tendanhmuc) {
    var self = this;
    self.MaNXB = ko.observable(manxb);
    self.MaDanhMuc = ko.observable(madanhmuc);
    self.MaSach = ko.observable(masach);
    self.TenSach = ko.observable(tensach);
    self.GiaBia = ko.observable(giabia);
    self.GiaBan = ko.observable(giaban);
    self.NgayNhap = ko.observable(ngaynhap);
    self.SoLuotMua = ko.observable(soluotmua);
    self.AnhBia = ko.observable(anhbia);
    self.TinhTrang = ko.observable(tinhtrang);
    self.LuotXem = ko.observable(luotxem);
    self.TenDanhMuc = ko.observable(tendanhmuc);
    self.TenNXB = ko.observable(tennxb);
    self.TenTinhTrang = ko.observable("");
}

var MyHinhAnh = function (mahinh, tenhinh, masach) {
    var self = this;
    self.MaHinh = ko.observable(mahinh);
    self.TenHinh = ko.observable(tenhinh);
    self.MaSach = ko.observable(masach);
}
var ProductViewModel = function (BookID, tongSpTrongGio) {
    var self = this;
    self.cur_anhbia = ko.observable("");

    self.book = ko.observable("");
    self.listhinh = ko.observableArray([]);
    self.tongsoluong = ko.observable(tongSpTrongGio);
    self.Init = function () {
        //alert(init);
        var jsondata = ko.toJSON(BookID);
        //get book
        $.ajax({
            url: '/Product/GetMyBook',
            method: "post",
            data: { id: BookID },
            success: function (result) {
                self.book(new CTBook(result.MaNXB, result.MaSach, result.MaDanhMuc, result.TenSach, result.GiaBan,
                                    result.GiaBia, result.TinhTrang, result.LuotXem, result.AnhBia,
                                    result.NgayNhap, result.SoLuotMua, result.TenNXB, result.TenDanhMuc));
                if (self.book().TinhTrang() == 0) self.book().TenTinhTrang("Còn hàng");
                else self.book().TenTinhTrang("Hết hàng");
                self.cur_anhbia(self.book().AnhBia());
            },
            error: function (e) {
                alert('Lỗi không thể kết nối đến server.');
            }
        });
        $.ajax({
            url: '/Product/GetHinh',
            method: "post",
            data: { id: BookID },
            success: function (result) {
                var temp = result;
                var listItems = ko.utils.arrayMap(temp, function (item) {
                    //alert(item.MaHinh);
                    return new MyHinhAnh(item.MaHinh, item.TenHinh, item.MaSach);
                });
                self.listhinh.push.apply(self.listhinh, listItems);
            }
            ,
            error: function (e) {
                //alert('Lỗi không thể kết nối đến server.');
            }
        });
    }
    self.Sorry = function () {
        $.notify("Xin lỗi bạn chưa đăng nhập nên không thể mua!", "warn");
    }
    self.AddCart = function (CTBook) {
        //alert(self.book().TinhTrang());
        if (self.book().TinhTrang() == 0) {
            $.ajax({
                url: '/Cart/AddBookFromID',
                method: "post",
                data: { id: BookID },
                success: function (result) {
                    $.notify("Đã thêm vào giỏ hàng!", "success", "top");
                    var temp = self.book().SoLuotMua();
                    temp = self.book().SoLuotMua(temp + 1);
                    self.tongsoluong(self.tongsoluong() + 1);
                }
                ,
                error: function (e) {
                }
            });
        }
        else alert("Sách này đã hết");
    }
    self.ChangeImage = function (Item) {
        self.cur_anhbia(Item.TenHinh());
    }

}
