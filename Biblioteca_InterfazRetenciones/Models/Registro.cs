using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_InterfazRetenciones.Models
{
    public class Registro
    {
        private string secuencia;
        private int agencia;
        private int cuenta;
        private int sufijo;
        private string usuario;
        private string fecha;
        private Decimal monto;
        private string ntva1;
        private string ntva2;
        private string ntva3;
        private string ntva4;
        private string desc1;
        private string desc2;
        private string desc3;
        private string desc4;
        private DateTime fecha1;
        private DateTime fecha2;
        private DateTime fechaequation;
        private DateTime fechaequation1;
        private string tranCode;
        private string userID;
        private string procCode;
        private string tiempo;
        private TimeSpan hora;
        private int hold;
        private string tipoTransaccion;
        private string grupoUsuario;
        private int numeroSecuencia;

        public string Secuencia { get => secuencia; set => secuencia = value; }
        public int Agencia { get => agencia; set => agencia = value; }
        public int Cuenta { get => cuenta; set => cuenta = value; }
        public int Sufijo { get => sufijo; set => sufijo = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public Decimal Monto { get => monto; set => monto = value; }
        public string Ntva1 { get => ntva1; set => ntva1 = value; }
        public string Ntva2 { get => ntva2; set => ntva2 = value; }
        public string Ntva3 { get => ntva3; set => ntva3 = value; }
        public string Ntva4 { get => ntva4; set => ntva4 = value; }
        public string Desc1 { get => desc1; set => desc1 = value; }
        public string Desc2 { get => desc2; set => desc2 = value; }
        public string Desc3 { get => desc3; set => desc3 = value; }
        public string Desc4 { get => desc4; set => desc4 = value; }
        public DateTime Fecha1 { get => fecha1; set => fecha1 = value; }
        public DateTime Fecha2 { get => fecha2; set => fecha2 = value; }
        public DateTime Fechaequation { get => fechaequation; set => fechaequation = value; }
        public DateTime Fechaequation1 { get => fechaequation1; set => fechaequation1 = value; }
        public string TranCode { get => tranCode; set => tranCode = value; }
        public string UserID { get => userID; set => userID = value; }
        public string ProcCode { get => procCode; set => procCode = value; }
        public string Tiempo { get => tiempo; set => tiempo = value; }
        public TimeSpan Hora { get => hora; set => hora = value; }
        public int Hold { get => hold; set => hold = value; }
        public string TipoTransaccion { get => tipoTransaccion; set => tipoTransaccion = value; }
        public string GrupoUsuario { get => grupoUsuario; set => grupoUsuario = value; }
        public int NumeroSecuencia { get => numeroSecuencia; set => numeroSecuencia = value; }
    }
}
