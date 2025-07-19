using System.Diagnostics;
using System.Numerics;

namespace ALTViewer
{
    public partial class MapViewer : Form
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
        private string outputPath = ""; // output path for exported files
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
        private List<BndSection> currentSections = null!; // current sections for the selected file
        private string selectedLevelFile = ""; // selected level file path
        public MapViewer()
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
            ToolTip tooltip = new ToolTip(); // no tooltips added yet
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(Label), typeof(ListBox) });
            ListLevels();
            fullScreen = new FullScreen(this);
        }
        // list all levels in the game
        public void ListLevels()
        {
            foreach (string level in levels)
            {
                string[] levelFiles = Directory.GetFiles(level, "*.MAP");
                foreach (string levelFile in levelFiles) { listBox1.Items.Add(Path.GetFileNameWithoutExtension(levelFile)); }
            }
        }
        // list box selection changed
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
                    selectedLevelFile = Path.Combine(level, selected + ".MAP"); // set selected level file path
                    label2.Text = "File Name : " + selected + ".MAP";
                    break; // exit after finding the first matching level
                }
            }
            // TODO : list all sections in the level file
            // TODO : add more sections to the list box
            // Monsters
            // Pickups
            // Boxes
            // Doors
            // D00? ???? OBJ1 ->
            // 44 30 30 ?? 00 00 02 94 <-> OBJ1
            // Destructibles??? Might just be boxes
            listBox2.Items.Clear(); // clear sections list box
            listBox2.Visible = true; // show sections list box
            // parse level data
            List<BndSection> levelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "MAP0"); // read MAP0 block
            using var br = new BinaryReader(new MemoryStream(levelSections[0].Data)); // read MAP0 data
            ushort vertCount = br.ReadUInt16();
            textBox2.Text = vertCount.ToString(); // display vertex count
            ushort quadCount = br.ReadUInt16();
            textBox3.Text = quadCount.ToString(); // display quad count
            ushort mapLength = br.ReadUInt16();
            textBox4.Text = mapLength.ToString(); // display map length
            ushort mapWidth = br.ReadUInt16();
            textBox5.Text = mapWidth.ToString(); // display map width
            ushort playerStartX = br.ReadUInt16();
            textBox6.Text = playerStartX.ToString(); // display player start X coordinate
            ushort playerStartY = br.ReadUInt16();
            textBox7.Text = playerStartY.ToString(); // display player start Y coordinate
            br.ReadBytes(2); // unknown 1
            ushort monsterCount = br.ReadUInt16();
            textBox8.Text = monsterCount.ToString(); // display monster count
            ushort pickupCount = br.ReadUInt16();
            textBox9.Text = pickupCount.ToString(); // display pickup count
            ushort boxCount = br.ReadUInt16();
            textBox10.Text = boxCount.ToString(); // display box count
            ushort doorCount = br.ReadUInt16();
            textBox11.Text = doorCount.ToString(); // display door count
            br.ReadBytes(2); // unknown 2
            ushort playerStartAngle = br.ReadUInt16();
            textBox12.Text = playerStartAngle.ToString(); // display player start angle
            br.ReadBytes(10); // unknown 3 & 4
            // vertice formula - multiply the value of these two bytes by 8 - (6 bytes for 3 points + 2 bytes zeros)
            var vertices = new List<(short X, short Y, short Z)>();
            for (int i = 0; i < vertCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short z = br.ReadInt16();
                br.ReadUInt16(); // padding
                vertices.Add((x, y, z));
            }
            // quad formula - the value of these 2 bytes multiply by 20 - (16 bytes dot indices and 4 bytes info)
            var quads = new List<(int A, int B, int C, int D, ushort TexIndex)>();
            for (int i = 0; i < quadCount; i++)
            {
                int a = br.ReadInt32();
                int b = br.ReadInt32();
                int c = br.ReadInt32();
                int d = br.ReadInt32();
                ushort texIndex = br.ReadUInt16();
                ushort flags = br.ReadUInt16(); // unused for now
                quads.Add((a, b, c, d, texIndex));
            }
            // size formula - for these bytes = multiply length by width and multiply the resulting value by 16 - (16 bytes describe one cell.)
            // monster formula = number of elements multiplied by 20 - (20 bytes per monster)
            var monsters = new List<(short Type, short X, short Y, short Z, int health, short drop)>();
            for (int i = 0; i < vertCount; i++)
            {
                short type = br.ReadInt16();
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short z = br.ReadInt16();
                int health = br.ReadInt32();
                short drop = br.ReadInt16();
                br.ReadBytes(13);
                monsters.Add((type, x, y, z, health, drop));
            }
            // pickup formula = number of elements multiplied by 8 - (8 bytes per pickup)
            var pickups = new List<(short X, short Y, short Type, short Amount, short Multiplier, short Z)>();
            for (int i = 0; i < vertCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short type = br.ReadInt16();
                short amount = br.ReadInt16();
                short multiplier = br.ReadInt16();
                br.ReadInt16(); // unk1
                short z = br.ReadInt16();
                br.ReadInt16(); // unk2
                pickups.Add((x, y, type, amount, multiplier, z));
            }
            // boxes formula = number of elements multiplied by 16 - (16 bytes per box)
            var boxes = new List<(short X, short Y, short Type)>();
            for (int i = 0; i < vertCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                short type = br.ReadInt16();
                short drop = br.ReadInt16();
                br.ReadBytes(12); // unknown bytes
                boxes.Add((x, y, type));
            }
            // doors formula = value multiplied by 8 - (8 bytes one element)
            var doors = new List<(short X, short Y, short Time, short Tag, short Rotation, short Index)>();
            for (int i = 0; i < vertCount; i++)
            {
                short x = br.ReadInt16();
                short y = br.ReadInt16();
                br.ReadInt16(); // unk1
                short time = br.ReadInt16();
                short tag = br.ReadInt16();
                br.ReadInt16(); // unk2
                short rotation = br.ReadInt16();
                short index = br.ReadInt16();
                doors.Add((x, y, time, tag, rotation, index));
            }
            //Door Models
            currentSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "D0"); // parse door sections from the selected level file
            foreach (var section in currentSections) { listBox2.Items.Add(section.Name); } // Populate ListBox with section names
            button3.Enabled = true;
        }
        // full screen toggle
        private void button1_Click(object sender, EventArgs e)
        {
            fullScreen.Toggle();
        }
        // close level button
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Visible = true;
            button2.Visible = false;
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            listBox1.ClearSelected(); // reset listbox
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
            label1.Text = "Mission Name : "; // reset mission name label
            label2.Text = "File Name : "; // reset file name label
            lastSelectedLevel = ""; // reset last selected level
        }
        // open level button
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Visible = false;
            button3.Visible = false;
            button2.Visible = true;
        }
        // select output path
        private void button4_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Select output folder to save exported files.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputPath = fbd.SelectedPath;
                textBox1.Text = outputPath; // update text box with selected path
                button5.Enabled = true; // enable export button
                button6.Enabled = true; // enable export all button
            }
        }
        // export selected map as OBJ
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { MessageBox.Show("Please select a model to export."); return; }
            // determine level number and file directory based on selected item
            string caseName = listBox1.SelectedItem!.ToString()!;
            string levelNumber = caseName.Substring(1, 3);
            string fileDirectory = levelNumber.Substring(0, 2) switch
            {
                "11" or "12" or "13" => levelPath1,
                "14" or "15" or "16" => levelPath2,
                "21" or "22" or "23" => levelPath3,
                "24" or "26" => levelPath4,
                "31" or "32" or "33" => levelPath5,
                "35" or "36" or "37" or "38" or "39" => levelPath6,
                "90" => levelPath7,
                _ => throw new Exception("Unknown section selected!")
            };
            // check if the file exists in the determined directory
            string textureDirectory = fileDirectory + "\\" + $"{levelNumber}GFX.B16";
            if (!File.Exists(textureDirectory))
            {
                MessageBox.Show($"Associated graphics file {caseName}.MAP does not exist!");
                return;
            }
            // determine the file directory for the selected map
            fileDirectory = fileDirectory + $"\\{caseName}.MAP";
            // parse the BND sections for UVs and model data
            List<BndSection> uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
            List<BndSection> levelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "MAP0");
            // parse extra sections to the level data
            foreach (BndSection section in levelSections)
            {
                File.WriteAllBytes(outputPath + $"\\{caseName}_{section.Name}.MAP", section.Data);
            }
            MessageBox.Show($"Exported {caseName} to {outputPath}!");
            //TODO : make new method for parsing level section data
            //ModelRenderer.ExportLevel(caseName, uvSections, levelSections[0].Data, $"{levelNumber}GFX", outputPath);
        }
        // export all maps as OBJ
        private void button6_Click(object sender, EventArgs e)
        {
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                button5_Click(null!, null!);
            }
            listBox1.SelectedIndex = previouslySelectedIndex; // restore previously selected index
        }
        // double click to open output path
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        { if (outputPath != "") { Process.Start(new ProcessStartInfo() { FileName = outputPath, UseShellExecute = true, Verb = "open" }); } }
    }
}