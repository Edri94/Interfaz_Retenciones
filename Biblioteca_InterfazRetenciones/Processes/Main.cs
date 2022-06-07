using Biblioteca_InterfazRetenciones.Data;
using Biblioteca_InterfazRetenciones.Helpers;
using Biblioteca_InterfazRetenciones.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
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
        string ArchivoHOLDS;
          
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

        public List<FAHLDLA> MaRegistros;
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
        Label pnlStatus;
        Label lblStatus;
        CheckBox chkRecibeHoldsLA;
        ProgressBar pbrMovimientos;


        Encriptacion encriptacion;
        FuncionesBD bd;
        ConexionAS400 as400;

        string gsFechaSQL;
        string RecibeHOLDLA;
        string PendHoldsLA;




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
            ref ProgressBar pbrMovimientos,
            ref Label pnlStatus,
            ref Label lblStatus)
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
            this.pnlStatus = pnlStatus;
            this.lblStatus = lblStatus;

        }


        public void Init()
        {
            int lnWDif;
            int lnHDif;
            string Str_Ruta;

            this.mbConexion = false;
            Str_Ruta = "ruta app.config";

            EstableceParametros();

            if (EstableceConexionBD() && as400.Conectar())
            {
                lblServer.Text = msDBSrvr;
                lblDataBase.Text = msDBName;
                lblDSNAS400.Text = msDSN400;

                this.gsFechaSQL = bd.obtenerFechaServidor();

                chkRecibeHoldsLA.Checked = (RecibeHOLDLA == "1") ? true : false;
                lblArchivoHoldLA.Text = ArchivoHOLDS;
                lblPendHoldsLA.Text = PendHoldsLA;
                lblLIB400.Text = msLibAS400;
                lblNextHoldLA.Text = this.gsFechaSQL + " 10:00";

                mnFirstTime = 1;


                Message_PnlStatus("CONECTADO....");

                RecibeHoldsLA();

            }


            


        }

        private void RecibeHoldsLA()
        {
            RecibeHolds(1);
            lblNextHoldLA.Text = DateTime.Parse(this.gsFechaSQL).AddMinutes(5).ToString("dd-MM-yyyy hh:mm");
        }

        private void RecibeHolds(int Agencia)
        {
            try
            {
                int lnContador;
                long lnOperacion;
                string LsHora;
                string lsMinutos;
                string lsSegundos;
                int lnNumRegistros;
                // Variable para eliminacion de cuentas
                string lsCtaCompleta;
                bool lbHOLDValido;

                pbrMovimientos.Value = 0; lbHOLDValido = false;
                Message("Recibiendo HOLDS KAPITI - TICKET");

              
                lsArchivoHOLDAS400 = lblArchivoHoldLA.Text;
                Messahe_Status("Buscando Holds de Houston...");
                      

                lnContador = 0;
                lnDatosHOLD = 0;

                msSQL400 = "SELECT COUNT(*) FROM " + msLibAS400 + "." + lsArchivoHOLDAS400 + " WHERE HPROC = ' '";

                OdbcDataReader dr = null;
                List<Map> registros = new List<Map>();

                registros.Add(new Map { Key = "lnNumRegistros", Type = "int" });            
                dr = as400.EjecutaSelect(msSQL400);
                registros = as400.LLenarMapToQuery(registros, dr);

                lnNumRegistros = Int32.Parse(registros[0].Value.ToString());

                lblPendHoldsLA.Text = lnNumRegistros.ToString("D6");

                if(lnNumRegistros > 0)
                {
                    pbrMovimientos.Maximum = lnNumRegistros + 1;
                    lblStatus.Text = "Recibiendo Holds de Houston ";


                    msSQL400 = $"Select HUSR, HAB, HAN, HAS, HTIM, HDES1, HDES2, HDES3, HDES4, HSTD, HEXD, HAMT, HEQD, HHLDN, HTOP, HSEC From {msLibAS400}.{lsArchivoHOLDAS400} Where HPROC = ' '";

                    dr = as400.EjecutaSelect(msSQL400);

                    MaRegistros = new List<FAHLDLA>();


                    //Nota: Crear un objeto de tipo TKTLIB.FAHLDLA para llenar esta lista
                    while (dr.Read())
                    {
                        MaRegistros.Add(new FAHLDLA
                        {
                            Usuario = dr.GetString(0),
                            Agencia = Int32.Parse(dr.GetString(1)),
                            Cuenta = Int32.Parse(dr.GetString(2)),                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                            Sufijo = Int32.Parse(dr.GetString(3)),
                            Hora =  new TimeSpan(Int32.Parse(dr.GetString(4).Substring(0, 2)), Int32.Parse(dr.GetString(4).Substring(2, 2)), Int32.Parse(dr.GetString(4).Substring(4, 2))),
                            Desc1 = dr.GetString(5),
                            Desc2 = dr.GetString(6),
                            Desc3 = dr.GetString(7),
                            Desc4 = dr.GetString(8),
                            Fecha1 = DateTime.ParseExact(dr.GetString(9), "yyyyMMdd", null),
                            Fecha2 = DateTime.ParseExact(dr.GetString(10), "yyyyMMdd", null),
                            Monto = Decimal.Parse(dr.GetString(11)),
                            Fechaequation = DateTime.ParseExact(dr.GetString(12), "yyyyMMdd", null),
                            Fechaequation1 = DateTime.ParseExact(dr.GetString(12), "yyyyMMdd", null),
                            Hold = Int32.Parse(dr.GetString(13)),
                            TipoTransaccion = dr.GetString(14),
                            NumeroSecuencia = Int32.Parse(dr.GetString(15)),

                        });
                        
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

       
        public void EstableceParametros()
        {
            encriptacion = new Encriptacion();

            msDBName = encriptacion.Decrypt(Funcion.getValueAppConfig("Nombre", "BD"));
            msDBSrvr = encriptacion.Decrypt(Funcion.getValueAppConfig("Servidor", "BD"));
            msDBuser = encriptacion.Decrypt(Funcion.getValueAppConfig("Usuario", "BD"));
            msDBPswd = encriptacion.Decrypt(Funcion.getValueAppConfig("Password", "BD"));

            msDSN400 = encriptacion.Decrypt(Funcion.getValueAppConfig("DSN", "AS400"));
            msLibAS400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Biblioteca", "AS400"));
            msUser400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Usuario", "AS400"));
            msPswd400 = encriptacion.Decrypt(Funcion.getValueAppConfig("Password", "AS400"));
            
            ArchivoHOLDS = encriptacion.Decrypt(Funcion.getValueAppConfig("ARCHIVOHOLDLA", "PARAMETROS"));
            RecibeHOLDLA = (Funcion.getValueAppConfig("RECIBEHOLDLA", "PARAMETROS"));
            PendHoldsLA = (Funcion.getValueAppConfig("PERIODOHOLDLA", "PARAMETROS"));      


            as400 = new ConexionAS400(msDSN400, msUser400, msPswd400);
           

        }

        public bool EstableceConexionBD()
        {
            try
            {
                string conn_str = $"Data source ={msDBSrvr}; uid ={msDBuser}; PWD ={msDBPswd}; initial catalog = {msDBName}";

                if (bd == null)
                {
                    bd = new FuncionesBD(conn_str);
                    bd.ActiveConnection = true;
                    this.mbConexion = true;
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                this.mbConexion = false;
            }

            return this.mbConexion;
        }

        private void Message_PnlStatus(string mensaje)
        {
            pnlStatus.ForeColor = Color.Green; 
            pnlStatus.Text = mensaje;
        }

        private void Message(string mensaje)
        {
            lblTransactionMessage.Text = mensaje;
        }


        private void Messahe_Status(string mensaje)
        {
            lblStatus.Text = mensaje;
        }


    }
}
