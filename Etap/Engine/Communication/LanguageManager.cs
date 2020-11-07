using Etap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Communication
{
    public class LanguageManager
    {
        private Dictionary<string, string> _valuesServer = new Dictionary<string, string>();

        public LanguageManager()
        {
            _valuesServer = new Dictionary<string, string>();
        }

        public void Init(string file = "")
        {
            if (file == string.Empty)
                file = Environment.CurrentDirectory + @"\content\locale.txt";

            if (this._valuesServer.Count > 0)
                this._valuesServer.Clear();

            string[] lines = System.IO.File.ReadAllLines(file);
            foreach (string line in lines)
            {
                string[] splitted = line.Split('=');
                _valuesServer.Add(splitted[0], splitted[1]);
            }

            int amount = this._valuesServer.Count;
            Logger.Info("Loaded " + amount + " language locales.");
        }

        public string TryGetValue(string value)
        {
            if (this._valuesServer.ContainsKey(value))
            {
                return this._valuesServer[value];
            }
            else
            {
                return "No language locale found for [" + value + "]";
            }
        }
    }
}
