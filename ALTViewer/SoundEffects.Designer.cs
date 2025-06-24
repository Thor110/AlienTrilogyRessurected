namespace ALTViewer
{
    partial class SoundEffects
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
            listBox1 = new ListBox();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            button2 = new Button();
            button3 = new Button();
            textBox1 = new TextBox();
            button4 = new Button();
            label2 = new Label();
            button5 = new Button();
            label3 = new Label();
            label4 = new Label();
            button6 = new Button();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(232, 424);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // button1
            // 
            button1.AccessibleDescription = "Play the selected sound file. ( you can also double click them in the list box )";
            button1.Location = new Point(250, 258);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Play Sound";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(250, 308);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(538, 128);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(250, 284);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 3;
            label1.Text = "Sound Length :";
            // 
            // button2
            // 
            button2.AccessibleDescription = "Export the selected file to the output directory in WAV format.";
            button2.Enabled = false;
            button2.Location = new Point(250, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 4;
            button2.Text = "Export";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AccessibleDescription = "Select output directory for the files.";
            button3.Location = new Point(250, 41);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "Output";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The output directory.";
            textBox1.Location = new Point(331, 42);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(457, 23);
            textBox1.TabIndex = 6;
            // 
            // button4
            // 
            button4.AccessibleDescription = "Export all files to the output directory in WAV format.";
            button4.Enabled = false;
            button4.Location = new Point(331, 13);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 7;
            button4.Text = "Export All";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(250, 75);
            label2.Name = "label2";
            label2.Size = new Size(476, 15);
            label2.TabIndex = 8;
            label2.Text = "Music is available in .ogg format within the game directory already. Thanks to the repack.";
            // 
            // button5
            // 
            button5.AccessibleDescription = "Open the music folder in explorer.";
            button5.Location = new Point(250, 93);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 9;
            button5.Text = "Music";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(250, 119);
            label3.Name = "label3";
            label3.Size = new Size(378, 15);
            label3.TabIndex = 10;
            label3.Text = "The audio files themselves are just raw audio data, 8-bit pcm, 11025Hz,";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(250, 135);
            label4.Name = "label4";
            label4.Size = new Size(502, 15);
            label4.TabIndex = 11;
            label4.Text = "for anyone that wants to view them manually, or use this tool to package them all as wav files.";
            // 
            // button6
            // 
            button6.AccessibleDescription = "Replace selected audio file with a .WAV file of your choosing.";
            button6.Enabled = false;
            button6.Location = new Point(250, 153);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 12;
            button6.Text = "Replace";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // checkBox1
            // 
            checkBox1.AccessibleDescription = "If this is checked the file you replace will be backed up with the extension .bak at the default SFX directory.";
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(331, 156);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(131, 19);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Backup Original File";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // SoundEffects
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBox1);
            Controls.Add(button6);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button5);
            Controls.Add(label2);
            Controls.Add(button4);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SoundEffects";
            Text = "SoundEffects";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private PictureBox pictureBox1;
        private Label label1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private Button button4;
        private Label label2;
        private Button button5;
        private Label label3;
        private Label label4;
        private Button button6;
        private CheckBox checkBox1;
    }
}