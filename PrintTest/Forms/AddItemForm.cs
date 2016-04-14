using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Printing;
using CommonPrinter.Utils;

namespace CommonPrinter
{
    public partial class
        AddItemForm : Form
    {
        public AddItemForm()
        {
            InitializeComponent();
            Init();
        }

        public AddItemForm(Database database)
        {
            InitializeComponent();
            this.database = database;
            Init();
        }

        // private Database db;
        private Database database;

        private BasePrinter printer;

        private void Init()
        {
            // db = new Database("Database"); 
            Inform();
            printer = new BasePrinter(database);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            printer.PrintItems();
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            database.AddItem(new Model.Item(TextBox1.Text, (float)numericUpDown1.Value));
            TextBox1.Text = "";
            numericUpDown1.Value = 0;
            Inform();
        }

        private void Inform()
        {
            var items = database.QueryAllItems();
            string results = "";
            foreach (var item in items)
            {
                results = String.Format(@"{0} ---$ {1}{3}{2}",
                    item.Name,
                    item.Price,
                    results,
                    Environment.NewLine);
            }
            messageBox.Text = results;
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
           numericUpDown1.Controls[1].Text = "";
        }
    }
}
