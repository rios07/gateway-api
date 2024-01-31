using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IServiceHelper
    {
        public abstract T Get<T>(string servicio, string metodo, List<string> args, out int codeResp,bool? ssl = false, bool raiseException = false);
        /// <summary>
        /// Realiza una solicitud http a un servicio/metodo
        /// ssl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <param name="metodo"></param>
        /// <param name="sender"></param>
        /// <param name="ssl">
        /// Parametro opcional Si null no se coloca nada Si true https, Si false http
        /// </param>
        /// <param name="raiseException">
        /// Parametro opcional Si true levanta exepciones si no se recibe un 200 o un objeto de un tipo no esperado
        /// </param>
        /// <param name="settings">
        /// Opciones de serializacion
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public abstract T Post<T>(string servicio, string metodo, object sender, out int codeResp, bool? ssl = false, bool raiseException = false, JsonSerializerSettings? settings = null);
        /// <summary>
        /// Realiza una solicitud Put a un servicio/metodo
        /// ssl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <param name="metodo"></param>
        /// <param name="sender"></param>
        /// <param name="ssl">
        /// Parametro opcional Si null no se coloca nada Si true https, Si false http
        /// </param>
        /// <param name="raiseException">
        /// Parametro opcional Si true levanta exepciones si no se recibe un 200 o un objeto de un tipo no esperado
        /// </param>
        /// <param name="settings">
        /// Opciones de serializacion
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public abstract T Put<T>(string servicio, string metodo, object sender, out int codeResp, bool? ssl = false, bool raiseException = false, JsonSerializerSettings? settings = null);
    }
}
