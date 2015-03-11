using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using ServiceStack;

namespace SSTest
{

    public class Global : System.Web.HttpApplication
    {
      
        public class AppHost : AppHostBase
        {
            //Tell ServiceStack the name of your application and where to find your services
            public AppHost() : base("Hello Web Services", typeof(AddressSearchService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        //Initialize your application singleton
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
      
    }
}