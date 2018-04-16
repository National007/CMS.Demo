//var layer;

//layui.use('form', function () {
//    layer = layui.layer;
//    });

$('<div class="loaded">\
    <div class="layui-layer-shade" id= "layui-layer-shade1" times= "1" style= "z-index:19900507;background-color:rgb(0, 0, 0); opacity: 0.3;" ></div >\
    <div class="layui-layer layui-layer-dialog layui-layer-msg" id= "layui-layer1" type= "dialog" style= "z-index: 19950521; width: 200px; top: 247px; left: 780px;" >\
    <div id="" class="layui-layer-content layui-layer-padding" >\
    <i class="layui-layer-ico layui-layer-ico16" ></i> 正在加载页面…</div>\
    <span class="layui-layer-setwin" ></span ></div >\
    <div class="layui-layer-move" ></div ></div>\
<script>$(window).load(function () { $(".loaded").remove(); });$(document).ready(function(){ $(".loaded").remove(); })</script>').appendTo($(document.body))
    
    
//$(function () {
//    layui.use('form', function () {
//        layui.layer.msg('正在加载页面…', {
//                icon: 16
//            , shade: 0.3
//            , time:false
//            });
//    });
    
//});