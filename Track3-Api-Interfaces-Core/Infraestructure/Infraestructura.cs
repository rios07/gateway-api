using System.Data;
using System.Text;
using Infraestructure.Cache;
using Infraestructure.track3.Models.configuracion;
using Infraestruture.DAO;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Track3_Api_Interfaces_Core.Aplication.DATA;


namespace Infraestructure
{
    public class Infraestructura
    {

        public const int DEFAULT_CACHE_LIFE_SPAN = 999999999;
        //private static IOracleDao _dao;
        public static bool development;

        public static IConfiguration _configuration { get; set; }
        public static CacheServices _CacheServices { get; set; } = CacheServices.GetInstance();


        public static string GetConnectionStringByName(string nombre)
        {
            return _configuration.GetConnectionString(nombre);
        }
        public static string GetSection(string section)
        {
            return _configuration.GetSection(section).Value;
        }

        public static string getValorByDetalle(string code)
        {
            if (_CacheServices.Exist(code, DEFAULT_CACHE_LIFE_SPAN))
            {
                return _CacheServices.Get(code).ToString();
            }
            else
            {
                using OracleDao dao = new OracleDao();

                dao.CrearConnection(GetConnectionStringByName(CONNECTIONS.TRACK3));
                try
                {
                    string value = dao.ExcecuteEscalar($"SELECT VALOR FROM GPS_CONFIG_DETALLE WHERE CODE = '{code}' ").ToString();
                    dao.CerrarConexion();
                    _CacheServices.Add(value, code, DEFAULT_CACHE_LIFE_SPAN);
                    return value;
                }
                catch (Exception ex)
                {
                    dao.CerrarConexion();
                    throw new Exception(ex.Message);
                }
            }

        }
        public static string getStringParametro(string code)
        {

            if (_CacheServices.Exist(code, DEFAULT_CACHE_LIFE_SPAN))
            {

                return _CacheServices.Get(code).ToString();
            }
            else
            {

                using OracleDao dao = new OracleDao();

                dao.CrearConnection(GetConnectionStringByName(CONNECTIONS.TRACK3));
                try
                {

                    string value = dao.GetStringParam($"SELECT VALOR FROM GPS_CONFIG_DETALLE WHERE CODE = '{code}' ").ToString();

                    dao.CerrarConexion();
                    _CacheServices.Add(value, code, DEFAULT_CACHE_LIFE_SPAN);

                    return value;
                }
                catch (Exception ex)
                {

                    dao.CerrarConexion();
                    throw new Exception(ex.Message);
                }
            }


        }

        public static List<GpsConfigDetalle> getListConfiguracion(string code)
        {

            if (_CacheServices.Exist(code))
            {
                return (List<GpsConfigDetalle>)_CacheServices.Get(code);
            }
            else
            {
                using OracleDao dao = new OracleDao();
                List<GpsConfigDetalle> retorno = new List<GpsConfigDetalle>();
                dao.CrearConnection(GetConnectionStringByName(CONNECTIONS.TRACK3));
                try
                {
                    string query =
                        $"SELECT CD.CODE,CD.VALOR FROM GPS_CONFIG_DETALLE CD INNER JOIN GPS_CONFIG_MAESTRO CM ON CD.CONFIG_MAESTRO_ID =CM.ID WHERE CM.CODE = '{code}' ";
                    DataTable dt = new DataTable();
                    dt = dao.ExcecuteAdapterFill(query, new List<OracleParameter>());
                    dao.CerrarConexion();
                    foreach (DataRow row in dt.Rows)
                    {
                        retorno.Add(new GpsConfigDetalle
                        {
                            VALOR = row["VALOR"].ToString(),
                            Code = row["CODE"].ToString()
                        });
                    }
                    _CacheServices.Add(retorno, code, DEFAULT_CACHE_LIFE_SPAN);
                    return retorno;
                }
                catch (Exception ex)
                {
                    dao.CerrarConexion();
                    throw new Exception(ex.Message);
                }

            }



        }



        public static string getStringParametro(OracleDao dao, string code)
        {

            if (_CacheServices.Exist(code, DEFAULT_CACHE_LIFE_SPAN))
            {

                return _CacheServices.Get(code).ToString();
            }
            else
            {
                try
                {

                    string value = dao.GetStringParam($"SELECT VALOR FROM GPS_CONFIG_DETALLE WHERE CODE = '{code}' ").ToString();
                    _CacheServices.Add(value, code, DEFAULT_CACHE_LIFE_SPAN);

                    return value;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }


        }

        public static GpsConfigDetalle getConfigValor(OracleDao dao, string code)
        {
            DataTable dt = new DataTable();
            GpsConfigDetalle retorno = null;
            StringBuilder sb = new StringBuilder();
            List<OracleParameter> parametros = new List<OracleParameter>();

            sb.Append(" SELECT ID, CODE, DESCRIPCION, FECHA_CREADO, USUARIO_CREADO , ACTIVO, VALOR   ");
            sb.Append(" FROM GPS_CONFIG_DETALLE  WHERE CODE = :CODE AND ACTIVO = 'S'                 ");
            sb.Append(" ORDER BY DESCRIPCION DESC                                                    ");
            parametros.Add(new OracleParameter(":CODE", code));
            dt = dao.ExcecuteAdapterFill(sb.ToString(), parametros);

            if(dt.Rows.Count > 0)
            {
                retorno = new GpsConfigDetalle(dt.Rows[0]);
            }
            return retorno;
        }


    }
}
