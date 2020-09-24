using System.Collections.Generic;
using HtmlAgilityPack;

namespace tthk_app.ParsingService
{
    public class ParserEngine
    {
        public static List<List<string>> ParseChanges()
        {
            var url = "https://www.tthk.ee/tunniplaani-muudatused/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            List<List<string>> changeRows = new List<List<string>>();
            var rows = doc.DocumentNode.SelectNodes("//table").Descendants("tr");
            foreach (var tr in rows)
            {
                List<string> changeList = new List<string>();
                foreach (var td in tr.ChildNodes)
                {
                    changeList.Add(td.InnerText.Trim());
                }

                changeRows.Add(changeList);
            }

            return changeRows;
        }
    }
}