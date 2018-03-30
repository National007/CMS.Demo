//工具
var tools = {
    //验证
    verify: {
        //手机或电话
        mobileOrTel: function (value) {
            //手机正则
            var mobileReg = /^[1][3,4,5,7,8][0-9]{9}$/;
            //固定电话正则
            var telReg = /^0\d{2,3}-?\d{7,8}$/;

            var isMobile = new RegExp(mobileReg).test(value);
            var isTelReg = new RegExp(telReg).test(value);

            if (isMobile == false && isTelReg == false) {
                return { status: false, msg: '请输入正确的电话号码！' };
            } else {
                return { status: true, msg: '验证通过！' };
            }
        },

        CertificateName: function(value) {
            var nameReg = /.*(身份证).*/;
            var isNameLike = new RegExp(nameReg).test(value);
            if (isNameLike== false) {
                return { status: false, msg: '' };
            } else {
                return { status: true, msg: '' };
            }
        },

        CertificateNum: function (value) {
            var numReg = /(^\d{15}$)|(^\d{17}(x|X|\d)$)/;
            var isNameLike = new RegExp(numReg).test(value);
            if (isNameLike == false) {
                return { status: false, msg: '请输入正确的身份证号码' };
            } else {
                return { status: true, msg: '' };
            }
        }
    },
    //获取字符串字节长度  一个中文两个字节；数字英文等为一个字节
    GetStringLength: function (str) {
        return str.replace(/[\u0391-\uFFE5]/g, "aa").length;  //先把中文替换成两个字节的英文，再计算长度
    }
}