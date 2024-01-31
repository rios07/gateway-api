using System.ComponentModel.DataAnnotations;
using Track3_Api_Interfaces_Core.Validators;

namespace Track3_Api_Interfaces_Core.Application.Requests
{
    public class RedirectionRequest
    {
        [Required]
        [MethodAttributeValidator]
        public string metodo { get; set; }
        /// <summary>
        /// Usuario que ejecuta request a redireccionar
        /// </summary>
        [Required]
        public string usuario { get; set; } = string.Empty;
        /// <summary>
        /// Metodo origen de la request
        /// </summary>
        [Required]
        public string origen { get; set; }
        /// <summary>
        /// Servicio destino de la request
        /// </summary>
        [Required]
        public string destino { get; set; }
        [Required]
        public string endpoint { get; set; }
        /// <summary>
        /// Carga util de la request 
        /// </summary>
        public object payload { get; set; }
        /// <summary>
        /// Carga util de la request 
        /// </summary>
        public List<string> argumentos { get; set; }
        /// <summary>
        /// Si true el llamado espera la respuesta del servicio externo
        /// Si false devuelve un 200 y ejecuta el llamado al servicio externo en segundo plano
        /// </summary>
        [Required]
        public bool necesitaRespuesta { get; set; } = true;
        public object? response { get; set; }
    }
}
