using Infraestruture.DAO;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Text;
using Track3_Api_Interfaces_Core.Aplication.DATA;
using Track3_Api_Interfaces_Core.Application.Requests;
using Track3_Api_Interfaces_Core.Infraestructure.Interfaces;

namespace Track3_Api_Interfaces_Core.Infraestructure
{
    public class ProcesosRepository: IProcesosRepository
    {
        private readonly IOracleDao _dao;
        private readonly IConfiguration _configuration;
        public ProcesosRepository(IOracleDao dao, IConfiguration configuration)
        {
            _dao = dao;
            _configuration = configuration;
        }
        public string AddProcesoAudit(RedirectionRequest request, int codeResp)
        {
            StringBuilder sb = new StringBuilder();
            List<OracleParameter> parametros = new List<OracleParameter>();
            _dao.CrearConnection(_configuration.GetConnectionString(CONNECTIONS.TRACK3));
            int id = _dao.ExcecuteEscalar(" SELECT GPS.SEQ_GPS_SERVICIOS_AUDIT.NEXTVAL FROM DUAL ");

            sb.Append(" INSERT INTO GPS_SERVICIOS_AUDIT(ID, USUARIO, ORIGEN, DESTINO, SOLICITUD, RESPUESTA, CODIGO_RESPUESTA) ");
            sb.Append(" VALUES(:ID, :USUARIO, :ORIGEN, :DESTINO, :SOLICITUD, :RESPUESTA, :CODIGO_RESPUESTA) ");

            parametros.Add(new OracleParameter(":ID", id));
            parametros.Add(new OracleParameter(":USUARIO", request.usuario));
            parametros.Add(new OracleParameter(":ORIGEN", request.origen));
            parametros.Add(new OracleParameter(":DESTINO", request.destino));
            parametros.Add(new OracleParameter(":SOLICITUD", request.payload.ToString()));
            parametros.Add(new OracleParameter(":RESPUESTA", request.response.ToString()));
            parametros.Add(new OracleParameter(":CODIGO_RESPUESTA", codeResp));

            string respuesta = _dao.AddOrUpdate(sb.ToString(), parametros);
            _dao.Commit();
            _dao.CerrarConexion();
            _dao.Dispose();
            if(respuesta != BASICMESSAGES.OK)
            {
                throw new Exception("Error al insertar dato");
            }
            return respuesta;
        }
    }
}
