using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CommonPrinter.Model
{
    public class Cajera
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public float InitialCash { get; set; }

        public Cajera() { }

        public Cajera(string Name,float InitialCash)
        {
            this.Name = Name;
            this.InitialCash = InitialCash;
        }

    }
}
