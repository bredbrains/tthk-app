using System.Collections.Generic;
using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace tthk_app.ParsingService
{
    public class Parser
    {
        public static List<string> GetChanges()
        {
            List<string> changesTableRows = new List<string>();
            WebClient client = new WebClient();
            string doc = client.DownloadString("https://www.tthk.ee/tunniplaani-muudatused/");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(doc);
            foreach (IElement element in document.QuerySelectorAll("tr"))
            {
                changesTableRows.Add(element.InnerHtml);
            }

            return changesTableRows;
        }
    }
}