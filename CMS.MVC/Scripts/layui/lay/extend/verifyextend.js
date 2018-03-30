layui.define(['form'], function (exports) {
    var form = layui.form;

    form.verify({
        //正整数
        integer: function (value, item) {
            var regu = /^[1-9]\d*$/;
            if (!new RegExp(regu).test(value)) {
                var msg = item.getAttribute('lay-erroMsg');
                return msg||'请输入正整数';
            }
        },
        //电话号码
        telephone: function (value, item) {
            var regu = /^0\d{2,3}-?\d{7,8}$/;
            if (!new RegExp(regu).test(value)) {
                var msg = item.getAttribute('lay-erroMsg');
                return msg || '请输入正确的电话号码！';
            }
        },
        //
        mobileOrTel: function (value, item) {
            //手机正则
            var mobileReg = /^[1][3,4,5,7,8][0-9]{9}$/;
            //固定电话正则
            var telReg = /^0\d{2,3}-?\d{7,8}$/;

            var isMobile = new RegExp(mobileReg).test(value);
            var isTelReg = new RegExp(telReg).test(value);

            if (isMobile == false && isTelReg == false) {
                var msg = item.getAttribute('lay-erroMsg');
                return msg || '请输入正确的电话号码！';
            }
        },
        //身份证号码
        CertificateNum: function (value,item) {
            var numReg = /(^\d{15}$)|(^\d{17}(x|X|\d)$)/;
            if (!new RegExp(numReg).test(value)) {
                var msg = item.getAttribute('lay-erroMsg');
                return msg || '请输入正确的身份证号码！';
            }
        }
    });
    exports('verifyextend');
});