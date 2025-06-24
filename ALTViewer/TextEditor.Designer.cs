namespace ALTViewer
{
    partial class TextEditor
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
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            textBox2 = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label12 = new Label();
            label13 = new Label();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(208, 424);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(248, 34);
            label1.Name = "label1";
            label1.Size = new Size(488, 15);
            label1.TabIndex = 1;
            label1.Text = "Note : Only four language options are available due to the way they are built into the game.";
            // 
            // richTextBox1
            // 
            richTextBox1.AccessibleDescription = "The mission briefing.";
            richTextBox1.Location = new Point(343, 242);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(445, 194);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The name of the mission.";
            textBox1.Location = new Point(343, 213);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(445, 23);
            textBox1.TabIndex = 3;
            // 
            // comboBox1
            // 
            comboBox1.AccessibleDescription = "The selected language.";
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(362, 52);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(244, 55);
            label2.Name = "label2";
            label2.Size = new Size(112, 15);
            label2.TabIndex = 5;
            label2.Text = "Selected Language :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(248, 216);
            label3.Name = "label3";
            label3.Size = new Size(89, 15);
            label3.TabIndex = 6;
            label3.Text = "Mission Name :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(226, 245);
            label4.Name = "label4";
            label4.Size = new Size(111, 15);
            label4.TabIndex = 7;
            label4.Text = "Mission Statement :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(489, 55);
            label5.Name = "label5";
            label5.Size = new Size(248, 15);
            label5.TabIndex = 8;
            label5.Text = "This is the language file you intend to replace.";
            // 
            // textBox2
            // 
            textBox2.AccessibleDescription = "Language name as shown in the menu in-game.";
            textBox2.Location = new Point(362, 81);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(121, 23);
            textBox2.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(256, 84);
            label6.Name = "label6";
            label6.Size = new Size(100, 15);
            label6.TabIndex = 10;
            label6.Text = "Language Name :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(489, 84);
            label7.Name = "label7";
            label7.Size = new Size(267, 15);
            label7.TabIndex = 11;
            label7.Text = "String length limited by the original string length.";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(290, 156);
            label8.Name = "label8";
            label8.Size = new Size(476, 15);
            label8.TabIndex = 12;
            label8.Text = "&&0 Is for dark green text, while &&1 is for light green highlighted text. [Mission Objectives]";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(290, 171);
            label9.Name = "label9";
            label9.Size = new Size(327, 15);
            label9.TabIndex = 13;
            label9.Text = "Remember to return to dark green after initiating a highlight.";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(290, 186);
            label10.Name = "label10";
            label10.Size = new Size(429, 15);
            label10.TabIndex = 14;
            label10.Text = "EG : \"&&0 Kill &&1Xenomorphs&&0 and destroy &&1barrels&&0 where you find them.\"";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(260, 156);
            label11.Name = "label11";
            label11.Size = new Size(29, 15);
            label11.TabIndex = 15;
            label11.Text = "Tip :";
            // 
            // radioButton1
            // 
            radioButton1.AccessibleDescription = "List the UI text entries.";
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(362, 12);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(60, 19);
            radioButton1.TabIndex = 16;
            radioButton1.Text = "UI Text";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AccessibleDescription = "List the mission briefings text.";
            radioButton2.AutoSize = true;
            radioButton2.Checked = true;
            radioButton2.Location = new Point(266, 12);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(90, 19);
            radioButton2.TabIndex = 17;
            radioButton2.TabStop = true;
            radioButton2.Text = "Mission Text";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(290, 124);
            label12.Name = "label12";
            label12.Size = new Size(503, 15);
            label12.TabIndex = 18;
            label12.Text = "Use # for empty lines, you can only use one at a time and they must be followed by some text.";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(260, 124);
            label13.Name = "label13";
            label13.Size = new Size(29, 15);
            label13.TabIndex = 19;
            label13.Text = "Tip :";
            // 
            // TextEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(textBox2);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Controls.Add(richTextBox1);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Name = "TextEditor";
            Text = "TextEditor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Label label1;
        private RichTextBox richTextBox1;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox textBox2;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label12;
        private Label label13;
    }
}