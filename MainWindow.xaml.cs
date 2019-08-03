using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using ServiceStack;
using ServiceStack.Text;

namespace StockGrabber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StockSearch.Text == "" || StockSearch.Text == "Enter stock ticker")
            {
                StockSearch.Text = "Invalid stock ticker";
                return;
            }

            //Create API query call. Get response and display.

            // retrieve monthly prices for Microsoft
            var symbol = "MSFT";
            var monthlyPrices = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={symbol}&apikey=demo&datatype=csv"
                .GetStringFromUrl().FromCsv<List<APIGateWay>>();

            monthlyPrices.PrintDump();

            // some simple stats
            var maxPrice = monthlyPrices.Max(u => u.Close);
            var minPrice = monthlyPrices.Min(u => u.Close);

            
            results.Text = string.Join(Environment.NewLine, maxPrice, minPrice);
        }

        protected string FormatXml(XmlNode xmlNode)
        {
            StringBuilder bob = new StringBuilder();


            // We will use stringWriter to push the formated xml into our StringBuilder bob.
            using (StringWriter stringWriter = new StringWriter(bob))
            {
                // We will use the Formatting of our xmlTextWriter to provide our indentation.
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                    xmlNode.WriteTo(xmlTextWriter);
                }
            }


            return bob.ToString();
        }
    }
}
