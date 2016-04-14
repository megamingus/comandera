using CommonPrinter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonPrinter
{
    public partial class Master : Form
    {
        public Master()
        {
            InitializeComponent();
            this.Init();
        }

        private Database database;

        private void Init()
        {
            database = new Database("Database");

            (new AddItemForm(database)).Show();
            (new AddBandejeroForm(database)).Show();
            (new AddCajeroForm(database)).Show();

            var bandejeros = database.QueryAllBandejeros();

            bandejeroCombo.Items.Add("Seleccione Bandejero");
            bandejeroCombo.SelectedIndex = 0;
            foreach (var bandejero in bandejeros)
            {
                bandejeroCombo.Items.Add(bandejero.Name);
            }

            //this.Hide();
            //this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // var bandejeros = database.QueryAllBandejeros();

            //foreach ( var band in bandejeros)
            //{
            //    var reporte = new ReporteBandejero(database, band);
            //    BasePrinter.Print(reporte.GenerarPagina());
            //}

            var bandejero = bandejeroCombo.Items[bandejeroCombo.SelectedIndex].ToString();

            var b = database.SelectBandejero(bandejero);

            if (b == null)
            {
                MessageBox.Show("Debe seleccionar un bandejero", "Error", MessageBoxButtons.OK);
                return;
            }
            
            var reporte = new ReporteBandejero(database, b);
            BasePrinter.Print(reporte.GenerarPagina());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var reporteCaja = new ReporteCaja(database);

            BasePrinter.Print(reporteCaja.GenerarPagina());

        }
    }
}
