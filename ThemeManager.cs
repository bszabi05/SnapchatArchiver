using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapchatArchiver
{
    public static class ThemeManager
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static void ApplyDarkMode(IntPtr handle)
        {
            try
            {
                int darkMode = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ThemeManager hiba: " + ex.Message);
            }
        }
    }
}
