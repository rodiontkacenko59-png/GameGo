using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // --- Старый UI ---
        TextBlock titleText;
        TextBlock soonText;
        DispatcherTimer timer;
        int dotCount = 0;
        bool showSoon = false;

        // --- Новый блок регистрации ---
        TextBox nickInput;
        Button avatarButton;
        Image avatarImage;
        string currentNick = "";
        string avatarPath = "";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs loadedEvent)
        {
            Background = Brushes.Black;

            Grid root = new Grid();
            this.Content = root;

            // --- GameGo ---
            titleText = new TextBlock
            {
                Text = "GameGo",
                Foreground = Brushes.White,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            root.Children.Add(titleText);

            // --- Soon ---
            soonText = new TextBlock
            {
                Text = "",
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            root.Children.Add(soonText);

            // --- Кнопки снизу ---
            root.Children.Add(CreateButton("Игры", 120));
            root.Children.Add(CreateButton("Новости", 250));
            root.Children.Add(CreateButton("Магазин", 380));

            // --- Таймер для Soon ---
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();

            // --- Блок регистрации ник + аватар ---
            StackPanel loginPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 70, 0, 0)
            };
            root.Children.Add(loginPanel);

            // Никнейм
            nickInput = new TextBox
            {
                Width = 150,
                Height = 25
            };
            loginPanel.Children.Add(nickInput);

            // Кнопка выбрать аватар
            avatarButton = new Button
            {
                Content = "Выбрать аватар",
                Width = 120,
                Height = 25,
                Margin = new Thickness(10, 0, 10, 0)
            };
            avatarButton.Click += AvatarButton_Click;
            loginPanel.Children.Add(avatarButton);

            // Аватарка справа от GameGo
            avatarImage = new Image
            {
                Width = 40,
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 20, 20, 0)
            };
            root.Children.Add(avatarImage);

            // Enter для входа
            nickInput.KeyDown += (senderKey, keyEvent) =>
            {
                if (keyEvent.Key == System.Windows.Input.Key.Enter) Login();
            };
        }

        // --- Создание кнопок ---
        Button CreateButton(string text, double x)
        {
            Button btn = new Button
            {
                Content = text,
                Width = 100,
                Height = 40,
                Background = new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(x, 0, 0, 40)
            };
            btn.Click += (senderBtn, argsBtn) =>
            {
                showSoon = true;
                dotCount = 0;
            };
            return btn;
        }

        void Timer_Tick(object senderTimer, EventArgs argsTimer)
        {
            if (!showSoon) return;
            dotCount = (dotCount + 1) % 4;
            soonText.Text = "Soon" + new string('.', dotCount);
        }

        // --- Выбор аватарки ---
        private void AvatarButton_Click(object senderBtn, RoutedEventArgs argsBtn)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
            if (dlg.ShowDialog() == true)
            {
                avatarPath = dlg.FileName;
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(avatarPath, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                avatarImage.Source = bmp;
            }
        }

        // --- Логика входа ---
        private void Login()
        {
            currentNick = nickInput.Text.Trim();
            if (string.IsNullOrEmpty(currentNick))
            {
                MessageBox.Show("Введите ник!");
                return;
            }

            // После входа обновляем UI: справа от GameGo
            titleText.Text = "GameGo — " + currentNick;
        }
    }
}