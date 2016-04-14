using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Globalization;

namespace CommonPrinter.Model
{
    public class Item
    {
        public Item() { }

        public Item(string name,float price)
        {
            this.Name = name;
            this.Price = price;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
