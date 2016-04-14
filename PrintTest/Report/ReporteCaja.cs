using CommonPrinter.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace CommonPrinter
{
    public class ReporteCaja
    {
        public List<QtyArticuloTotal> Articulos { get; set; }

        public MontoTotal Montos;

        public float SaldoInicial;
        
        public ReporteCaja(Database database){
            Articulos = database.Query<QtyArticuloTotal>("select sum(qty) as 'Cantidad', name as 'NombreArticulo', price*sum(qty) as 'Total' from itemQty inner join Item where item.id=itemqty.itemid group by item.name");

            Montos = database.Query<MontoTotal>("select sum(total) - sum(initialChange) as 'Total',sum(total) as 'TotalTickets', sum(initialChange) as 'TotalCambio' from Orden").ToArray()[0];

            var cajera = database.QueryAllCajeras().ToList().FirstOrDefault();

            SaldoInicial = cajera.InitialCash;            
        }

        public List<PrintItem> GenerarPagina()
        {
            var page = new List<PrintItem>();
            page.Add(new PrintItem(String.Format("Caja {0}", Environment.NewLine), new Font("Arial", 24)));
            page.Add(new PrintItem(String.Format("{0}Fecha {1}{0}{0}", Environment.NewLine, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")), new Font("Arial", 10)));
            foreach(var a in Articulos)
            {
                page.Add(
                    new PrintItem(
                        String.Format("{1} {2}   ${3}{0}{0}", Environment.NewLine, a.Cantidad,a.NombreArticulo,a.Total),
                        new Font("Arial", 10)));           
            }

            page.Add(new PrintItem(String.Format("Venta Total  ${1}{0}{0}Cambio  ${2}{0}{0}", Environment.NewLine, Montos.Total,SaldoInicial), new Font("Calibri", 16)));
            page.Add(new PrintItem(String.Format("Total en Caja  ${1}{0}", Environment.NewLine,Montos.Total+SaldoInicial), new Font("Calibri", 18)));
            page.Add(new PrintItem(String.Format("{0}@Megamingus{0}", Environment.NewLine), new Font("Calibri", 12)));
            return page;
        }
    }
    public class MontoTotal
    {
        public float Total { get; set; }
        public float TotalTickets { get; set; }
        public float TotalCambio { get; set; }
    }
    public class QtyArticuloTotal
    {
        public int Cantidad { get; set; }
        public float Total { get; set; }
        public string NombreArticulo { get; set; }
    }
}
