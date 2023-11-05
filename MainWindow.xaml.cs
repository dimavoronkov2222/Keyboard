using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Keyboard
{
    internal partial class MainWindow : Window
    {
        private string generatedText;
        private string typedText;
        private DateTime startTime;
        private bool isSessionActive;
        public MainWindow()
        {
            InitializeComponent();
            isSessionActive = false;
            this.KeyDown += new KeyEventHandler(Window_KeyDown);
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!isSessionActive)
            {
                generatedText = generatetext();
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
        private string generatetext()
        {
            string text = "DSA";
            return text;
        }

        private void UpdateStatistics()
        {
            // обновление статистики, такой как скорость и количество ошибок
        }
        private void HighlightKey(Key key)
        {
            // подсветка нажатой клавиши на виртуальной клавиатуре
        }
        private void UpdateDifficulty()
        {
            // обновление сложности в соответствии с настройками пользователя
        }
        private void UpdateKeyboardLayout()
        {
            // обновление отображения клавиатуры при нажатии клавиш Shift и Caps Lock
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
