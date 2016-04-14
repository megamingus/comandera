using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPrinter.Model
{
    public class Bandejero
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set;   }

        public Bandejero() { }

        public Bandejero(string Name)
        {
            this.Name = Name;
        }
    }
}
