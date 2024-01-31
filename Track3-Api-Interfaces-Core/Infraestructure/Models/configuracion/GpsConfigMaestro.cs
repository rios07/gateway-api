using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.track3.Models.configuracion
{
    public class GpsConfigMaestro
    {
        [Key]
        public Int64 Id { get; set; }
        public string Code { get; set; }
        public string Descripcion { get; set; }
        public string Activo { get; set; }
        public string UsuarioCreado { get; set; }

        public GpsConfigMaestro() { }

        ~GpsConfigMaestro()
        {

        }
    }
}