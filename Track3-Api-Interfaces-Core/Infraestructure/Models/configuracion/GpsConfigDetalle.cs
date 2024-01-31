using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.track3.Models.configuracion
{
    public class GpsConfigDetalle
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CONFIG_MAESTRO_ID { get; set; }
        public string Code { get; set; }
        public string VALOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string ACTIVO { get; set; }
        public string UsuarioCreado { get; set; }
        ~GpsConfigDetalle()
        {

        }
        public GpsConfigDetalle()
        {

        }
        public GpsConfigDetalle(DataRow row)
        {
            ID = int.Parse(row["ID"].ToString());
            Code = row["CODE"].ToString();
            DESCRIPCION = row["DESCRIPCION"].ToString();
            VALOR = row["VALOR"].ToString();
            ACTIVO = row["ACTIVO"].ToString();
            UsuarioCreado = row["USUARIO_CREADO"].ToString();
        }
    }
}
