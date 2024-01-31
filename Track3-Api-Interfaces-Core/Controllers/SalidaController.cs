using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Track3_Api_Interfaces_Core.Application.ApplicationMain;
using Track3_Api_Interfaces_Core.Application.DATA;
using Track3_Api_Interfaces_Core.Application.Interfaces;
using Track3_Api_Interfaces_Core.Application.Requests;

namespace Track3_Api_Interfaces_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalidaController : ControllerBase
    {
        private readonly IRedirectionApp _redirection;
        public SalidaController(IRedirectionApp redirection)
        {
            _redirection = redirection;
        }

        [HttpPost("redirect")]
        public object Post([FromBody] RedirectionRequest solicitudDeRedireccion)
        {
            BaseResult<object> respuesta = new BaseResult<object>();
            try
            {
                respuesta.Data = _redirection.Redirect(solicitudDeRedireccion);
            }
            catch(Exception e)
            {
                respuesta.oError.Codigo = 1;
                respuesta.oError.Mensaje= e.Message;
                //return BadRequest(e.Message);
            }
            return Ok(respuesta);
        }
    }
}
