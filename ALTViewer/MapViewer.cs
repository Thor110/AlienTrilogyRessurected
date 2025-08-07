using System.Diagnostics;

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
        private List<BndSection> doorSections = null!; // door sections for the selected file
        private List<BndSection> liftSections = null!; // lift sections for the selected file
        private string selectedLevelFile = ""; // selected level file path
        private List<(short X, short Y, short Z)> vertices = new();
        private List<(int A, int B, int C, int D, ushort TexIndex)> quads = new();
        private List<(byte Type, byte X, byte Y, byte Z, byte Health, byte Drop, short Speed)> monsters = new();
        private List<(byte X, byte Y, byte Type, byte Amount, byte Multiplier, byte Z)> pickups = new();
        private List<(byte X, byte Y, byte Type)> boxes = new();
        private List<(byte X, byte Y, byte Time, byte Tag, byte Rotation, byte Index)> doors = new();
        private List<(byte X, byte Y, byte Z)> lifts = new();
        private bool exporting;
        private byte[] remainder = null!; // remainder of the file data after parsing
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
            // door models
            listBox2.Items.Clear(); // clear sections list box
            listBox7.Items.Clear(); // clear sections list box
            //Door Models parsed separately for now
            doorSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "D0"); // parse door sections from the selected level file
            foreach (var section in doorSections) { listBox2.Items.Add(section.Name); } // Populate ListBox with section names
            //Lift Models parsed separately for now
            liftSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "L0"); // parse lift sections from the selected level file
            foreach (var section in liftSections) { listBox7.Items.Add(section.Name); } // Populate ListBox with section names
            if (exporting) { return; } // if exporting, do not parse level data meant for viewing
            // clear lists
            vertices.Clear();
            quads.Clear();
            monsters.Clear();
            pickups.Clear();
            boxes.Clear();
            doors.Clear();
            // parse level data -> skip 20 bytes in rather than using ParseBndFormSections in future
            List<BndSection> levelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "MAP0"); // read MAP0 block
            using var ms = new MemoryStream(levelSections[0].Data);
            using var br = new BinaryReader(ms);
            //using var br = new BinaryReader(new MemoryStream(levelSections[0].Data)); // read MAP0 data
            ushort vertCount = br.ReadUInt16();             // vertex count
            textBox2.Text = vertCount.ToString();           // display vertex count
            ushort quadCount = br.ReadUInt16();             // quad count
            textBox3.Text = quadCount.ToString();           // display quad count
            ushort mapLength = br.ReadUInt16();             // map length
            textBox4.Text = mapLength.ToString();           // display map length
            ushort mapWidth = br.ReadUInt16();              // map width
            textBox5.Text = mapWidth.ToString();            // display map width
            ushort playerStartX = br.ReadUInt16();          // player start X coordinate
            textBox6.Text = playerStartX.ToString();        // display player start X coordinate
            ushort playerStartY = br.ReadUInt16();          // player start Y coordinate
            textBox7.Text = playerStartY.ToString();        // display player start Y coordinate
            byte unknown = br.ReadByte();                   // unknown object type ( possibly lights )
            textBox21.Text = unknown.ToString();            // display lift count
            br.ReadByte();                                  // unknown 1 ( unused? 128 on all levels ) - possibly lighting related
            //MessageBox.Show($"Monster : {ms.Position}");  // 14 + 20 = 34 ( L111LEV.MAP )
            ushort monsterCount = br.ReadUInt16();          // monster count
            textBox8.Text = monsterCount.ToString();        // display monster count
            ushort pickupCount = br.ReadUInt16();           // pickup count
            textBox9.Text = pickupCount.ToString();         // display pickup count
            ushort boxCount = br.ReadUInt16();              // box count
            textBox10.Text = boxCount.ToString();           // display box count
            ushort doorCount = br.ReadUInt16();             // door count
            textBox11.Text = doorCount.ToString();          // display door count
            ushort liftCount = br.ReadUInt16();             // lift count
            textBox20.Text = liftCount.ToString();          // display lift count
            ushort playerStartAngle = br.ReadUInt16();      // player start angle
            textBox12.Text = playerStartAngle.ToString();   // display player start angle
            br.ReadBytes(10);                               // unknown 3 & 4
            // these 10 bytes
            //2 - always different  ( unknown )
            //2 - always 0x4040     ( unknown )
            //2 - always different  ( unknown )
            //2 - always different  ( unknown )
            //2 - always 0x0000     ( padding )
            // vertice formula - multiply the value of these two bytes by 8 - (6 bytes for 3 points + 2 bytes zeros)
            br.BaseStream.Seek(vertCount * 8, SeekOrigin.Current);
            // quad formula - the value of these 2 bytes multiply by 20 - (16 bytes dot indices and 4 bytes info)
            br.BaseStream.Seek(quadCount * 20, SeekOrigin.Current);
            //MessageBox.Show($"{ms.Position}"); // 323148 + 20 = 323168 ( L111LEV.MAP )
            // size formula - for these bytes = multiply length by width and multiply the resulting value by 16 - (16 bytes describe one cell.)
            // collision 16
            //4//2//2//1//1//1//1//2//1//1
            br.BaseStream.Seek(mapLength * mapWidth * 16, SeekOrigin.Current); // skip cell size data for now
            br.BaseStream.Seek(unknown * 8, SeekOrigin.Current); // skip up to monster data ( 568 L111LEV.MAP )
            // monster formula = number of elements multiplied by 20 - (20 bytes per monster)
            //MessageBox.Show($"{ms.Position}"); //477708 + 20 = 477728 ( L111LEV.MAP )
            for (int i = 0; i < monsterCount; i++) // 28
            {
                byte type = br.ReadByte();
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte z = br.ReadByte();
                br.ReadByte(); // another unknown byte
                byte health = br.ReadByte();
                byte drop = br.ReadByte();
                br.ReadBytes(7); // unknown bytes
                short speed = br.ReadInt16();
                br.ReadBytes(4); // unknown bytes
                monsters.Add((type, x, y, z, health, drop, speed));
            }
            //MessageBox.Show($"Pickups : {ms.Position}"); // 478268 + 20 = 478288 ( L111LEV.MAP )
            // pickup formula = number of elements multiplied by 8 - (8 bytes per pickup)
            for (int i = 0; i < pickupCount; i++) // 28
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte type = br.ReadByte();
                byte amount = br.ReadByte();
                byte multiplier = br.ReadByte();
                br.ReadByte(); // unk1
                byte z = br.ReadByte();
                br.ReadByte(); // unk2
                pickups.Add((x, y, type, amount, multiplier, z));
            }
            //MessageBox.Show($"Boxes : {ms.Position}"); // 478492 + 20 = 478512 + 568 = 479080 ( L111LEV.MAP )
            // boxes formula = number of elements multiplied by 16 - (16 bytes per box)
            for (int i = 0; i < boxCount; i++) // 44 -> 44 objects in L111LEV.MAP ( Barrels, Boxes, Switches )
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte type = br.ReadByte();
                byte drop = br.ReadByte(); // 0 = Pickup 2 = Enemy
                br.ReadBytes(2); // unknown bytes
                byte dropOne = br.ReadByte(); // index of first pickup dropped
                byte dropTwo = br.ReadByte(); // index of second pickup dropped
                br.ReadBytes(8); // unknown bytes
                boxes.Add((x, y, type));
            }
            //MessageBox.Show($"Doors : {ms.Position}"); // 479196 + 20 = 479216 + 568 = 479784 ( L111LEV.MAP )
            // doors formula = value multiplied by 8 - (8 bytes one element)
            for (int i = 0; i < doorCount; i++) // 6 -> 6 doors in L111LEV.MAP
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                br.ReadByte(); // unk1
                byte time = br.ReadByte();
                byte tag = br.ReadByte();
                br.ReadByte(); // unk2
                byte rotation = br.ReadByte();
                byte index = br.ReadByte();
                doors.Add((x, y, time, tag, rotation, index));
            }
            // lifts formula = value multiplied by 16 - (16 bytes one element)
            for (int i = 0; i < liftCount; i++) // 16 doors in L141LEV.MAP
            {
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte z = br.ReadByte();
                br.ReadBytes(13); // unknown bytes
                lifts.Add((x, y, z));
            }
            //MessageBox.Show($"{br.BaseStream.Position}");
            // clear list boxes
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            // populate list boxes
            for (int i = 0; i < monsters.Count; i++) { listBox3.Items.Add($"Monster {i}"); }
            for (int i = 0; i < pickups.Count; i++) { listBox4.Items.Add($"Pickup {i}"); }
            for (int i = 0; i < boxes.Count; i++) { listBox5.Items.Add($"Object {i}"); }
            for (int i = 0; i < doors.Count; i++) { listBox6.Items.Add($"Door {i}"); }
            // display remaining bytes
            long remainingBytes = br.BaseStream.Length - br.BaseStream.Position;
            textBox19.Text = remainingBytes.ToString();
            // dump remaining bytes
            remainder = br.ReadBytes((int)remainingBytes);
            //
            button3.Enabled = true; // enable open level button
        }
        // full screen toggle
        private void button1_Click(object sender, EventArgs e) { fullScreen.Toggle(); }
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
                button7.Enabled = true; // enable dump remainder button
                button8.Enabled = true; // enable dump all remainders button
                button9.Enabled = true; // enable export doors button
                button10.Enabled = true; // enable export all doors button
                button11.Enabled = true; // enable export lifts button
                button12.Enabled = true; // enable export all lifts button
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
            ModelRenderer.ExportLevel(caseName, uvSections, levelSections[0].Data, $"{levelNumber}GFX", outputPath, checkBox1.Checked, checkBox2.Checked);
            if (!exporting)
            {
                GenerateDebugTextures();
                MessageBox.Show($"Exported {caseName} with UVs!");
            }
        }
        // export all maps as OBJ
        private void button6_Click(object sender, EventArgs e)
        {
            exporting = true;
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                button5_Click(null!, null!);
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
            exporting = false;

            GenerateDebugTextures();
            MessageBox.Show($"Exported all levels with UVs!");
        }
        private void GenerateDebugTextures(bool lifts = false)
        {
            if (checkBox1.Checked) { ModelRenderer.GenerateFlagTextures(outputPath, listBox1.SelectedItem!.ToString()!, lifts); } // Generate textures for known flags
            else if (checkBox2.Checked) { ModelRenderer.GenerateUnknownTextures(outputPath, listBox1.SelectedItem!.ToString()!); } // Generate textures for unknown flags
        }
        // double click to open output path
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        { if (outputPath != "") { Process.Start(new ProcessStartInfo() { FileName = outputPath, UseShellExecute = true, Verb = "open" }); } }
        // monsters
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox4, listBox5, listBox6 });
            //
            label21.Visible = true;
            label22.Visible = true;
            label23.Visible = true;
            label24.Visible = true;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            label30.Visible = false;
            //
            textBox13.Text = $"{monsters[listBox3.SelectedIndex].X}";
            textBox14.Text = $"{monsters[listBox3.SelectedIndex].Y}";
            textBox15.Text = $"{monsters[listBox3.SelectedIndex].Z}";
            textBox16.Text = $"{monsters[listBox3.SelectedIndex].Type}";
            textBox17.Text = $"{monsters[listBox3.SelectedIndex].Health}";
            textBox18.Text = $"{monsters[listBox3.SelectedIndex].Drop}";
        }
        // pickups
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox5, listBox6 });
            //
            label21.Visible = true;
            label22.Visible = true;
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = true;
            label26.Visible = true;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            label30.Visible = false;
            //
            textBox13.Text = $"{pickups[listBox4.SelectedIndex].X}";
            textBox14.Text = $"{pickups[listBox4.SelectedIndex].Y}";
            textBox15.Text = $"{pickups[listBox4.SelectedIndex].Z}";
            textBox16.Text = $"{pickups[listBox4.SelectedIndex].Type}";
            textBox17.Text = $"{pickups[listBox4.SelectedIndex].Amount}";
            textBox18.Text = $"{pickups[listBox4.SelectedIndex].Multiplier}";
        }
        // boxes
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox4, listBox6 });
            //
            label21.Visible = false;
            label22.Visible = true;
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            label30.Visible = false;
            //
            textBox13.Text = $"{boxes[listBox5.SelectedIndex].X}";
            textBox14.Text = $"{boxes[listBox5.SelectedIndex].Y}";
            textBox16.Text = $"{boxes[listBox5.SelectedIndex].Type}";
            textBox15.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
        }
        // doors
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox4, listBox5 });
            //
            label21.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = true;
            label28.Visible = true;
            label29.Visible = true;
            label30.Visible = true;
            //
            textBox13.Text = $"{doors[listBox6.SelectedIndex].X}";
            textBox14.Text = $"{doors[listBox6.SelectedIndex].Y}";
            textBox15.Text = $"{doors[listBox6.SelectedIndex].Time}";
            textBox16.Text = $"{doors[listBox6.SelectedIndex].Tag}";
            textBox17.Text = $"{doors[listBox6.SelectedIndex].Rotation}";
            textBox18.Text = $"{doors[listBox6.SelectedIndex].Index}";
        }
        // Refresh all list boxes to clear selections and reset indices
        private void RefreshListBoxes(ListBox[] listBoxes)
        {
            foreach (var listBox in listBoxes)
            {
                listBox.SelectedIndexChanged -= GetHandlerFor(listBox);
                listBox.BeginUpdate();
                listBox.ClearSelected();
                listBox.SelectedIndex = -1;
                listBox.SelectedItem = null;
                listBox.EndUpdate();
                listBox.SelectedIndexChanged += GetHandlerFor(listBox);
            }
        }
        // Get the appropriate event handler for the given list box
        private EventHandler GetHandlerFor(ListBox listBox)
        {
            return listBox switch
            {
                ListBox lb when lb == listBox3 => listBox3_SelectedIndexChanged!,
                ListBox lb when lb == listBox4 => listBox4_SelectedIndexChanged!,
                ListBox lb when lb == listBox5 => listBox5_SelectedIndexChanged!,
                ListBox lb when lb == listBox6 => listBox6_SelectedIndexChanged!,
                _ => throw new ArgumentException("Unknown list box")
            };
        }
        // debug texture flags
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { if (checkBox1.Checked) { checkBox2.Checked = false; } }
        // debug unknown flags
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { if (checkBox2.Checked) { checkBox1.Checked = false; } }
        // dump remainder of the file data
        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { MessageBox.Show("Please select a level first!"); return; }
            File.WriteAllBytes(Path.Combine(outputPath, $"remainder_{listBox1.SelectedItem!.ToString()!}.bin"), remainder);
            MessageBox.Show("Remainder dumped.");
        }
        // dump all remainders from all levels data
        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                File.WriteAllBytes(Path.Combine(outputPath, $"remainder_{listBox1.SelectedItem!.ToString()!}.bin"), remainder);
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
            MessageBox.Show("All remainders dumped.");
        }
        // door models
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // unused for now
        }
        // lift models
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            // unused for now
        }
        // export door as OBJ
        private void button9_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox2.SelectedIndex;
            if (listBox1.SelectedIndex == -1) { MessageBox.Show("Please select a level first."); return; }
            if (selectedIndex == -1) { MessageBox.Show("Please select a door first."); return; }
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
            string textureDirectory = fileDirectory + "\\" + $"{levelNumber}GFX.B16";
            if (!File.Exists(textureDirectory))
            {
                MessageBox.Show($"Associated graphics file {caseName}.MAP does not exist!");
                return;
            }
            // parse the BND sections for UVs and model data
            List<BndSection> uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
            ModelRenderer.ExportDoorLift($"{listBox1.SelectedItem!.ToString()!}_DOOR{selectedIndex:D2}", uvSections, doorSections[selectedIndex].Data, $"{levelNumber}GFX", outputPath, checkBox1.Checked, checkBox2.Checked);
            if (!exporting)
            {
                if (!checkBox2.Checked) { GenerateDebugTextures(); } // Generate debug textures if not exporting unknowns
                MessageBox.Show("Door model exported.");
            }
        }
        // export all doors as OBJ
        private void button10_Click(object sender, EventArgs e)
        {
            exporting = true;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                for (int d = 0; d < listBox2.Items.Count; d++) // loop through all levels and export each map
                {
                    listBox2.SelectedIndex = d;
                    button9_Click(null!, null!);
                }
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            exporting = false;
            if (!checkBox2.Checked) { GenerateDebugTextures(); } // Generate debug textures if not exporting unknowns
            MessageBox.Show("All door models exported.");
        }
        // export lift as OBJ
        private void button11_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox7.SelectedIndex;
            if (listBox1.SelectedIndex == -1) { MessageBox.Show("Please select a level first."); return; }
            if (selectedIndex == -1) { MessageBox.Show("Please select a lift first."); return; }
            if (liftSections.Count == 0 && !exporting) { MessageBox.Show("No lift sections found for the selected level!"); return; }
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
            string textureDirectory = fileDirectory + "\\" + $"{levelNumber}GFX.B16";
            if (!File.Exists(textureDirectory))
            {
                MessageBox.Show($"Associated graphics file {caseName}.MAP does not exist!");
                return;
            }
            // parse the BND sections for UVs and model data
            List<BndSection> uvSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(textureDirectory), "BX");
            ModelRenderer.ExportDoorLift($"{listBox1.SelectedItem!.ToString()!}_LIFT{selectedIndex:D2}", uvSections, liftSections[selectedIndex].Data, $"{levelNumber}GFX", outputPath, checkBox1.Checked, checkBox2.Checked);
            if (!exporting)
            {
                if (!checkBox2.Checked) { GenerateDebugTextures(true); } // Generate debug textures if not exporting unknowns
                MessageBox.Show("Lift model exported.");
            }
        }
        // export all lifts as OBJ
        private void button12_Click(object sender, EventArgs e)
        {
            exporting = true;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                if (liftSections == null || liftSections.Count == 0) { continue; }
                for (int d = 0; d < listBox7.Items.Count; d++) // loop through all levels and export each map
                {
                    listBox7.SelectedIndex = d;
                    button11_Click(null!, null!);
                }
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            exporting = false;
            if (!checkBox2.Checked) { GenerateDebugTextures(true); } // Generate debug textures if not exporting unknowns
            MessageBox.Show("All lift models exported.");
        }
        // export level collison as OBJ
        private void button13_Click(object sender, EventArgs e)
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
            // determine the file directory for the selected map
            fileDirectory = fileDirectory + $"\\{caseName}.MAP";
            // parse the BND sections for UVs and model data
            List<BndSection> levelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(fileDirectory), "MAP0");
            ModelRenderer.ExportCollision(caseName, levelSections[0].Data, outputPath);
            if (!exporting)
            {
                MessageBox.Show($"Exported {caseName} collison!");
            }
        }
        // export all level collisons as OBJ
        private void button14_Click(object sender, EventArgs e)
        {
            exporting = true;
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                button13_Click(null!, null!);
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
            exporting = false;
            MessageBox.Show($"Exported all levels collision!");
        }
    }
}