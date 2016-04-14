using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CommonPrinter.Model
{
    public class ItemQty
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int OrdenId { get; set; }

        public int qty { get; set; }

        public ItemQty() { }

        public ItemQty(Item item,Orden orden,int qty)
        {
            this.ItemId = item.Id;
            this.OrdenId = orden.Id;
            this.qty = qty;
        }

        private Orden _orden;

        [Ignore]
        public Orden Orden
        {
            get { return _orden; }
            set
            {
                if (value.Id == OrdenId)
                    _orden = value;
            }
        }

        private Item _item;

        [Ignore]
        public Item Item
        {
            get { return _item; }
            set
            {
                if (value.Id == ItemId)
                    _item = value;
            }
        }
    }
}
