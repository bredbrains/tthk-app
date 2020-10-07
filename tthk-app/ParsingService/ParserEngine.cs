using System.Collections.Generic;
using HtmlAgilityPack;

namespace tthk_app.ParsingService
{
    public class ParserEngine
    {
        private static string _cellText;

        public static List<List<string>> ParseChanges()
        {
            var url = "https://www.tthk.ee/tunniplaani-muudatused/";
            List<string> stopList = new List<string>() {"Kuupäev", "Rühm", "Tund", "Õpetaja", "Ruum", "", "\n", "\t"};
            var web = new HtmlWeb();
            var doc = web.Load(url);
            CachingService cachingService = new CachingService(doc.ToString());
            List<List<string>> changeRows = new List<List<string>>();
            var rows = doc.DocumentNode.SelectNodes("//table").Descendants("tr");
            foreach (var tr in rows)
            {
                List<string> changeList = new List<string>();
                foreach (var td in tr.ChildNodes)
                {
                    _cellText = td.InnerText.Trim();
                    if (!stopList.Contains(_cellText))
                    {
                        changeList.Add(_cellText);
                    }
                }

                if (changeList.Count != 0)
                {
                    changeRows.Add(changeList);
                }
            }

            return changeRows;
        }
    }
}