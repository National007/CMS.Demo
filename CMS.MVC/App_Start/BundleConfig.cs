using System;
using System.Web;
using System.Web.Optimization;

namespace CMS.MVC
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/mvc/basecss").Include(
                      "~/Scripts/layui/css/layui.css?r="+ (new Random().Next(10000000)),
                      "~/css/public.css?r=" + (new Random().Next(10000000))
                      ));

            //基础js
            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
                "~/Scripts/jquery-{version}.js?r=" + (new Random().Next(10000000)),
                "~/Scripts/vue.min.js?r=" + (new Random().Next(10000000)),
                "~/Scripts/common.js?r=" + (new Random().Next(10000000))
                ));

            bundles.Add(new ScriptBundle("~/bundles/layuijs").Include(
                "~/Scripts/layui/layui.js?r=" + (new Random().Next(10000000)),
                "~/Scripts/layui/config/layui.config.js?r=" + (new Random().Next(10000000))
                ));
        }
    }
}
