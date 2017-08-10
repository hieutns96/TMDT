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
var PageItem = function (first, last, islast) {
    var self = this;
    self.First = first;
    self.Last = last;
    self.IsLast = islast;


}
var Page = function (pagenum) {
    var self = this;
    self.PageNum = ko.observable(pagenum);
}


var CatalogViewModel = function () {
    var self = this;
    self.rvsb = ko.observable(false);
    self.cur_searchname = ko.observable("");
    self.cur_searchby = ko.observable("");
    self.cur_madm = ko.observable(-1);
    self.cur_manxb = ko.observable(-1);

    self.curPageItem = ko.observable();
    self.listpage = ko.observableArray(); //list trang
    self.listbook = ko.observableArray(); //list sách
    self.preve = ko.observable(false);
    self.nexte = ko.observable(false);

    self.tongsoluong = ko.observable(0);

    self.InitModel = function (search, tongSoLuong) {
        //alert(search.slice(-1));
        if (search.slice(-1) == 1) //tên
        {
            self.cur_searchname(search.substring(0, search.length - 1));
            self.cur_searchby("ten");
            //alert("ten:"+self.cur_searchname()+ "madm:"+self.cur_madm()+"manxb:" + self.cur_manxb() +"searchby:"+self.cur_searchby());
        }
        if (search.slice(-1) == 2) //mã danh mục
        {
            self.cur_madm(search.substring(0, search.length - 1));
            self.cur_searchby("dm")
            //alert("ten:" + self.cur_searchname() + "madm:" + self.cur_madm() + "manxb:" + self.cur_manxb() + "searchby:" + self.cur_searchby());
        }
        if (search.slice(-1) == 3) //mã nhà xuất bản
        {
            self.cur_manxb(search.substring(0, search.length - 1));
            self.cur_searchby("nxb");
            //alert("ten:" + self.cur_searchname() + "madm:" + self.cur_madm() + "manxb:" + self.cur_manxb() + "searchby:" + self.cur_searchby());
        }
        self.tongsoluong(tongSoLuong);
        self.InitPageItem();

    }

    self.InitPageItem = function () {
        //alert(self.cur_searchname())
        $.ajax({
            url: "/Catalog/InitPageItem",
            method: "post",
            data: {
                searchname: self.cur_searchname(),
                madm: self.cur_madm(),
                manxb: self.cur_manxb(),
                searchby: self.cur_searchby()
            },
            success: function (result) {
                var first = result.First;
                var last = result.Last;
                self.curPageItem(result);
                self.InitListPage(first, last);
            }
        })
        self.InitListBook(1);
    }
    self.InitListPage = function (first, last) {
        self.listpage.removeAll();
        for (var pagenum = first; pagenum <= last; pagenum++) {
            //alert(pagenum);
            self.listpage.push(new Page(pagenum));
        }
    }
    self.Next = function () {
        $.ajax({
            url: "/Catalog/GetNextPageItem",
            method: "post",
            data: {
                curfirst: self.curPageItem().First,
                curlast: self.curPageItem().Last,
                islast: self.curPageItem().IsLast,
                searchname: self.cur_searchname(),
                madm: self.cur_madm(),
                manxb: self.cur_manxb(),
                searchby: self.cur_searchby()
            },
            success: function (result) {
                self.curPageItem(result); //gán lại curpageitem
                self.InitListPage(result.First, result.Last);
            }
        })
    }
    self.Prev = function () {
        var jsondata = ko.toJSON(self.curPageItem());
        //alert(jsondata);
        $.ajax({
            url: "/Catalog/GetPrevPageItem",
            method: "post",
            data: {
                curfirst: self.curPageItem().First,
                curlast: self.curPageItem().Last,
                islast: self.curPageItem().IsLast,
                searchname: self.cur_searchname(),
                madm: self.cur_madm(),
                manxb: self.cur_manxb(),
                searchby: self.cur_searchby()
            },
            success: function (result) {
                self.InitListPage(result.First, result.Last);
            }
        })
    }
    self.PageClick = function (Page) {
        self.listbook.removeAll();
        self.InitListBook(Page.PageNum());
        for (var i = self.curPageItem().First; i <= self.curPageItem().Last; i++) {
            $('#' + i).css('font-weight', 'normal');
        }
    }
    self.InitListBook = function (PageNum) {
        $.ajax({
            url: "/Catalog/InitListBook",
            data: {
                pagenum: PageNum,
                searchname: self.cur_searchname(),
                madm: self.cur_madm(),
                manxb: self.cur_manxb(),
                searchby: self.cur_searchby()
            },
            success: function (result) {
                if (result.length == 0) {
                    $("#result").html("<center><span style='font-size:20px;'>Không có sản phẩm nào</span></center>");
                    self.rvsb(true);
                }
                else {
                    self.preve(true);
                    self.nexte(true);
                    var listItems = ko.utils.arrayMap(result, function (item) {
                        return new MyBook(item.MaNXB, item.MaSach, item.MaDanhMuc, item.TenSach, item.GiaBan, item.GiaBia, item.TinhTrang, item.LuotXem, item.AnhBia, item.NgayNhap)
                    });
                    self.rvsb(true);
                    self.listbook.push.apply(self.listbook, listItems);
                    $('#' + PageNum).css('font-weight', 'bold');
                }
            }
        })
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

    self.Sorry = function (MyBook) {
        $.notify("Xin lỗi bạn chưa đăng nhập nên không thể mua!", "warn");
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
                    $.notify("Đã thêm vào giỏ hàng!", "success", "top");
                    // alert(result);
                    var str = result;
                    var res = str.split(" ");
                    self.tongsoluong(self.tongsoluong() + 1);
                    //self.tongsoluong(res[0]);
                    //self.tongtien(res[1]);
                },
                error: function (e) {
                    alert('Lỗi không thể kết nối đến server.');
                }
            });
        }
        else { alert("Sách này đã ngưng bán!"); }
    }
}