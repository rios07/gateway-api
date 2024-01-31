using Track3_Api_Interfaces_Core.Application.DATA;
using Track3_Api_Interfaces_Core.Application.Requests;

namespace Track3_Api_Interfaces_Core.Application.Interfaces
{
    public interface IRedirectionApp
    {
        public abstract object Redirect(RedirectionRequest redirectionRequest);
    }
}
