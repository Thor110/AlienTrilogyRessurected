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
        private List<(byte Type, byte X, byte Y, byte Z,
            byte Rotation,
            byte Health, byte Drop,
            byte Unk2, byte Difficulty, byte Unk4, byte Unk5, byte Unk6, byte Unk7, byte Unk8,
            byte Speed,
            byte Unk10, byte Unk11, byte Unk12, byte Unk13,
            long Offset)> monsters = new();
        private List<(byte X, byte Y, byte Type, byte Amount, byte Multiplier, byte Z, byte Unk2, long Offset)> pickups = new();
        private List<(byte X, byte Y, byte ObjectType, byte DropType,
            byte Unk1, byte Unk2, byte DropOne, byte DropTwo, byte Unk3, byte Unk4, byte Unk5, byte Unk7, byte Rotation,
            long Offset)> objects = new();
        private List<(byte X, byte Y, byte Unk1, byte Time, byte Tag, byte Rotation, byte Index, long Offset)> doors = new();
        private List<(byte X, byte Y, byte Z,
            byte Unk1, byte Unk2, byte Unk3, byte Unk4, byte Unk5, byte Unk6, byte Unk7, byte Unk8, byte Unk9, byte Unk10, byte Unk11, byte Unk12, byte Unk13,
            long Offset)> lifts = new();
        private bool exporting;
        private byte[] remainder = null!; // remainder of the file data after parsing
        private bool patch;
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
            // check if the game is patched
            string patchDirectory = Utilities.CheckDirectory() + "SECT90\\L906LEV.MAP";
            byte[] patched = File.ReadAllBytes(patchDirectory);
            using var ms = new MemoryStream(patched);
            using var read = new BinaryReader(ms);
            read.BaseStream.Seek(0x50BC8, SeekOrigin.Current);
            byte check = read.ReadByte();
            if (check != 0xFF) { patch = true; }
            // if it is more patches will be applied when exporting the levels
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
            //if (exporting) { return; } // if exporting, do not parse level data meant for viewing
            // clear lists
            vertices.Clear();
            quads.Clear();
            monsters.Clear();
            pickups.Clear();
            objects.Clear();
            doors.Clear();
            lifts.Clear();
            // clear textboxes
            textBox13.Text = "";
            textBox14.Text = "";
            textBox16.Text = "";
            textBox15.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";
            textBox26.Text = "";
            textBox27.Text = "";
            textBox28.Text = "";
            textBox29.Text = "";
            textBox30.Text = "";
            textBox31.Text = "";
            textBox32.Text = "";
            textBox33.Text = "";
            textBox34.Text = "";
            textBox35.Text = "";
            textBox36.Text = "";
            textBox37.Text = "";
            textBox23.Text = "";
            // parse level data -> skip 20 bytes in rather than using ParseBndFormSections in future
            List<BndSection> levelSections = TileRenderer.ParseBndFormSections(File.ReadAllBytes(selectedLevelFile), "MAP0"); // read MAP0 block
            using var ms = new MemoryStream(levelSections[0].Data);
            using var br = new BinaryReader(ms); // currently just reading the MAP0 block so that an accurate remainder is known
            //using var br = new BinaryReader(new MemoryStream(File.ReadAllBytes(selectedLevelFile))); // read entire .MAP file
            //br.BaseStream.Seek(20, SeekOrigin.Current);     // skip 20 byte header
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
            ushort monsterCount = br.ReadUInt16();          // monster count
            textBox8.Text = monsterCount.ToString();        // display monster count
            ushort pickupCount = br.ReadUInt16();           // pickup count
            textBox9.Text = pickupCount.ToString();         // display pickup count
            ushort objectCount = br.ReadUInt16();           // object count
            textBox10.Text = objectCount.ToString();        // display object count
            ushort doorCount = br.ReadUInt16();             // door count
            textBox11.Text = doorCount.ToString();          // display door count
            ushort liftCount = br.ReadUInt16();             // lift count
            textBox20.Text = liftCount.ToString();          // display lift count
            ushort playerStartAngle = br.ReadUInt16();      // player start angle
            textBox12.Text = playerStartAngle.ToString();   // display player start angle
            // unknown bytes
            ushort unknown1 = br.ReadUInt16();              //2 - always different  ( unknown )
            // Chapter 1 ( unknown 1 )
            // L111LEV - 0A 00
            // L112LEV - 1D 00
            // L113LEV - 6D 00
            // L122LEV - 16 00
            // L131LEV - 52 00
            // L114LEV - 6B 00
            // L141LEV - 16 00
            // L115LEV - 6D 00
            // L154LEV - 5D 00
            // L155LEV - 03 00
            // L161LEV - 30 00
            // L162LEV - 00 00
            // Chapter 2 ( unknown 1 )
            // L211LEV - 6F 00
            // L212LEV - B0 00
            // L213LEV - 06 00
            // L222LEV - 9E 00
            // L242LEV - 00 00
            // L231LEV - 17 00
            // L232LEV - 62 00
            // L243LEV - 00 00
            // L262LEV - 0A 00
            // L263LEV - 02 00
            // Chapter 3 ( unknown 1 )
            // L311LEV - 0A 00
            // L321LEV - 12 00
            // L331LEV - 1C 00
            // L322LEV - 12 00
            // L351LEV - 1D 00
            // L352LEV - 0F 00
            // L323LEV - 12 00
            // L371LEV - 1A 00
            // L353LEV - 0F 00
            // L324LEV - 12 00
            // L381LEV - 1C 00
            // L325LEV - 18 00
            // L391LEV - 0C 00
            br.ReadBytes(2);                                //2 - always 0x4040     ( unknown )
            ushort unknown2 = br.ReadUInt16();              //2 - always different  ( unknown )
            // Chapter 1 ( unknown 2 )
            // L111LEV - 14 00
            // L112LEV - 57 01
            // L113LEV - 34 00
            // L122LEV - 71 00
            // L131LEV - 8F 00
            // L114LEV - 2C 00
            // L141LEV - 30 00
            // L115LEV - 34 00
            // L154LEV - 01 01
            // L155LEV - 0C 00
            // L161LEV - 54 00
            // L162LEV - 00 00
            // Chapter 2 ( unknown 2 )
            // L211LEV - D1 00
            // L212LEV - EC 00
            // L213LEV - 1D 00
            // L222LEV - 28 01
            // L242LEV - 00 00
            // L231LEV - 69 00
            // L232LEV - 74 01
            // L243LEV - 00 00
            // L262LEV - 60 00
            // L263LEV - 0C 00
            // Chapter 3 ( unknown 2 )
            // L311LEV - 52 00
            // L321LEV - 54 00
            // L331LEV - 7E 00
            // L322LEV - 54 00
            // L351LEV - 8E 00
            // L352LEV - 78 00
            // L323LEV - 54 00
            // L371LEV - 8E 00
            // L353LEV - 78 00
            // L324LEV - 54 00
            // L381LEV - 78 00
            // L325LEV - 54 00
            // L391LEV - 55 00
            ushort enemyTypes = br.ReadUInt16();            // Available Enemy Types
            // Chapter 1 ( unknown 3 )
            // L111LEV - 22 00 // 2 / 6
            // L112LEV - 22 00 // 2 / 6 / 16
            // L113LEV - 00 00 // null
            // L122LEV - 22 04 // 2 / 6 / 11 / 16
            // L131LEV - 26 04 // 2 / 6 / 11 / 3
            // L114LEV - 00 00 // null
            // L141LEV - 23 00 // 6 / 1 / 2
            // L115LEV - 00 00 // null
            // L154LEV - 23 10 // 6 / 1 / 2 / 13 / 16 / 17 / 19
            // L155LEV - 0C 00 // 18 / 16 / 19
            // L161LEV - A7 02 // 6 / 1 / 2 / 8 / 10 / 3
            // L162LEV - 43 00 // 7 / 1 / 2
            // Chapter 2 ( unknown 3 )
            // L211LEV - 0E 08 // 4 / 12 / 2 / 3
            // L212LEV - 0A 08 // 4 / 2 / 12
            // L213LEV - 02 08 // 2 / 12
            // L222LEV - 0B 08 // 1 / 2 / 12 / 4 / 17 / 19
            // L242LEV - 00 00 // null
            // L231LEV - 14 21 // 14 / 5 / 3 / 9
            // L232LEV - 13 10 // 1 / 2 / 5 / 13 / 16 / 17
            // L243LEV - 00 00 // null
            // L262LEV - 17 02 // 5 / 1 / 2 / 10 / 3
            // L263LEV - 43 00 // 7 / 1 / 2
            // Chapter 3 ( unknown 3 )
            // L311LEV - 10 20 // 5 / 14
            // L321LEV - 02 00 // 2
            // L331LEV - 12 21 // 5 / 14 / 2 / 9
            // L322LEV - 08 00 // 4
            // L351LEV - 24 12 // 6 / 13 / 10 / 3
            // L352LEV - 00 00 // null
            // L323LEV - 10 00 // 5
            // L371LEV - 20 10 // 6 / 13
            // L353LEV - 00 00 // null
            // L324LEV - 22 00 // 2 / 6
            // L381LEV - 23 00 // 6 / 1 / 2
            // L325LEV - 36 00 // 3 / 5 / 2 / 6
            // L391LEV - 43 00 // 7 / 1 / 2
            br.ReadBytes(2);                                //2 - always 0x0000     ( padding )
            // vertice formula - multiply the value of these two bytes by 8 - (6 bytes for 3 points + 2 bytes zeros)
            br.BaseStream.Seek(vertCount * 8, SeekOrigin.Current);
            // quad formula - the value of these 2 bytes multiply by 20 - (16 bytes dot indices and 4 bytes info)
            br.BaseStream.Seek(quadCount * 20, SeekOrigin.Current);
            // size formula - for these bytes = multiply length by width and multiply the resulting value by 16 - (16 bytes describe one cell.)
            // collision 16 //4//2//2//1//1//1//1//2//1//1
            br.BaseStream.Seek(mapLength * mapWidth * 16, SeekOrigin.Current); // skip cell size data for now
            br.BaseStream.Seek(unknown * 8, SeekOrigin.Current); // skip up to monster data ( 568 L111LEV.MAP )
            // monster formula = number of elements multiplied by 20 - (20 bytes per monster)
            for (int i = 0; i < monsterCount; i++) // 28
            {
                long offset = br.BaseStream.Position + 20;  // offset for reference ( L111LEV.MAP - Monster 0 )
                byte type = br.ReadByte();          // type of the monster
                // Monster Types (0x)
                // 01 - Egg
                // 02 - Face Hugger
                // 03 - Chest Burster
                // 04 - Bambi
                // 05 - Dog Alien
                // 06 - Warrior Drone
                // 07 - Queen
                // 08 - Ceiling Warrior Drone
                // 09 - Ceiling Dog Alien
                // 0A - Colonist
                // 0B - Guard
                // 0C - Soldier
                // 0D - Synthetic
                // 0E - Handler
                // 0F - Value not used in any level ( possibly the player )
                // 10 - Horizontal Steam Vent
                // 11 - Horizontal Flame Vent
                // 12 - Vertical Steam Vent
                // 13 - Vertical Flame Vent
                byte x = br.ReadByte();             // x coordinate of the monster
                byte y = br.ReadByte();             // y coordinate of the monster
                byte z = br.ReadByte();             // z coordinate of the monster
                byte rotation = br.ReadByte();      // rotation of the monster
                // 00 - North       // Y+
                // 01 - North East  // X+ Y+
                // 02 - East        // X+
                // 03 - South East  // X+ Y-
                // 04 - South       // Y-
                // 05 - South West  // X- Y-
                // 06 - West        // X-
                // 07 - North West  // X- Y+
                byte health = br.ReadByte();        // health of the monster
                byte drop = br.ReadByte();          // index of object to be dropped
                byte unk2 = br.ReadByte();          // 
                byte difficulty = br.ReadByte();    // 0 - Easy, 1 - Medium, 2 - Hard
                byte unk4 = br.ReadByte();          // 
                byte unk5 = br.ReadByte();          // 
                byte unk6 = br.ReadByte();          // 
                byte unk7 = br.ReadByte();          // 
                byte unk8 = br.ReadByte();          // 
                byte speed = br.ReadByte();         // speed of the monster
                br.ReadByte();                      // only ever 0 across every level in the game
                byte unk10 = br.ReadByte();         // 
                byte unk11 = br.ReadByte();         // 
                byte unk12 = br.ReadByte();         // 
                byte unk13 = br.ReadByte();         // 
                monsters.Add((type, x, y, z, rotation, health, drop,
                    unk2, difficulty, unk4, unk5, unk6, unk7, unk8,
                    speed,
                    unk10, unk11, unk12, unk13,
                    offset));
            }
            // L111LEV - 22 00 // 2 / 6
            // L131LEV - 26 04 // 2 / 6 / 11 / 3
            // for testing purposes only
            /*BinaryUtility.ReplaceByte(0x34, 0x26, "L111LEV.MAP");
            BinaryUtility.ReplaceByte(0x35, 0x04, "L111LEV.MAP");
            foreach (var enemy in enemies)
            {
                if(enemy.Type != 6)
                {
                    BinaryUtility.ReplaceByte(enemy.Offset, 0x03, "L111LEV.MAP");
                }
                else
                {
                    BinaryUtility.ReplaceByte(enemy.Offset, 0x0B, "L111LEV.MAP");
                }
            }*/
            // pickup formula = number of elements multiplied by 8 - (8 bytes per pickup)
            for (int i = 0; i < pickupCount; i++) // 28
            {
                long offset = br.BaseStream.Position + 20;  // offset for reference
                byte x = br.ReadByte();             // x coordinate of the pickup
                byte y = br.ReadByte();             // y coordinate of the pickup
                byte type = br.ReadByte();          // pickup type
                // Pickup Types (0x)
                // 00 - Pistol
                // 01 - Shotgun
                // 02 - Pulse Rifle
                // 03 - Flame Thrower
                // 04 - Smartgun
                // 05 - Nothing / Unused
                // 06 - Seismic Charge
                // 07 - Battery
                // 08 - Night Vision Goggles
                // 09 - Pistol Clip
                // 0A - Shotgun Cartridge
                // 0B - Pulse Rifle Clip
                // 0C - Grenades
                // 0D - Flamethrower Fuel
                // 0E - Smartgun Ammunition
                // 0F - Identity Tag
                // 10 - Shotgun Shell
                // 11 - Hypo Pack
                // 12 - Acid Vest
                // 13 - Body Suit
                // 14 - Medi Kit
                // 15 - Derm Patch
                // 16 - Auto Mapper
                // 17 - Adrenaline Burst
                // 18 - Derm Patch
                // 19 - Shoulder Lamp
                // 1A - Shotgun Cartridge       ( Cannot be picked up )
                // 1B - Grenades                ( Cannot be picked up )
                // 1C - Crashes when near the object
                // 1D - Crashes when near the object
                // 1E - Crashes when near the object
                // 1F - Nothing / Unused
                // 20 - Crashes when near the object
                byte amount = br.ReadByte();        // amount of the pickup
                byte multiplier = br.ReadByte();    // multiplier for the pickup
                br.ReadByte();                      // padding / unused / zero for every pickup across every level
                byte z = br.ReadByte();             // only ever 0 or 1 across every level in the game
                byte unk2 = br.ReadByte();          // unk2 is always the same as amount for ammunition
                pickups.Add((x, y, type, amount, multiplier, z, unk2, offset));
            }
            // boxes formula = number of elements multiplied by 16 - (16 bytes per box)
            for (int i = 0; i < objectCount; i++) // 44 -> 44 objects in L111LEV.MAP ( Barrels, Boxes, Switches )
            {
                long offset = br.BaseStream.Position + 20;  // offset for reference
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte objectType = br.ReadByte();
                // My Object Types (int) - indented = unused
                    // less than 20 - a box that cannot be blown up
                // 20 - a regular box that can be blown up ( or an egg husk if in chapter 3 )
                // 21 - destructible walls
                // 22 - another small switch, the difference is at the bottom of the model ( lightning is drawn )
                // 23 - barrel explodes.
                // 24 - switch with animation ( small switch )
                // 25 - double stacked boxes ( two boxes on top of each other that can be blown up )
                // 26 - wide switch with zipper
                // 27 - wide switch without zipper
                // 28 - an empty object that can be shot
                // 29 - an empty object that can be shot through, something will spawn on death
                    // 30 - is not used across any level in the game
                    // 31 - a regular box that can be blown up
                // 32 - Strange Little Yellow Square
                // 33 - Steel Coil
                    // 34 - Strange Unused Shape
                    // 35 - Light Pylon With No Texture, Completely Red...
                    // 36 - Strange Tall Square ( improperly textured )
                    // 37 - Egg Husk Shape ( untextured )
                    // 38 - a regular box that can be blown up
                    // 39 - a regular box that can be blown up
                    // 40 - a regular box that can be blown up
                    // 41 - a regular box that can be blown up
                byte dropType = br.ReadByte();      // 0 = Pickup 2 = Enemy
                byte unk1 = br.ReadByte();
                byte unk2 = br.ReadByte();          // only ever 0 or 10 across every level in the game
                byte dropOne = br.ReadByte();       // index of first pickup dropped
                byte dropTwo = br.ReadByte();       // index of second pickup dropped
                byte unk3 = br.ReadByte();
                byte unk4 = br.ReadByte();
                byte unk5 = br.ReadByte();
                br.ReadByte();                      // only ever 0 across every level in the game
                byte unk7 = br.ReadByte();
                br.ReadByte();                      // only ever 0 across every level in the game
                byte rotation = br.ReadByte();      // 0 / 2 / 4 / 6
                br.ReadByte();                      // only ever 0 across every level in the game
                objects.Add((x, y, objectType, dropType, unk1, unk2, dropOne, dropTwo, unk3, unk4, unk5, unk7, rotation, offset));
            }
            // doors formula = value multiplied by 8 - (8 bytes one element)
            for (int i = 0; i < doorCount; i++) // 6 -> 6 doors in L111LEV.MAP
            {
                long offset = br.BaseStream.Position + 20;  // offset for reference
                byte x = br.ReadByte();             // x coordinate of the door
                byte y = br.ReadByte();             // y coordinate of the door
                byte unk1 = br.ReadByte();          // only ever 64 or 0 across every level in the game
                byte time = br.ReadByte();          // door open time
                byte tag = br.ReadByte();           // door tag
                br.ReadByte();                      // only ever 0 across every level in the game
                byte rotation = br.ReadByte();      // 0 / 2 / 4 / 6
                // Byte Direction  Facing
                // 00 - North   // Y+
                // 02 - East    // X+
                // 04 - South   // Y-
                // 06 - West    // X-
                byte index = br.ReadByte();         // index of the door model in the BND file
                doors.Add((x, y, unk1, time, tag, rotation, index, offset));
            }
            // lifts formula = value multiplied by 16 - (16 bytes one element)
            for (int i = 0; i < liftCount; i++) // 16 doors in L141LEV.MAP
            {
                long offset = br.BaseStream.Position + 20;  // offset for reference
                byte x = br.ReadByte();
                byte y = br.ReadByte();
                byte z = br.ReadByte();
                byte unk1 = br.ReadByte();
                byte unk2 = br.ReadByte();
                byte unk3 = br.ReadByte();
                byte unk4 = br.ReadByte();
                byte unk5 = br.ReadByte();
                byte unk6 = br.ReadByte();
                byte unk7 = br.ReadByte();
                byte unk8 = br.ReadByte();
                byte unk9 = br.ReadByte();
                byte unk10 = br.ReadByte();
                byte unk11 = br.ReadByte();
                byte unk12 = br.ReadByte();
                byte unk13 = br.ReadByte();
                lifts.Add((x, y, z,
                    unk1, unk2, unk3, unk4, unk5, unk6, unk7, unk8, unk9, unk10, unk11, unk12, unk13,
                    offset));
            }
            textBox22.Text = $"{br.BaseStream.Position + 20:X2}"; // display data remainder offset plus header
            // clear list boxes
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox8.Items.Clear();
            // populate list boxes
            for (int i = 0; i < monsters.Count; i++) { listBox3.Items.Add($"Monster {i}"); }
            for (int i = 0; i < pickups.Count; i++) { listBox4.Items.Add($"Pickup {i}"); }
            for (int i = 0; i < objects.Count; i++) { listBox5.Items.Add($"Object {i}"); }
            for (int i = 0; i < doors.Count; i++) { listBox6.Items.Add($"Door {i}"); }
            for (int i = 0; i < lifts.Count; i++) { listBox8.Items.Add($"Lift {i}"); }
            // display remaining bytes
            long remainingBytes = br.BaseStream.Length - br.BaseStream.Position;
            textBox19.Text = remainingBytes.ToString();
            // dump remaining bytes
            remainder = br.ReadBytes((int)remainingBytes);
            // not used for now
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
            ModelRenderer.ExportLevel(caseName, uvSections, levelSections[0].Data, $"{levelNumber}GFX", outputPath, checkBox1.Checked, checkBox2.Checked, patch);
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
            // TODO : re-enable this event handler removal when done determining all the unknown bytes
            //listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged!;
            int previouslySelectedIndex = listBox1.SelectedIndex; // store previously selected index
            for (int i = 0; i < listBox1.Items.Count; i++) // loop through all levels and export each map
            {
                listBox1.SelectedIndex = i;
                button5_Click(null!, null!);
            }
            if (previouslySelectedIndex != -1) { listBox1.SelectedIndex = previouslySelectedIndex; } // restore previously selected index
            //listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged!;
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
            RefreshListBoxes(new ListBox[] { listBox4, listBox5, listBox6, listBox8 });
            int index = listBox3.SelectedIndex;
            textBox13.Text = $"Type : {monsters[index].Type}";
            textBox14.Text = $"X : {monsters[index].X}";
            textBox15.Text = $"Y : {monsters[index].Y}";
            textBox16.Text = $"Z : {monsters[index].Z}";
            textBox17.Text = $"Rotation : {monsters[index].Rotation}";
            textBox18.Text = $"Health : {monsters[index].Health}";
            textBox24.Text = $"Drop : {monsters[index].Drop}";
            textBox25.Text = $"Unk2 : {monsters[index].Unk2}";
            textBox26.Text = $"Difficulty : {monsters[index].Difficulty}";
            textBox27.Text = $"Unk4 : {monsters[index].Unk4}";
            textBox28.Text = $"Unk5 : {monsters[index].Unk5}";
            textBox29.Text = $"Unk6 : {monsters[index].Unk6}";
            textBox30.Text = $"Unk7 : {monsters[index].Unk7}";
            textBox31.Text = $"Unk8 : {monsters[index].Unk8}";
            textBox32.Text = $"Speed : {monsters[index].Speed}";
            textBox33.Text = "Unused : 0";
            textBox34.Text = $"Unk10 : {monsters[index].Unk10}";
            textBox35.Text = $"Unk11 : {monsters[index].Unk11}";
            textBox36.Text = $"Unk12 : {monsters[index].Unk12}";
            textBox37.Text = $"Unk13 : {monsters[index].Unk13}";
            textBox23.Text = $"{monsters[index].Offset:X2}";
        }
        // pickups
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox5, listBox6, listBox8 });
            int index = listBox4.SelectedIndex;
            textBox13.Text = $"X : {pickups[index].X}";
            textBox14.Text = $"Y : {pickups[index].Y}";
            textBox15.Text = $"Type : {pickups[index].Type}";
            textBox16.Text = $"Amount : {pickups[index].Amount}";
            textBox17.Text = $"Multiplier : {pickups[index].Multiplier}";
            textBox18.Text = "Unused : 0";
            textBox24.Text = $"Z : {pickups[index].Z}";
            textBox25.Text = $"Unk2 : {pickups[index].Unk2}";
            textBox26.Text = "null";
            textBox27.Text = "null";
            textBox28.Text = "null";
            textBox29.Text = "null";
            textBox30.Text = "null";
            textBox31.Text = "null";
            textBox32.Text = "null";
            textBox33.Text = "null";
            textBox34.Text = "null";
            textBox35.Text = "null";
            textBox36.Text = "null";
            textBox37.Text = "null";
            textBox23.Text = $"{pickups[index].Offset:X2}";
        }
        // objects
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox4, listBox6, listBox8 });
            int index = listBox5.SelectedIndex;
            textBox13.Text = $"X : {objects[index].X}";
            textBox14.Text = $"Y : {objects[index].Y}";
            textBox16.Text = $"ObjectType : {objects[index].ObjectType}";
            textBox15.Text = $"DropType : {objects[index].DropType}";
            textBox17.Text = $"Unk1 : {objects[index].Unk1}";
            textBox18.Text = $"Unk2 : {objects[index].Unk2}";
            textBox24.Text = $"DropOne : {objects[index].DropOne}";
            textBox25.Text = $"DropTwo : {objects[index].DropTwo}";
            textBox26.Text = $"Unk3 : {objects[index].Unk3}";
            textBox27.Text = $"Unk4 : {objects[index].Unk4}";
            textBox28.Text = $"Unk5 : {objects[index].Unk5}";
            textBox29.Text = "Unused : 0";
            textBox30.Text = $"Unk7 : {objects[index].Unk7}";
            textBox31.Text = "Unused : 0";
            textBox32.Text = $"Rotation : {objects[index].Rotation}";
            textBox33.Text = "Unused : 0";
            textBox34.Text = "null";
            textBox35.Text = "null";
            textBox36.Text = "null";
            textBox37.Text = "null";
            textBox23.Text = $"{objects[index].Offset:X2}";
        }
        // doors
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox4, listBox5, listBox8 });
            int index = listBox6.SelectedIndex;
            textBox13.Text = $"X : {doors[index].X}";
            textBox14.Text = $"Y : {doors[index].Y}";
            textBox15.Text = $"Unk1 : {doors[index].Unk1}";
            textBox16.Text = $"Time : {doors[index].Time}";
            textBox17.Text = $"Tag : {doors[index].Tag}";
            textBox18.Text = "Unused : 0";
            textBox24.Text = $"Rotation : {doors[index].Rotation}";
            textBox25.Text = $"Index : {doors[index].Index}";
            textBox26.Text = "null";
            textBox27.Text = "null";
            textBox28.Text = "null";
            textBox29.Text = "null";
            textBox30.Text = "null";
            textBox31.Text = "null";
            textBox32.Text = "null";
            textBox33.Text = "null";
            textBox34.Text = "null";
            textBox35.Text = "null";
            textBox36.Text = "null";
            textBox37.Text = "null";
            textBox23.Text = $"{doors[index].Offset:X2}";
        }
        // lifts
        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxes(new ListBox[] { listBox3, listBox4, listBox5, listBox6 });
            int index = listBox8.SelectedIndex;
            textBox13.Text = $"X : {lifts[index].X}";
            textBox14.Text = $"Y : {lifts[index].Y}";
            textBox16.Text = $"Z : {lifts[index].Z}";
            textBox15.Text = $"Unk1 : {lifts[index].Unk1}";
            textBox17.Text = $"Unk2 : {lifts[index].Unk2}";
            textBox18.Text = $"Unk3 : {lifts[index].Unk3}";
            textBox24.Text = $"Unk4 : {lifts[index].Unk4}";
            textBox25.Text = $"Unk5 : {lifts[index].Unk5}";
            textBox26.Text = $"Unk6 : {lifts[index].Unk6}";
            textBox27.Text = $"Unk7 :  {lifts[index].Unk7}";
            textBox28.Text = $"Unk8 :  {lifts[index].Unk8}";
            textBox29.Text = $"Unk9 :  {lifts[index].Unk9}";
            textBox30.Text = $"Unk10 :  {lifts[index].Unk10}";
            textBox31.Text = $"Unk11 :  {lifts[index].Unk11}";
            textBox32.Text = $"Unk12 :  {lifts[index].Unk12}";
            textBox33.Text = $"Unk13 :  {lifts[index].Unk13}";
            textBox34.Text = "null";
            textBox35.Text = "null";
            textBox36.Text = "null";
            textBox37.Text = "null";
            textBox23.Text = $"{lifts[index].Offset:X2}";
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
                ListBox lb when lb == listBox8 => listBox8_SelectedIndexChanged!,
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