using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_InterfazRetenciones.Models
{
    public class Producto
    {
        public FAHLDLA fAHLDLA { get; set; }
        public byte agencia { get; set; }
        public Int16 producto { get; set; }
        public Int16 status_producto { get; set; }
        public Int16 concepto_definido { get; set; }
        public Int32 producto_contratado { get; set; }
        public DateTime fecha_vencimiento { get; set; }

        public static Producto GteProducto(SqlDataReader dr, FAHLDLA  fAHLDLA = null)
        {
            Producto pr = new Producto();
            try
            {
                while (dr.Read())
                {

                    pr.agencia = dr.GetByte(0);
                    pr.producto = dr.GetInt16(1);
                    pr.status_producto = dr.GetInt16(2);
                    pr.concepto_definido = dr.GetInt16(3);

                    if(fAHLDLA != null)
                    {
                        pr.fAHLDLA = fAHLDLA;
                    }
                    
                }

                return pr;
            }
            catch (Exception ex)
            {
                return pr;
            }
        }
    }
}
