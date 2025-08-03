namespace ALTViewer
{
    public partial class ALTViewer : Form
    {
        public string patchDirectory = "";
        public ALTViewer()
        {
            InitializeComponent();
            patchDirectory = Utilities.CheckDirectory() + "SECT90\\L906LEV.MAP";
            byte[] patched = File.ReadAllBytes(patchDirectory);
            using var ms = new MemoryStream(patched);
            using var br = new BinaryReader(ms);
            br.BaseStream.Seek(0x50BC8, SeekOrigin.Current);
            int check = br.ReadInt32(); // this checks if the patch v1 has been applied ( first five fixes )
            if (check == -1) { button6.Visible = true; }
        }
        // create new form method
        private void newForm(Form form)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = this.Location;
            form.Show();
            this.Hide();
            form.FormClosed += (s, args) => this.Show();
            form.Move += (s, args) => { if (this.Location != form.Location) { this.Location = form.Location; } };
        }
        private void button1_Click(object sender, EventArgs e) { newForm(new TextEditor()); }
        private void button2_Click(object sender, EventArgs e) { newForm(new ModelViewer()); }
        private void button3_Click(object sender, EventArgs e) { newForm(new GraphicsViewer()); }
        private void button4_Click(object sender, EventArgs e) { newForm(new SoundEffects()); }
        private void button5_Click(object sender, EventArgs e) { newForm(new MapViewer()); }
        // patch changes
        private void button6_Click(object sender, EventArgs e)
        {
            BinaryUtility.ReplaceByte(0x51342, 0x04, patchDirectory); // fix 3 ( left rail )
            BinaryUtility.ReplaceByte(0x51A0E, 0x04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x51EBE, 0x04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x5236E, 0x04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x52AEE, 0x04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x22BF9, 0x04, patchDirectory); // fix 4 ( left rail )
            List<Tuple<long, byte[]>> replacements = new List<Tuple<long, byte[]>>() { Tuple.Create(0x50BC8L, new byte[] { 0xB4, 0x22, 0x00, 0x00 }) };
            BinaryUtility.ReplaceBytes(replacements, patchDirectory); // fix 1 ( triangle )
            replacements = new List<Tuple<long, byte[]>>() { Tuple.Create(0x45680L, new byte[] { 0x75, 0x00 }) };
            BinaryUtility.ReplaceBytes(replacements, patchDirectory); // fix 5 ( bridge section )
            replacements = new List<Tuple<long, byte[]>>() { Tuple.Create(0x22BF9L, new byte[] { 0x01, 0x04 }) };
            BinaryUtility.ReplaceBytes(replacements, patchDirectory); //0x22BF9 == 01 04 ( railing texture fix )
            // NOTE : This face still isn't visible
            replacements = new List<Tuple<long, byte[]>>() { Tuple.Create(0x50BE8L, new byte[] { 0x75, 0x00 }) };
            BinaryUtility.ReplaceBytes(replacements, patchDirectory); //0x50BE8 == 75 00 ( under railing texture fix )
            // NOTE : This face still isn't visible
            long[] bridge = { 0x456A8L, 0x457E8L, 0x45810L, 0x45838L, 0x45860L, 0x45888L,                       // fix 5 ( bridge section 1 )
            0x41C60L, 0x41C38L,  0x41C10L, 0x41BE8L, 0x41BC0L, 0x41B98L,                                        // fix 5 ( bridge section 2 )
            0x3D00CL, 0x3CA94L, 0x36E64L, 0x36068L,                                                             // fix 5 ( bridge section 3 )
            0x35F00L, 0x36CC0L, 0x37B5CL, 0x38908L, 0x396A0L, 0x3A474L, 0x3AD34L, 0x3C044L, 0x3C8F0L, 0x3CEA4L, // fix 5 ( bridge section 4 )
            0x35C44, 0x35C6C, 0x35C94, 0x35CBC, 0x35CE4, 0x35D0C,                                               // fix 5 ( bridge section 5 )
            0x31BD0, 0x31BA8, 0x31B80, 0x31B58, 0x31B30, 0x31B08,                                               // fix 5 ( bridge section 6 )
            0x3C620, 0x3CDF0, 0x3D1EC, 0x3D5E8, 0x3DB38, 0x3E164,                                               // fix 5 ( bridge section 7 )
            0x3E1DC, 0x3DBB0, 0x3D660, 0x3D264, 0x3CE68, 0x3C698 };                                             // fix 5 ( bridge section 8 )
            foreach (long value in bridge) { BinaryUtility.ReplaceByte(value, 0x75, patchDirectory); }          // fix 5 ( bridge section )
            button6.Visible = false; // hide button after patching
            MessageBox.Show("Patch applied successfully!");
        }
    }
}
