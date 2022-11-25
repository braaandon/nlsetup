using Newtonsoft.Json;
using System;
using System.IO;

namespace nlsetup
{
    internal class NLConfig
    {
        public static NLConfig Load()
        {
            try
            {
                var contents = File.ReadAllText("config.json");
                return JsonConvert.DeserializeObject<NLConfig>(contents);
            }
            catch (Exception)
            {

                var config = new NLConfig();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
                return config;
            }
        }

        public string HotkeyModifier = "Control";
        public string[] Hotkeys = {"F1", "F2", "F3", "F4", "F5"};
        public string AppPath = @"C:\Program Files (x86)\Steam\steamapps\common\Destiny 2\destiny2.exe";
    }
}
