using Track3_Api_Interfaces_Core.Application.Requests;

namespace Track3_Api_Interfaces_Core.Application.Interfaces
{
    public interface IRedirectionsManager
    {
        public abstract void LogRedirection(RedirectionRequest request, string servicio, string metodo, int codeResp);
    }
}
