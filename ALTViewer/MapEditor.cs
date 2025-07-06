namespace ALTViewer
{
    public partial class MapEditor : Form
    {
        public string gameDirectory = "";
        public string levelPath1 = "";
        public string levelPath2 = "";
        public string levelPath3 = "";
        public string levelPath4 = "";
        public string levelPath5 = "";
        public string levelPath6 = "";
        public string levelPath7 = "";
        public string[] levels = null!;
        public List<string> missions = new List<string>
        {
            "1.1.1 Entrance", "1.1.2 Outer Complex", "1.1.3 Ammunition Dump 1", "1.2.2 Recreation Rooms", "1.3.1 Medical Laboratory",
            "1.1.4 Ammunition Dump 2", "1.4.1 Garage", "1.1.5 Ammunition Dump 3", "1.5.4 Atmosphere Sub-level", "1.5.5 Security Catwalks",
            "1.6.1 Atmosphere Sub-Basement", "1.6.2 Queen's Lair", "2.1.1 Living Area", "2.1.2 Canteen", "2.1.3 Meeting Tower",
            "2.2.2 Leadworks", "2.4.2 Tunnels and Ducts", "2.3.1 Mining and Smelting", "2.3.2 Furnace Controls", "2.4.3 Tunnels and Ducts",
            "2.6.2 Lead Mould", "2.6.3 Queen's Lair", "3.1.1 Tunnels", "3.2.1 Pilot's Chamber", "3.3.1 Canyons and Catacombs",
            "3.2.2 Pilot's Chamber", "3.5.1 Secrets", "3.5.2 Inorganics 1", "3.2.3 Pilot's Chamber", "Secret Level",
            "3.7.1 Droplifts", "3.5.3 Inorganics 2", "3.2.4 Pilot's Chamber", "3.8.1 Egg Chambers", "3.2.5 Pilot's Chamber",
            "3.9.1 Queen's lair",
            "Multiplayer Level" + " 1", "Multiplayer Level" + " 2", "Multiplayer Level" + " 3", "Multiplayer Level" + " 4", "Multiplayer Level" + " 5",
            "Multiplayer Level" + " 6", "Multiplayer Level" + " 7", "Multiplayer Level" + " 8", "Multiplayer Level" + " 9", "Multiplayer Level" + " 10"
        };
        private string lastSelectedLevel = "";
        FullScreen fullScreen;
        public MapEditor()
        {
            InitializeComponent();
            gameDirectory = Utilities.CheckDirectory();
            levelPath1 = gameDirectory + "SECT11";
            levelPath2 = gameDirectory + "SECT12";
            levelPath3 = gameDirectory + "SECT21";
            levelPath4 = gameDirectory + "SECT22";
            levelPath5 = gameDirectory + "SECT31";
            levelPath6 = gameDirectory + "SECT32";
            levelPath7 = gameDirectory + "SECT90";
            levels = new string[] { levelPath1, levelPath2, levelPath3, levelPath4, levelPath5, levelPath6, levelPath7 };
            //ToolTip tooltip = new ToolTip(); // no tooltips added yet
            //ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox) });
            ListLevels();
            fullScreen = new FullScreen(this);
        }
        public void ListLevels()
        {
            foreach (string level in levels)
            {
                string[] levelFiles = Directory.GetFiles(level, "*.MAP");
                foreach (string levelFile in levelFiles) { listBox1.Items.Add(Path.GetFileNameWithoutExtension(levelFile)); }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = listBox1.SelectedItem!.ToString()!; // get selected item
            if (selected == lastSelectedLevel) { return; } // do not reselect same file
            lastSelectedLevel = selected; // store last selected file
            if (listBox1.SelectedIndex < missions.Count) { label1.Text = "Mission Name : " + missions[listBox1.SelectedIndex]; } // display level name
            else { label1.Text = "Mission Name : " + "Unknown"; }
            foreach (string level in levels) // determine level folder based on selected item
            {
                if (File.Exists(Path.Combine(level, selected + ".MAP")))
                {
                    label2.Text = "File Name : " + selected + ".MAP";
                    break; // exit after finding the first matching level
                }
            }
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fullScreen.Toggle();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Visible = true;
            button2.Enabled = false;
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            listBox1.ClearSelected(); // reset listbox
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
            label1.Text = "Mission Name : "; // reset mission name label
            label2.Text = "File Name : "; // reset file name label
            lastSelectedLevel = ""; // reset last selected level
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Visible = false;
            button3.Enabled = false;
            button2.Enabled = true;
        }
    }
}
