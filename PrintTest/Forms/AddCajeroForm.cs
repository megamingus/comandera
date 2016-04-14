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
    public partial class AddCajeroForm : Form
    {
        private Database database;

        public AddCajeroForm()
        {
            InitializeComponent();
        }

        public AddCajeroForm(Database database)
        {
            InitializeComponent();
            this.database = database;
            Inform();
            numericUpDown1.Enter += (s, e) => numericUpDown1.Controls[1].Text = "";
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            database.AddCajera(new Model.Cajera(TextBox1.Text, (float)numericUpDown1.Value));
            TextBox1.Text = "";
            numericUpDown1.Value = 0;
            Inform();
           
        }

        private void Inform()
        {
            var items = database.QueryAllCajeras();
            string results = "";
            foreach (var item in items)
            {
                results = String.Format(@"{0} ---$ {1}{3}{2}",
                    item.Name,
                    item.InitialCash,
                    results,
                    Environment.NewLine);
            }
            messageBox.Text = results;
        }
    }
}
