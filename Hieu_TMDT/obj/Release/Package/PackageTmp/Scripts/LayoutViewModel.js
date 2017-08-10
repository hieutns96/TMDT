var Slider = function (id, hinh, title, body) {
    var self = this;
    self.Id = id;
    self.Hinh = hinh;
    self.Title = title;
    self.Body = body;
}

var SliderModel = function () {
    var self = this;
    //var self.searchname=self;

    var listslider = ko.observableArray();

    self.InitSlider = function () {
        $.ajax({
            url: '/Layout/GetSliders',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                var temp = result
                if (temp != null && temp.length > 0) {
                    var listItems = ko.utils.arrayMap(temp, function (item) {
                        return new Slider(item.ID, item.HINH, item.TITLE, item.BODY);
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

}