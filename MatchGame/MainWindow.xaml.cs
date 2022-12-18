using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            var listEmoji = new List<String>()
            {
                "🐒", "🦍", "🦧", "🦮", "🐕", "‍🦺", "🐩", "🐕", "🐈", "🐅", "🐆", "🐎", "🦌",
                "🦏", "🦛", "🐂", "🐃", "🐄", "🐖", "🐏", "🐑", "🐐", "🐪", "🐫", "🦙", "🦘",
                "🦥", "🦨", "🦡", "🐘", "🐁", "🐀", "🦔", "🐇", "🐿", "🦎", "🐊", "🐢", "🐍",
                "🐉", "🦕", "🦖", "🦦", "🦈", "🐬", "🐳", "🐋", "🐟", "🐠", "🐡", "🦐", "🦑",
                "🐙", "🦞", "🦀", "🦆", "🐓", "🦃", "🦅", "🕊", "🦢", "🦜", "🦩", "🦚", "🦉",
                "🐦", "🐧", "🐥", "🦇", "🦋", "🐌", "🐛", "🦟", "🦗", "🐜", "🐝", "🐞", "🦂", "🕷",
            };

            Random random = new Random();
            List<String> animalEmoji = new List<String>();
            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(listEmoji.Count);
                animalEmoji.Add(listEmoji[index]);
                animalEmoji.Add(listEmoji[index]);
                listEmoji.RemoveAt(index);
            }

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }

                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }
        }
        
        /// <summary>
        /// Если щелчок сделан на первом животном в паре, сохранить информацию о том, на каком элементе TextBlock щелкнул пользователь,
        /// и убрать животное с экрана. Если это второе животное в паре, либо убрать его с экрана (если животные составляют пару),
        /// либо вернуть на экран первое животное (если животные разные).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
