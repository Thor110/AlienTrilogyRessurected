namespace ALTViewer
{
    partial class GraphicsViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicsViewer));
            pictureBox1 = new PictureBox();
            listBox1 = new ListBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton4 = new RadioButton();
            listBox2 = new ListBox();
            label1 = new Label();
            button2 = new Button();
            button3 = new Button();
            textBox1 = new TextBox();
            button4 = new Button();
            comboBox1 = new ComboBox();
            button5 = new Button();
            button6 = new Button();
            checkBox1 = new CheckBox();
            button7 = new Button();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            label4 = new Label();
            comboBox2 = new ComboBox();
            label5 = new Label();
            numericUpDown1 = new NumericUpDown();
            button9 = new Button();
            numericUpDown2 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(407, 46);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(256, 256);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(192, 424);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // radioButton1
            // 
            radioButton1.AccessibleDescription = "Show files from the GFX folder.";
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(216, 21);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(71, 19);
            radioButton1.TabIndex = 3;
            radioButton1.TabStop = true;
            radioButton1.Text = "Graphics";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AccessibleDescription = "Show files from the NME folder.";
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(216, 46);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(69, 19);
            radioButton2.TabIndex = 4;
            radioButton2.Text = "Enemies";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AccessibleDescription = "Show files from the SECT## folders.";
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(216, 71);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(57, 19);
            radioButton3.TabIndex = 5;
            radioButton3.Text = "Levels";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // radioButton4
            // 
            radioButton4.AccessibleDescription = "Show files from the LANGUAGE folder.";
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(216, 96);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(59, 19);
            radioButton4.TabIndex = 6;
            radioButton4.TabStop = true;
            radioButton4.Text = "Panels";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton4_CheckedChanged;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(210, 147);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(120, 259);
            listBox2.TabIndex = 7;
            listBox2.Visible = false;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(223, 129);
            label1.Name = "label1";
            label1.Size = new Size(94, 15);
            label1.TabIndex = 8;
            label1.Text = "Palette Selection";
            label1.Visible = false;
            // 
            // button2
            // 
            button2.AccessibleDescription = "Export the selected frame to the output directory in PNG format.";
            button2.Enabled = false;
            button2.Location = new Point(407, 308);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 9;
            button2.Text = "Export";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AccessibleDescription = "Export all frames to the output directory in PNG format.";
            button3.Enabled = false;
            button3.Location = new Point(588, 308);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 10;
            button3.Text = "Export All";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The output directory.";
            textBox1.Location = new Point(407, 12);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(381, 23);
            textBox1.TabIndex = 11;
            // 
            // button4
            // 
            button4.AccessibleDescription = "Select output directory for the files.";
            button4.Location = new Point(326, 11);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 12;
            button4.Text = "Output";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // comboBox1
            // 
            comboBox1.AccessibleDescription = "Changes the selected frame of the texture.";
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(488, 308);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(94, 23);
            comboBox1.TabIndex = 13;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button5
            // 
            button5.AccessibleDescription = "Replace selected texture file with a .PNG file of your choosing.";
            button5.Enabled = false;
            button5.Location = new Point(407, 337);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 14;
            button5.Text = "Replace";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.AccessibleDescription = "Restore file from the backup if it exists.";
            button6.Enabled = false;
            button6.Location = new Point(588, 337);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 15;
            button6.Text = "Restore";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // checkBox1
            // 
            checkBox1.AccessibleDescription = "If this is checked the file you replace will be backed up with the extension .bak at the default SFX directory if a backup doesn't already exist.";
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Enabled = false;
            checkBox1.Location = new Point(495, 341);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(86, 19);
            checkBox1.TabIndex = 16;
            checkBox1.Text = "Backup File";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.AccessibleDescription = "Opens the palette editor window.";
            button7.Location = new Point(210, 412);
            button7.Name = "button7";
            button7.Size = new Size(120, 23);
            button7.TabIndex = 18;
            button7.Text = "Edit Current Palette";
            button7.UseVisualStyleBackColor = true;
            button7.Visible = false;
            button7.Click += button7_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(334, 416);
            label2.Name = "label2";
            label2.Size = new Size(332, 15);
            label2.TabIndex = 19;
            label2.Text = "Note : Only palette files actually used by the game are loaded.";
            label2.Visible = false;
            // 
            // button1
            // 
            button1.AccessibleDescription = "Export everything at once  to the output directory in PNG format.";
            button1.Enabled = false;
            button1.Location = new Point(668, 412);
            button1.Name = "button1";
            button1.Size = new Size(120, 23);
            button1.TabIndex = 20;
            button1.Text = "Export Everything";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(334, 391);
            label3.Name = "label3";
            label3.Size = new Size(397, 15);
            label3.TabIndex = 21;
            label3.Text = "Note : Palette selection is disabled when the files have embedded palettes.";
            label3.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(334, 366);
            label4.Name = "label4";
            label4.Size = new Size(434, 15);
            label4.TabIndex = 22;
            label4.Text = "Note : Palettes should be 768 bytes, or 672 for BONESHIP, COLONY or PRISHOLD.";
            label4.Visible = false;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(669, 67);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(94, 23);
            comboBox2.TabIndex = 23;
            comboBox2.Visible = false;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(668, 46);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 24;
            label5.Text = "Sub-frames";
            label5.Visible = false;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown1.Location = new Point(688, 96);
            numericUpDown1.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(75, 23);
            numericUpDown1.TabIndex = 25;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // button9
            // 
            button9.Location = new Point(688, 154);
            button9.Name = "button9";
            button9.Size = new Size(75, 23);
            button9.TabIndex = 28;
            button9.Text = "Detect";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(688, 125);
            numericUpDown2.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(75, 23);
            numericUpDown2.TabIndex = 26;
            numericUpDown2.Value = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // GraphicsViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button9);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(label5);
            Controls.Add(comboBox2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(button7);
            Controls.Add(checkBox1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(comboBox1);
            Controls.Add(button4);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(listBox2);
            Controls.Add(radioButton4);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(listBox1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "GraphicsViewer";
            Text = "GraphicsViewer";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private ListBox listBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private ListBox listBox2;
        private Label label1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private Button button4;
        private ComboBox comboBox1;
        private Button button5;
        private Button button6;
        private CheckBox checkBox1;
        private Button button7;
        private Label label2;
        private Button button1;
        private Label label3;
        private Label label4;
        private ComboBox comboBox2;
        private Label label5;
        private NumericUpDown numericUpDown1;
        private Button button9;
        private NumericUpDown numericUpDown2;
    }
}