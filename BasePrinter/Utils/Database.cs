using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
using CommonPrinter.Model;
using System.Globalization;

namespace CommonPrinter.Utils
{
    public class Database : SQLiteConnection
    {
        
        public Database(string path) : base(path)
        {
            CreateTable<Item>();
            CreateTable<Bandejero>();
            CreateTable<Orden>();
            CreateTable<ItemQty>();
            CreateTable<Cajera>();
        }

        public int AddItem(Item item)
        {
           return Insert(item);
        }

        public IEnumerable<Item> QueryAllItems()
        {
            return (from i in Table<Item>() select i) ;//Table<Item>().Where(x => 1 == 1) ;
        }

        internal int AddCajera(Cajera cajera)
        {
            return Insert(cajera);
        }

        public IEnumerable<Cajera> QueryAllCajeras()
        {
            return (from i in Table<Cajera>() select i);//Table<Item>().Where(x => 1 == 1) ;
        }


        internal int AddBandejero(Bandejero bandejero)
        {
            return Insert(bandejero);
        }


        public IEnumerable<Bandejero> QueryAllBandejeros()
        {
            return (from i in Table<Bandejero>() select i);//Table<Item>().Where(x => 1 == 1) ;
        }

        internal Bandejero SelectBandejero(string name)
        {
            return (Table<Bandejero>().Where(b=> b.Name==name)).FirstOrDefault();
        }

        public Orden AddOrden(Orden orden)
        {
            Insert(orden);

            return (Table<Orden>().Where(o => o.Time == orden.Time)).FirstOrDefault();
        }

        public IEnumerable<ItemQty> QueryAllItemQty()
        {
            return (from i in Table<ItemQty>() select i);//Table<Item>().Where(x => 1 == 1) ;
        }


        public IEnumerable<Orden> QueryAllOrders()
        {
            return (from i in Table<Orden>() select i);//Table<Item>().Where(x => 1 == 1) ;
        }

        /*
public IEnumerable<Valuation> QueryValuations(Stock stock)
{
return Table<Valuation>().Where(x => x.StockId == stock.Id);
}
public Valuation QueryLatestValuation(Stock stock)
{
return Table<Valuation>().Where(x => x.StockId == stock.Id).OrderByDescending(x => x.Time).Take(1).FirstOrDefault();
}
public Stock QueryStock(string stockSymbol)
{
return (from s in Table<Stock>()
 where s.Symbol == stockSymbol
 select s).FirstOrDefault();
}
public IEnumerable<Stock> QueryAllStocks()
{
return from s in Table<Stock>()
orderby s.Symbol
select s;
}

public void UpdateStock(string stockSymbol)
{
//
// Ensure that there is a valid Stock in the DB
//
var stock = QueryStock(stockSymbol);
if (stock == null)
{
stock = new Stock { Symbol = stockSymbol };
Insert(stock);
}

//
// When was it last valued?
//
var latest = QueryLatestValuation(stock);
var latestDate = latest != null ? latest.Time : new DateTime(1950, 1, 1);

//
// Get the latest valuations
//
try
{
var newVals = new YahooScraper().GetValuations(stock, latestDate + TimeSpan.FromHours(23), DateTime.Now);
InsertAll(newVals);
}
catch (System.Net.WebException ex)
{
Console.WriteLine(ex);
}
}
*/
    }
}
