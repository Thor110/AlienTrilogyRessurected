namespace ALTViewer
{
    public partial class ALTViewer : Form
    {
        public ALTViewer()
        {
            InitializeComponent();
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

    }
}
