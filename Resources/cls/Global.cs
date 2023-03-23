using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ManageMaterialPBA
{
    class Global
    {
        public static string dbFile = @"WinRemoteDesktop.db";

        public static bool IsServerAddress(string s)
        {
            string text1 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}(:\d{1,5})?$"; //(:?)\d{1,5}$
            return Regex.IsMatch(s, text1);
        }

        public static void WinMessage(string msg, string caption = "")
        {
            MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
