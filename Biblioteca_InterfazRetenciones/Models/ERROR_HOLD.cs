using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_InterfazRetenciones.Models
{
    public class ERROR_HOLD
    {
        public int NumMensaje { get; set; }
        public string UsuarioKap { get; set; }
        public string Agencia { get; set; }
        public string Cuenta { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string Desc4 { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEquation { get; set; }
        public int Hold { get; set; }
        public string TipoTransaccion { get; set; }
    }
}
