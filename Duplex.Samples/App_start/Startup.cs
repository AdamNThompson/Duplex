﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Duplex.Samples.ViewModels;
using Duplex;
using System.Web.Routing;
using SignalR.Hosting.AspNet.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Startup), "Start")]

namespace Duplex
{
    public class Startup
    {
        public static void Start()
        {
            RouteTable.Routes.MapConnection<TestViewModel>("test", "test/{*operation}");
        }
    }
}