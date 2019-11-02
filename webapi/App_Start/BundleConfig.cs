using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace webapi
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
             bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Site.css"));
        }
    }
}
