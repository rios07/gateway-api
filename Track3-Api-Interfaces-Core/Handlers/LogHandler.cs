using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Values;
using Track3_Api_Interfaces_Core.Application.Interfaces;
using Track3_Api_Interfaces_Core.Application.Requests;

namespace Track3_Api_Interfaces_Core.Handlers
{
    public class LogHandler: DelegatingHandler
    {
        private IHttpContextAccessor _httpContext;
        private IRedirectionsManager _manager;
        public LogHandler(IHttpContextAccessor httpContext, IServiceProvider services)
        {
            _manager = services.CreateScope().ServiceProvider.GetRequiredService<IRedirectionsManager>();
            _httpContext = httpContext;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var downstreamRoute = _httpContext.HttpContext.Items?.DownstreamRouteHolder()?.Route?.DownstreamRoute?.FirstOrDefault();
            HttpResponseMessage response = new HttpResponseMessage();
            RedirectionRequest redireccion = new RedirectionRequest();
            redireccion.origen = _httpContext.HttpContext.Request.Path;
            redireccion.destino = request.RequestUri.ToString();
            int codeREsp = 0;
            try
            {
                redireccion.payload = JsonConvert.DeserializeObject(await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                redireccion.payload = "";
            }
            try
            {
                response = base.SendAsync(request, cancellationToken).Result;
                redireccion.response = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                codeREsp = (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                codeREsp = 500;
                redireccion.response = ex.Message;
            }

            new Thread(delegate ()
            {
                _manager.LogRedirection(redireccion,"","",codeREsp);
            }).Start();
            
            return response;
        }
    }
}
