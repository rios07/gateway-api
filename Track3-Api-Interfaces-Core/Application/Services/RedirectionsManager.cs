using System;
using Track3_Api_Interfaces_Core.Application.Interfaces;
using Track3_Api_Interfaces_Core.Application.Requests;
using Track3_Api_Interfaces_Core.Infraestructure.Interfaces;
using Track3_Api_Interfaces_Core.Logger;

namespace Track3_Api_Interfaces_Core.Application.Services
{
    public class RedirectionsManager: IRedirectionsManager
    {
        private readonly IProcesosRepository _procesosRepository;
        private Logs logs;
        public RedirectionsManager(IProcesosRepository procesos, IWebHostEnvironment env)
        {
            _procesosRepository = procesos;
            logs = new Logs(env);
            logs.Proceso = "Errores";
            logs.Lineas = new List<string>();

        }
        public void LogRedirection(RedirectionRequest request, string servicio, string metodo, int codeResp)
        {
            try
            {
                request.response = request.response == null ? "" : request.response;
                request.payload = request.payload == null ? "" : request.payload;
                request.usuario = request.usuario == "" ? "-" : request.usuario;
                request.destino = (servicio=="" && metodo=="")? request.destino : servicio + "/" + metodo;
                _procesosRepository.AddProcesoAudit(request, codeResp);
            }
            catch(Exception ex)
            {
                logs.AddLog(ex.Message, LoggerLevels.ERROR);
                logs.GrabarLogs();
            }
        }

    }
}
