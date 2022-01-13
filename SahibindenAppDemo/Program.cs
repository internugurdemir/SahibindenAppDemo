using SahibindenAppDemo.Models.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SahibindenAppDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            int num1 = 0;
            int id = 1;
            
            // Display title as the C# console SahibindenAppDemo.
            Console.WriteLine("Sahibinden.com Datas");
            Console.WriteLine("------------------------\n");

            // Ask the user "what user wants to do with data".
            Console.WriteLine("Press 1 if you want to see datas of https://www.sahibinden.com/ as list\n");

            //Get User request
            string enteredInput = Console.ReadLine();

            //Check User request if it fits with the acceptance

            while (!checkInput(enteredInput))
            {
                //Error Message
                Console.WriteLine("You have entered unidentified option \n ");
                // Ask the user again to type the first number.
                Console.WriteLine("Press 1 if you want to see datas of https://www.sahibinden.com/ as list\n");

                enteredInput = Console.ReadLine();

                //Check User request if it fits with the acceptance


            };


            num1 = Convert.ToInt32(enteredInput);

            /*
             * GET data from https://www.sahibinden.com whatever user choose.
             */


            Uri url = new Uri("https://www.sahibinden.com");
            WebClient client = new WebClient();
            string html = client.DownloadString(url);

            #region HtmlAgilityPack Explanation
            /*
           * This is an agile HTML parser that builds a read/write DOM and supports plain XPATH or XSLT
           *  (you actually don't HAVE to understand XPATH nor XSLT to use it, don't worry...).
           *  It is a .NET code library that allows you to parse "out of the web" HTML files.
           *  The parser is very tolerant with "real world" malformed HTML.
           *  The object model is very similar to what proposes System.Xml, but for HTML documents (or streams).
           */
            #endregion

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            HtmlAgilityPack.HtmlNodeCollection advertNodes = document.DocumentNode.SelectNodes("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li/a");


            List<Advert> adverts = new List<Advert>();



            if (num1 == 1)
            {
                foreach (var item in advertNodes)
                {

                    adverts.Add(
                        new Advert
                        {
                            AdvertID = id,
                            AdvertName = item.GetAttributeValue("title", ""),
                            AdvertURL = url + item.GetAttributeValue("href", "").ToString(),

                        });

                    id++;

                    
                }




                foreach (var item in adverts)
                {
                    Uri itemUrl = new Uri(item.AdvertURL);
                    WebClient itemClient = new WebClient();
                    string itemHtml = client.DownloadString(itemUrl);

                    HtmlAgilityPack.HtmlDocument newDocument = new HtmlAgilityPack.HtmlDocument();
                    newDocument.LoadHtml(itemHtml);


                    if (newDocument.DocumentNode.SelectSingleNode("//*[@id='classifiedDetail']/div/div[2]/div[2]/h3")!=null)
                    {
                        HtmlAgilityPack.HtmlNode newAdvertNodePrice = newDocument.DocumentNode.SelectSingleNode("//*[@id='classifiedDetail']/div/div[2]/div[2]/h3");

                        item.AdvertPrice = newAdvertNodePrice.InnerText.Split("TL")[0].ToString() + " TL";



                        HtmlAgilityPack.HtmlNode newAdvertNodeDescription = newDocument.DocumentNode.SelectSingleNode("//*[@id='classifiedDescription']/text()[normalize-space(.) != '']");

                        item.AdvertDetails = newAdvertNodeDescription.InnerText.Trim();

                    }
                }


                //For average price of listed adverts
                int length = adverts.Count();
                var priceList = new int[] { Convert.ToInt32(adverts.Select(a=>a.AdvertPrice))};
                double avg = Queryable.Average(priceList.AsQueryable());
                Console.WriteLine("Average = " + avg);



                // Create a file to write to.
                var path = "TextFile.txt";

                string text = "Sahibinden.com " + DateTime.Today.ToString() + " Adverts" + Environment.NewLine;
                int newId = 1;
                foreach (var item in adverts)
                {

                    text += Environment.NewLine +
                            newId + ") " +
                            item.AdvertName + " -- " +
                            item.AdvertPrice;
                    
                    newId++;
                }
                File.WriteAllText(path, text);

                Console.WriteLine("text written");

            }


        }

        //Request Acceptance criterias. Request must be 1 or 2
        private static bool checkInput(string enteredInput)
        {

            if (Convert.ToInt32(enteredInput) == 1)
                return true;
            return false;
        }
    }
}
