﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(webapi.Startup))]

namespace webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
