using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Yahoo.Yui.Compressor;

namespace MVCConfigurator
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include("~/Scripts/modernizr-{version}.js",new YuiCompressor())
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js"));
                
            

            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/Site.css")
                
                );
                
        }
    }
    public class YuiCompressor : IItemTransform
    {

        public string Process(string includedVirtualPath, string input)
        {
            return new JavaScriptCompressor().Compress(input);
        }
    }
}