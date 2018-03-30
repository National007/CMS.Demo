layui.define(['jquery'], function (exports) {
    var $ = layui.jquery;
    var sys = function () {
        this.config = {
            elem: "#tabrender",
            tabfilter: "tabfilter"
        };
    }
    sys.prototype.set = function (options) {
        var _this = this;
        $.extend(true, _this.config, options);
        return _this;
    }
    //layuitab 与数据表格 高度
    sys.prototype.layuitabBindDatagrid = function () {
        var _this = this;
        var elem = _this.config.elem;
        //1,找到默认被初始化，进行初始化方法
        var tabElem = $(elem).find('li');
        tabElem.each(function (i, tabItem) {
            if ($(tabItem).data('hasinit')) {
                var callback = eval($(tabItem).attr('rendFun'));
                if (typeof callback === 'functon') {
                    callback.call();
                }
            }
        })
        //2,给tab绑定事情
        layui.use('element', function () {
            var $ = layui.jquery,
                element = layui.element, //Tab的切换功能，切换事件监听等，需要依赖element模块
                tabfilter = _this.config.tabfilter;
            element.on('tab(' + tabfilter +')', function (data) {
                var thisTab = data.elem.find('li.layui-this');
                if (!$(thisTab).data('hasinit')) {
                    var callback = eval($(thisTab).attr('rendFun'));
                    if (typeof callback === 'function') {
                        callback.call();
                    }
                    $(thisTab).data('hasinit', true);
                }
            });
        })
    }

    var tabrender = new sys();
    exports('tabrender', function (options) {
        return tabrender.set(options);
    });
});