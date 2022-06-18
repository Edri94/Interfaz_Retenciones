using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_InterfazRetenciones.Models
{
    public class ERROR_MOVIMIENTO_DIRECTO
    {
        public int NumMensaje { get; set; }
        public string Agencia { get; set; }
        public string Cuenta { get; set; }
        public string Sufijo { get; set; }
        public string Referencia { get; set; }
        public DateTime FechaValor { get; set; }
        public decimal Monto { get; set; }
        public string Narrativa1 { get; set; }
        public string Narrativa2 { get; set; }
        public string Narrativa3 { get; set; }
        public string Narrativa4 { get; set; }
        public DateTime FechaMov { get; set; }
        public string CodTran { get; set; }
        public string UsuarioKap { get; set; }
        public string TipoTransaccion { get; set; }
    }
}
