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
            pictureBox1 = new PictureBox();
            button1 = new Button();
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(407, 46);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(320, 240);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.AccessibleDescription = "Automatically detect the palette again incase you have changed it and forgotten which palette it used.";
            button1.Location = new Point(210, 413);
            button1.Name = "button1";
            button1.Size = new Size(120, 23);
            button1.TabIndex = 1;
            button1.Text = "Detect Palette";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
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
            radioButton3.AccessibleDescription = "Show files from the level folders.";
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
            button2.AccessibleDescription = "Export the selected file to the output directory in PNG format.";
            button2.Enabled = false;
            button2.Location = new Point(407, 292);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 9;
            button2.Text = "Export";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AccessibleDescription = "Export all listed files to the output directory in PNG format.";
            button3.Enabled = false;
            button3.Location = new Point(652, 292);
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
            // GraphicsViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "GraphicsViewer";
            Text = "GraphicsViewer";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
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
    }
}