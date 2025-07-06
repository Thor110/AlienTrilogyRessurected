namespace ALTViewer
{
    public static class Utilities
    {
        public static bool UnsavedChanges(string reason, Action saveAction, FormClosingEventArgs? e = null)
        {
            var result = MessageBox.Show(
                $"You have unsaved changes. Do you want to save before {reason}?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel)
            {
                if (e != null) { e.Cancel = true; }
                return true; // signal cancellation
            }
            else if (result == DialogResult.Yes) { saveAction(); } // delegate call to save

            return false;
        }
        public static string CheckDirectory()
        {
            string gameDirectory = "";
            if (File.Exists("Run.exe")) { gameDirectory = "HDD\\TRILOGY\\CD\\"; }
            else if (File.Exists("TRILOGY.EXE")) { gameDirectory = "CD\\"; }
            return gameDirectory;
        }
    }
}
