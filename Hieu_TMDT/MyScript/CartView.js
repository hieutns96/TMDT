    var Item=function(manxb,masach,madanhmuc,tensach,giaban,giabia,tinhtrang,luotxem,anhbia,ngaynhap,soluotmua,soluongtronggio)
    {
        var self = this;

        self.MaNXB =ko.observable(manxb);
        self.MaDanhMuc=ko.observable(madanhmuc);
        self.MaSach=ko.observable(masach);
        self.TenSach=ko.observable(tensach);
        self.GiaBia =ko.observable(giabia);
        self.GiaBan=ko.observable(giaban);
        self.NgayNhap=ko.observable(ngaynhap);
        self.SoLuotMua =ko.observable(soluotmua);
        self.AnhBia=ko.observable(anhbia);
        self.TinhTrang=ko.observable(tinhtrang);
        self.LuotXem = ko.observable(luotxem);
        self.SoLuongTrongGio = ko.observable(soluongtronggio).extend({
            required: { params: true, message: '*số lượng không được để trống hay nhập kí tự' },
            min: { params: 1, message: '*Số lượng tối thiểu là 1' },
            max: { params: 50, message: '*Số lượng tối đa là 50' },
            number: { params: true }
        });

        this.Errors = ko.validation.group(this);
        this.isValid = ko.computed(function () {
            return self.Errors().length == 0;
        });
    }

    var CartViewModel=function()
    {
        var self = this;
        self.listcart = ko.observableArray([]);
        self.tongtien = ko.observable(0);
        self.tongsoluong = ko.observable(0);
        self.curindex = ko.observable();

        self.myValidate = ko.observable(true);

        self.InitCart = function()
        {
           
            $.ajax({
                url: '/Cart/GetCart',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    var temp = result;
                    var tongsoluong = 0;
                    var tongtien = 0;
                    var index = 0;

                    if (temp != null && temp.length > 0) {
                        var listItems = ko.utils.arrayMap(temp, function (item) {
                            //tong so luong
                            tongsoluong += item.SoLuongTrongGio;
                            //tongtien
                            tongtien += item.SoLuongTrongGio * item.GiaBan;
                            return new Item(item.MaNXB, item.MaSach, item.MaDanhMuc, item.TenSach, item.GiaBan, item.GiaBia, item.TinhTrang, item.LuotXem, item.AnhBia, item.NgayNhap,item.SoLuotMua,item.SoLuongTrongGio);
                            
                        });
                        self.tongsoluong(tongsoluong);
                        self.tongtien(tongtien);
                        self.curindex(index);
                        self.listcart.push.apply(self.listcart,listItems);
                    }
                },
                error: function (e) {
                    //alert('Lỗi không thể kết nối đến server.');
                }
            });
        }
        //tính tổng tiền, tổng số lượng
        self.onChange=function(item)
        {
        

            var tongtien = 0;
            var tongsoluong = 0;
            ko.utils.arrayForEach(self.listcart(), function (item) {

                if (parseInt(item.SoLuongTrongGio()) > 50) {
                    item.SoLuongTrongGio(50);
                }
                //tong so luong
                tongsoluong+=parseInt(item.SoLuongTrongGio());
                //tongtien
                tongtien += parseInt(item.SoLuongTrongGio()) * item.GiaBan();
            })
            self.tongtien(tongtien);
            self.tongsoluong(tongsoluong);
        }

        //xóa giỏ hàng
        self.RemoveCart=function()
        {
            var r = confirm("Bạn chắc là muốn hủy giỏ hàng");
            if (r == true) {
                //ajax xóa giỏ
                $.ajax({
                    url: '/Cart/DelCart',
                    method: "post",
                    success: function (result) {
                        if (result == 1) {
                            self.listcart.removeAll()
                            //alert("Đã hủy toàn bộ giỏ hàng");
                            $.notify("Đã hủy toàn bộ giỏ hàng","warn");
                            window.location.href = "/Cart/ViewCart";
                        }
                    },
                    error: function () { alert("Hủy bị lỗi");}
                });
            }
        }

        //xóa item
        self.RemoveItem=function(item)
        {
         
            $.ajax({
                url: '/Cart/DelBook',
                method: "post",
                data:{id:item.MaSach()},
                success: function (result) {
                   // alert(result);
                    if (result == -1) alert("Lỗi xóa item");
                    if (result == 1) { $.notify("Đã Xóa", "success",{position:"bottom"}); }
                    else {
                        window.location.href="/Cart/ViewCart";
                    }
                    self.listcart.remove(item);
                    self.onChange();
                },
                error: function (e) {
                    //alert('Lỗi xóa Item');
                }
            });
        }

        //cập nhật (save)
        self.UpdateCart=function()
        {
            var jsondata = ko.toJSON(self.listcart());
            //alert(jsondata);
                $.ajax({
                    url: '/Cart/SaveCart',
                    method: "post",
                    contentType: 'application/json; charset=utf-8',
                    data:jsondata,
                    success: function (result) {
                        if (result == 1) {
                            //alert(result);
                            //alert("Save thành công");
                            $.notify("Lưu thành công","success");
                        }
                        else { alert("Save bị lỗi");}
                    },
                    error: function () { alert("Save bị lỗi"); }
                });
        }
        self.Order = function () {
            for (var i = 0; i < self.listcart().length; i++)
            {
                if (self.listcart()[i].SoLuongTrongGio() > 50 || self.listcart()[i].SoLuongTrongGio() === '' || self.listcart()[i].SoLuongTrongGio() === '0')
                {
                    self.myValidate(false);
                    break;
                }
                self.myValidate(true);
            }
            if (self.myValidate() === true) {
                window.location.href = "/Cart/GoToOrder";
            }
        }
    }


 

