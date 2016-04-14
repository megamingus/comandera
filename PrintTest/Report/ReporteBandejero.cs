using CommonPrinter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using CommonPrinter.Model;

namespace CommonPrinter
{
    class ReporteBandejero
    {
        public List<QtyArticuloTotal> ArticulosVendidos { get; set; }
        public List<QtyArticuloTotal> ArticulosDePrimeraCarga { get; set; }
        public MontoTotal Montos;

        public Bandejero bandejero;

        public ReporteBandejero(Database database, Bandejero bandejero)
        {
            this.bandejero = bandejero;

            ArticulosVendidos = database.Query<QtyArticuloTotal>("select sum(qty) as 'Cantidad', name as 'NombreArticulo', price*sum(qty) as 'Total' from itemQty inner join Item on item.id=itemqty.itemid inner join orden on orden.id=itemqty.ordenid where bandejeroid=? and PrimerCarga='0' group by item.name", bandejero.Id);

            ArticulosDePrimeraCarga = database.Query<QtyArticuloTotal>("select sum(qty) as 'Cantidad', name as 'NombreArticulo', price*sum(qty) as 'Total' from itemQty inner join Item on item.id=itemqty.itemid inner join orden on orden.id=itemqty.ordenid where bandejeroid=? and PrimerCarga='1' group by item.name", bandejero.Id);

            Montos = database.Query<MontoTotal>("select sum(total) - sum(initialChange) as 'Total',sum(total) as 'TotalTickets', sum(initialChange) as 'TotalCambio' from Orden where bandejeroid=?",bandejero.Id).ToArray()[0];

        }

        public List<PrintItem> GenerarPagina()
        {
            var page = new List<PrintItem>();
            page.Add(new PrintItem(String.Format("{0}Fecha {1}{0}{0}", Environment.NewLine, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")), new Font("Arial", 10)));
            page.Add(new PrintItem(String.Format("{1}{0}{0}Venta Contado{0}{0}", Environment.NewLine,bandejero.Name), new Font("Arial", 24)));
            var subtotalContado = 0f;
            foreach (var a in ArticulosVendidos)
            {
                page.Add(
                    new PrintItem(
                        String.Format("{1} {2}   ${3}{0}{0}", Environment.NewLine, a.Cantidad, a.NombreArticulo, a.Total),
                        new Font("Arial", 10)));
                subtotalContado += a.Total;
            }

            page.Add(new PrintItem(String.Format("{0}Subtotal contado ${1}", Environment.NewLine, subtotalContado), new Font("Calibri", 16)));

            page.Add(new PrintItem(String.Format("{0}A Saldar{0}", Environment.NewLine, bandejero.Name), new Font("Arial", 45)));
            var subtotalaASaldar = 0f;
            page.Add(new PrintItem(String.Format("Detalle prod inicial {0}", Environment.NewLine), new Font("Calibri", 16)));

            foreach (var a in ArticulosDePrimeraCarga)
            {
                page.Add(
                    new PrintItem(
                        String.Format("{1} {2}{0}{0}", Environment.NewLine, a.Cantidad, a.NombreArticulo, a.Total),
                        new Font("Arial", 10)));
                subtotalaASaldar += a.Total;
            }
            page.Add(new PrintItem(String.Format("Subtotal en prod  ${1}{0}", Environment.NewLine, subtotalaASaldar), new Font("Calibri", 16)));
            page.Add(new PrintItem(String.Format("{0}Dinero Inicial  ${1}{0}", Environment.NewLine, Montos.TotalCambio), new Font("Calibri", 16)));

            page.Add(new PrintItem(String.Format("{0}Total a Saldar  ${1}{0}", Environment.NewLine, subtotalaASaldar+Montos.TotalCambio), new Font("Calibri", 20)));


            page.Add(new PrintItem(String.Format("{0}{0}VENTA TOTAL  ${1}{0}", Environment.NewLine, Montos.Total), new Font("Calibri", 20)));

            //page.Add(new PrintItem(String.Format("Venta Total  ${1}{0}{0}Cambio  ${2}{0}{0}", Environment.NewLine, Montos.Total, SaldoInicial), new Font("Calibri", 16)));
            //page.Add(new PrintItem(String.Format("Total en Caja  ${1}{0}", Environment.NewLine, Montos.Total + SaldoInicial), new Font("Calibri", 18)));
            page.Add(new PrintItem(String.Format("{0}@Megamingus{0}", Environment.NewLine), new Font("Calibri", 12)));
            return page;
        }
    }
}
