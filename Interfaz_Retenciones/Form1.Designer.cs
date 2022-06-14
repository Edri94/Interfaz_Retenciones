
namespace Interfaz_Retenciones
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTiempo = new System.Windows.Forms.Label();
            this.lblFechaSistema = new System.Windows.Forms.Label();
            this.txtLIB400 = new System.Windows.Forms.Label();
            this.txtArchivoHoldLA = new System.Windows.Forms.Label();
            this.lblDSNAS400 = new System.Windows.Forms.Label();
            this.lblDataBase = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Label();
            this.lblTransactionMessage = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRecibeHoldsLA = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNextHoldLA = new System.Windows.Forms.Label();
            this.lblPendHoldsLA = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbrMovimientos = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDBSrvr = new System.Windows.Forms.TextBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtDBUser = new System.Windows.Forms.TextBox();
            this.txtDBPswd = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDSN400 = new System.Windows.Forms.TextBox();
            this.txtNextHoldLA = new System.Windows.Forms.TextBox();
            this.txtUser400 = new System.Windows.Forms.TextBox();
            this.txtPswd400 = new System.Windows.Forms.TextBox();
            this.txtPeriodoHoldLA = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblTiempo);
            this.groupBox1.Controls.Add(this.lblFechaSistema);
            this.groupBox1.Controls.Add(this.txtLIB400);
            this.groupBox1.Controls.Add(this.txtArchivoHoldLA);
            this.groupBox1.Controls.Add(this.lblDSNAS400);
            this.groupBox1.Controls.Add(this.lblDataBase);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.pnlStatus);
            this.groupBox1.Controls.Add(this.lblTransactionMessage);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(742, 322);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 270);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Fecha del ticket:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(320, 207);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 20);
            this.label11.TabIndex = 4;
            this.label11.Text = "Biblioteca AS/400:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(320, 162);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 20);
            this.label10.TabIndex = 4;
            this.label10.Text = "Archivo AS/400:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(320, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 20);
            this.label9.TabIndex = 4;
            this.label9.Text = "DNS AS/400:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(320, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 20);
            this.label8.TabIndex = 4;
            this.label8.Text = "Base de Datos:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(320, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Servidor Ticket:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTiempo
            // 
            this.lblTiempo.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTiempo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTiempo.ForeColor = System.Drawing.Color.Lime;
            this.lblTiempo.Location = new System.Drawing.Point(405, 270);
            this.lblTiempo.Name = "lblTiempo";
            this.lblTiempo.Size = new System.Drawing.Size(82, 29);
            this.lblTiempo.TabIndex = 3;
            this.lblTiempo.Text = "00:00:00";
            // 
            // lblFechaSistema
            // 
            this.lblFechaSistema.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFechaSistema.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFechaSistema.ForeColor = System.Drawing.Color.Lime;
            this.lblFechaSistema.Location = new System.Drawing.Point(207, 270);
            this.lblFechaSistema.Name = "lblFechaSistema";
            this.lblFechaSistema.Size = new System.Drawing.Size(192, 29);
            this.lblFechaSistema.TabIndex = 3;
            // 
            // txtLIB400
            // 
            this.txtLIB400.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtLIB400.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLIB400.ForeColor = System.Drawing.Color.Lime;
            this.txtLIB400.Location = new System.Drawing.Point(461, 203);
            this.txtLIB400.Name = "txtLIB400";
            this.txtLIB400.Size = new System.Drawing.Size(266, 29);
            this.txtLIB400.TabIndex = 3;
            // 
            // txtArchivoHoldLA
            // 
            this.txtArchivoHoldLA.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtArchivoHoldLA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArchivoHoldLA.ForeColor = System.Drawing.Color.Lime;
            this.txtArchivoHoldLA.Location = new System.Drawing.Point(461, 158);
            this.txtArchivoHoldLA.Name = "txtArchivoHoldLA";
            this.txtArchivoHoldLA.Size = new System.Drawing.Size(266, 29);
            this.txtArchivoHoldLA.TabIndex = 3;
            // 
            // lblDSNAS400
            // 
            this.lblDSNAS400.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDSNAS400.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDSNAS400.ForeColor = System.Drawing.Color.Lime;
            this.lblDSNAS400.Location = new System.Drawing.Point(461, 113);
            this.lblDSNAS400.Name = "lblDSNAS400";
            this.lblDSNAS400.Size = new System.Drawing.Size(266, 29);
            this.lblDSNAS400.TabIndex = 3;
            // 
            // lblDataBase
            // 
            this.lblDataBase.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDataBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDataBase.ForeColor = System.Drawing.Color.Lime;
            this.lblDataBase.Location = new System.Drawing.Point(461, 68);
            this.lblDataBase.Name = "lblDataBase";
            this.lblDataBase.Size = new System.Drawing.Size(266, 29);
            this.lblDataBase.TabIndex = 3;
            // 
            // lblServer
            // 
            this.lblServer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblServer.ForeColor = System.Drawing.Color.Lime;
            this.lblServer.Location = new System.Drawing.Point(461, 23);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(266, 29);
            this.lblServer.TabIndex = 3;
            // 
            // pnlStatus
            // 
            this.pnlStatus.AutoSize = true;
            this.pnlStatus.Location = new System.Drawing.Point(84, 172);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(133, 20);
            this.pnlStatus.TabIndex = 2;
            this.pnlStatus.Text = "Interfaz Apagada";
            // 
            // lblTransactionMessage
            // 
            this.lblTransactionMessage.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblTransactionMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTransactionMessage.Location = new System.Drawing.Point(17, 40);
            this.lblTransactionMessage.Name = "lblTransactionMessage";
            this.lblTransactionMessage.Size = new System.Drawing.Size(286, 105);
            this.lblTransactionMessage.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkRecibeHoldsLA);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblNextHoldLA);
            this.groupBox2.Controls.Add(this.lblPendHoldsLA);
            this.groupBox2.Location = new System.Drawing.Point(13, 340);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 170);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Numero de operacion pendientes por recibir";
            // 
            // chkRecibeHoldsLA
            // 
            this.chkRecibeHoldsLA.AutoSize = true;
            this.chkRecibeHoldsLA.Location = new System.Drawing.Point(34, 96);
            this.chkRecibeHoldsLA.Name = "chkRecibeHoldsLA";
            this.chkRecibeHoldsLA.Size = new System.Drawing.Size(163, 24);
            this.chkRecibeHoldsLA.TabIndex = 6;
            this.chkRecibeHoldsLA.Text = "Hold\'s de houston";
            this.chkRecibeHoldsLA.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Holds de houston:";
            // 
            // lblNextHoldLA
            // 
            this.lblNextHoldLA.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblNextHoldLA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNextHoldLA.ForeColor = System.Drawing.Color.Lime;
            this.lblNextHoldLA.Location = new System.Drawing.Point(197, 91);
            this.lblNextHoldLA.Name = "lblNextHoldLA";
            this.lblNextHoldLA.Size = new System.Drawing.Size(289, 29);
            this.lblNextHoldLA.TabIndex = 3;
            // 
            // lblPendHoldsLA
            // 
            this.lblPendHoldsLA.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPendHoldsLA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPendHoldsLA.ForeColor = System.Drawing.Color.Lime;
            this.lblPendHoldsLA.Location = new System.Drawing.Point(197, 52);
            this.lblPendHoldsLA.Name = "lblPendHoldsLA";
            this.lblPendHoldsLA.Size = new System.Drawing.Size(289, 29);
            this.lblPendHoldsLA.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pbrMovimientos);
            this.groupBox3.Controls.Add(this.lblStatus);
            this.groupBox3.Location = new System.Drawing.Point(13, 516);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(741, 73);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // pbrMovimientos
            // 
            this.pbrMovimientos.Location = new System.Drawing.Point(276, 34);
            this.pbrMovimientos.Name = "pbrMovimientos";
            this.pbrMovimientos.Size = new System.Drawing.Size(422, 20);
            this.pbrMovimientos.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 34);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(144, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Movimientos Kapiti:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDBPswd);
            this.groupBox4.Controls.Add(this.txtDBUser);
            this.groupBox4.Controls.Add(this.txtDBName);
            this.groupBox4.Controls.Add(this.txtDBSrvr);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(773, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(471, 192);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Comunicacion con SQL Server";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtPswd400);
            this.groupBox5.Controls.Add(this.txtUser400);
            this.groupBox5.Controls.Add(this.txtPeriodoHoldLA);
            this.groupBox5.Controls.Add(this.txtNextHoldLA);
            this.groupBox5.Controls.Add(this.txtDSN400);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(773, 219);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(472, 226);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Comunicacion con AS400";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nombre del Servidor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Base de datos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Login de Usuario";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Password de Usuario";
            // 
            // txtDBSrvr
            // 
            this.txtDBSrvr.Location = new System.Drawing.Point(198, 33);
            this.txtDBSrvr.Name = "txtDBSrvr";
            this.txtDBSrvr.Size = new System.Drawing.Size(267, 26);
            this.txtDBSrvr.TabIndex = 1;
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(198, 72);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(267, 26);
            this.txtDBName.TabIndex = 1;
            // 
            // txtDBUser
            // 
            this.txtDBUser.Location = new System.Drawing.Point(198, 111);
            this.txtDBUser.Name = "txtDBUser";
            this.txtDBUser.Size = new System.Drawing.Size(267, 26);
            this.txtDBUser.TabIndex = 1;
            // 
            // txtDBPswd
            // 
            this.txtDBPswd.Location = new System.Drawing.Point(198, 150);
            this.txtDBPswd.Name = "txtDBPswd";
            this.txtDBPswd.Size = new System.Drawing.Size(267, 26);
            this.txtDBPswd.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "DSN (ODBC):";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 20);
            this.label13.TabIndex = 0;
            this.label13.Text = "Biblioteca de";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 138);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(129, 20);
            this.label14.TabIndex = 0;
            this.label14.Text = "Login de Usuario";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 182);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(159, 20);
            this.label15.TabIndex = 0;
            this.label15.Text = "Password de Usuario";
            // 
            // txtDSN400
            // 
            this.txtDSN400.Location = new System.Drawing.Point(198, 44);
            this.txtDSN400.Name = "txtDSN400";
            this.txtDSN400.Size = new System.Drawing.Size(267, 26);
            this.txtDSN400.TabIndex = 1;
            // 
            // txtNextHoldLA
            // 
            this.txtNextHoldLA.Location = new System.Drawing.Point(199, 89);
            this.txtNextHoldLA.Name = "txtNextHoldLA";
            this.txtNextHoldLA.Size = new System.Drawing.Size(188, 26);
            this.txtNextHoldLA.TabIndex = 1;
            // 
            // txtUser400
            // 
            this.txtUser400.Location = new System.Drawing.Point(198, 132);
            this.txtUser400.Name = "txtUser400";
            this.txtUser400.Size = new System.Drawing.Size(267, 26);
            this.txtUser400.TabIndex = 1;
            // 
            // txtPswd400
            // 
            this.txtPswd400.Location = new System.Drawing.Point(198, 176);
            this.txtPswd400.Name = "txtPswd400";
            this.txtPswd400.Size = new System.Drawing.Size(267, 26);
            this.txtPswd400.TabIndex = 1;
            // 
            // txtPeriodoHoldLA
            // 
            this.txtPeriodoHoldLA.Location = new System.Drawing.Point(393, 89);
            this.txtPeriodoHoldLA.Name = "txtPeriodoHoldLA";
            this.txtPeriodoHoldLA.Size = new System.Drawing.Size(72, 26);
            this.txtPeriodoHoldLA.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 603);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTransactionMessage;
        private System.Windows.Forms.Label txtLIB400;
        private System.Windows.Forms.Label txtArchivoHoldLA;
        private System.Windows.Forms.Label lblDSNAS400;
        private System.Windows.Forms.Label lblDataBase;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label pnlStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTiempo;
        private System.Windows.Forms.Label lblFechaSistema;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkRecibeHoldsLA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNextHoldLA;
        private System.Windows.Forms.Label lblPendHoldsLA;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar pbrMovimientos;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDBPswd;
        private System.Windows.Forms.TextBox txtDBUser;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtDBSrvr;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPswd400;
        private System.Windows.Forms.TextBox txtUser400;
        private System.Windows.Forms.TextBox txtNextHoldLA;
        private System.Windows.Forms.TextBox txtDSN400;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPeriodoHoldLA;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}

