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
            int check = br.ReadInt32(); // this checks if the patch v1 has been applied ( first three fixes )
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
            BinaryUtility.ReplaceByte(0x51342, 04, patchDirectory); // fix 3 ( left rail )
            BinaryUtility.ReplaceByte(0x51A0E, 04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x51EBE, 04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x5236E, 04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x52AEE, 04, patchDirectory); // fix 2 ( right rails )
            BinaryUtility.ReplaceByte(0x22BF9, 04, patchDirectory); // fix 4 ( left rail )
            List<Tuple<long, byte[]>> replacements = new List<Tuple<long, byte[]>>() { Tuple.Create(0x50BC8L, new byte[] { 0xB4, 0x22, 0x00, 0x00 }) };
            BinaryUtility.ReplaceBytes(replacements, patchDirectory);
            button6.Visible = false; // hide button after patching
            MessageBox.Show("Patch applied successfully!");
        }
    }
}
