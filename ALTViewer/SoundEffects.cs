using System.Media;
using System.Text;
using System.Diagnostics;

namespace ALTViewer
{
    public partial class SoundEffects : Form
    {
        private SoundPlayer soundPlayer = null!;
        private string outputPath = "";
        public string gameDirectory = ""; // default directories
        public SoundEffects()
        {
            InitializeComponent();
            if (File.Exists("Run.exe"))
            {
                gameDirectory = "HDD\\TRILOGY\\CD\\SFX\\";
            }
            else if (File.Exists("TRILOGY.EXE"))
            {
                gameDirectory = "CD";
                button5.Visible = false;
                label2.Visible = false;
            }
            ToolTip tooltip = new ToolTip();
            ToolTipHelper.EnableTooltips(this.Controls, tooltip, new Type[] { typeof(PictureBox), typeof(Label), typeof(ListBox) });
            DetectAudioFiles(); // Automatically detect audio files on form load
        }
        // Detect audio files in the default game directory
        public void DetectAudioFiles()
        {
            try
            {
                string[] audioFiles = Directory.GetFiles($"{gameDirectory}", "*.RAW");
                if (audioFiles.Length == 0) { MessageBox.Show("No audio files found in the specified directory."); return; }
                listBox1.Items.Clear();
                foreach (string file in audioFiles) { listBox1.Items.Add(Path.GetFileNameWithoutExtension(file)); }
            }
            catch (Exception ex) { MessageBox.Show("Error detecting audio files: " + ex.Message); }
        }
        private bool FileExists(string fileName) { return File.Exists($"{gameDirectory}{fileName}.RAW"); }
        // list box selection changed event
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FileExists(listBox1.SelectedItem!.ToString()!)) { MessageBox.Show("Selected audio file does not exist."); button2.Enabled = false; return; } // safety first
            string selectedFile = $"{gameDirectory}{listBox1.SelectedItem}.RAW";
            byte[] audioData = File.ReadAllBytes(selectedFile); // load selected audio file
            pictureBox1.Image = DrawWaveform(audioData, 538, 128); // redraw waveform and update labels
            button6.Enabled = true; // enable replace button
            if (File.Exists(selectedFile + ".BAK")) { button7.Enabled = true; } // enable restore button if backup exists
            else { button7.Enabled = false; } // disable restore button if no backup exists
        }
        // play sound method
        private void PlayRawSound()
        {
            if (!FileExists(listBox1.SelectedItem!.ToString()!)) { MessageBox.Show("Selected audio file does not exist."); button2.Enabled = false; return; } // safety first
            byte[] rawData = File.ReadAllBytes($"{gameDirectory}{listBox1.SelectedItem}.RAW");
            byte[] wavHeader = CreateWavHeader(rawData.Length);
            using var ms = new MemoryStream();
            ms.Write(wavHeader, 0, wavHeader.Length);
            ms.Write(rawData, 0, rawData.Length);
            ms.Position = 0;
            try
            {
                soundPlayer?.Stop(); // stop any currently playing sound
                soundPlayer = new SoundPlayer(ms);
                soundPlayer.Play();
            }
            catch (Exception ex) { MessageBox.Show($"Error playing sound: {ex.Message}"); }
        }
        // create wav header
        public static byte[] CreateWavHeader(int dataSize, int sampleRate = 11025, short bitsPerSample = 8, short channels = 1)
        {
            int byteRate = sampleRate * channels * bitsPerSample / 8;
            short blockAlign = (short)(channels * bitsPerSample / 8);
            int chunkSize = 36 + dataSize;
            using (var ms = new MemoryStream(44))
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.ASCII.GetBytes("RIFF"));
                bw.Write(chunkSize);
                bw.Write(Encoding.ASCII.GetBytes("WAVE"));
                bw.Write(Encoding.ASCII.GetBytes("fmt "));
                bw.Write(16); // PCM
                bw.Write((short)1); // format = PCM
                bw.Write(channels);
                bw.Write(sampleRate);
                bw.Write(byteRate);
                bw.Write(blockAlign);
                bw.Write(bitsPerSample);
                bw.Write(Encoding.ASCII.GetBytes("data"));
                bw.Write(dataSize);
                return ms.ToArray();
            }
        }
        // draw waveform from raw audio data
        private Bitmap DrawWaveform(byte[] wavData, int width, int height, int sampleRate = 11025)
        {
            var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            short[] samples = Extract8BitMonoSamples(wavData, sampleRate);
            int samplesPerPixel = samples.Length / width;
            for (int x = 0; x < width; x++)
            {
                int start = x * samplesPerPixel;
                int max = 0;
                for (int i = 0; i < samplesPerPixel && (start + i) < samples.Length; i++)
                {
                    int val = Math.Abs((int)samples[start + i]);
                    if (val > max) max = val;
                }
                float normalized = max / (float)short.MaxValue; // correct scaling
                int y = (int)(normalized * height / 2);
                g.DrawLine(Pens.LimeGreen, x, height / 2 - y, x, height / 2 + y);
            }
            return bmp;
        }
        // extract 8-bit mono samples from WAV data
        private short[] Extract8BitMonoSamples(byte[] wavData, int sampleRate)
        {
            using var ms = new MemoryStream(wavData);
            using var br = new BinaryReader(ms);
            br.BaseStream.Seek(44, SeekOrigin.Begin); // skip WAV header
            int sampleCount = wavData.Length - 44;
            short[] samples = new short[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                byte b = br.ReadByte();
                // Convert unsigned 8-bit (0-255) to signed short (-32768 to 32767) scale
                samples[i] = (short)Math.Clamp((b - 128) * 256, short.MinValue, short.MaxValue);
            }
            double duration = sampleCount / (double)sampleRate;
            if (duration > 60)
            {
                int minutes = (int)duration / 60;
                int seconds = (int)duration % 60;
                label1.Text = $"Sound Length : {minutes:D2}:{seconds:D2}";
            }
            else { label1.Text = $"Sound Length : {duration:F2} seconds"; }
            return samples;
        }
        // play sound on double click
        private void listBox1_DoubleClick(object sender, EventArgs e) { PlayRawSound(); }
        // play sound on button click
        private void button1_Click(object sender, EventArgs e) { PlayRawSound(); }
        // export to file button click
        private void button2_Click(object sender, EventArgs e)
        {
            if (!FileExists(listBox1.SelectedItem!.ToString()!)) { MessageBox.Show("Selected audio file does not exist."); button2.Enabled = false; return; } // safety first
            ExtractToFile(outputPath, listBox1.SelectedItem!.ToString()!);
            MessageBox.Show($"Audio file extracted to : {outputPath}");
        }
        // export to file method
        private void ExtractToFile(string outputPath, string chosenFile)
        {
            byte[] rawData = File.ReadAllBytes($"{gameDirectory}{chosenFile}.RAW");
            using var fs = new FileStream(Path.Combine(outputPath, $"{chosenFile}.WAV"), FileMode.Create);
            byte[] wavHeader = CreateWavHeader(rawData.Length);
            fs.Write(wavHeader, 0, wavHeader.Length);
            fs.Write(rawData, 0, rawData.Length);
        }
        // select output path button click
        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select output folder to save the WAV file.";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    outputPath = fbd.SelectedPath;
                    textBox1.Text = outputPath; // update text box with selected path
                    button2.Enabled = true; // enable export button
                    button4.Enabled = true; // enable export all button
                }
            }
        }
        // export all button click
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.Items)
            {
                string audio = item.ToString()!;
                if (!FileExists(audio)) // safety first
                {
                    MessageBox.Show($"Audio file {audio} does not exist.");
                    continue;
                }
                else { ExtractToFile(outputPath, audio); }
            }
            MessageBox.Show($"Audio files extracted to : {outputPath}");
        }
        // music button click
        private void button5_Click(object sender, EventArgs e)
        {
            string musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CD\\ALT");
            if (!Directory.Exists(musicPath)) { MessageBox.Show("Directory not found, please reinstall the game."); return; }
            Process.Start(new ProcessStartInfo() { FileName = musicPath, UseShellExecute = true, Verb = "open" });
        }
        // replace sound button click
        private void button6_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV Files (*.wav)|*.wav|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select a Container (.wav) file";
                if (openFileDialog.ShowDialog() == DialogResult.OK) { ReplaceWAV(openFileDialog.FileName); }
            }
        }
        private void ReplaceWAV(string filepath)
        {
            using (var br = new BinaryReader(File.OpenRead(filepath)))
            {
                string riff = Encoding.ASCII.GetString(br.ReadBytes(4)); // Read the RIFF header
                if (riff != "RIFF") { MessageBox.Show("File is not a valid WAV file."); return; }
                br.ReadInt32(); // Skip file size
                string wave = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (wave != "WAVE") { MessageBox.Show("File is not a valid WAV file."); return; }
                while (br.BaseStream.Position < br.BaseStream.Length) // Search for the 'data' chunk
                {
                    string chunkID = Encoding.ASCII.GetString(br.ReadBytes(4));
                    int chunkSize = br.ReadInt32();
                    if (chunkID == "fmt ") // Check for 'fmt ' and 'data' chunks
                    {
                        byte[] fmtData = br.ReadBytes(chunkSize);
                        ushort audioFormat = BitConverter.ToUInt16(fmtData, 0);
                        ushort numChannels = BitConverter.ToUInt16(fmtData, 2);
                        int sampleRate = BitConverter.ToInt32(fmtData, 4);
                        ushort bitsPerSample = BitConverter.ToUInt16(fmtData, 14);
                        if (audioFormat != 1 || numChannels != 1 || sampleRate != 11025 || bitsPerSample != 8)
                        { MessageBox.Show("Unsupported WAV format. Only 8-bit PCM mono at 11025Hz is supported."); return; }
                    }
                    else if (chunkID == "data")
                    {
                        if (chunkSize > int.MaxValue) { MessageBox.Show("Audio data chunk is too large to handle."); return; } // limit would really be uint
                        string item = listBox1.SelectedItem!.ToString()!;
                        string original = $"{gameDirectory}{item}.RAW";
                        string backup = original + ".BAK";
                        if (checkBox1.Checked && !File.Exists(backup)) { File.Copy(original, backup); } // create backup if it doesn't exist
                        byte[] audioData = br.ReadBytes(chunkSize);
                        using var fs = new FileStream(original, FileMode.Create);
                        fs.Write(audioData, 0, audioData.Length);
                        pictureBox1.Image = DrawWaveform(audioData, 538, 128, 11025); // re-draw the selected wave form to match the new file
                        button7.Enabled = true;
                        MessageBox.Show($"{item}.RAW : replaced successfully.");
                        return;
                    }
                    else { br.BaseStream.Seek(chunkSize + (chunkSize % 2), SeekOrigin.Current); } // Skip this chunk
                }
                MessageBox.Show("No audio data found in WAV file.");
            }
        }
        // restore backup button click
        private void button7_Click(object sender, EventArgs e)
        {
            string selectedFile = $"{gameDirectory}{listBox1.SelectedItem}.RAW";
            File.Copy($"{selectedFile}.BAK", selectedFile, true);
            File.Delete($"{selectedFile}.BAK");
            button7.Enabled = false;
            pictureBox1.Image = DrawWaveform(File.ReadAllBytes(selectedFile), 538, 128); // redraw waveform and update labels
            MessageBox.Show("Backup successfully restored!");
        }
    }
}
