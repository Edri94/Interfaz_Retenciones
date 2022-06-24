using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Biblioteca_InterfazRetenciones.Data;
using Biblioteca_InterfazRetenciones.Helpers;
using Biblioteca_InterfazRetenciones.Models;
using Biblioteca_InterfazRetenciones.Processes;
 

namespace Interfaz_Retenciones
{
    public partial class Form1 : Form
    {
        Main main;
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
        bool Bandera;

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

        Encriptacion encriptacion;
        FuncionesBD bd;
        ConexionAS400 as400;

        string gsFechaSQL;
        string RecibeHOLDLA;
        string PendHoldsLA;

        DateTime fecha_servidor;
        TimeSpan hora_servidor;


        public Form1()
        {
            this.Height = 659;
            this.Width = 786;

            InitializeComponent();
        }


        /// <summary>
        /// Actualiza archivo del AS400  FAHLDLA
        /// </summary>
        /// <param name="Valido"></param>
        private void ActualizaHOLD_AS400(bool Valido)
        {
            try
            {
                msSQL400 = "UPDATE " + msLibAS400 + "." + lsArchivoHOLDAS400;

                //Si el Hold se realizo la modificacion
                string[] letras = { "A", "M", "D" };
                if (Valido == true && Funcion.Contains(lsTipoTranHold, letras))
                {
                    msSQL400 += $" set HPROC = '{lsTipoTranHold}' ";
                }
                else
                {
                    msSQL400 += $" set HPROC = 'X' ";
                }

                msSQL400 += $"WHERE HAN = '{MaRegistros[lnDatosHOLD].Cuenta}' AND HHLDN = {MaRegistros[lnDatosHOLD].Hold} AND HEQD = '{MaRegistros[lnDatosHOLD].Fechaequation1.ToString("yyyyMMdd")}' AND HTOP = '{MaRegistros[lnDatosHOLD].TipoTransaccion}'";

                int resultado = as400.EjecutaActualizacion(msSQL400);
            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Compara los Registros de AS400 con los registros de Ticket (nunca se usa)
        /// </summary>
        /// <param name="Indice"></param>
        /// <returns></returns>
        private bool ComparaHOLD_400Ticket(int Indice)
        {
            return false;
        }

        /// <summary>
        /// Inserta un mensaje de error provocado en el sistema (nunca se usa)
        /// </summary>
        /// <param name="Mensaje"></param>
        /// <param name="Indice"></param>
        private void RegistraError(string Mensaje, int Indice)
        {
            string lsIdentity;
            ERROR_MOVIMIENTO_DIRECTO emv = null;

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
                    BITACORA_ERROR_TICKET bitacora = new BITACORA_ERROR_TICKET { mensaje = Mensaje, tipo_msg = "M" };

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@mensaje", bitacora.mensaje);
                    command.Parameters.AddWithValue("@tipo_mensaje", bitacora.tipo_msg);
                    command.Parameters.Add("@ID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    msSQL = $@"
                    Insert Into {msDBName}..BITACORA_ERRORES_TICKET 
                    (fecha, mensaje, tipo_msg) 
                    values 
                    (getdate(), @mensaje, @tipo_mensaje) SET @ID = SCOPE_IDENTITY()";

                    command.CommandText = msSQL;

                    int resultado = command.ExecuteNonQuery();
                    var identity = command.Parameters["@ID"].Value;
                    
                    
                    if (Indice > -1)
                    {
                        msSQL = @"
                            Insert Into TICKET..ERROR_MOVIMIENTO_DIRECTO 
                            values (@identity, @agencia, @cuenta, @sufijo, @referencia, @fecha_valor, @monto, @narrativa1, @narrativa2, @narrativa3, @narrativa4, @fecha_mov, @cod_tran, @usuario_kap, @tipo_transaccion)";

                        command.CommandText = msSQL;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@identity", emv.NumMensaje);
                        command.Parameters.AddWithValue("@agencia", emv.Agencia);
                        command.Parameters.AddWithValue("@cuenta", emv.Cuenta);
                        command.Parameters.AddWithValue("@sufijo", emv.Sufijo);
                        command.Parameters.AddWithValue("@referencia", emv.Referencia);
                        command.Parameters.AddWithValue("@fecha_valor", emv.FechaValor);
                        command.Parameters.AddWithValue("@monto", emv.Monto);
                        command.Parameters.AddWithValue("@narrativa1", emv.Narrativa1);
                        command.Parameters.AddWithValue("@narrativa2", emv.Narrativa2);
                        command.Parameters.AddWithValue("@narrativa3", emv.Narrativa3);
                        command.Parameters.AddWithValue("@narrativa4", emv.Narrativa4);
                        command.Parameters.AddWithValue("@fecha_mov", emv.FechaMov);
                        command.Parameters.AddWithValue("@cod_tran", emv.CodTran);
                        command.Parameters.AddWithValue("@usuario_kap", emv.UsuarioKap);
                        command.Parameters.AddWithValue("@tipo_transaccion", emv.TipoTransaccion);

                        resultado = command.ExecuteNonQuery();

                        transaction.Commit();
                    }

                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Inserta un mensaje de error de recepción Hold provocado en el sistema
        /// </summary>
        /// <param name="Mensaje">mensaje de error</param>
        /// <param name="Indice">index de registro dodne se produjo el error</param>
        private void RegistraErrorHold(string Mensaje, int Indice)
        {
            ERROR_HOLD eh;

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

        
        /// <summary>
        /// Recibe los movimientos
        /// </summary>
        /// <param name="Agencia">numero Agencia</param>
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

                timer1.Enabled = false;

                pbrMovimientos.Value = 0;

                lbHOLDValido = false;
                Message_Transaction("Recibiendo HOLDS KAPITI - TICKET");


                lsArchivoHOLDAS400 = txtArchivoHoldLA.Text;
                Message_Status("Buscando Holds de Houston...");


                lnContador = 0;
                lnDatosHOLD = 0;

                msSQL400 = $"SELECT COUNT(*) FROM {msLibAS400}.{lsArchivoHOLDAS400} WHERE HPROC = ' '";

                OdbcDataReader dr = null;
                List<Map> registros = new List<Map>();

                registros.Add(new Map { Key = "lnNumRegistros", Type = "int" });
                dr = as400.EjecutaSelect(msSQL400);
                registros = as400.LLenarMapToQuery(registros, dr);

                lnNumRegistros = Int32.Parse(registros[0].Value.ToString());

                lblPendHoldsLA.Text = lnNumRegistros.ToString("D6");

                if (lnNumRegistros > 0)
                {
                    pbrMovimientos.Maximum = lnNumRegistros + 1;
                    lblStatus.Text = "Recibiendo Holds de Houston ";


                    msSQL400 = $"Select HUSR, HAB, HAN, HAS, HTIM, HDES1, HDES2, HDES3, HDES4, HSTD, HEXD, HAMT, HEQD, HHLDN, HTOP, HSEC From {msLibAS400}.{lsArchivoHOLDAS400} Where HPROC = ' '";

                    dr = as400.EjecutaSelect(msSQL400);

                    MaRegistros = new List<FAHLDLA>();

                    if (dr != null)
                    {
                        //Nota: Crear un objeto de tipo TKTLIB.FAHLDLA para llenar esta lista
                        while (dr.Read())
                        {
                            MaRegistros.Add(new FAHLDLA
                            {
                                Usuario = dr.GetString(0),
                                Agencia = Int32.Parse(dr.GetString(1)),
                                Cuenta = Int32.Parse(dr.GetString(2)),
                                Sufijo = Int32.Parse(dr.GetString(3)),
                                Hora = new TimeSpan(Int32.Parse(dr.GetString(4).Substring(0, 2)), Int32.Parse(dr.GetString(4).Substring(2, 2)), Int32.Parse(dr.GetString(4).Substring(4, 2))),
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

                            if (lnContador < pbrMovimientos.Maximum)
                            {
                                pbrMovimientos.Value = pbrMovimientos.Value + 1;
                            }

                            lblStatus.Text = "Recibiendo Holds de HO. (" + lnContador + " Registros)";
                        }

                        pbrMovimientos.Value = 0;

                        if (lnContador > 0)
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

                            if (Int32.Parse(lblPendHoldsLA.Text) > 0)
                            {
                                lblPendHoldsLA.Text = (Int32.Parse(lblPendHoldsLA.Text) - 1).ToString();
                            }

                            lsTipoTranHold = MaRegistros[lnDatosHOLD].TipoTransaccion;
                            lsCtaCompleta = MaRegistros[lnDatosHOLD].Agencia.ToString();
                            lsCtaCompleta = lsCtaCompleta + MaRegistros[lnDatosHOLD].Cuenta;
                            lsCtaCompleta = lsCtaCompleta + MaRegistros[lnDatosHOLD].Sufijo;
                            //R: pedimos el tipo de usuario LB@@ o TK@@
                            lsUsuario = MaRegistros[lnDatosHOLD].Usuario;

                            if (lsUsuario.Contains("TK@@"))
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

                            if (dr_1 == null)
                            {
                                lbHOLDValido = false;
                                //Inserta el BITACORA_ERROR_HOLD
                                RegistraErrorHold("No existe la Cuenta Eje " + lsCtaCompleta, lnDatosHOLD);
                            }

                            productos.Add(Producto.GetProducto(dr_1, MaRegistros[lnDatosHOLD]));

                            dr_1.Close();

                            mnAgencia = productos[lnDatosHOLD].agencia;
                            mnProducto = productos[lnDatosHOLD].producto;
                            mnStatus = productos[lnDatosHOLD].status_producto;
                            mnConcepto = productos[lnDatosHOLD].concepto_definido;





                            //Si el Hold No es Valido (NO se guarda en Ticket), solo lo marca
                            if (lbHOLDValido == false)
                            {
                                ActualizaHOLD_AS400(lbHOLDValido);
                            }
                            //Valida si existe el reg en Tkt antes de insertarlo
                            else
                            {
                                if (lsUsuario != "TK@@")
                                {
                                    GuardaHolds(lnDatosHOLD);
                                }
                                else
                                {
                                    lsNoHold = MaRegistros[lnDatosHOLD].Desc1;

                                    if (Funcion.IsNumeric(lsNoHold))
                                    {
                                        ActualizaHOLD_TKT(lnDatosHOLD);
                                    }
                                    else
                                    {
                                        ActualizaHOLDTKT_AS400(lbHOLDValido);
                                    }
                                }
                            }

                        }
                    }
                    pbrMovimientos.Value = 0;
                    Message_Transaction("");
                }
                MovPendientes();
                lblStatus.Text = "Hold's Kapiti";
                timer1.Enabled = true;
                this.Text = "Interfaz de Movimientos de las Agencias";

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Guardando en tablas de SQL Server
        /// </summary>
        /// <param name="Indice"></param>
        private void GuardaHolds(int Indice)
        {
            try
            {
                int lnProdCont = 0;
                string lsFechaVenc = String.Empty;
                int lnStatusProducto = 0;
                bool lbHOLDValido = false; ;
                int lnStatusProd = 0;
                int LnAgencia = 0;


                SqlDataReader dr;

                if (MaRegistros[Indice].TipoTransaccion != "A")
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

                    dr = bd.ejecutarConsulta(msSQL);

                    if (dr != null)
                    {
                        while (dr.Read())
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


                //Inicia Transacción para registrar el Hold en Ticket y marcar en 400
                using (SqlConnection connection = new SqlConnection(bd.connectionString))
                {

                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    transaction = connection.BeginTransaction("trnscGuardaHolds");
                    command.Connection = connection;
                    command.Transaction = transaction;

                    switch (MaRegistros[Indice].TipoTransaccion)
                    {
                        case "A":
                            msSQL = $@"
                                Insert into {msDBName}..PRODUCTO_CONTRATADO 
                                (producto, cuenta_cliente, clave_producto_contratado, fecha_contratacion, fecha_vencimiento, status_producto, agencia) 
                                values 
                                (@producto, @cuenta_cliente, @clave_producto_contratado, @fecha_contratacion, @fecha_vencimiento, @status_producto, @agencia)
                                SET @ID = SCOPE_IDENTITY()
                            ";

                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@producto", mnProducto);
                            command.Parameters.AddWithValue("@cuenta_cliente", MaRegistros[Indice].Cuenta);
                            command.Parameters.AddWithValue("@clave_producto_contratado", " ");
                            command.Parameters.AddWithValue("@fecha_contratacion", MaRegistros[Indice].Fecha1);
                            command.Parameters.AddWithValue("@fecha_vencimiento", MaRegistros[Indice].Fecha2);
                            command.Parameters.AddWithValue("@status_producto", mnStatus);
                            command.Parameters.AddWithValue("@agencia", mnAgencia);
                            command.Parameters.Add("@ID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                            command.CommandText = msSQL;


                            if (command.ExecuteNonQuery() < 1)
                            {
                                RegistraErrorHold("Al insertar en PRODUCTO_CONTRATADO ", Indice);
                                ActualizaHOLD_AS400(lbHOLDValido);
                            }
                            else
                            {
                                lnProdCont = Int32.Parse(command.Parameters["@ID"].Value.ToString());

                                msSQL = $@"
                                    insert into {msDBName}..HOLD 
                                    (producto_contratado, fecha_equation, hold, descripcion1, descripcion2, descripcion3, descripcion4,usuario_equation,secuencia_hold) 
                                    values 
                                    (@producto_contratado, @fecha_equation, @hold, @descripcion1, @descripcion2, @descripcion3, @descripcion4, @usuario_equation, @secuencia_hold)";

                                command.Parameters.Clear();


                                string tmp_fechaequation = MaRegistros[Indice].Fechaequation.ToString("yyyy-MM-dd") + " " + MaRegistros[Indice].Hora.ToString();

                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                command.Parameters.AddWithValue("@fecha_equation", tmp_fechaequation);
                                command.Parameters.AddWithValue("@hold", MaRegistros[Indice].Hold);
                                command.Parameters.AddWithValue("@descripcion1", MaRegistros[Indice].Desc1);
                                command.Parameters.AddWithValue("@descripcion2", MaRegistros[Indice].Desc2);
                                command.Parameters.AddWithValue("@descripcion3", MaRegistros[Indice].Desc3);
                                command.Parameters.AddWithValue("@descripcion4", MaRegistros[Indice].Desc4);
                                command.Parameters.AddWithValue("@usuario_equation", MaRegistros[Indice].Usuario);
                                command.Parameters.AddWithValue("@secuencia_hold", MaRegistros[Indice].NumeroSecuencia);

                                command.CommandText = msSQL;


                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Al insertar en la tabla HOLD", Indice);

                                    ActualizaHOLD_AS400(lbHOLDValido);
                                }
                                else
                                {

                                    msSQL = $@" 
                                        insert into TICKET..CONCEPTO
                                        (producto_contratado, concepto_definido, valor_concepto)
                                        values
                                        (@producto_contratado, @concepto_definido, @valor_concepto)";

                                    command.Parameters.Clear();

                                    command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                    command.Parameters.AddWithValue("@concepto_definido", mnConcepto);
                                    command.Parameters.AddWithValue("@valor_concepto", MaRegistros[Indice].Monto);

                                    command.CommandText = msSQL;

                                    if (command.ExecuteNonQuery() < 0)
                                    {
                                        RegistraErrorHold("Al insertar en la tabla CONCEPTO", Indice);
                                        ActualizaHOLD_AS400(lbHOLDValido);
                                    }
                                }

                            }

                            msSQL = $"Select prefijo_agencia from CATALOGOS..AGENCIA AG where agencia = {mnAgencia}";

                            dr = bd.ejecutarConsulta(msSQL);

                            if (dr == null)
                            {
                                RegistraErrorHold("Ocurrio un error al Seleccionar la agencia." + lnProdCont, Indice);
                                ActualizaHOLD_AS400(lbHOLDValido);
                            }
                            else
                            {
                                LnAgencia = Int32.Parse(bd.LLenarMapToQuery(new Map { Key = "LnAgencia", Type = "string" }, dr).Value.ToString());

                                msSQL = $"insert into {msDBName}..EVENTO_PRODUCTO (producto_contratado, fecha_evento, status_producto, comentario_evento, usuario) values(@producto_contratado, @fecha_evento, @status_producto, @comentario_evento, @usuario)";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                command.Parameters.AddWithValue("@fecha_evento", fecha_servidor);
                                command.Parameters.AddWithValue("@status_producto", Funcion.Mid(LnAgencia.ToString(), 1, 2) + "02");
                                command.Parameters.AddWithValue("@comentario_evento", "Alta de Hold " + " " + MaRegistros[Indice].Usuario);
                                command.Parameters.AddWithValue("@usuario", 133);

                                command.CommandText = msSQL;


                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al insertar el Evento de alta de Hold." + lnProdCont, Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);
                                }

                            }
                            break;

                        case "M":
                            if (DateTime.ParseExact(lsFechaVenc, "yyyyMMdd", null) != MaRegistros[Indice].Fecha2)
                            {
                                if (MaRegistros[Indice].Fecha2 <= fecha_servidor && DateTime.ParseExact(lsFechaVenc, "yyyyMMdd", null) > fecha_servidor)
                                {
                                    msSQL = $"UPDATE {msDBName}..PRODUCTO_CONTRATADO set fecha_vencimiento = @fecha_vencimiento, status_producto = @status_producto WHERE producto_contratado = @producto_contratado";

                                    command.Parameters.Clear();

                                    command.Parameters.AddWithValue("@fecha_vencimiento", MaRegistros[Indice].Fecha2);
                                    command.Parameters.AddWithValue("@status_producto", Funcion.Mid(lnStatusProducto.ToString(), 1, 2) + "22");
                                    command.Parameters.AddWithValue("@producto_contratado", lnProdCont);

                                    command.CommandText = msSQL;

                                    if (command.ExecuteNonQuery() < 0)
                                    {
                                        RegistraErrorHold("Ocurrio un error al actualizar los datos del Hold.", Indice);
                                        ActualizaHOLD_AS400(lbHOLDValido);

                                    }

                                }
                                else if (MaRegistros[Indice].Fecha2 > fecha_servidor && DateTime.ParseExact(lsFechaVenc, "yyyyMMdd", null) <= fecha_servidor)
                                {
                                    msSQL = $"UPDATE {msDBName}..PRODUCTO_CONTRATADO SET fecha_vencimiento = @fecha_vencimiento, status_producto = @status_producto WHERE producto_contratado = @producto_contratado";

                                    command.Parameters.Clear();

                                    command.Parameters.AddWithValue("@fecha_vencimiento", MaRegistros[Indice].Fecha2);
                                    command.Parameters.AddWithValue("@status_producto", Funcion.Mid(lnStatusProducto.ToString(), 1, 2) + "02");
                                    command.Parameters.AddWithValue("@producto_contratado", lnProdCont);

                                    command.CommandText = msSQL;

                                    if (command.ExecuteNonQuery() < 0)
                                    {
                                        RegistraErrorHold("Ocurrio un error al actualizar los datos del Hold.", Indice);
                                        ActualizaHOLD_AS400(lbHOLDValido);

                                    }
                                }
                            }
                            else
                            {
                                msSQL = $"UPDATE {msDBName}..PRODUCTO_CONTRATADO SET fecha_vencimiento = @fecha_vencimiento WHERE producto_contratado = @producto_contratado";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@fecha_vencimiento", MaRegistros[Indice].Fecha2);
                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al actualizar los datos del Hold.", Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);

                                }

                                msSQL = $"UPDATE {msDBName}..PRODUCTO_CONTRATADO SET fecha_contratacion = @fecha_contratacion WHERE producto_contratado = @producto_contratado";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@fecha_vencimiento", MaRegistros[Indice].Fecha1);
                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al actualizar los datos del Hold.", Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);

                                }

                                msSQL = $"UPDATE {msDBName}..HOLD SET descripcion1 = @descripcion1, descripcion2 = @descripcion2, descripcion3 = @descripcion3, descripcion4 = @descripcion4 WHERE producto_contratado = @producto_contratado AND hold = @hold";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@descripcion1", MaRegistros[Indice].Desc1);
                                command.Parameters.AddWithValue("@descripcion2", MaRegistros[Indice].Desc2);
                                command.Parameters.AddWithValue("@descripcion3", MaRegistros[Indice].Desc3);
                                command.Parameters.AddWithValue("@descripcion4", MaRegistros[Indice].Desc4);
                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                command.Parameters.AddWithValue("@hold", MaRegistros[Indice].Hold);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al actualizar en la tabla Hold.", Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);
                                }

                                msSQL = $"UPDATE {msDBName}..CONCEPTO SET valor_concepto = @valor_concepto WHERE producto_contratado = @producto_contratado AND concepto_definido = @concepto_definido";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@valor_concepto", MaRegistros[Indice].Monto);
                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                command.Parameters.AddWithValue("@concepto_definido", mnConcepto);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al actualizar en la tabla CONCEPTO el Mant. Hold.", Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);
                                }
                                msSQL = $"insert into {msDBName}..EVENTO_PRODUCTO(producto_contratado, fecha_evento, status_producto, comentario_evento, usuario) values( @producto_contratado, @fecha_evento, @status_producto, @comentario_evento, @usuario)";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                                command.Parameters.AddWithValue("@fecha_evento", "getdate()");
                                command.Parameters.AddWithValue("@status_producto", lnStatusProd);
                                command.Parameters.AddWithValue("@comentario_evento", "Mantenimiento Automático de Hold " + " " + MaRegistros[Indice].Usuario);
                                command.Parameters.AddWithValue("@usuario", 133);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    RegistraErrorHold("Ocurrio un error al insertar el Evento de Manto. de Hold." + lnProdCont, Indice);
                                    ActualizaHOLD_AS400(lbHOLDValido);
                                }


                            }
                            break;

                        case "D":
                            msSQL = $"UPDATE {msDBName}..PRODUCTO_CONTRATADO set status_producto = @status_producto WHERE producto_contratado  = @producto_contratado AND cuenta_cliente = @cuenta_cliente AND agencia = @agencia AND '{MaRegistros[Indice].Fecha2.ToString("yyyy-MM-dd hh:mm:ss")}' >= '{fecha_servidor}'";

                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@status_producto", Funcion.Mid(lnStatusProd.ToString(), 1, 2) + 42);
                            command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                            command.Parameters.AddWithValue("@cuenta_cliente", MaRegistros[Indice].Cuenta);
                            command.Parameters.AddWithValue("@agencia", "Mantenimiento Automático de Hold " + " " + MaRegistros[Indice].Usuario);

                            command.CommandText = msSQL;

                            if (command.ExecuteNonQuery() < 0)
                            {
                                RegistraErrorHold("Ocurrio un error al actualizar los datos del Hold.", Indice);
                                ActualizaHOLD_AS400(lbHOLDValido);
                            }

                            msSQL = $"insert into {msDBName}..EVENTO_PRODUCTO(producto_contratado, fecha_evento, status_producto, comentario_evento, usuario) usuario(@producto_contratado, @fecha_evento, @status_producto, @comentario_evento, @usuario)";

                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@producto_contratado", lnProdCont);
                            command.Parameters.AddWithValue("@fecha_evento", fecha_servidor);
                            command.Parameters.AddWithValue("@status_producto", Funcion.Mid(lnStatusProd.ToString(), 1, 2) + 42);
                            command.Parameters.AddWithValue("@comentario_evento", "Cancelación Automática de Hold " + " " + MaRegistros[Indice].Usuario);
                            command.Parameters.AddWithValue("@usuario", 133);

                            command.CommandText = msSQL;

                            if (command.ExecuteNonQuery() < 0)
                            {
                                RegistraErrorHold("Ocurrio un error al insertar el Evento de Manto. de Hold." + lnProdCont, Indice);
                                ActualizaHOLD_AS400(lbHOLDValido);
                            }

                            break;

                    }
                    command.Transaction.Commit();
                    ActualizaHOLD_AS400(lbHOLDValido);


                }
            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Establece conexion con la base de datos de SQL Server
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Despliega los datos de configuracion de la tabla PARAMETROS
        /// </summary>
        /// <returns></returns>
        private bool DespliegaDatos()
        {
            string lsCadena;
            int lnIndice;
            bool despliegaDatos = false;

            try
            {
                lblFechaSistema.Text = fecha_servidor.ToString();
                gsFechaSQL = fecha_servidor.ToString();
            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }

            return true;

        }


        /// <summary>
        /// Se ejecuta cuando el checkbox "Holds de houston" canbia de valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRecibeHoldsLA_CheckedChanged(object sender, EventArgs e)
        {
            if(chkRecibeHoldsLA.Checked)
            {
                lblStatus.Text = "Holds Kapiti Houston";
            }
            else
            {
                lblPendHoldsLA.Text = "";
            }
           
        }

        /// <summary>
        /// Ejecuta funcion RecibeHolds y al terminar escribe fehca del siguiente proceso
        /// </summary>
        private void RecibeHoldsLA()
        {
            RecibeHolds(1);
            lblNextHoldLA.Text = DateTime.Parse(this.gsFechaSQL).AddMinutes(5).ToString("dd-MM-yyyy hh:mm");
        }

        /// <summary>
        /// Reescribe fechas en archivo de configuracion
        /// </summary>
        private void SaveDates()
        {
            try
            {
                Funcion.SetParameterAppSettings("PERIODOHOLDLA", txtPeriodoHoldLA.Text, "PARAMETROS");
                Funcion.SetParameterAppSettings("FECHAHOLDLA", txtNextHoldLA.Text, "PARAMETROS");
            }
            catch (Exception ex )
            {

                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Inicia la Transmisión y refresca el estatus de la información
        /// </summary>
        private void Start()
        {
            if (!cblmc_ObtieneUsuario(msUSREQUGC, mn_numuserGC))
            {
                MessageBox.Show("No fue Posible Obtener el Usuario  de la Agencia Gran Caimán", "Obtención de Usuario");
            }
            else
            {
                msClaveModParams = ValorParametro("INTFMOVS_PARAMS");
            }
            pnlStatus.Visible = false;

            if (DespliegaDatos())
            {
                timer1.Enabled = true;
                timer1.Interval = 15000;
            }
            MovPendientes();
        }

        /// <summary>
        /// Detiene la transmision
        /// </summary>
        private void Detener()
        {
            pnlStatus.Visible = true;
            Message_PnlStatus("DESCONECTANDO");
            SaveDates();
        }


        /// <summary>
        /// Obtener el número de registros pendientes de bajarse de los Movimientos de Kapiti L.A., Kapiti  G.C. y Hold L.A. si éstos existen
        /// </summary>
        private void MovPendientes()
        {
            OdbcDataReader dr;
            try
            {
                Message_Transaction("Actualizando Pendientes a Recibir.");
                lblPendHoldsLA.Text = "0";

                if (chkRecibeHoldsLA.Checked == true)
                {
                    msSQL400 = $"SELECT COUNT(*) FROM {msLibAS400}.{txtArchivoHoldLA.Text}  Where HPROC <> 'A' and HPROC <> 'M' and HPROC <> 'D' and HPROC <> 'U' and HPROC <> 'X' and HPROC <> 'S'";

                    dr = as400.EjecutaSelect(msSQL400);

                    if (dr != null)
                    {
                        lblPendHoldsLA.Text = as400.LLenarMapToQuery(new Map { Key = "cuenta", Type = "int" }, dr).Value.ToString();
                    }
                }

                mdRefreshTime = fecha_servidor.AddMinutes(5);
                Message_Transaction("");

            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }
        }


        /// <summary>
        /// Carga Formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            txtNextHoldLA.GotFocus += txtNextHoldLA_GotFocus;

            main = new Main();

            
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Init();
        }

       

        /// <summary>
        /// Despliega un mensaje en la pantalla de Procesos
        /// </summary>
        /// <param name="mensaje"></param>
        private void Message_Transaction(string mensaje)
        {
            lblTransactionMessage.Text = mensaje;
        }

        /// <summary>
        /// Cada certo tiempo verifica la configuración para lanzar los procesos de movimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            string LsFechaSistema;
            string LsHora;
            int idTarea = 0;


            //***********************************************************************************
            //LsHora = Hora(DateTime.ParseExact(txtNextHoldLA.Text, "dd-MM-yyyy hh:mm", null).ToString(), Int32.Parse(txtPeriodoHoldLA.Text));

            //string extract = Funcion.Mid(DateTime.Parse(LsHora).ToString("hh:mm"), 0, 2);
            //if (Funcion.Mid(DateTime.Parse(LsHora).ToString("hh:mm"), 1, 2) == "00")
            //{

            //}
            //***********************************************************************************


            if (timer1.Interval != 0)
            {
                if (chkRecibeHoldsLA.Checked)
                {
                    if (DateTime.ParseExact(txtNextHoldLA.Text, "dd-MM-yyyy hh:mm", null).ToString("dd-MM-yyyy hh:mm") == fecha_servidor.ToString("dd-MM-yyyy hh:mm"))
                    {
                        idTarea = 3;
                        LsHora = Hora(DateTime.ParseExact(txtNextHoldLA.Text, "dd-MM-yyyy hh:mm", null).ToString(), Int32.Parse(txtPeriodoHoldLA.Text));

                        if (Funcion.Mid(DateTime.Parse(LsHora).ToString("hh:mm"),1,2) == "00")
                        {
                            LsFechaSistema = bd.obtenerFechaServidor(true);
                            lblFechaSistema.Text = LsFechaSistema;
                            txtNextHoldLA.Text = Fecha(LsFechaSistema) + " " + Hora(txtNextHoldLA.Text, Int32.Parse(txtPeriodoHoldLA.Text));
                        }
                        else
                        {
                            txtNextHoldLA.Text = Fecha(txtNextHoldLA.Text) + " " + Hora(txtNextHoldLA.Text, Int32.Parse(txtPeriodoHoldLA.Text));
                        }

                        if (chkRecibeHoldsLA.Checked) RecibeHolds(1);
                    }
                }
            }

            if (idTarea != 0) AjustaTimers(idTarea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNextHoldLA_TextChanged(object sender, EventArgs e)
        {
            lblNextHoldLA.Text = txtNextHoldLA.Text;
        }

        /// <summary>
        /// Se desencadena cuando pierde el FOCUS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNextHoldLA_Leave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se desencadena cuando obtiene el FOCUS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNextHoldLA_GotFocus(object sender, EventArgs e)
        {
            timer2.Enabled = false;

        }

        /// <summary>
        /// Otiene el número de usuario recibido como parámetro
        /// </summary>
        /// <param name="ps_nombre"></param>
        /// <param name="pn_usuarioGC"></param>
        /// <returns></returns>
        private bool cblmc_ObtieneUsuario(string ps_nombre, int pn_usuarioGC)
        {
            bool cblmc_ObtieneUsuario = false;
            string lsSQL = String.Empty;

            try
            {
                pn_usuarioGC = -1;
                lsSQL = $"SELECT usuario FROM CATALOGOS..{gs_Usuario} WHERE login = '{ps_nombre}'";

                SqlDataReader dr = bd.ejecutarConsulta(lsSQL);

                if (dr != null)
                {
                    pn_usuarioGC = Int32.Parse(bd.LLenarMapToQuery(new Map { Key = "usuario", Type = "smallint" }, dr).Value.ToString());
                    cblmc_ObtieneUsuario = true;
                }
                else
                {
                    cblmc_ObtieneUsuario = false;
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }

            return cblmc_ObtieneUsuario;
        }

        /// <summary>
        /// Devuelve el valor de un parámetro de la tabla PARAMETRIZACION
        /// </summary>
        /// <param name="Parametro"></param>
        /// <returns></returns>
        private string ValorParametro(string Parametro)
        {
            string ls_sql = String.Empty;
            string ValorParametro = String.Empty;
            try
            {
                ls_sql = $"Select valor from PARAMETRIZACION where codigo = '{Parametro}'";

                SqlDataReader dr = bd.ejecutarConsulta(ls_sql);

                if (dr != null)
                {
                    ValorParametro = bd.LLenarMapToQuery(new Map { Key = "valor", Type = "string" }, dr).Value.ToString().Trim();
                }
                else
                {
                    MessageBox.Show($"Error al Obtener Dato Parametrizado. {Environment.NewLine} Notifique al Departamento de Sistemas! {Environment.NewLine} Código de Parametro: {Parametro}", "Error");
                }
            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }
            return ValorParametro;
        }

        /// <summary>
        /// Se desencadena al presionar una telca sobre el input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNextHoldLA_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProceso"></param>
        private void AjustaTimers(int idProceso)
        {
            DateTime fecha_tmp = DateTime.Parse(txtNextHoldLA.Text);
            if (hora_servidor > TimeSpan.ParseExact(fecha_tmp.ToString("hh:mm"), "hh:mm", null))
            {
                txtNextHoldLA.Text = fecha_tmp + hora_servidor.ToString("hh:mm");
            }
        }

        /// <summary>
        /// Muestra un mensaje en el lbael de STATUS 
        /// </summary>
        /// <param name="mensaje"></param>
        private void Message_PnlStatus(string mensaje)
        {
            pnlStatus.ForeColor = Color.Green;
            pnlStatus.Text = mensaje;
        }


        /// <summary>
        /// Actualiza datos en tablas de SQL Server
        /// </summary>
        /// <param name="Indice"></param>
        private void ActualizaHOLD_TKT(int Indice)
        {
            int lnProdCont = 0;
            string lsFechaVenc = String.Empty;
            int lnStatusProducto = 0;
            bool lbHOLDValido = false;
            int lnStatusProd = 0;
            int LnAgencia = 0;
            SqlDataReader dr;

            try
            {
                lbHOLDValido = false;

                if (MaRegistros[Indice].TipoTransaccion != "A")
                {
                    msSQL = $@"
                        Select 
                            PC.producto_contratado 
                        FROM 
                            {msDBName}..PRODUCTO_CONTRATADO PC 
                            INNER JOIN {msDBName}..HOLD H ON PC.producto_contratado = H.producto_contratado
                            INNER JOIN {msDBName}..HOLD_RETIRO HR ON PC.producto_contratado = HR.producto_contratado
                            INNER JOIN {msDBName}..CONCEPTO C ON PC.producto_contratado = C.producto_contratado
                            INNER JOIN {msDBName}..CONCEPTO_DEFINIDO CD ON PC.producto_contratado = {lsNoHold}
                        WHERE
                            CD.concepto_definido_global = {lnConceptoDefinidoGlobal}
                    ";

                    dr = bd.ejecutarConsulta(msSQL);

                    if (dr != null)
                    {
                        lnProdCont = Int32.Parse(bd.LLenarMapToQuery(new Map { Key = "producto_contratado", Type = "int" }, dr).ToString());
                    }
                    else
                    {
                        RegistraErrorHold("Al Buscar Hold en TKT para Mantenimiento", Indice);
                        ActualizaHOLDTKT_AS400(lbHOLDValido);
                    }
                    msSQL = $"Select status_producto from {msDBName}..PRODUCTO_CONTRATADO where producto_contratado  = {lnProdCont}";

                    dr = bd.ejecutarConsulta(msSQL);

                    if (dr != null)
                    {
                        RegistraErrorHold("Ocurrio un error al obtener el status del Hold.", Indice);
                        ActualizaHOLDTKT_AS400(lbHOLDValido);
                    }
                    else
                    {
                        lnStatusProd = Int32.Parse(bd.LLenarMapToQuery(new Map { Key = "status_producto", Type = "int" }, dr).ToString());
                    }
                }

                using (SqlConnection connection = new SqlConnection(bd.connectionString))
                {

                    switch (MaRegistros[Indice].TipoTransaccion)
                    {
                        case "A":

                            connection.Open();

                            SqlCommand command = connection.CreateCommand();
                            SqlTransaction transaction;

                            transaction = connection.BeginTransaction("trnscActualizaHOLDTKT");
                            command.Connection = connection;
                            command.Transaction = transaction;


                            msSQL = $@"
                                UPDATE 
                                    {msDBName}....HOLD
                                SET
                                    hold = @hold, secuencia_hold = @secuencia_hold
                                FROM
                                    {msDBName}..PRODUCTO_CONTRATADO PC
                                    INNER JOIN {msDBName}..HOLD H PC.producto_contratado = H.producto_contratado  
                                    INNER JOIN {msDBName}..HOLD_RETIRO HR ON PC.producto_contratado = HR.producto_contratado
                                WHERE
                                    status_producto = @status_producto {lnStatusProductoGlobal}
                                ";


                            command.Parameters.Clear();

                            command.Parameters.AddWithValue("@hold", MaRegistros[Indice].Hold);
                            command.Parameters.AddWithValue("@secuencia_hold", MaRegistros[Indice].NumeroSecuencia);
                            command.Parameters.AddWithValue("@status_producto", lnStatusProductoGlobal);

                            command.CommandText = msSQL;

                            if (command.ExecuteNonQuery() < 1)
                            {
                                RegistraErrorHold("Al insertar en HOLD ", Indice);
                                ActualizaHOLDTKT_AS400(lbHOLDValido);
                            }
                            else
                            {
                                msSQL = $@"
                                UPDATE 
                                        {msDBName}....HOLD_RETIRO
                                    SET
                                        hold = @hold
                                    FROM
                                        {msDBName}..PRODUCTO_CONTRATADO PC
                                        INNER JOIN {msDBName}..HOLD H PC.producto_contratado = H.producto_contratado  
                                        INNER JOIN {msDBName}..HOLD_RETIRO HR ON PC.producto_contratado = HR.producto_contratado
                                    WHERE
                                        status_producto = @status_producto {lnStatusProductoGlobal}
                                ";

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@hold", MaRegistros[Indice].Hold);
                                command.Parameters.AddWithValue("@status_producto", lnStatusProductoGlobal);

                                command.CommandText = msSQL;

                                if (command.ExecuteNonQuery() < 1)
                                {
                                    RegistraErrorHold("Al insertar en la tabla HOLD_RETIRO", Indice);
                                    ActualizaHOLDTKT_AS400(lbHOLDValido);
                                }

                                command.Transaction.Commit();
                            }
                            break;

                        case "M":
                            ActualizaHOLDTKT_AS400(lbHOLDValido);
                            break;

                        case "D":
                            ActualizaHOLDTKT_AS400(lbHOLDValido);
                            break;
                    }
                }
                ActualizaHOLDTKT_AS400(lbHOLDValido);
                mnProdCont = 0;
                mnOperDef = 0;
                msTiempo = "";
                msTipoMovimiento = "";


            }
            catch (Exception ex)
            {

                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Actualiza en el archivo de AS400 FAHLDLA
        /// </summary>
        /// <param name="Valido"></param>
        private void ActualizaHOLDTKT_AS400(bool Valido)
        {
            try
            {
                msSQL400 = $"UPDATE {msLibAS400}.{lsArchivoHOLDAS400}";

                if (Valido)
                {
                    msSQL400 += (lsTipoTranHold == "A") ? "set HPROC = 'M' " : "";
                    msSQL400 += (lsTipoTranHold == "M") ? "set HPROC = 'X' " : "";
                    msSQL400 += (lsTipoTranHold == "D") ? "set HPROC = 'X' " : "";
                }
                else
                {
                    msSQL400 += "set HPROC = 'X'";
                }

                msSQL400 += $"WHERE HAN = '{MaRegistros[lnDatosHOLD].Cuenta}'";
                msSQL400 += $"AND HHLDN = {MaRegistros[lnDatosHOLD].Hold}";
                msSQL400 += $"AND HEQD = '{MaRegistros[lnDatosHOLD].Fechaequation1}'";
                msSQL400 += $"AND HTOP = '{MaRegistros[lnDatosHOLD].TipoTransaccion}'";

                as400.EjecutaActualizacion(msSQL400);


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }


        /// <summary>
        /// Proceso INICIAL
        /// </summary>
        public void Init()
        {
            int lnWDif;
            int lnHDif;
            string Str_Ruta;

            this.mbConexion = false;
            Str_Ruta = "ruta app.config";


            EstableceParametros();

            if (!Bandera)
            {
                Funcion.SetParameterAppSettings("BANDERA", "1", "PARAMETROS");
            }
            else
            {

            }

            if (EstableceConexionBD() && as400.Conectar())
            {
                fecha_servidor = DateTime.Parse(bd.obtenerFechaServidor());
                hora_servidor = TimeSpan.Parse(fecha_servidor.ToString("hh:mm:ss"));

                txtLIB400.Text = msLibAS400;
                txtDSN400.Text = msDSN400;
                txtUser400.Text = msUser400;
                txtPswd400.Text = msPswd400;
                txtDBUser.Text = msDBuser;
                txtDBPswd.Text = msDBPswd;
                txtDBName.Text = msDBName;
                txtDBSrvr.Text = msDBSrvr;

                lblServer.Text = msDBSrvr;
                lblDataBase.Text = msDBName;
                lblDSNAS400.Text = msDSN400;

                this.gsFechaSQL = bd.obtenerFechaServidor();

                chkRecibeHoldsLA.Checked = (RecibeHOLDLA == "1") ? true : false;
                txtArchivoHoldLA.Text = ArchivoHOLDS;
                
                string fecha_tmp = Funcion.getValueAppConfig("FECHAHOLDLA", "PARAMETROS");
                txtPeriodoHoldLA.Text = Funcion.getValueAppConfig("PERIODOHOLDLA", "PARAMETROS");
                DateTime fecha_appconfig = DateTime.Parse(fecha_tmp);
                txtNextHoldLA.Text = fecha_appconfig.ToString("dd-MM-yyyy") + " 10:00";


                mnFirstTime = 1;


                Message_PnlStatus("CONECTANDO....");
                Start();
                Message_PnlStatus("EN LINEA....");
                RecibeHoldsLA();
                Detener();

            }
        }

        /// <summary>
        /// Obtiene los parametros de configuracion
        /// </summary>
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
            Bandera = (Funcion.getValueAppConfig("PERIODOHOLDLA", "PARAMETROS") == "1") ? true : false;


            as400 = new ConexionAS400(msDSN400, msUser400, msPswd400);


        }



        private void Message_Status(string mensaje)
        {
            lblStatus.Text = mensaje;
        }

        public object GetValueDr(SqlDataReader dr)
        {
            while (dr.Read())
            {
                return dr.GetValue(0);
            }
            return null;

        }

       

        private string Hora(string Origen, int Minutos)
        {
            string lsHoraCalculo = Origen;
            DateTime temp;
            
            if(lsHoraCalculo == "")
            {
                lsHoraCalculo = Hora(DateTime.Now.ToString("hh:mm"), -5);
            }
            else if (!DateTime.TryParse(lsHoraCalculo, out temp))
            {
                lsHoraCalculo = Hora(DateTime.Now.ToString("hh:mm"), -5);
            }
            else
            {
                lsHoraCalculo = DateTime.Parse(lsHoraCalculo).ToString("hh:mm");
            }
            return DateTime.Parse(lsHoraCalculo).AddMinutes(Minutos).ToString("hh:mm");
        }

        private string Fecha(string Origen)
        {
            string lsFechaCalculo = Origen.Trim();
            DateTime temp;

            if (lsFechaCalculo == "")
            {
                lsFechaCalculo = bd.obtenerFechaServidor();
            }
            else if(!DateTime.TryParse(lsFechaCalculo, out temp))
            {
                lsFechaCalculo = bd.obtenerFechaServidor();
            }
            
            return DateTime.Parse(lsFechaCalculo).ToString("dd-MM-yyyy");
        }

        
    }
}



