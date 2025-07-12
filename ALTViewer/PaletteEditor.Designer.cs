namespace ALTViewer
{
    partial class PaletteEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditor));
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            comboBox1 = new ComboBox();
            button4 = new Button();
            button5 = new Button();
            checkBox1 = new CheckBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pictureBox2 = new PictureBox();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.AccessibleDescription = "Save the palette, updating the original file.";
            button1.Enabled = false;
            button1.Location = new Point(95, 295);
            button1.Name = "button1";
            button1.Size = new Size(119, 23);
            button1.TabIndex = 0;
            button1.Text = "Save Palette";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AccessibleDescription = "Restore a backup of the palette file.";
            button2.Enabled = false;
            button2.Location = new Point(95, 324);
            button2.Name = "button2";
            button2.Size = new Size(119, 23);
            button2.TabIndex = 1;
            button2.Text = "Restore Backup";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AccessibleDescription = "Undo changes that you have made to the active palette.";
            button3.Enabled = false;
            button3.Location = new Point(95, 353);
            button3.Name = "button3";
            button3.Size = new Size(119, 23);
            button3.TabIndex = 2;
            button3.Text = "Undo Changes";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(308, 32);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(256, 256);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(371, 295);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button4
            // 
            button4.AccessibleDescription = "Export the palette as a .PAL file.";
            button4.Location = new Point(95, 382);
            button4.Name = "button4";
            button4.Size = new Size(119, 23);
            button4.TabIndex = 5;
            button4.Text = "Export Palette";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.AccessibleDescription = "Import a palette from a .PAL file.";
            button5.Location = new Point(95, 411);
            button5.Name = "button5";
            button5.Size = new Size(119, 23);
            button5.TabIndex = 6;
            button5.Text = "Import Palette";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // checkBox1
            // 
            checkBox1.AccessibleDescription = "If this is checked the palette you replace will be backed up with the extension .bak at the file directory if a backup doesn't already exist.";
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(220, 327);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(104, 19);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Backup Palette";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(443, 325);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 8;
            comboBox2.Visible = false;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(369, 328);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 9;
            label1.Text = "Sub-frames";
            label1.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(249, 386);
            label2.Name = "label2";
            label2.Size = new Size(298, 15);
            label2.TabIndex = 10;
            label2.Text = "Note : Trimmed colours have a red cross through them.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(249, 415);
            label3.Name = "label3";
            label3.Size = new Size(270, 15);
            label3.TabIndex = 11;
            label3.Text = "Note : Unused colours have a slash through them.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Black;
            label4.ForeColor = Color.White;
            label4.Location = new Point(358, 149);
            label4.Name = "label4";
            label4.Size = new Size(145, 15);
            label4.TabIndex = 12;
            label4.Text = "Detecting unused colours.";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Black;
            pictureBox2.Location = new Point(308, 32);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(256, 256);
            pictureBox2.TabIndex = 13;
            pictureBox2.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(249, 357);
            label5.Name = "label5";
            label5.Size = new Size(319, 15);
            label5.TabIndex = 14;
            label5.Text = "Note : Transparent colours have a white plus through them.";
            // 
            // PaletteEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 441);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox2);
            Controls.Add(checkBox1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(comboBox1);
            Controls.Add(pictureBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "PaletteEditor";
            Text = "PaletteEditor";
            FormClosing += PaletteEditor_FormClosing;
            Shown += PaletteEditor_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private PictureBox pictureBox1;
        private ComboBox comboBox1;
        private Button button4;
        private Button button5;
        private CheckBox checkBox1;
        private ComboBox comboBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox pictureBox2;
        private Label label5;
    }
}