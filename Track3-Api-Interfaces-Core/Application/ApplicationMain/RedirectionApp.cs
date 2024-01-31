using Application.interfaces;
using Newtonsoft.Json;
using Ocelot.Responses;
using System.Threading;
using Track3_Api_Interfaces_Core.Application.DATA;
using Track3_Api_Interfaces_Core.Application.Interfaces;
using Track3_Api_Interfaces_Core.Application.Requests;
using Track3_Api_Interfaces_Core.Consts;

namespace Track3_Api_Interfaces_Core.Application.ApplicationMain
{
    public class RedirectionApp: IRedirectionApp
    {
        private readonly IServiceHelper _sericeHelper;
        private IRedirectionsManager _manager;
        private readonly IConfiguration _config;
        //private readonly Logs
        public RedirectionApp(IHostEnvironment env, IServiceHelper serviceHelper, IRedirectionsManager manager, IConfiguration config) 
        {
            _sericeHelper = serviceHelper;
            _manager = manager;
            _config = config;
        }
        public object Redirect(RedirectionRequest redirectionRequest)
        {
            
            object respuesta = new object();
            var servicio = _config.GetSection(redirectionRequest.destino).Value;
            var metodo = _config.GetSection(redirectionRequest.endpoint).Value;
            var request = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(redirectionRequest.payload));
            int codeResp = 0;
            switch (redirectionRequest.metodo)
            {
                case HTTPMETHODS.GET:
                    respuesta = _sericeHelper.Get<object>(servicio,  metodo,redirectionRequest.argumentos, out codeResp,raiseException: true );
                    break;
                case HTTPMETHODS.POST:
                    respuesta = _sericeHelper.Post<object>(servicio, metodo, request, out codeResp, raiseException: true );
                    break;
                case HTTPMETHODS.PUT:
                    respuesta = _sericeHelper.Put<object>(servicio, metodo, redirectionRequest.payload, out codeResp, raiseException: true );
                    break;
                case HTTPMETHODS.DELETE:
                    break;
                case HTTPMETHODS.PATCH:
                    break;
            }

            redirectionRequest.response = respuesta;

            new Thread(delegate ()
            {
                _manager.LogRedirection(redirectionRequest,servicio, metodo, codeResp);
            }).Start();

            return respuesta;
        }
    }
}
