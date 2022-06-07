using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_InterfazRetenciones.Models
{
    public class FAHLDLA
    {
        private string usuario;
        private int agencia;
        private int cuenta;
        private int sufijo;
        private TimeSpan hora;
        private string desc1;
        private string desc2;
        private string desc3;
        private string desc4;
        private DateTime fecha1;
        private DateTime fecha2;
        private Decimal monto;
        private DateTime fechaequation;
        private DateTime fechaequation1;
        private int hold;
        private string tipoTransaccion;
        private int numeroSecuencia;

        public string Usuario { get => usuario; set => usuario = value; }
        public int Agencia { get => agencia; set => agencia = value; }
        public int Cuenta { get => cuenta; set => cuenta = value; }
        public int Sufijo { get => sufijo; set => sufijo = value; }
        public TimeSpan Hora { get => hora; set => hora = value; }
        public string Desc1 { get => desc1; set => desc1 = value; }
        public string Desc2 { get => desc2; set => desc2 = value; }
        public string Desc3 { get => desc3; set => desc3 = value; }
        public string Desc4 { get => desc4; set => desc4 = value; }
        public DateTime Fecha1 { get => fecha1; set => fecha1 = value; }
        public DateTime Fecha2 { get => fecha2; set => fecha2 = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public DateTime Fechaequation { get => fechaequation; set => fechaequation = value; }
        public DateTime Fechaequation1 { get => fechaequation1; set => fechaequation1 = value; }
        public int Hold { get => hold; set => hold = value; }
        public string TipoTransaccion { get => tipoTransaccion; set => tipoTransaccion = value; }
        public int NumeroSecuencia { get => numeroSecuencia; set => numeroSecuencia = value; }
    }
}
