using Microsoft.Owin;
using Owin;
using System;
using Evote.Models;

[assembly: OwinStartupAttribute(typeof(Evote.Startup))]
namespace Evote
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
        }
    }
}
