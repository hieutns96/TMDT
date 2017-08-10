var MyBook = function (manxb, masach, madanhmuc, tensach, giaban, giabia, tinhtrang, luotxem, anhbia, ngaynhap, soluotmua) {
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
}
var HomeViewModel = function () {
    var self = this;
    self.listnew = ko.observableArray([]);
    self.listsell = ko.observableArray([]);
    self.tongtien = ko.observable(0);
    self.tongsoluong = ko.observable(0);

    self.InitSell = function () {
        $.ajax({
            url: '/Home/GetBestSeller',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                var temp = result.Data;
                if (temp != null && temp.length > 0) {
                    var listItems = ko.utils.arrayMap(temp, function (item) {
                        ///alert(item.MaSach);
                        return new MyBook(item.MaNXB, item.MaSach, item.MaDanhMuc, item.TenSach, item.GiaBan, item.GiaBia, item.TinhTrang, item.LuotXem, item.AnhBia, item.NgayNhap)
                    });
                    self.listsell.push.apply(self.listsell, listItems);
                    //if (self.List().length > 0)    self.Current(self.List()[0]);
                }
            },
            error: function (e) {
                //alert('Lỗi không thể kết nối đến server.');
            }
        });
    }

    self.InitNew = function () {
        $.ajax({
            url: '/Home/GetNewBooks',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                var temp = result.Data;
                if (temp != null && temp.length > 0) {
                    var listItems = ko.utils.arrayMap(temp, function (item) {
                        //alert(item.MaSach);
                        return new MyBook(item.MaNXB, item.MaSach, item.MaDanhMuc, item.TenSach, item.GiaBan, item.GiaBia, item.TinhTrang, item.LuotXem, item.AnhBia, item.NgayNhap)
                    });
                    self.listnew.push.apply(self.listnew, listItems);
                    //if (self.List().length > 0)    self.Current(self.List()[0]);
                }
            },
            error: function (e) {
                //alert('Lỗi không thể kết nối đến server.');
            }
        });
    }
    self.Sorry = function (MyBook) {
        $.notify("Xin lỗi bạn chưa đăng nhập nên không thể mua!", null, null, "warn");
    }
    self.AddCartClick = function (MyBook) {
        if (MyBook.TinhTrang() == 0) {
            jsondata = ko.toJSON(MyBook);
            //alert(jsondata);
            $.ajax({
                url: '/Cart/AddBook',
                method: 'post',
                contentType: 'application/json; charset=utf-8',
                data: jsondata,
                success: function (result) {
                    // alert(result);
                    var str = result;
                    var res = str.split(" ");
                    self.tongsoluong(res[0]);
                    self.tongtien(res[1]);
                    $.notify("Đã thêm vào giỏ hàng!", "success", "top");
                },
                error: function (e) {
                    alert('Lỗi không thể kết nối đến server.');
                }
            });
        }
        else { //alert("Sách này đã ngưng bán!"); 
            $.notify("Sách này đã ngưng bán!", "warn");
        }
    }
    self.InitTT = function (tsl, tt) {
        self.tongtien(tt);
        self.tongsoluong(tsl);
    }
    self.GoToProduct = function (item) {
        var tensach = item.TenSach().replace(/\ /g, '-')
        var link = "/Product/Product/";
        //tăng số lượt xem lên 1
        $.ajax({
            url: '/Product/GoToProDuct',
            method: "post",
            data: { BookID: item.MaSach() },
            success: function (result) {
                if (result == "") alert('Lỗi không thể kết nối đến server.');
                else {

                    window.location.href = link + result + "/" + tensach + "/";
                }
            },
            error: function (e) {
                alert('Lỗi không thể kết nối đến server.');
            }
        });
    }
}





