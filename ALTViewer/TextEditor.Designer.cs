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
            label1.Location = new Point(260, 26);
            label1.Name = "label1";
            label1.Size = new Size(488, 15);
            label1.TabIndex = 1;
            label1.Text = "Note : Only four language options are available due to the way they are built into the game.";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(343, 242);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(445, 194);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(343, 213);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(445, 23);
            textBox1.TabIndex = 3;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(372, 62);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(254, 65);
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
            label5.Location = new Point(499, 65);
            label5.Name = "label5";
            label5.Size = new Size(248, 15);
            label5.TabIndex = 8;
            label5.Text = "This is the language file you intend to replace.";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(372, 91);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(121, 23);
            textBox2.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(266, 94);
            label6.Name = "label6";
            label6.Size = new Size(100, 15);
            label6.TabIndex = 10;
            label6.Text = "Language Name :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(499, 94);
            label7.Name = "label7";
            label7.Size = new Size(267, 15);
            label7.TabIndex = 11;
            label7.Text = "String length limited by the original string length.";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(290, 139);
            label8.Name = "label8";
            label8.Size = new Size(476, 15);
            label8.TabIndex = 12;
            label8.Text = "&&0 Is for dark green text, while &&1 is for light green highlighted text. [Mission Objectives]";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(290, 154);
            label9.Name = "label9";
            label9.Size = new Size(327, 15);
            label9.TabIndex = 13;
            label9.Text = "Remember to return to dark green after initiating a highlight.";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(290, 169);
            label10.Name = "label10";
            label10.Size = new Size(429, 15);
            label10.TabIndex = 14;
            label10.Text = "EG : \"&&0 Kill &&1Xenomorphs&&0 and destroy &&1barrels&&0 where you find them.\"";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(260, 139);
            label11.Name = "label11";
            label11.Size = new Size(29, 15);
            label11.TabIndex = 15;
            label11.Text = "Tip :";
            // 
            // TextEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}