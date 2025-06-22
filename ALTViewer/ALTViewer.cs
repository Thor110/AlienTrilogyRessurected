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
    }
}
