using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaskZero
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> Tasks { get; set; }
        private bool isPanelVisible = false;

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskItem>();
            TaskList.ItemsSource = Tasks;

            // Position window at the right edge of the screen
            this.Left = SystemParameters.WorkArea.Width - 8;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
            
            // Make sure the window is visible
            this.Show();
            this.Activate();

            // Add key event handler for the textbox
            NewTaskTextBox.KeyDown += NewTaskTextBox_KeyDown;
        }

        private void NotchControl_MouseEnter(object sender, MouseEventArgs e)
        {
            NotchControl.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
        }

        private void NotchControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isPanelVisible)
            {
                NotchControl.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void NotchControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleTaskPanel();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleTaskPanel();
        }

        private void NewTaskTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!isPanelVisible)
            {
                ToggleTaskPanel();
            }
        }

        private void ToggleTaskPanel()
        {
            if (!isPanelVisible)
            {
                // Show panel and resize window
                TaskPanel.Visibility = Visibility.Visible;
                var widthAnimation = new DoubleAnimation
                {
                    From = 8,
                    To = 288,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                this.BeginAnimation(WidthProperty, widthAnimation);

                var heightAnimation = new DoubleAnimation
                {
                    From = 60,
                    To = 400,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                this.BeginAnimation(HeightProperty, heightAnimation);

                var leftAnimation = new DoubleAnimation
                {
                    From = SystemParameters.WorkArea.Width - 8,
                    To = SystemParameters.WorkArea.Width - 288,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                this.BeginAnimation(LeftProperty, leftAnimation);

                // Update notch icon to show close symbol
                NotchIcon.Data = Geometry.Parse("M 0,0 L 8,8 M 0,8 L 8,0");
                NewTaskTextBox.Focus();
            }
            else
            {
                // Hide panel and resize window
                var widthAnimation = new DoubleAnimation
                {
                    From = 288,
                    To = 8,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                widthAnimation.Completed += (s, e) => TaskPanel.Visibility = Visibility.Collapsed;
                this.BeginAnimation(WidthProperty, widthAnimation);

                var heightAnimation = new DoubleAnimation
                {
                    From = 400,
                    To = 60,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                this.BeginAnimation(HeightProperty, heightAnimation);

                var leftAnimation = new DoubleAnimation
                {
                    From = SystemParameters.WorkArea.Width - 288,
                    To = SystemParameters.WorkArea.Width - 8,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                this.BeginAnimation(LeftProperty, leftAnimation);

                // Update notch icon to show arrow
                NotchIcon.Data = Geometry.Parse("M 8,0 L 0,30 L 8,60 Z");
            }
            isPanelVisible = !isPanelVisible;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
            {
                Tasks.Add(new TaskItem { Title = NewTaskTextBox.Text });
                NewTaskTextBox.Clear();
                NewTaskTextBox.Focus();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button.DataContext as TaskItem;
            if (task != null)
            {
                Tasks.Remove(task);
            }
        }

        private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTask_Click(sender, e);
                if (string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
                {
                    ToggleTaskPanel();
                }
            }
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
} 