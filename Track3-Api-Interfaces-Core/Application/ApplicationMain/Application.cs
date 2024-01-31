using Newtonsoft.Json;
using System.Net;
using System.Text;
using Application.interfaces;
using System.Collections.Specialized;
using System.Security.Policy;
using System;
using System.Net.Http;
using Infraestructure;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class ServiceHelper : IServiceHelper
    {
        //private readonly HttpClient httpClient;
        public ServiceHelper()
        {
            //httpClient = new HttpClient();
        }

        public T Get<T>(string servicio, string metodo, List<string> args, out int codeResp, bool? ssl = false, bool raiseException = false)
        {
            string protocol = (ssl.HasValue ? ssl.Value : false) ? "https" : "http";
            codeResp = 0;
            Uri webApiUrl = new Uri($"{protocol}{Infraestructura.GetSection(servicio)}/{metodo}");
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = webApiUrl;

            string queryString = string.Empty;
            args.ForEach(s => queryString += $"/{s}");


            HttpResponseMessage response;

            try
            {
                response = httpClient.GetAsync(webApiUrl + queryString).Result;
                codeResp = (int)  response.StatusCode;
            }
            catch (Exception e)
            {
                if (raiseException)
                    throw new Exception($"{e.Message}");
                return default(T);
            }

            if ((response.StatusCode != HttpStatusCode.OK) && (response.StatusCode != HttpStatusCode.BadRequest))
            {
                codeResp = (int)response.StatusCode;
                if (raiseException)
                    throw new Exception($"{response.StatusCode} | {response.ReasonPhrase}");
                return default(T);
            }
            T resultado = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            if (resultado is null && raiseException)
                throw new Exception("No fue posible deserializar el objeto en el formato requerido");
            return resultado;
        }
        /// <summary>
        /// Realiza una solicitud http a un servicio/metodo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicio"></param>
        /// <param name="metodo"></param>
        /// <param name="sender"></param>
        /// <param name="ssl"></param>
        /// <param name="raiseException"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Post<T>(string servicio, string metodo, object sender, out int codeResp, bool? ssl = false, bool raiseException = false, JsonSerializerSettings? settings = null)
        {
            string protocol = ssl is null ? ""  : ssl.Value ? "https://" : "http://";
            Uri webApiUrl = new Uri($"{protocol}{servicio}/{metodo}");
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = webApiUrl;
            codeResp = 0;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(sender, settings), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response;
            try
            {
                response = httpClient.PostAsync(webApiUrl.ToString(), content).Result;
                codeResp = (int)response.StatusCode;
            }
            catch(Exception e)
            {
                if (raiseException)
                    throw new Exception($"{e.Message}");
                return default(T);
            }
            if ((response.StatusCode!=HttpStatusCode.OK) && (response.StatusCode != HttpStatusCode.BadRequest))
            {
                codeResp = (int)response.StatusCode;
                if (raiseException)
                    throw new Exception($"{response.StatusCode} | {response.ReasonPhrase}");
                return default(T);
            }

            T resultado = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            if (resultado is null && raiseException)
                throw new Exception("No fue posible deserializar el objeto en el formato requerido");
            return resultado;
        }

        public T Put<T>(string servicio, string metodo, object sender, out int codeResp, bool? ssl = false, bool raiseException = false, JsonSerializerSettings? settings = null)
        {
            string protocol = ssl is null ? "" : ssl.Value ? "https://" : "http://";
            Uri webApiUrl = new Uri($"{protocol}{servicio}/{metodo}");
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = webApiUrl;
            codeResp = 0;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(sender, settings), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response;

            try
            {
                response = httpClient.PutAsync(webApiUrl.ToString(), content).Result;
                codeResp = (int)response.StatusCode;
            }
            catch (Exception e)
            {
                if (raiseException)
                    throw new Exception($"{e.Message}");
                return default(T);
            }

            if ((response.StatusCode != HttpStatusCode.OK) && (response.StatusCode != HttpStatusCode.BadRequest))
            {
                codeResp = (int)response.StatusCode;
                if (raiseException)
                    throw new Exception($"{response.StatusCode} | {response.ReasonPhrase}");
                return default(T);
            }

            T resultado = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            if (resultado is null && raiseException)
                throw new Exception("No fue posible deserializar el objeto en el formato requerido");
            return resultado;
        }
    }
}