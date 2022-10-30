using System;
using NetLimiter.Service;

namespace nlsetup
{
    internal class Program
    {
        static string destinyPath = @"C:\program files (x86)\steam\steamapps\common\destiny 2\destiny2.exe";

        static Filter CreateD2Filter(string name, ushort start = 0, ushort end = 0)
        {
            var filt = new Filter(name);
            filt.Functions.Add(new FFAppIdEqual(new AppId(destinyPath)));
            if (start > 0 && end > 0) filt.Functions.Add(new FFRemotePortInRange(new PortRangeFilterValue(start, end)));
            return filt;
        }

        static void Main(string[] args)
        {
            var client = new NLClient();

            try
            {
                client.Connect();
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Attempting to run through NL Protect");
            client.UACElevate();

            client.SetRegistrationData("", "");
            Console.WriteLine("Registered License Information");

            Filter[] filters = {
                CreateD2Filter("Destiny 2"),
                CreateD2Filter("30k", 30000, 30009), 
                CreateD2Filter("27k", 27015, 27200),
                CreateD2Filter("3074", 3074, 3074),
                CreateD2Filter("7500", 7500, 7509)
            };

            foreach(Filter filter in filters)
            {
                uint bps = 1;

                if (filter.Name == "Destiny 2")
                    bps = 800;

                client.AddFilter(filter);
                client.AddRule(filter.Id, new LimitRule(RuleDir.In, bps));
                client.AddRule(filter.Id, new LimitRule(RuleDir.Out, bps));

                Console.WriteLine($"Created Filter \"{filter.Name}\"");
            }

            foreach(Rule rule in client.Rules)
            {
                rule.IsEnabled = false;
                client.UpdateRule(rule);
            }
        }
    }
}
