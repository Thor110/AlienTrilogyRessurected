namespace ALTViewer
{
    partial class MapViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapViewer));
            listBox1 = new ListBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            textBox1 = new TextBox();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            listBox2 = new ListBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            textBox6 = new TextBox();
            textBox7 = new TextBox();
            textBox8 = new TextBox();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            textBox9 = new TextBox();
            textBox10 = new TextBox();
            textBox11 = new TextBox();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            textBox12 = new TextBox();
            label13 = new Label();
            label14 = new Label();
            listBox3 = new ListBox();
            listBox4 = new ListBox();
            listBox5 = new ListBox();
            listBox6 = new ListBox();
            label15 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            textBox13 = new TextBox();
            textBox14 = new TextBox();
            textBox15 = new TextBox();
            textBox16 = new TextBox();
            textBox17 = new TextBox();
            textBox18 = new TextBox();
            textBox19 = new TextBox();
            label31 = new Label();
            listBox7 = new ListBox();
            label32 = new Label();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            textBox20 = new TextBox();
            label33 = new Label();
            label34 = new Label();
            textBox21 = new TextBox();
            label35 = new Label();
            listBox8 = new ListBox();
            label36 = new Label();
            textBox22 = new TextBox();
            textBox23 = new TextBox();
            label37 = new Label();
            textBox24 = new TextBox();
            textBox25 = new TextBox();
            textBox26 = new TextBox();
            textBox27 = new TextBox();
            textBox28 = new TextBox();
            textBox29 = new TextBox();
            textBox30 = new TextBox();
            textBox31 = new TextBox();
            textBox32 = new TextBox();
            textBox33 = new TextBox();
            textBox34 = new TextBox();
            textBox35 = new TextBox();
            textBox36 = new TextBox();
            textBox37 = new TextBox();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(92, 559);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(289, 16);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 1;
            label1.Text = "Mission Name : ";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(110, 16);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 2;
            label2.Text = "File Name : ";
            // 
            // button1
            // 
            button1.AccessibleDescription = "Toggle full screen.";
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(620, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Full Screen";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AccessibleDescription = "Close the level.";
            button2.Location = new Point(701, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 4;
            button2.Text = "Close Level";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AccessibleDescription = "Open the selected level.";
            button3.Enabled = false;
            button3.Location = new Point(701, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "Open Level";
            button3.UseVisualStyleBackColor = true;
            button3.Visible = false;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The output directory.";
            textBox1.Location = new Point(302, 548);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(800, 23);
            textBox1.TabIndex = 6;
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
            // 
            // button4
            // 
            button4.AccessibleDescription = "Select output directory for the files.";
            button4.Location = new Point(221, 548);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 7;
            button4.Text = "Output";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.AccessibleDescription = "Export the selected level as an OBJ file.";
            button5.Enabled = false;
            button5.Location = new Point(110, 548);
            button5.Name = "button5";
            button5.Size = new Size(105, 23);
            button5.TabIndex = 8;
            button5.Text = "Export Level";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.AccessibleDescription = "Export all levels as OBJ files.";
            button6.Enabled = false;
            button6.Location = new Point(110, 519);
            button6.Name = "button6";
            button6.Size = new Size(105, 23);
            button6.TabIndex = 9;
            button6.Text = "Export All Levels";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(856, 183);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(120, 169);
            listBox2.TabIndex = 10;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // textBox2
            // 
            textBox2.AccessibleDescription = "Number of vertices in the level model.";
            textBox2.Location = new Point(209, 50);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 11;
            // 
            // textBox3
            // 
            textBox3.AccessibleDescription = "Number of quads in the level model.";
            textBox3.Location = new Point(209, 79);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 12;
            // 
            // textBox4
            // 
            textBox4.AccessibleDescription = "Length of the level grid.";
            textBox4.Location = new Point(209, 108);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 13;
            // 
            // textBox5
            // 
            textBox5.AccessibleDescription = "Width of the level grid.";
            textBox5.Location = new Point(209, 137);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(100, 23);
            textBox5.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(153, 53);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 15;
            label3.Text = "Vertices :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(159, 82);
            label4.Name = "label4";
            label4.Size = new Size(47, 15);
            label4.TabIndex = 16;
            label4.Text = "Quads :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(156, 111);
            label5.Name = "label5";
            label5.Size = new Size(50, 15);
            label5.TabIndex = 17;
            label5.Text = "Length :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(161, 140);
            label6.Name = "label6";
            label6.Size = new Size(45, 15);
            label6.TabIndex = 18;
            label6.Text = "Width :";
            // 
            // textBox6
            // 
            textBox6.AccessibleDescription = "Player start X coordinate.";
            textBox6.Location = new Point(209, 166);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(100, 23);
            textBox6.TabIndex = 19;
            // 
            // textBox7
            // 
            textBox7.AccessibleDescription = "Player start Y coordinate.";
            textBox7.Location = new Point(209, 195);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(100, 23);
            textBox7.TabIndex = 20;
            // 
            // textBox8
            // 
            textBox8.AccessibleDescription = "Number of monsters in the level.";
            textBox8.Location = new Point(209, 253);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(100, 23);
            textBox8.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(159, 169);
            label7.Name = "label7";
            label7.Size = new Size(47, 15);
            label7.TabIndex = 22;
            label7.Text = "Start X :";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(159, 198);
            label8.Name = "label8";
            label8.Size = new Size(47, 15);
            label8.TabIndex = 23;
            label8.Text = "Start Y :";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(144, 256);
            label9.Name = "label9";
            label9.Size = new Size(62, 15);
            label9.TabIndex = 24;
            label9.Text = "Monsters :";
            // 
            // textBox9
            // 
            textBox9.AccessibleDescription = "Number of pickups in the level.";
            textBox9.Location = new Point(209, 282);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new Size(100, 23);
            textBox9.TabIndex = 25;
            // 
            // textBox10
            // 
            textBox10.AccessibleDescription = "Number of objects in the level.";
            textBox10.Location = new Point(209, 311);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(100, 23);
            textBox10.TabIndex = 26;
            // 
            // textBox11
            // 
            textBox11.AccessibleDescription = "Number of doors in the level.";
            textBox11.Location = new Point(209, 340);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(100, 23);
            textBox11.TabIndex = 27;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(152, 285);
            label10.Name = "label10";
            label10.Size = new Size(54, 15);
            label10.TabIndex = 28;
            label10.Text = "Pickups :";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(153, 314);
            label11.Name = "label11";
            label11.Size = new Size(53, 15);
            label11.TabIndex = 29;
            label11.Text = "Objects :";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(162, 343);
            label12.Name = "label12";
            label12.Size = new Size(44, 15);
            label12.TabIndex = 30;
            label12.Text = "Doors :";
            // 
            // textBox12
            // 
            textBox12.AccessibleDescription = "Player start rotation.";
            textBox12.Location = new Point(209, 398);
            textBox12.Name = "textBox12";
            textBox12.ReadOnly = true;
            textBox12.Size = new Size(100, 23);
            textBox12.TabIndex = 31;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(121, 401);
            label13.Name = "label13";
            label13.Size = new Size(85, 15);
            label13.TabIndex = 32;
            label13.Text = "Start Rotation :";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(856, 165);
            label14.Name = "label14";
            label14.Size = new Size(81, 15);
            label14.TabIndex = 33;
            label14.Text = "Door Models :";
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 15;
            listBox3.Location = new Point(478, 372);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(120, 169);
            listBox3.TabIndex = 34;
            listBox3.SelectedIndexChanged += listBox3_SelectedIndexChanged;
            // 
            // listBox4
            // 
            listBox4.FormattingEnabled = true;
            listBox4.ItemHeight = 15;
            listBox4.Location = new Point(604, 372);
            listBox4.Name = "listBox4";
            listBox4.Size = new Size(120, 169);
            listBox4.TabIndex = 35;
            listBox4.SelectedIndexChanged += listBox4_SelectedIndexChanged;
            // 
            // listBox5
            // 
            listBox5.FormattingEnabled = true;
            listBox5.ItemHeight = 15;
            listBox5.Location = new Point(730, 372);
            listBox5.Name = "listBox5";
            listBox5.Size = new Size(120, 169);
            listBox5.TabIndex = 36;
            listBox5.SelectedIndexChanged += listBox5_SelectedIndexChanged;
            // 
            // listBox6
            // 
            listBox6.FormattingEnabled = true;
            listBox6.ItemHeight = 15;
            listBox6.Location = new Point(856, 372);
            listBox6.Name = "listBox6";
            listBox6.Size = new Size(120, 169);
            listBox6.TabIndex = 37;
            listBox6.SelectedIndexChanged += listBox6_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(478, 354);
            label15.Name = "label15";
            label15.Size = new Size(62, 15);
            label15.TabIndex = 38;
            label15.Text = "Monsters :";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(604, 354);
            label16.Name = "label16";
            label16.Size = new Size(54, 15);
            label16.TabIndex = 39;
            label16.Text = "Pickups :";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(730, 354);
            label17.Name = "label17";
            label17.Size = new Size(53, 15);
            label17.TabIndex = 40;
            label17.Text = "Objects :";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(856, 354);
            label18.Name = "label18";
            label18.Size = new Size(44, 15);
            label18.TabIndex = 41;
            label18.Text = "Doors :";
            // 
            // textBox13
            // 
            textBox13.Location = new Point(428, 50);
            textBox13.Name = "textBox13";
            textBox13.ReadOnly = true;
            textBox13.Size = new Size(100, 23);
            textBox13.TabIndex = 42;
            // 
            // textBox14
            // 
            textBox14.Location = new Point(428, 79);
            textBox14.Name = "textBox14";
            textBox14.ReadOnly = true;
            textBox14.Size = new Size(100, 23);
            textBox14.TabIndex = 43;
            // 
            // textBox15
            // 
            textBox15.Location = new Point(428, 108);
            textBox15.Name = "textBox15";
            textBox15.ReadOnly = true;
            textBox15.Size = new Size(100, 23);
            textBox15.TabIndex = 44;
            // 
            // textBox16
            // 
            textBox16.Location = new Point(428, 137);
            textBox16.Name = "textBox16";
            textBox16.ReadOnly = true;
            textBox16.Size = new Size(100, 23);
            textBox16.TabIndex = 48;
            // 
            // textBox17
            // 
            textBox17.Location = new Point(428, 166);
            textBox17.Name = "textBox17";
            textBox17.ReadOnly = true;
            textBox17.Size = new Size(100, 23);
            textBox17.TabIndex = 49;
            // 
            // textBox18
            // 
            textBox18.Location = new Point(428, 195);
            textBox18.Name = "textBox18";
            textBox18.ReadOnly = true;
            textBox18.Size = new Size(100, 23);
            textBox18.TabIndex = 50;
            // 
            // textBox19
            // 
            textBox19.AccessibleDescription = "Remaining bytes at the end of the MAP0 section which have not been processed.";
            textBox19.Location = new Point(876, 12);
            textBox19.Name = "textBox19";
            textBox19.ReadOnly = true;
            textBox19.Size = new Size(100, 23);
            textBox19.TabIndex = 60;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(788, 15);
            label31.Name = "label31";
            label31.Size = new Size(82, 15);
            label31.TabIndex = 61;
            label31.Text = "Unread Bytes :";
            // 
            // listBox7
            // 
            listBox7.FormattingEnabled = true;
            listBox7.ItemHeight = 15;
            listBox7.Location = new Point(982, 183);
            listBox7.Name = "listBox7";
            listBox7.Size = new Size(120, 169);
            listBox7.TabIndex = 62;
            listBox7.SelectedIndexChanged += listBox7_SelectedIndexChanged;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(982, 165);
            label32.Name = "label32";
            label32.Size = new Size(72, 15);
            label32.TabIndex = 63;
            label32.Text = "Lift Models :";
            // 
            // checkBox1
            // 
            checkBox1.AccessibleDescription = "For exporting levels with all faces mapped to their in-game flags, for debugging.";
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(221, 522);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(91, 19);
            checkBox1.TabIndex = 64;
            checkBox1.Text = "Debug Flags";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AccessibleDescription = "For exporting levels with all faces mapped to their in-game unknown bytes, for debugging.";
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(221, 497);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(115, 19);
            checkBox2.TabIndex = 65;
            checkBox2.Text = "Debug Unknown";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // button7
            // 
            button7.Enabled = false;
            button7.Location = new Point(982, 12);
            button7.Name = "button7";
            button7.Size = new Size(120, 23);
            button7.TabIndex = 66;
            button7.Text = "Dump Remainder";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Enabled = false;
            button8.Location = new Point(982, 41);
            button8.Name = "button8";
            button8.Size = new Size(120, 23);
            button8.TabIndex = 67;
            button8.Text = "Dump Remainders";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.AccessibleDescription = "Export the selected door as an OBJ file.";
            button9.Enabled = false;
            button9.Location = new Point(856, 139);
            button9.Name = "button9";
            button9.Size = new Size(120, 23);
            button9.TabIndex = 68;
            button9.Text = "Export Door";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.AccessibleDescription = "Export all doors as OBJ files.";
            button10.Enabled = false;
            button10.Location = new Point(110, 490);
            button10.Name = "button10";
            button10.Size = new Size(105, 23);
            button10.TabIndex = 69;
            button10.Text = "Export All Doors";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button11
            // 
            button11.AccessibleDescription = "Export the selected lift as an OBJ file.";
            button11.Enabled = false;
            button11.Location = new Point(982, 139);
            button11.Name = "button11";
            button11.Size = new Size(120, 23);
            button11.TabIndex = 70;
            button11.Text = "Export Lift";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.AccessibleDescription = "Export all lifts as OBJ files.";
            button12.Enabled = false;
            button12.Location = new Point(110, 461);
            button12.Name = "button12";
            button12.Size = new Size(105, 23);
            button12.TabIndex = 71;
            button12.Text = "Export All Lifts";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button13
            // 
            button13.Enabled = false;
            button13.Location = new Point(856, 110);
            button13.Name = "button13";
            button13.Size = new Size(120, 23);
            button13.TabIndex = 72;
            button13.Text = "Export Collision";
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // button14
            // 
            button14.Enabled = false;
            button14.Location = new Point(982, 110);
            button14.Name = "button14";
            button14.Size = new Size(120, 23);
            button14.TabIndex = 73;
            button14.Text = "Export All Collision";
            button14.UseVisualStyleBackColor = true;
            button14.Click += button14_Click;
            // 
            // textBox20
            // 
            textBox20.AccessibleDescription = "Number of lifts in the level.";
            textBox20.Location = new Point(209, 369);
            textBox20.Name = "textBox20";
            textBox20.ReadOnly = true;
            textBox20.Size = new Size(100, 23);
            textBox20.TabIndex = 74;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(171, 372);
            label33.Name = "label33";
            label33.Size = new Size(35, 15);
            label33.TabIndex = 75;
            label33.Text = "Lifts :";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(142, 227);
            label34.Name = "label34";
            label34.Size = new Size(64, 15);
            label34.TabIndex = 77;
            label34.Text = "Unknown :";
            // 
            // textBox21
            // 
            textBox21.AccessibleDescription = "Unknown list of objects, possibly lights.";
            textBox21.Location = new Point(209, 224);
            textBox21.Name = "textBox21";
            textBox21.ReadOnly = true;
            textBox21.Size = new Size(100, 23);
            textBox21.TabIndex = 76;
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(982, 355);
            label35.Name = "label35";
            label35.Size = new Size(35, 15);
            label35.TabIndex = 79;
            label35.Text = "Lifts :";
            // 
            // listBox8
            // 
            listBox8.FormattingEnabled = true;
            listBox8.ItemHeight = 15;
            listBox8.Location = new Point(982, 373);
            listBox8.Name = "listBox8";
            listBox8.Size = new Size(120, 169);
            listBox8.TabIndex = 78;
            listBox8.SelectedIndexChanged += listBox8_SelectedIndexChanged;
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(765, 44);
            label36.Name = "label36";
            label36.Size = new Size(105, 15);
            label36.TabIndex = 80;
            label36.Text = "Remainder Offset :";
            // 
            // textBox22
            // 
            textBox22.AccessibleDescription = "Offset of the remaining bytes at the end of the MAP0 section which have not been processed.";
            textBox22.Location = new Point(876, 41);
            textBox22.Name = "textBox22";
            textBox22.ReadOnly = true;
            textBox22.Size = new Size(100, 23);
            textBox22.TabIndex = 81;
            // 
            // textBox23
            // 
            textBox23.AccessibleDescription = "Offset of the selected entity.";
            textBox23.Location = new Point(876, 70);
            textBox23.Name = "textBox23";
            textBox23.ReadOnly = true;
            textBox23.Size = new Size(100, 23);
            textBox23.TabIndex = 82;
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Location = new Point(777, 73);
            label37.Name = "label37";
            label37.Size = new Size(92, 15);
            label37.TabIndex = 83;
            label37.Text = "Selected Offset :";
            // 
            // textBox24
            // 
            textBox24.Location = new Point(428, 224);
            textBox24.Name = "textBox24";
            textBox24.ReadOnly = true;
            textBox24.Size = new Size(100, 23);
            textBox24.TabIndex = 84;
            // 
            // textBox25
            // 
            textBox25.Location = new Point(428, 253);
            textBox25.Name = "textBox25";
            textBox25.ReadOnly = true;
            textBox25.Size = new Size(100, 23);
            textBox25.TabIndex = 85;
            // 
            // textBox26
            // 
            textBox26.Location = new Point(428, 282);
            textBox26.Name = "textBox26";
            textBox26.ReadOnly = true;
            textBox26.Size = new Size(100, 23);
            textBox26.TabIndex = 86;
            // 
            // textBox27
            // 
            textBox27.Location = new Point(428, 311);
            textBox27.Name = "textBox27";
            textBox27.ReadOnly = true;
            textBox27.Size = new Size(100, 23);
            textBox27.TabIndex = 87;
            // 
            // textBox28
            // 
            textBox28.Location = new Point(620, 50);
            textBox28.Name = "textBox28";
            textBox28.ReadOnly = true;
            textBox28.Size = new Size(100, 23);
            textBox28.TabIndex = 97;
            // 
            // textBox29
            // 
            textBox29.Location = new Point(620, 79);
            textBox29.Name = "textBox29";
            textBox29.ReadOnly = true;
            textBox29.Size = new Size(100, 23);
            textBox29.TabIndex = 96;
            // 
            // textBox30
            // 
            textBox30.Location = new Point(620, 108);
            textBox30.Name = "textBox30";
            textBox30.ReadOnly = true;
            textBox30.Size = new Size(100, 23);
            textBox30.TabIndex = 95;
            // 
            // textBox31
            // 
            textBox31.Location = new Point(620, 137);
            textBox31.Name = "textBox31";
            textBox31.ReadOnly = true;
            textBox31.Size = new Size(100, 23);
            textBox31.TabIndex = 94;
            // 
            // textBox32
            // 
            textBox32.Location = new Point(620, 166);
            textBox32.Name = "textBox32";
            textBox32.ReadOnly = true;
            textBox32.Size = new Size(100, 23);
            textBox32.TabIndex = 93;
            // 
            // textBox33
            // 
            textBox33.Location = new Point(620, 195);
            textBox33.Name = "textBox33";
            textBox33.ReadOnly = true;
            textBox33.Size = new Size(100, 23);
            textBox33.TabIndex = 92;
            // 
            // textBox34
            // 
            textBox34.Location = new Point(620, 224);
            textBox34.Name = "textBox34";
            textBox34.ReadOnly = true;
            textBox34.Size = new Size(100, 23);
            textBox34.TabIndex = 91;
            // 
            // textBox35
            // 
            textBox35.Location = new Point(620, 253);
            textBox35.Name = "textBox35";
            textBox35.ReadOnly = true;
            textBox35.Size = new Size(100, 23);
            textBox35.TabIndex = 90;
            // 
            // textBox36
            // 
            textBox36.Location = new Point(620, 282);
            textBox36.Name = "textBox36";
            textBox36.ReadOnly = true;
            textBox36.Size = new Size(100, 23);
            textBox36.TabIndex = 89;
            // 
            // textBox37
            // 
            textBox37.Location = new Point(620, 311);
            textBox37.Name = "textBox37";
            textBox37.ReadOnly = true;
            textBox37.Size = new Size(100, 23);
            textBox37.TabIndex = 88;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(385, 53);
            label19.Name = "label19";
            label19.Size = new Size(37, 15);
            label19.TabIndex = 98;
            label19.Text = "Start :";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(557, 314);
            label20.Name = "label20";
            label20.Size = new Size(57, 15);
            label20.TabIndex = 99;
            label20.Text = "Enemies :";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(332, 256);
            label21.Name = "label21";
            label21.Size = new Size(90, 15);
            label21.TabIndex = 100;
            label21.Text = "Pickups/Doors :";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(534, 198);
            label22.Name = "label22";
            label22.Size = new Size(80, 15);
            label22.TabIndex = 101;
            label22.Text = "Objects/Lifts :";
            // 
            // MapViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1114, 583);
            Controls.Add(label22);
            Controls.Add(label21);
            Controls.Add(label20);
            Controls.Add(label19);
            Controls.Add(textBox28);
            Controls.Add(textBox29);
            Controls.Add(textBox30);
            Controls.Add(textBox31);
            Controls.Add(textBox32);
            Controls.Add(textBox33);
            Controls.Add(textBox34);
            Controls.Add(textBox35);
            Controls.Add(textBox36);
            Controls.Add(textBox37);
            Controls.Add(textBox27);
            Controls.Add(textBox26);
            Controls.Add(textBox25);
            Controls.Add(textBox24);
            Controls.Add(label37);
            Controls.Add(textBox23);
            Controls.Add(textBox22);
            Controls.Add(label36);
            Controls.Add(label35);
            Controls.Add(listBox8);
            Controls.Add(label34);
            Controls.Add(textBox21);
            Controls.Add(label33);
            Controls.Add(textBox20);
            Controls.Add(button14);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(label32);
            Controls.Add(listBox7);
            Controls.Add(label31);
            Controls.Add(textBox19);
            Controls.Add(textBox18);
            Controls.Add(textBox17);
            Controls.Add(textBox16);
            Controls.Add(textBox15);
            Controls.Add(textBox14);
            Controls.Add(textBox13);
            Controls.Add(label18);
            Controls.Add(label17);
            Controls.Add(label16);
            Controls.Add(label15);
            Controls.Add(listBox6);
            Controls.Add(listBox5);
            Controls.Add(listBox4);
            Controls.Add(listBox3);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(textBox12);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(textBox11);
            Controls.Add(textBox10);
            Controls.Add(textBox9);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textBox8);
            Controls.Add(textBox7);
            Controls.Add(textBox6);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(listBox2);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MapViewer";
            Text = "Map Viewer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private Button button4;
        private Button button5;
        private Button button6;
        private ListBox listBox2;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox11;
        private Label label10;
        private Label label11;
        private Label label12;
        private TextBox textBox12;
        private Label label13;
        private Label label14;
        private ListBox listBox3;
        private ListBox listBox4;
        private ListBox listBox5;
        private ListBox listBox6;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private TextBox textBox16;
        private TextBox textBox17;
        private TextBox textBox18;
        private TextBox textBox19;
        private Label label31;
        private ListBox listBox7;
        private Label label32;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private Button button14;
        private TextBox textBox20;
        private Label label33;
        private Label label34;
        private TextBox textBox21;
        private Label label35;
        private ListBox listBox8;
        private Label label36;
        private TextBox textBox22;
        private TextBox textBox23;
        private Label label37;
        private TextBox textBox24;
        private TextBox textBox25;
        private TextBox textBox26;
        private TextBox textBox27;
        private TextBox textBox28;
        private TextBox textBox29;
        private TextBox textBox30;
        private TextBox textBox31;
        private TextBox textBox32;
        private TextBox textBox33;
        private TextBox textBox34;
        private TextBox textBox35;
        private TextBox textBox36;
        private TextBox textBox37;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
    }
}