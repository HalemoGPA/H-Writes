
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NAudio.Wave;

namespace H_Writes
{

    public partial class MainWindow : Window
    {
        private int letterSound = 0; // Index to track the current typing sound
        private string[] typingSounds = { "first.wav", "second.wav", "third.wav" }; // Array of typing sound filenames
        private WaveOut waveOut; // Audio playback controller

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the audio playback controller
            waveOut = new WaveOut();
            var audioFile = new AudioFileReader(typingSounds[letterSound]);
            waveOut.Init(audioFile);

            // Event handler for key presses
            this.KeyDown += MainWindow_KeyDown;

            // Set the cursor and focus to the text input
            this.Cursor = Cursors.None;
            TextInput.Focus();


        }



        private async void TextInput_KeyUp(object sender, KeyEventArgs e)
        {
            await PlayTypingSoundAsync(); // Play typing sound asynchronously on key up event
        }

        private async Task PlayTypingSoundAsync()
        {

            var audioFile = new AudioFileReader(typingSounds[letterSound]);

            // Configure audio playback
            waveOut.DeviceNumber = -1;
            waveOut.Init(audioFile);
            int delayDuration = 10;

            // Play the sound asynchronously after a delay
            var playTask = Task.Run(async () =>
            {
                await Task.Delay(delayDuration);
                Dispatcher.Invoke(() => waveOut.Play());
            });

            await playTask;

            // Update the typing sound index
            letterSound = (letterSound + 1) % typingSounds.Length;


        }

        private void ExitFullscreen()
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close(); // Close the application on Escape key press
        }
    }
}
