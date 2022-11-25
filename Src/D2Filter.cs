using NetLimiter.Service;
using System;
using System.Windows.Forms;

namespace nlsetup
{
    internal class D2Filter
    {
        public string Name;
        public Keys Hotkey;
        public KeyModifiers Modifiers;

        private uint _bps;
        private ushort _portStart;
        private ushort _portEnd;

        private NLClient _client;

        private Filter _filt;
        private Rule _rule;

        private Filter _filtModel;
        private Rule _ruleModel;

        public D2Filter(NLClient client, string appPath, string name, Keys hotkey, KeyModifiers modifiers, uint bps, ushort portStart = 0, ushort portEnd = 0)
        {
            Name = name;
            Hotkey = hotkey;
            Modifiers = modifiers;
            _bps = bps;
            _portStart = portStart;
            _portEnd = portEnd;

            _client = client;

            _filtModel = new Filter(Name);
            _filtModel.Functions.Add(new FFAppIdEqual(new AppId(appPath)));
            if (_portStart > 0 && _portEnd > 0) _filtModel.Functions.Add(new FFRemotePortInRange(new PortRangeFilterValue(_portStart, _portEnd)));

            _ruleModel = new LimitRule(RuleDir.In, _bps);

            _filt = _client.Filters.Find(x => x.Name == Name);

            if (!Exists())
            {
                _filt = _client.AddFilter(_filtModel);
                _rule = _client.AddRule(_filtModel.Id, _ruleModel);
            } else
            {
                _rule = _client.Rules.Find(x => x.FilterId == _filt.Id);
            }

            _rule.IsEnabled = false;
            _client.UpdateRule(_rule);

            if (hotkey != Keys.None)
            {
                HotKeyManager.RegisterHotKey(Hotkey, Modifiers);
                HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>((object sender, HotKeyEventArgs e) =>
                {
                    if (e.Key == Hotkey && e.Modifiers == Modifiers)
                    {
                        Toggle();
                    }
                });
            } 
        }

        public bool Exists()
        {
            return _filt != null;
        }

        public bool IsEnabled()
        {
            return _rule.IsEnabled;
        }

        public void Toggle()
        {
            _rule.IsEnabled = !_rule.IsEnabled;
            _client.UpdateRule(_rule);
        }
    }
}
