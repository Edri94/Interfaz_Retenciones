using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Biblioteca_InterfazRetenciones.Processes;
 

namespace Interfaz_Retenciones
{
    public partial class Form1 : Form
    {
        Main main;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            main = new Main(
             ref lblTransactionMessage,
             ref lblServer,
             ref lblDataBase,
             ref lblDSNAS400,
             ref lblArchivoHoldLA,
             ref lblLIB400,
             ref lblFechaSistema,
             ref lblTiempo,
             ref lblPendHoldsLA,    
             ref lblNextHoldLA,
             ref chkRecibeHoldsLA,
             ref pbrMovimientos,
             ref pnlStatus,
             ref lblStatus);

            main.Init();
        }
    }
}
