using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetLimiter.Service;

namespace nlsetup
{
    internal class Program
    {
        static void Main()
        {
            var cfg = NLConfig.Load();
            var client = new NLClient();

            try
            {
                client.Connect();
            } catch (Exception)
            {
                // LOGGER: program exit (reason: e)
                return;
            }

            var keysConverter = new KeysConverter();

            var filters = new List<D2Filter>
            {
                new D2Filter(client, cfg.AppPath, "Destiny 2", Util.GetKey(cfg.Hotkeys[0]), Util.GetModifier(cfg.HotkeyModifier), 800),
                new D2Filter(client, cfg.AppPath, "3074", Util.GetKey(cfg.Hotkeys[1]), Util.GetModifier(cfg.HotkeyModifier), 1, 3074, 3074),
                new D2Filter(client, cfg.AppPath, "30000", Util.GetKey(cfg.Hotkeys[2]), Util.GetModifier(cfg.HotkeyModifier), 1, 30000, 30009),
                new D2Filter(client, cfg.AppPath, "27000", Util.GetKey(cfg.Hotkeys[3]), Util.GetModifier(cfg.HotkeyModifier), 1, 27015, 27200),
                new D2Filter(client, cfg.AppPath, "7500", Util.GetKey(cfg.Hotkeys[4]), Util.GetModifier(cfg.HotkeyModifier), 1, 7500, 7509)
            };

            Console.WriteLine("Activate any hotkey to start");

            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>((sender, e) =>
            {
                Console.Clear();

                foreach (var filt in filters)
                {
                    Console.WriteLine("{0} [{1}]", filt.Name, (filt.IsEnabled() ? "ON" : "OFF"));
                }
            });

            Console.ReadLine();
        }
    }
}
