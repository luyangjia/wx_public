using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WxPay2017.API.WEBAPI.Core
{
    public class OptionsHttpMessageHandler : DelegatingHandler
    {
        private HttpConfiguration config;
        public OptionsHttpMessageHandler(HttpConfiguration config)
        {
            this.config = config;
        }
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK));
                var apiExplorer = config.Services.GetApiExplorer();

                var headers = request.Content.Headers;//.Allow;
                var data = request.GetRouteData();
                var controllerRequested = request.GetRouteData().Values["controller"] as string;
                var supportedMethods = apiExplorer.ApiDescriptions
                    .Where(d =>
                    {
                        var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                        return string.Equals(
                            controller, controllerRequested, StringComparison.OrdinalIgnoreCase);
                    })
                    .Select(d => d.HttpMethod.Method)
                    .Distinct();

                if (!supportedMethods.Any())
                    return Task.Factory.StartNew(
                        () => request.CreateResponse(HttpStatusCode.NotFound));

                return Task.Factory.StartNew(() =>
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    //resp.Headers.Add("Access-Control-Allow-Origin", "*");
                    //resp.Headers.Add("Access-Control-Allow-Methods", string.Join(",", supportedMethods));
                    //resp.Headers.Add("Access-Control-Allow-Headers", "*, Content-Type, X-Request-With, Accept, version");

                    return resp;
                });
            }

            return base.SendAsync(request, cancellationToken);
        }

    }
}