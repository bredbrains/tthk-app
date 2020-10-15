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
            List<List<string>> changeRows = new List<List<string>>();
            var rows = doc.DocumentNode.SelectNodes("//table").Descendants("tr");
            foreach (var tr in rows)
            {
                List<string> changeRow = new List<string>();
                foreach (var td in tr.ChildNodes)
                {
                    _cellText = td.InnerText.Trim();
                    if (!stopList.Contains(_cellText))
                    {
                        changeRow.Add(_cellText);
                    }
                }

                if (changeRow.Count != 0)
                {
                    changeRows.Add(changeRow);
                }
            }

            return changeRows;
        }
    }
}