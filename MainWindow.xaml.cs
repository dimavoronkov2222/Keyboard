using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
namespace Keyboard
{
    internal partial class MainWindow : Window
    {
        private string generatedText;
        private string typedText;
        private DateTime startTime;
        private bool isSessionActive;
        int number;
        public MainWindow()
        {
            InitializeComponent();
            isSessionActive = false;
            this.KeyDown += new KeyEventHandler(Window_KeyDown);
            DifficultySlider.ValueChanged += (s, e) => UpdateDifficulty();
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!isSessionActive)
            {
                generatedText = generatetext(number);
                ExpectedText.Text = generatedText;
                typedText = "";
                InputText.Text = typedText;
                startTime = DateTime.Now;
                isSessionActive = true;
                EnableControls();
            }
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (isSessionActive)
            {
                isSessionActive = false;
                TimeSpan elapsedTime = DateTime.Now - startTime;
                int correctCharacters = CalculateCorrectCharacters(typedText, generatedText);
                double speed1 = correctCharacters / elapsedTime.TotalMinutes;
                int errors = typedText.Length - correctCharacters;
                speed.Text = speed1.ToString();
                fails.Text = errors.ToString();
                DisableControls();
            }
        }
        private int CalculateCorrectCharacters(string typedText, string generatedText)
        {
            int correctCharacters = 0;
            for (int i = 0; i < Math.Min(typedText.Length, generatedText.Length); i++)
            {
                if (typedText[i] == generatedText[i])
                {
                    correctCharacters++;
                }
            }
            return correctCharacters;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isSessionActive)
            {
                if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.CapsLock)
                {
                    UpdateKeyboardLayout();
                }
                if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    typedText += e.Key.ToString();
                    InputText.Text = typedText;
                    UpdateStatistics();
                    HighlightKey(e.Key);
                }
            }
        }
        private string generatetext(int difficulty)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < difficulty; i++)
            {
                int index = random.Next(alphabet.Length);
                text.Append(alphabet[index]);
            }
            return text.ToString();
        }
        private void UpdateStatistics()
        {
            if (isSessionActive)
            {
                TimeSpan elapsedTime = DateTime.Now - startTime;
                int correctCharacters = CalculateCorrectCharacters(typedText, generatedText);
                double speed2 = correctCharacters / elapsedTime.TotalMinutes;
                int errors = typedText.Length - correctCharacters;
                speed.Text = speed2.ToString();
                fails.Text = errors.ToString();
            }
        }
        private void HighlightKey(Key key)
        {
            ClearHighlight(KeyboardGrid);
            string keyName = key.ToString();
            Border keyBorder = FindBorderByName(KeyboardGrid, keyName);
            if (keyBorder != null)
            {
                keyBorder.Background = Brushes.Yellow;
            }
        }
        private void ClearHighlight(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is Border border)
                {
                    border.Background = Brushes.White;
                }
                else if (child is Panel childPanel)
                {
                    ClearHighlight(childPanel);
                }
            }
        }
        private Border FindBorderByName(Panel panel, string name)
        {
            foreach (var child in panel.Children)
            {
                if (child is Border border && border.Name == name)
                {
                    return border;
                }
                else if (child is Panel childPanel)
                {
                    Border found = FindBorderByName(childPanel, name);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }
        private void UpdateDifficulty()
        {
            int difficulty = (int)DifficultySlider.Value;
            generatedText = generatetext(difficulty);
            ExpectedText.Text = generatedText;
            diffi.Text = difficulty.ToString();
            number = difficulty;
        }
        private void UpdateKeyboardLayout()
        {
            bool isShiftPressed = System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift);
            bool isCapsLockToggled = System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock);
            if (isShiftPressed || isCapsLockToggled)
            {
                foreach (var border in KeyboardGrid.Children.OfType<Border>())
                {
                    var textBlock = border.Child as TextBlock;
                    if (textBlock != null)
                    {
                        textBlock.Text = textBlock.Text.ToUpper();
                    }
                }
            }
            else
            {
                foreach (var border in KeyboardGrid.Children.OfType<Border>())
                {
                    var textBlock = border.Child as TextBlock;
                    if (textBlock != null)
                    {
                        textBlock.Text = textBlock.Text.ToLower();
                    }
                }
            }
        }
        private void DisableControls()
        {
            Start.IsEnabled = true;
            Stop.IsEnabled = false;
        }
        private void EnableControls()
        {
            Start.IsEnabled = false;
            Stop.IsEnabled = true;
        }
    }
}