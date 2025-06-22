namespace ALTViewer
{
    public partial class TextEditor : Form
    {
        public bool setup;
        public List<string> languages = new List<string> { "English", "Français", "Italiano", "Español" };
        public List<string> missions = new List<string>
        {
            "1.1.1 Entrance", "1.1.2 Outer Complex", "1.1.3 Ammunition Dump 1", "1.2.2 Recreation Rooms", "1.3.1 Medical Laboratory",
            "1.1.4 Ammunition Dump 2", "1.4.1 Garage", "1.1.5 Ammunition Dump 3", "1.5.4 Atmosphere Sub-level", "1.5.5 Security Catwalks",
            "1.6.1 Atmosphere Sub-Basement", "1.6.2 Queen's Lair", "2.1.1 Living Area", "2.1.2 Canteen", "2.1.3 Meeting Tower",
            "2.2.2 Leadworks", "2.4.2 Tunnels and Ducts", "2.3.1 Mining and Smelting", "2.3.2 Furnace Controls", "2.4.3 Tunnels and Ducts",
            "2.6.2 Lead Mould", "2.6.3 Queen's Lair", "3.1.1 Tunnels", "3.2.1 Pilot's Chamber", "3.3.1 Canyons and Catacombs",
            "3.2.2 Pilot's Chamber", "3.5.1 Secrets", "3.5.2 Inorganics 1", "3.2.3 Pilot's Chamber", "3.7.1 Droplifts",
            "3.5.3 Inorganics 2", "3.2.4 Pilot's Chamber", "3.8.1 Egg Chambers", "3.2.5 Pilot's Chamber", "3.9.1 Queen's lair"
        };
        public TextEditor()
        {
            InitializeComponent();
            foreach (string language in languages) { comboBox1.Items.Add(language); }
            foreach (string mission in missions) { listBox1.Items.Add(mission); }
            comboBox1.SelectedIndex = 0; // Default to English
            setup = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = languages[comboBox1.SelectedIndex];
            if (!setup) { return; }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = missions[listBox1.SelectedIndex];
        }
    }
}
