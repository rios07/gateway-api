using Track3_Api_Interfaces_Core.Application.Requests;

namespace Track3_Api_Interfaces_Core.Infraestructure.Interfaces
{
    public interface IProcesosRepository
    {
        public abstract string AddProcesoAudit(RedirectionRequest request, int codeResp);
    }
}
