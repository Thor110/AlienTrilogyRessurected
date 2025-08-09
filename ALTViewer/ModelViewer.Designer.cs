namespace ALTViewer
{
    partial class ModelViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelViewer));
            listBox1 = new ListBox();
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            button3 = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 214);
            listBox1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.AccessibleDescription = "The output directory.";
            textBox1.Location = new Point(365, 12);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(423, 23);
            textBox1.TabIndex = 1;
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
            // 
            // button1
            // 
            button1.AccessibleDescription = "Select output directory for the files.";
            button1.Location = new Point(284, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Output";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AccessibleDescription = "Export the selected item as OBJ files.";
            button2.Enabled = false;
            button2.Location = new Point(138, 12);
            button2.Name = "button2";
            button2.Size = new Size(140, 23);
            button2.TabIndex = 4;
            button2.Text = "Export as OBJ";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(138, 191);
            label1.Name = "label1";
            label1.Size = new Size(364, 15);
            label1.TabIndex = 5;
            label1.Text = "Note : Models from OPTOBJ uses the graphics from the OPTGFX file.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(138, 211);
            label2.Name = "label2";
            label2.Size = new Size(434, 15);
            label2.TabIndex = 6;
            label2.Text = "Note : Models from OBJ3D and PICKMOD use the graphics from the PICKGFX file.";
            // 
            // button3
            // 
            button3.Enabled = false;
            button3.Location = new Point(138, 41);
            button3.Name = "button3";
            button3.Size = new Size(140, 23);
            button3.TabIndex = 7;
            button3.Text = "Export all as OBJ";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(138, 171);
            label3.Name = "label3";
            label3.Size = new Size(406, 15);
            label3.TabIndex = 8;
            label3.Text = "Note : M036 and M039 from OBJ3D are untextured and unused in the game.";
            // 
            // ModelViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 237);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(listBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ModelViewer";
            Text = "Model Viewer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Button button3;
        private Label label3;
    }
}