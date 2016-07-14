using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace CommonPrinter.Utils
{
    class BasePrinter
    {
        private Database db;

        public BasePrinter(Database db)
        {
            this.db = db;
        }


        public static void Print(List<PrintItem> toPrint)
        {
            PrintDocument p = new PrintDocument();
            p.DefaultPageSettings.PaperSize = new PaperSize("Commandera", 284, 12898);
            // 12898  = 157 * 82
            p.DefaultPageSettings.Landscape = false;

            p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                var size = 0f;
                Image img = Image.FromFile(".\\imagen.png");
                var height =(int) ((284f / img.Width) * img.Height);
                e1.Graphics.DrawImage(img, new Rectangle(0, 0, 284 , height));

                size += height+10;

                
                foreach (var pi in toPrint)
                {
                    e1.Graphics.DrawString(pi.Text, pi.Font, new SolidBrush(Color.Black), new RectangleF(0, size, 284, p.DefaultPageSettings.PrintableArea.Height));

                    size += e1.Graphics.MeasureString(pi.Text, pi.Font, 284).Height;
                }
                //Image img = Image.FromFile(".\\nxYbeGG.png");
                //e1.Graphics.DrawImage(img, new Rectangle(150, 0, 150, 150));

            };
            try
            {
                p.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        public static void Print(string toPrint)
        {
            PrintDocument p = new PrintDocument();
            p.DefaultPageSettings.PaperSize = new PaperSize("Commandera", 284, 12898);
            // 12898  = 157 * 82
            p.DefaultPageSettings.Landscape = false;

            p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                //var size = e1.Graphics.MeasureString("toPixel", new Font("Arial", 8));
                //for (int i = 200; i < 300; i++)
                //{
                //    e1.Graphics.DrawString("toPixel " + i, new Font("Arial", 8), new SolidBrush(Color.Black), new RectangleF(0, size.Height * i, p.DefaultPageSettings.PrintableArea.Width, size.Height * i + size.Height));
                //    e1.Graphics.DrawLine(SystemPens.ActiveCaptionText, new PointF(0, size.Height * i), new PointF(i, size.Height * i));
                //}
                Image img = Image.FromFile(".\\nxYbeGG.png");
                e1.Graphics.DrawImage(img, new Rectangle(150, 0, 150, 150));

                e1.Graphics.DrawString(toPrint, new Font("Arial", 20), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                var size = e1.Graphics.MeasureString(toPrint, new Font("Arial", 20));
                //    var sett = "";
                //foreach (var popo in p.PrinterSettings.PaperSizes) {
                //    sett += string.Format("{1}{0}",Environment.NewLine,popo.ToString());
                //}
                //e1.Graphics.DrawString(sett, new Font("Arial", 14), new SolidBrush(Color.Black), new RectangleF(0, size.Height, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

            };
            try
            {
                p.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }


        public void PrintItems()
        {
            string s = @"";
            var items = db.QueryAllItems();
            foreach (var it in items)
            {
                s += String.Format(@"{0} ... .... ...${1}{2}",
                    it.Name,
                    it.Price,
                    Environment.NewLine);
            }

            Print(s);
        }

    }

    public class PrintItem
    {
        public string Text { get; set; }

        public Font Font { get; set; }

        public PrintItem(string text, Font font)
        {
            this.Text = text;
            this.Font = font;
        }
    }
}
