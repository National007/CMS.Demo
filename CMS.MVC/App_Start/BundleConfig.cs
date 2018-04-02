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
                     "~/Scripts/layui/css/layui.css",
                     "~/css/public.css"
                     ));

            //基础js
            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/vue.min.js",
                "~/Scripts/common.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/layuijs").Include(
                "~/Scripts/layui/layui.js",
                "~/Scripts/layui/config/layui.config.js"
                ));
        }
    }
}
