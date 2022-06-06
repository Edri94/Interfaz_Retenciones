using Biblioteca_InterfazRetenciones.Helpers;
using Biblioteca_InterfazRetenciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca_InterfazRetenciones.Processes
{
    public class Main
    {
        //Variables Conexion
        string msDBName;
        string msDBSrvr;
        string msDBuser;
        string msDBPswd;

        string msDSN400;
        string msLibAS400;
        string msUser400;
        string msPswd400;
          
        // Control de conexión a la base de datos de SQL
        public bool mbConexion;
        public byte mnFirstTime;
        public long mnProdCont;
        public long mnAgencia;
        public long mnProducto;
        public long mnStatus;
        public long mnConcepto;
        public long mnOperDef;
        public string msTiempo;
        public string msSQL;
        public string msSQL400;
        public string msTipoMovimiento;
        public DateTime mdRefreshTime;
        // Clave de seguridad para modificar los parámetros de conexión
        public string msClaveModParams;
        // Constante para guardar el nombre del usuario de transacciones Gran Caimán
        const string msUSREQUGC = "EQUGC";
        public int mn_numuserGC;
        // Se agrega variable para guardar el nombre de la tabla de usuario
        const string gs_Usuario = "USUARIO";
        // R: variables de la modificacion del hold para TKT
        public string lsUsuario;
        public int lnProductoGlobal;
        public int lnStatusProducto;
        public int lnConceptoDefinidoGlobal;
        public int lnStatusProductoGlobal;
        public string lsNoHold;

        public List<Registro> MaRegistros;
        public string lsArchivoHOLDAS400;
        public int lnDatosHOLD;
        // Variable de Tipo de Transacción
        public string lsTipoTranHold;
        public string lsTipoTransaccionM;


        Label lblTransactionMessage;
        Label lblServer;
        Label lblDataBase;
        Label lblDSNAS400;
        Label lblArchivoHoldLA;
        Label lblLIB400;
        Label lblFechaSistema;
        Label lblTiempo;
        Label lblPendHoldsLA;
        Label lblNextHoldLA;
        CheckBox chkRecibeHoldsLA;
        ProgressBar pbrMovimientos;


        Encriptacion encriptacion;


        

        public Main(
            ref Label lblTransactionMessage, 
            ref Label lblServer, 
            ref Label lblDataBase, 
            ref Label lblDSNAS400,
            ref Label lblArchivoHoldLA,
            ref Label lblLIB400, 
            ref Label lblFechaSistema, 
            ref Label lblTiempo, 
            ref Label lblPendHoldsLA, 
            ref Label lblNextHoldLA, 
            ref CheckBox chkRecibeHoldsLA, 
            ref ProgressBar pbrMovimientos)
        {

            this.lblTransactionMessage = lblTransactionMessage;
            this.lblServer = lblServer;
            this.lblDataBase = lblDataBase;
            this.lblDSNAS400 = lblDSNAS400;
            this.lblArchivoHoldLA = lblArchivoHoldLA;
            this.lblLIB400 = lblLIB400;
            this.lblFechaSistema = lblFechaSistema;
            this.lblTiempo = lblTiempo;
            this.lblPendHoldsLA = lblPendHoldsLA;
            this.lblNextHoldLA = lblNextHoldLA;
            this.chkRecibeHoldsLA = chkRecibeHoldsLA;
            this.pbrMovimientos = pbrMovimientos;

        }


        public void Init()
        {
            encriptacion = new Encriptacion();

            int lnWDif;
            int lnHDif;
            string Str_Ruta;

            this.mbConexion = false;
            Str_Ruta = "ruta app.config";
            


        }

        public void EstableceParametros()
        {
            msDBName = encriptacion.Decrypt(Funcion.getValueAppConfig("Nombre", "BD"));
            msDBSrvr = encriptacion.Decrypt(Funcion.getValueAppConfig("Servidor", "BD"));
            msDBuser = encriptacion.Decrypt(Funcion.getValueAppConfig("Usuario", "BD"));
            msDBPswd = encriptacion.Decrypt(Funcion.getValueAppConfig("Password", "BD"));

            msDSN400 = encriptacion.Decrypt(Funcion.getValueAppConfig("DSN", "AS400"));
            msLibAS400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Biblioteca", "AS400"));
            msUser400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Usuario", "AS400"));
            msPswd400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Password", "AS400"));
        }

    }
}
