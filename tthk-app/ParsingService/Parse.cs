using Microsoft.Scripting.Hosting;

namespace tthk_app.ParsingService
{
    public class Parse
    {
        public static dynamic[] GetChanges()
        {
            ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile("parser.py", scope);
            dynamic function = scope.GetVariable("changes");
            string url = "http://www.tthk.ee/tunniplaani-muudatused/";
            var result = function(url);
            return result;
        }
    }
}