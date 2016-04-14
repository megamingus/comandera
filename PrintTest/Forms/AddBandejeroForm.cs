using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonPrinter.Utils;

namespace CommonPrinter
{
    public partial class AddBandejeroForm : Form
    {
        private Database database;

        public AddBandejeroForm()
        {
            InitializeComponent();
        }

        public AddBandejeroForm(Database database)
        {
            InitializeComponent();
            this.database = database;
            Inform();
        }

        private void messageBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void addItem_Click(object sender, EventArgs e)
        {
            database.AddBandejero(new Model.Bandejero(TextBox1.Text));
            TextBox1.Text = "";
            Inform();
        }

        private void Inform()
        {
            var items = database.QueryAllBandejeros();
            string results = "";
            foreach (var item in items)
            {
                results = String.Format(@"{0}  {2}{1}",
                    item.Name,
                    results,
                    Environment.NewLine);
            }
            messageBox.Text = results;
        }
    

        private void TextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
