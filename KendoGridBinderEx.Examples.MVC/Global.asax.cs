﻿using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using FluentValidation.Mvc;
using KendoGridBinderEx.Examples.MVC.Unity;
using StackExchange.Profiling;

namespace KendoGridBinderEx.Examples.MVC
{
    public class MvcApplication : HttpApplication
    {
        private readonly bool _miniProfilerEnabled = ApplicationConfig.MiniProfilerEnabled;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new MyDateTimeBinder());

            InitAutoMapper();

            if (_miniProfilerEnabled)
            {
                MiniProfilerEF.Initialize();
            }

            UnityBootstrapper.Initialise();

            FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_BeginRequest()
        {
            if (_miniProfilerEnabled)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            if (_miniProfilerEnabled)
            {
                MiniProfiler.Stop();
            }
        }

        private static void InitAutoMapper()
        {
            // Call static method 'InitAutoMapper' on all controllers.
            var assemblies = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName.Contains("KendoGridBinderEx.Examples.MVC.Controllers") && t.Name.EndsWith("Controller")).ToList();
            foreach (var controller in assemblies)
            {
                var methodInfo = controller.GetMethod("InitAutoMapper");

                if (methodInfo != null)
                {
                    methodInfo.Invoke(controller, new object[] { });
                }
            }

            Mapper.AssertConfigurationIsValid();
        }
    }
}