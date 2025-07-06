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
            string gameDirectory = Utilities.CheckDirectory();
            if (gameDirectory == "")
            {
                MessageBox.Show("Game directory not found. Please ensure you are running this application from the correct game directory.");
                return;
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new ALTViewer());
        }
    }
}