using System.Collections.Generic;
using System.Net;
using AngleSharp.Html.Parser;

namespace tthk_app.ParsingService
{
    public class Parser
    {
        public static List<List<string>> GetChanges()
        {
            List<List<string>> changesTableRows = new List<List<string>>();
            List<string> changesRowCells = new List<string>();
            WebClient client = new WebClient();
            string doc = client.DownloadString("https://www.tthk.ee/tunniplaani-muudatused/");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(doc);
            foreach (var changesTableRow in document.QuerySelectorAll("tr"))
            {
                foreach (var changesTableCell in changesTableRows.)
                {
                    changesRowCells.Add(changesTableCell.InnerHtml);
                }

                changesTableRows.Add(changesRowCells);
            }

            return changesTableRows;
        }
    }
}