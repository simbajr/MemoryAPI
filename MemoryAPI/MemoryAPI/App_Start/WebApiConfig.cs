﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using MyMemories;
using System.Web.Http.Dispatcher;

namespace MemoryAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{namepsace}/{controller}/{id}/{modifier}/{id2}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    id2 = RouteParameter.Optional,
                    modifier = RouteParameter.Optional
                }

            );

            //config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
