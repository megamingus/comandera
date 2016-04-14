using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CommonPrinter.Model
{
    public class Orden
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int Cajeraid { get; set; }

        [Indexed]
        public int BandejeroId { get; set; }

        public string Time { get; set; }

        public float Total { get; set; }

        public float InitialChange { get; set; }

        public bool PrimerCarga { get; set; }


        public Orden() { }

        public Orden(Bandejero b, float total, bool primeraCarga, float initialChange)
        {
            this.Cajeraid = 1; //solo 1 cajera
            this.BandejeroId = b.Id;
            this.InitialChange = initialChange;
            this.PrimerCarga = primeraCarga;
            this.Total = total;

            this.Time = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            this.Qtys = new List<ItemQty>();
        }



        [Ignore]
        public List<ItemQty> Qtys { get; private set; }

        private Bandejero _Bandejero;

        [Ignore]
        public Bandejero Bandejero
        {
            get { return _Bandejero; }
            set
            {
                if (value.Id == BandejeroId)
                    _Bandejero = value;
            }
        }

        private Cajera _cajera;

        [Ignore]
        public Cajera cajera
        {
            get { return _cajera; }
            set
            {
                if (value.Id == Cajeraid)
                    _cajera = value;
            }
        }

        public void addItemQty(ItemQty itq)
        {
            if (itq.OrdenId == this.Id)
                Qtys.Add(itq);
        }

    }
}
