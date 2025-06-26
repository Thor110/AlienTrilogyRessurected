using System.Diagnostics;

namespace ALTViewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Process[] processes = Process.GetProcessesByName("ALTViewer");
            if (processes.Length > 1) { return; }
            if (!File.Exists("Run.exe")) { MessageBox.Show("This toolkit is only designed to work with the repacked version of the game for now!"); return; }
            ApplicationConfiguration.Initialize();
            Application.Run(new ALTViewer());
        }
    }
}