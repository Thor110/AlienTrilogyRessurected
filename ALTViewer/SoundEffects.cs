using System.Media;
using System.Text;

namespace ALTViewer
{
    public partial class SoundEffects : Form
    {
        private SoundPlayer soundPlayer = null!;
        public SoundEffects()
        {
            InitializeComponent();
            DetectAudioFiles(); // Automatically detect audio files on form load
        }
        public void DetectAudioFiles()
        {
            try
            {
                string[] audioFiles = Directory.GetFiles("TRILOGY\\CD\\SFX", "*.RAW");
                if (audioFiles.Length == 0) { MessageBox.Show("No audio files found in the specified directory."); return; }
                listBox1.Items.Clear();
                foreach (string file in audioFiles) { listBox1.Items.Add(Path.GetFileNameWithoutExtension(file)); }
            }
            catch (Exception ex) { MessageBox.Show("Error detecting audio files: " + ex.Message); }
        }
        private bool FileExists(string fileName) { return File.Exists($"TRILOGY\\CD\\SFX\\{fileName}.RAW"); }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FileExists(listBox1.SelectedItem!.ToString()!)) { MessageBox.Show("Selected audio file does not exist."); return; } // safety first
            byte[] audioData = File.ReadAllBytes($"TRILOGY\\CD\\SFX\\{listBox1.SelectedItem}.RAW"); // load selected audio file
            pictureBox1.Image = DrawWaveform(audioData, 538, 128); // redraw waveform and update labels
        }
        // play sound method
        private void PlayRawSound()
        {
            if (!FileExists(listBox1.SelectedItem!.ToString()!)) { MessageBox.Show("Selected audio file does not exist."); return; } // safety first
            byte[] rawData = File.ReadAllBytes($"TRILOGY\\CD\\SFX\\{listBox1.SelectedItem}.RAW");
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
            using (MemoryStream ms = new MemoryStream(44))
            using (BinaryWriter bw = new BinaryWriter(ms))
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
                float normalized = max / (float)short.MaxValue;
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
                samples[i] = (short)((b - 128) * 256);
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
    }
}
