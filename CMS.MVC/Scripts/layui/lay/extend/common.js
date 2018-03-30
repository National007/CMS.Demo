layui.define(['jquery'], function (exports) {
    var $ = layui.jquery;
    //form serialize
    $.fn.extend({
        serializeObject: function () {
            var that = this;
            if (that[0].tagName !== 'FORM') {
                var elems = that.find('[name]').clone();
                that = $('<form>').append(elems);
            }
            var a, o, h, i, e;
            a = that.serializeArray();
            o = {};
            h = o.hasOwnProperty;
            for (i = 0; i < a.length; i++) {
                e = a[i];
                if (!h.call(o, e.name)) {
                    o[e.name] = e.value;
                }
            }
            return o;
        },
        otherSerializeObject: function () {

        },
        loadObject: function (data) {
            if (typeof data === 'string') {
                //url
            } else {
                loadData(this, data);
            }
        },
    });
    function loadData(othis, data) {
        for (var name in data) {
            var val = data[name];
            //dealwith checkbox,radio
            var rr = $(othis).find('input[name=' + name + '][type=checkbox]', 'input[name=' + name + '][type=radio]');
            rr.prop('checked', false);
            rr.each(function () {
                //null,undefined,0,'',false,'N','false' = false else true
                if (val === "N" || val === "false" || val === "" || val === "0") {
                    val = false;
                } else if (val === "Y" || val === "true" || val === "1") {
                    val = true;
                }
                $(this).prop('checked', Boolean(val));
            });
            if (!rr.length) {
                $('input[name=' + name + ']', othis).val(val);
                $('textarea[name=' + name + ']', othis).val(val);
                $('select[name=' + name + ']', othis).val(val);
                //extends load span and add formatter callback
                var _span = $('span[name=' + name + ']', othis)
                var formatter = eval(_span.data("formatter"));
                if (typeof formatter === 'function') {
                    val = formatter.call(null, val, null);
                }
                _span.html(val);
            }
        }
    }
    (function (original) {
        $.fn.clone = function () {
            var result = original.apply(this, arguments),
                my_textareas = this.find('textarea').add(this.filter('textarea')),
                result_textareas = result.find('textarea').add(result.filter('textarea')),
                my_selects = this.find('select').add(this.filter('select')),
                result_selects = result.find('select').add(result.filter('select'));

            for (var i = 0, l = my_textareas.length; i < l; ++i) $(result_textareas[i]).val($(my_textareas[i]).val());
            for (var i = 0, l = my_selects.length; i < l; ++i) result_selects[i].selectedIndex = my_selects[i].selectedIndex;

            return result;
        };
    })($.fn.clone);
    //exports
    exports('common');
});