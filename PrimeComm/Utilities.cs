using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PrimeComm.Properties;

namespace PrimeComm
{
    static internal class Utilities
    {
        public static void InvokeIfRequired(this Control c, MethodInvoker action)
        {
            if (c.InvokeRequired) c.Invoke(action); else action();
        }

        public static string CreateTemporalFileFromText(string text=null)
        {
            var t = Path.GetTempFileName();
            File.Delete(t);
            Directory.CreateDirectory(t);

            // Get name
            try
            {
                if (!String.IsNullOrEmpty(text))
                    return SaveText(text, t);

                if (Clipboard.ContainsText())
                    return SaveText(Clipboard.GetText(TextDataFormat.UnicodeText).Trim(), t);
                
                if (Clipboard.ContainsImage())
                {
                    t = Path.Combine(t, PrimeLib.Utilities.GetRandomImageName() + ".png");
                    Clipboard.GetImage().Save(t);

                    return t;
                }
            }
            catch
            {
            }
            return null;
        }

        private static string SaveText(string text, string t)
        {
            if (!String.IsNullOrEmpty(text))
            {
                var m = new Regex(Settings.Default.RegexProgramName, RegexOptions.IgnoreCase).Match(text);
                var name = PrimeLib.Utilities.GetRandomProgramName();

                if (m.Success)
                    name = m.Groups["name"].Value;

                t = Path.Combine(t, name + ".txt");
                File.WriteAllText(t, text, Encoding.BigEndianUnicode);
                return t;
            }
            else
                throw new Exception("Nothing to save");
        }
    }
}