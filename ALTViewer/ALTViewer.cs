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
            byte check = br.ReadByte(); // this checks if the patch v1 has been applied ( first five fixes )
            if (check == 0xFF) { button6.Visible = true; }
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
            0x35C44L, 0x35C6CL, 0x35C94L, 0x35CBCL, 0x35CE4L, 0x35D0CL,                                         // fix 5 ( bridge section 5 )
            0x31BD0L, 0x31BA8L, 0x31B80L, 0x31B58L, 0x31B30L, 0x31B08L,                                         // fix 5 ( bridge section 6 )
            0x3C620L, 0x3CDF0L, 0x3D1ECL, 0x3D5E8L, 0x3DB38L, 0x3E164L,                                         // fix 5 ( bridge section 7 )
            0x3E1DCL, 0x3DBB0L, 0x3D660L, 0x3D264L, 0x3CE68L, 0x3C698L };                                       // fix 5 ( bridge section 8 )
            foreach (long value in bridge) { BinaryUtility.ReplaceByte(value, 0x75, patchDirectory); }          // fix 5 ( bridge section )
            // fix texture flips
            long[] flip02 = { 0x3A476L, 0x35D36L, 0x35E26L, 0x35D86L, 0x35DD6L, 0x35E76L, 0x35EC6L, 0x2549AL,
            0x278B2L, 0x293A6L, 0x2B0B6L, 0x2D2EEL, 0x2F116L, 0x30476L, 0x31DB2L,
            0x5132EL, 0x4F51AL, 0x4E2D2L, 0x4CFEAL, 0x4BE1AL,
            0x458B2L, 0x45902L,
            0x408B2L, 0x3E936L, 0x41B72L, 0x4123AL, 0x3EB66L, 0x3DDE2L
            };
            foreach (long value in flip02) { BinaryUtility.ReplaceByte(value, 0x02, patchDirectory); }
            long[] flip00 = { 0x35D5EL, 0x35DAEL, 0x35DFEL, 0x35E4EL, 0x35E9EL, 0x254C2L, 0x269EEL,
            0x2A0DAL, 0x2E496L, 0x30FDEL,
            0x4EC46L, 0x4D8FAL, 0x4C8BAL, 0x4B442L,
            0x458DAL, 0x4592AL,
            0x3F552L, 0x41B4AL, 0x3FACAL, 0x3E40EL, 0x3D842L, 0x3D306L
            };
            foreach (long value in flip00) { BinaryUtility.ReplaceByte(value, 0x00, patchDirectory); }
            //L905LEV.MAP
            patchDirectory = Utilities.CheckDirectory() + "SECT90\\L905LEV.MAP";
            long[] flip00905 = {
                0x56ADA, 0x57A8E, 0x41356, 0x4645A, 0x46432, 0x4A8F2, 0x50F4A, 0x5186E, 0x5211A, 0x50FC2, 0x51896, 0x52142, 0x51012, 0x518BE, 0x5216A, 0x5103A,
                0x518E6, 0x52192, 0x42C42, 0x4434E, 0x44222, 0x44236, 0x42B2A, 0x4410A, 0x42B66, 0x42B7A, 0x42B8E, 0x442AE, 0x42BCA, 0x42BF2, 0x462B6, 0x4619E,
                0x461DA, 0x461EE, 0x46216, 0x4622A, 0x46266, 0x48796, 0x4867E, 0x48692, 0x49E52, 0x49D3A, 0x49D76, 0x49D8A, 0x49D9E, 0x49DDA, 0x49E16, 0x488C2,
                0x487AA, 0x487D2, 0x462F2, 0x4632E, 0x4637E
            };
            foreach (long value in flip00905) { BinaryUtility.ReplaceByte(value, 0x00, patchDirectory); }
            //
            button6.Visible = false; // hide button after patching
            MessageBox.Show("Patch applied successfully!");
        }
    }
}
