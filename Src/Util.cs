using System.Windows.Forms;

namespace nlsetup
{
    internal class Util
    {
        static private KeysConverter keysConverter = new KeysConverter();

        static public Keys GetKey(string key)
        {
            return (Keys)keysConverter.ConvertFromString(key);
        }

        static public KeyModifiers GetModifier(string modifier)
        {
            switch (modifier)
            {
                case "NoRepeat":
                    return KeyModifiers.NoRepeat;
                case "Control":
                    return KeyModifiers.Control;
                case "Windows":
                    return KeyModifiers.Windows;
                case "Alt":
                    return KeyModifiers.Alt;
                case "Shift":
                    return KeyModifiers.Shift;
                default:
                    return KeyModifiers.Control;
            }
        }
    }
}
