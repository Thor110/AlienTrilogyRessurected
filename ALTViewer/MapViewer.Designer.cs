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
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(92, 424);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(110, 12);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 1;
            label1.Text = "Mission Name : ";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(110, 27);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 2;
            label2.Text = "File Name : ";
            // 
            // button1
            // 
            button1.AccessibleDescription = "Toggle full screen.";
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(713, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Full Screen";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AccessibleDescription = "Close the level.";
            button2.Location = new Point(632, 12);
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
            button3.Location = new Point(632, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "Open Level";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The output directory.";
            textBox1.Location = new Point(302, 415);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(486, 23);
            textBox1.TabIndex = 6;
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
            // 
            // button4
            // 
            button4.AccessibleDescription = "Select output directory for the files.";
            button4.Location = new Point(221, 415);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 7;
            button4.Text = "Output";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.AccessibleDescription = "Export the selected level as OBJ files.";
            button5.Enabled = false;
            button5.Location = new Point(110, 415);
            button5.Name = "button5";
            button5.Size = new Size(105, 23);
            button5.TabIndex = 8;
            button5.Text = "Export as OBJ";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.AccessibleDescription = "Export all levels as OBJ files.";
            button6.Enabled = false;
            button6.Location = new Point(110, 386);
            button6.Name = "button6";
            button6.Size = new Size(105, 23);
            button6.TabIndex = 9;
            button6.Text = "Export all as OBJ";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(110, 90);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(105, 289);
            listBox2.TabIndex = 10;
            listBox2.Visible = false;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(526, 12);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 11;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(526, 41);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 12;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(302, 13);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 13;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(302, 42);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(100, 23);
            textBox5.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(470, 15);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 15;
            label3.Text = "Vertices :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(476, 44);
            label4.Name = "label4";
            label4.Size = new Size(47, 15);
            label4.TabIndex = 16;
            label4.Text = "Quads :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(249, 16);
            label5.Name = "label5";
            label5.Size = new Size(50, 15);
            label5.TabIndex = 17;
            label5.Text = "Length :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(254, 45);
            label6.Name = "label6";
            label6.Size = new Size(45, 15);
            label6.TabIndex = 18;
            label6.Text = "Width :";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(526, 70);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(100, 23);
            textBox6.TabIndex = 19;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(526, 99);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(100, 23);
            textBox7.TabIndex = 20;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(302, 71);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(100, 23);
            textBox8.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(476, 73);
            label7.Name = "label7";
            label7.Size = new Size(47, 15);
            label7.TabIndex = 22;
            label7.Text = "Start X :";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(476, 102);
            label8.Name = "label8";
            label8.Size = new Size(47, 15);
            label8.TabIndex = 23;
            label8.Text = "Start Y :";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(237, 74);
            label9.Name = "label9";
            label9.Size = new Size(62, 15);
            label9.TabIndex = 24;
            label9.Text = "Monsters :";
            // 
            // textBox9
            // 
            textBox9.Location = new Point(302, 100);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new Size(100, 23);
            textBox9.TabIndex = 25;
            // 
            // textBox10
            // 
            textBox10.Location = new Point(302, 129);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(100, 23);
            textBox10.TabIndex = 26;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(302, 158);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(100, 23);
            textBox11.TabIndex = 27;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(245, 103);
            label10.Name = "label10";
            label10.Size = new Size(54, 15);
            label10.TabIndex = 28;
            label10.Text = "Pickups :";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(255, 132);
            label11.Name = "label11";
            label11.Size = new Size(44, 15);
            label11.TabIndex = 29;
            label11.Text = "Boxes :";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(255, 161);
            label12.Name = "label12";
            label12.Size = new Size(44, 15);
            label12.TabIndex = 30;
            label12.Text = "Doors :";
            // 
            // textBox12
            // 
            textBox12.Location = new Point(526, 128);
            textBox12.Name = "textBox12";
            textBox12.ReadOnly = true;
            textBox12.Size = new Size(100, 23);
            textBox12.TabIndex = 31;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(438, 131);
            label13.Name = "label13";
            label13.Size = new Size(85, 15);
            label13.TabIndex = 32;
            label13.Text = "Start Rotation :";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(110, 72);
            label14.Name = "label14";
            label14.Size = new Size(86, 15);
            label14.TabIndex = 33;
            label14.Text = "Doors Models :";
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 15;
            listBox3.Location = new Point(290, 255);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(120, 154);
            listBox3.TabIndex = 34;
            listBox3.SelectedIndexChanged += listBox3_SelectedIndexChanged;
            // 
            // listBox4
            // 
            listBox4.FormattingEnabled = true;
            listBox4.ItemHeight = 15;
            listBox4.Location = new Point(416, 255);
            listBox4.Name = "listBox4";
            listBox4.Size = new Size(120, 154);
            listBox4.TabIndex = 35;
            listBox4.SelectedIndexChanged += listBox4_SelectedIndexChanged;
            // 
            // listBox5
            // 
            listBox5.FormattingEnabled = true;
            listBox5.ItemHeight = 15;
            listBox5.Location = new Point(542, 255);
            listBox5.Name = "listBox5";
            listBox5.Size = new Size(120, 154);
            listBox5.TabIndex = 36;
            listBox5.SelectedIndexChanged += listBox5_SelectedIndexChanged;
            // 
            // listBox6
            // 
            listBox6.FormattingEnabled = true;
            listBox6.ItemHeight = 15;
            listBox6.Location = new Point(668, 255);
            listBox6.Name = "listBox6";
            listBox6.Size = new Size(120, 154);
            listBox6.TabIndex = 37;
            listBox6.SelectedIndexChanged += listBox6_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(290, 237);
            label15.Name = "label15";
            label15.Size = new Size(56, 15);
            label15.TabIndex = 38;
            label15.Text = "Monsters";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(416, 237);
            label16.Name = "label16";
            label16.Size = new Size(48, 15);
            label16.TabIndex = 39;
            label16.Text = "Pickups";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(542, 237);
            label17.Name = "label17";
            label17.Size = new Size(38, 15);
            label17.TabIndex = 40;
            label17.Text = "Boxes";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(668, 237);
            label18.Name = "label18";
            label18.Size = new Size(38, 15);
            label18.TabIndex = 41;
            label18.Text = "Doors";
            // 
            // textBox13
            // 
            textBox13.Location = new Point(688, 153);
            textBox13.Name = "textBox13";
            textBox13.ReadOnly = true;
            textBox13.Size = new Size(100, 23);
            textBox13.TabIndex = 42;
            // 
            // textBox14
            // 
            textBox14.Location = new Point(688, 182);
            textBox14.Name = "textBox14";
            textBox14.ReadOnly = true;
            textBox14.Size = new Size(100, 23);
            textBox14.TabIndex = 43;
            // 
            // textBox15
            // 
            textBox15.Location = new Point(688, 211);
            textBox15.Name = "textBox15";
            textBox15.ReadOnly = true;
            textBox15.Size = new Size(100, 23);
            textBox15.TabIndex = 44;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(662, 156);
            label19.Name = "label19";
            label19.Size = new Size(20, 15);
            label19.TabIndex = 45;
            label19.Text = "X :";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(662, 185);
            label20.Name = "label20";
            label20.Size = new Size(20, 15);
            label20.TabIndex = 46;
            label20.Text = "Y :";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(662, 214);
            label21.Name = "label21";
            label21.Size = new Size(20, 15);
            label21.TabIndex = 47;
            label21.Text = "Z :";
            // 
            // MapViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label21);
            Controls.Add(label20);
            Controls.Add(label19);
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
            Text = "LevelViewer";
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
        private Label label19;
        private Label label20;
        private Label label21;
    }
}