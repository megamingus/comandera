﻿using CommonPrinter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonPrinter.Model;


namespace Tickets
{
    public partial class MassPrint : Form
    {
        public MassPrint()
        {
            InitializeComponent();
        }

        Database db;

        private void Form1_Load(object sender, EventArgs e)
        {
            db = new Database("Database");

            var items = db.QueryAllItems();

            fromTicketNumber = new NumericUpDown();
            fromTicketNumber.Text = "Desde";

            toTicketNumber = new NumericUpDown();
            toTicketNumber.Text = "Hasta";

            fromTicketNumber.Top = toTicketNumber.Top = 75;

            fromTicketNumber.Left = 30;
            toTicketNumber.Left = fromTicketNumber.Right + 50;

            fromTicketNumber.Maximum = toTicketNumber.Maximum = 10000000;

            fromTicketNumber.Enter += (s, ev) => fromTicketNumber.Controls[1].Text = "";
            toTicketNumber.Enter += (s, ev) => toTicketNumber.Controls[1].Text = "";

            fromTicketNumber.TabIndex = 0;
            toTicketNumber.TabIndex = 0;

            fromTicketNumber.Font = toTicketNumber.Font = new Font("Arial", 14);

            this.Controls.Add(fromTicketNumber);
            this.Controls.Add(toTicketNumber);

            var i = 0;
            foreach (var item in items)
            {
                CreateField(item, i++);
            }

            var bandejeros = db.QueryAllBandejeros();

            bandejeroCombo.Items.Add("Seleccione Bandejero");
            bandejeroCombo.SelectedIndex = 0;
            foreach (var bandejero in bandejeros)
            {
                bandejeroCombo.Items.Add(bandejero.Name);
            }

            footer = new TextBox();
            footer.Multiline = true;
            footer.Font = new Font("Arial", 14);
            footer.Height = fromTicketNumber.Height * 3;
            footer.Width = 330;
            footer.Left = 30;
            footer.Top = i * (fromTicketNumber.Height + 10) + Top;
            footer.TabIndex = i + 1;

            this.Height += footer.Height;

            this.Controls.Add(footer);
        }

        TextBox footer;

        NumericUpDown fromTicketNumber;
        NumericUpDown toTicketNumber;

        int Top = 130;

        float total;

        List<NumericUpDown> fields = new List<NumericUpDown>();

        Dictionary<Item, int> itemsDictionary = new Dictionary<Item, int>();

        private void CreateField(Item item, int place)
        {
            var qty = new NumericUpDown();
            var productLabel = new Label();
            var totalLabel = new Label();
            totalLabel.Text = "0";

            fields.Add(qty);

            totalLabel.Font = productLabel.Font = qty.Font = new Font("Arial", 14);

            productLabel.Width = 200;

            qty.TabIndex = place + 1;

            qty.Minimum = 0;
            qty.Maximum = 1;

            totalLabel.Top = productLabel.Top = qty.Top = Top + place * (totalLabel.Height + 10);
            qty.Left = 30;
            productLabel.Left = qty.Left + 10 + qty.Width;
            totalLabel.Left = productLabel.Left + productLabel.Width + 10;

            this.Height += totalLabel.Height + 10;

            qty.Enter += (s, e) => qty.Controls[1].Text = "";

            productLabel.Text = String.Format(@"{0}..........{1}", item.Name, item.Price);
            qty.ValueChanged += (s, e) =>
            {
                itemsDictionary.Remove(item);
                if (qty.Value != 0)
                    itemsDictionary.Add(item, (int)qty.Value);
                var value = item.Price * (float)qty.Value;
                totalLabel.Text = (value).ToString();
                total = CalcTotal();
                textBox1.Text = total.ToString();
            };

            this.Controls.Add(qty);
            this.Controls.Add(productLabel);
            this.Controls.Add(totalLabel);
        }


        private float CalcTotal()
        {
            return itemsDictionary.Aggregate(0f, (sum, kv) => sum += kv.Key.Price * kv.Value);
        }

        private void ImprimirMasivo(Orden orden, int from, int to, string footer = null)
        {
            for (var i = from; i <= to; i++)
                ImprimirItem(orden, i, footer);
        }

        private void ImprimirItem(Orden orden, int id, string footer)
        {
            var toPrint = new List<PrintItem>();

            var bandejero = bandejeroCombo.Items[bandejeroCombo.SelectedIndex];

            //toPrint.Add(new PrintItem(String.Format("MicroEstadio {0}", Environment.NewLine), new Font("Arial", 24)));
            toPrint.Add(new PrintItem(String.Format("{0}Ticket Nro.: {1}{0}", Environment.NewLine, id), new Font("Arial", 10)));
            //toPrint.Add(new PrintItem(String.Format("{1}{0}", Environment.NewLine, bandejero.ToString()), new Font("Arial", 20)));
            toPrint.Add(new PrintItem(String.Format("{0}", Environment.NewLine), new Font("Arial", 8)));

            foreach (var kv in itemsDictionary)
            {
                toPrint.Add(
                    new PrintItem(
                        String.Format("{2} ${3}{0}", Environment.NewLine, kv.Value, kv.Key.Name, kv.Value * kv.Key.Price),
                        new Font("Arial", 22)));
                toPrint.Add(
                    new PrintItem(
                        String.Format("{0}", Environment.NewLine, kv.Value, kv.Key.Name, kv.Value * kv.Key.Price),
                        new Font("Arial", 8)));
            }
            //if (primeraCarga.Checked)
            //    toPrint.Add(new PrintItem(String.Format("Cambio ${1}{0}", Environment.NewLine, cambio.Value), new Font("Arial", 22)));
            //toPrint.Add(new PrintItem(String.Format("{0}", Environment.NewLine), new Font("Arial", 14)));
            //toPrint.Add(new PrintItem(String.Format("Total  ${1}{0}", Environment.NewLine, orden.Total), new Font("Arial", 34)));
            if (!string.IsNullOrEmpty(footer))
                toPrint.Add(new PrintItem(String.Format(footer + "{0}", Environment.NewLine), new Font("Arial", 12)));

            toPrint.Add(new PrintItem(String.Format("{0}@Megamingus{0}", Environment.NewLine), new Font("Calibri", 12)));

            BasePrinter.Print(toPrint);

            //var bandejero = bandejeroCombo.Items[bandejeroCombo.SelectedIndex];

            //var ticket = String.Format("TICKET {2}{0}{1}{0}{0}", Environment.NewLine, bandejero,ticketId);

            //var lista = itemsDictionary.Aggregate("", (add, kv) => add += String.Format("{1} x {2}  {0} .......................${4}{0}", Environment.NewLine, kv.Value, kv.Key.Name, kv.Key.Price, kv.Value * kv.Key.Price));
            //ticket += lista;
            //if (primeraCarga.Checked)
            //    ticket += String.Format("Cambio{1} .......................${0}{1}", cambio.Value,Environment.NewLine);
            //ticket += String.Format("{0}TOTAL............${1}{0}{0}@Megamingus{0}.", Environment.NewLine, CalcTotal());
            //BasePrinter.Print(ticket);
        }

        private async Task<Orden> GuardarOrden(Bandejero b)
        {

            await Task.Delay(1000);

            var order = db.AddOrden(new Orden(b, CalcTotal(), false, 0f));

            foreach (var kv in itemsDictionary)
                db.Insert(new ItemQty(kv.Key, order, kv.Value));

            return order;
        }


        private async void OnPrintOrder(object sender, EventArgs e)
        {
            var bandejero = bandejeroCombo.Items[bandejeroCombo.SelectedIndex].ToString();

            var b = db.SelectBandejero(bandejero);
            if (b == null)
            {
                MessageBox.Show("Debe seleccionar un bandejero", "Error", MessageBoxButtons.OK);
                return;
            }

            var id = await GuardarOrden(b);
            ImprimirMasivo(id, (int)fromTicketNumber.Value, (int)toTicketNumber.Value, footer.Text);
            Reset();
        }

        private void Reset()
        {
            foreach (var f in fields)
            {
                f.Value = 0;
            }
            bandejeroCombo.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
