using Biblioteca_InterfazRetenciones.Data;
using Biblioteca_InterfazRetenciones.Helpers;
using Biblioteca_InterfazRetenciones.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
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
                List<Producto> productos = new List<Producto>();

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

                        lnContador++;

                        if(lnContador < pbrMovimientos.Maximum)
                        {
                            pbrMovimientos.Value = pbrMovimientos.Value + 1; 
                        }

                        lblStatus.Text = "Recibiendo Holds de HO. (" + lnContador + " Registros)";
                    }
                    pbrMovimientos.Value = 0;

                    if(lnContador > 0)
                    {
                        lnContador--;
                    }

                    for (lnDatosHOLD = 0; lnDatosHOLD <= lnContador; lnDatosHOLD++)
                    {
                        lbHOLDValido = true;


                        if (lnDatosHOLD < pbrMovimientos.Maximum)
                        {
                            pbrMovimientos.Value = lnDatosHOLD;
                        }
                        int tmp_lnContador = lnContador + 1;
                        int tmp_lnDatosHOLD = lnDatosHOLD + 1;

                        lblStatus.Text = $"Registrando Holds de HO. ({lnDatosHOLD} de  {tmp_lnContador})";

                        if(Int32.Parse(lblPendHoldsLA.Text) > 0)
                        {
                            lblPendHoldsLA.Text = (Int32.Parse(lblPendHoldsLA.Text) - 1).ToString();
                        }

                        lsTipoTranHold = MaRegistros[lnDatosHOLD].TipoTransaccion;
                        lsCtaCompleta = MaRegistros[lnDatosHOLD].Agencia.ToString();
                        lsCtaCompleta = lsCtaCompleta + MaRegistros[lnDatosHOLD].Cuenta;
                        lsCtaCompleta = lsCtaCompleta + MaRegistros[lnDatosHOLD].Sufijo;
                        //R: pedimos el tipo de usuario LB@@ o TK@@
                        lsUsuario = MaRegistros[lnDatosHOLD].Usuario;

                        if(lsUsuario.Contains("TK@@"))
                        {
                            lnProductoGlobal = 12;
                            lnStatusProducto = 2;
                            lnConceptoDefinidoGlobal = 10;
                            lnStatusProductoGlobal = 8002;
                        }
                        else
                        {
                            lnProductoGlobal = 14;
                            lnStatusProducto = 7;
                            lnConceptoDefinidoGlobal = 20;
                            lnStatusProductoGlobal = 8007;
                        }

                        //'Obtenemos la agencia, el producto y el Status del producto
                        msSQL = $@"
                            Select  
	                            AG.agencia
	                            ,PR.producto
	                            ,SP.status_producto
	                            ,CD.concepto_definido 
                            From  
	                            {msDBName}..STATUS_PRODUCTO SP
	                            INNER JOIN {msDBName}..PRODUCTO PR ON PR.agencia = SP.agencia AND SP.producto = PR.producto 
	                            INNER JOIN {msDBName}..CONCEPTO_DEFINIDO CD ON PR.agencia  = CD.agencia  AND PR.producto = CD.producto 
	                            INNER JOIN CATALOGOS..AGENCIA AG ON PR.agencia = AG.agencia 
                            Where 
	                            PR.producto_global = {lnProductoGlobal} 
	                            and 
	                            SP.status_producto_global = {lnStatusProducto} 
	                            and 
	                            CD.concepto_definido_global = {lnConceptoDefinidoGlobal} 
	                            and 
	                            AG.prefijo_agencia = '{MaRegistros[lnDatosHOLD].Agencia}'";

                        SqlDataReader dr_1 = bd.ejecutarConsulta(msSQL);

                        if(dr_1 == null)
                        {
                            lbHOLDValido = false;
                            //Inserta el BITACORA_ERROR_HOLD
                            RegistraErrorHold("No existe la Cuenta Eje " + lsCtaCompleta, lnDatosHOLD);
                        }
                       
                        productos.Add(Producto.GteProducto(dr_1, MaRegistros[lnDatosHOLD]));

                        dr_1.Close();

                        mnAgencia = productos[lnDatosHOLD].agencia;
                        mnProducto = productos[lnDatosHOLD].producto;
                        mnStatus = productos[lnDatosHOLD].status_producto;
                        mnConcepto = productos[lnDatosHOLD].concepto_definido;


                      


                        //Si el Hold No es Valido (NO se guarda en Ticket), solo lo marca
                        if(lbHOLDValido ==false)
                        {
                            ActualizaHOLD_AS400(lbHOLDValido);
                        }
                        //Valida si existe el reg en Tkt antes de insertarlo
                        else
                        {
                            if(lsUsuario != "TK@@")
                            {
                                GuardaHolds(lnDatosHOLD);
                            }
                            else
                            {

                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void GuardaHolds(int Indice)
        {
            try
            {
                int lnProdCont;
                string lsFechaVenc;
                int lnStatusProducto;
                bool lbHOLDValido;
                int lnStatusProd;
                int LnAgencia;

                lbHOLDValido = false;

                if(MaRegistros[Indice].TipoTransaccion != "A")
                {

                    msSQL = $@"
                        Select  
	                        pc.producto_contratado
	                        ,  pc.fecha_vencimiento
	                        ,  pc.status_producto  
                        from  
	                        {msDBName}..HOLD h
	                        INNER JOIN {msDBName}..PRODUCTO_CONTRATADO pc ON h.producto_contratado  = pc.producto_contratado
	                        INNER JOIN {msDBName}..PRODUCTO pr ON pc.producto = pr.producto  
                        where  
	                        pr.producto_global = 12  
	                        and 
	                        fecha_vencimiento >= convert(char(10),getdate(),110)  
	                        and 
	                        pc.agencia = {mnAgencia} 
	                        and 
	                        pc.cuenta_cliente = '{MaRegistros[Indice].Cuenta}' 
	                        and 
	                        h.hold = {MaRegistros[Indice].Hold};
                    ";

                    SqlDataReader dr =  bd.ejecutarConsulta(msSQL);

                    if(dr != null)
                    {
                        while(dr.Read())
                        {
                            lnProdCont = dr.GetInt32(0);
                            lsFechaVenc = dr.GetDateTime(1).ToString();
                            lnStatusProducto = dr.GetInt32(2);

                            msSQL = $"Select status_producto from {msDBName}..PRODUCTO_CONTRATADO  where producto_contratado  = {lnProdCont}";
                        }
                        dr.Close();

                        dr = bd.ejecutarConsulta(msSQL);

                        if (dr == null)
                        {
                            RegistraErrorHold("Ocurrio un error al obtener el status del Hold.", Indice);
                            ActualizaHOLD_AS400(lbHOLDValido);
                        }
                        else
                        {
                            lnStatusProd = Int32.Parse(bd.LLenarMapToQuery(new Map { Key = "lnStatusProd" }, dr).Value.ToString());
                        }
                        
                    }
                    else
                    {
                        RegistraErrorHold("Al Buscar Hold en TKT para Mantenimiento", Indice);
                        ActualizaHOLD_AS400(lbHOLDValido);
                    }

                   

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ActualizaHOLD_AS400(bool Valido)
        {
            try
            {
                msSQL400 = "UPDATE " + msLibAS400 + "." + lsArchivoHOLDAS400;

                //Si el Hold se realizo la modificacion
                string[] letras = { "A", "M", "D" };
                if(Valido == true && Funcion.Contains(lsTipoTranHold, letras))
                {
                    msSQL400 += $" set HPROC = '{lsTipoTranHold}' ";
                }
                else
                {
                    msSQL400 += $" set HPROC = 'X' ";
                }

                msSQL400 += $"WHERE HAN = '{MaRegistros[lnDatosHOLD].Cuenta}' AND HHLDN = {MaRegistros[lnDatosHOLD].Hold} AND HEQD = '{MaRegistros[lnDatosHOLD].Fechaequation1}' AND HTOP = '{MaRegistros[lnDatosHOLD].TipoTransaccion}'";

                int resultado = as400.EjecutaActualizacion(msSQL400);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void RegistraErrorHold(string Mensaje, int Indice)
        {
            using (SqlConnection connection = new SqlConnection(bd.connectionString))
            {
               
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("trnscBitacoraError");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {

                    BITACORA_ERROR_TICKET bitacora = new BITACORA_ERROR_TICKET { mensaje = Mensaje, tipo_msg = "H" };

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@mensaje", bitacora.mensaje);
                    command.Parameters.AddWithValue("@tipo_mensaje", bitacora.tipo_msg);
                    command.Parameters.Add("@ID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    msSQL = $@"
                    Insert Into TICKET..BITACORA_ERRORES_TICKET 
                    (fecha, mensaje, tipo_msg) 
                    values 
                    (getdate(), @mensaje, @tipo_mensaje) SET @ID = SCOPE_IDENTITY()";

                    command.CommandText = msSQL;
                    
                    int resultado = command.ExecuteNonQuery();
                    var identity = command.Parameters["@ID"].Value;


                    if (Indice > -1)
                    {
                        msSQL = @"
                            Insert Into TICKET..ERROR_HOLDS 
                            values (@identity, @usuario, @agencia, @cuenta, @sufijo, @desc1, @desc2, @desc3, @desc4, @fecha1, @fecha2, @monto, @fechaequation, @hold, @tipotransc)";

                        command.CommandText = msSQL;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@identity", identity);
                        command.Parameters.AddWithValue("@usuario", MaRegistros[Indice].Usuario);
                        command.Parameters.AddWithValue("@agencia", MaRegistros[Indice].Agencia);
                        command.Parameters.AddWithValue("@cuenta", MaRegistros[Indice].Cuenta);
                        command.Parameters.AddWithValue("@sufijo", MaRegistros[Indice].Sufijo);
                        command.Parameters.AddWithValue("@desc1", MaRegistros[Indice].Desc1);
                        command.Parameters.AddWithValue("@desc2", MaRegistros[Indice].Desc2);
                        command.Parameters.AddWithValue("@desc3", MaRegistros[Indice].Desc3);
                        command.Parameters.AddWithValue("@desc4", MaRegistros[Indice].Desc4);
                        command.Parameters.AddWithValue("@fecha1", MaRegistros[Indice].Fecha1.ToString("yyyy-MM-dd hh:mm:ss"));
                        command.Parameters.AddWithValue("@fecha2", MaRegistros[Indice].Fecha2.ToString("yyyy-MM-dd hh:mm:ss"));
                        command.Parameters.AddWithValue("@monto", MaRegistros[Indice].Monto);
                        command.Parameters.AddWithValue("@fechaequation", MaRegistros[Indice].Fechaequation);
                        command.Parameters.AddWithValue("@hold", MaRegistros[Indice].Hold);
                        command.Parameters.AddWithValue("@tipotransc", MaRegistros[Indice].TipoTransaccion);

                        resultado = command.ExecuteNonQuery();

                        transaction.Commit();


                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
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

        public object GetValueDr(SqlDataReader dr)
        {
            while(dr.Read())
            {
                return dr.GetValue(0);
            }
            return null;

        }


    }
}
