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
            this.Left = SystemParameters.WorkArea.Width - 15;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
            
            // Make sure the window is visible
            this.Show();
            this.Activate();
        }

        private void NotchControl_MouseEnter(object sender, MouseEventArgs e)
        {
            NotchControl.Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
        }

        private void NotchControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isPanelVisible)
            {
                NotchControl.Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
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

        private void ToggleTaskPanel()
        {
            if (!isPanelVisible)
            {
                // Show panel and resize window
                TaskPanel.Visibility = Visibility.Visible;
                var widthAnimation = new DoubleAnimation
                {
                    From = 20,
                    To = 320,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                this.BeginAnimation(WidthProperty, widthAnimation);

                var leftAnimation = new DoubleAnimation
                {
                    From = SystemParameters.WorkArea.Width - 20,
                    To = SystemParameters.WorkArea.Width - 320,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                this.BeginAnimation(LeftProperty, leftAnimation);

                // Update notch icon to show close symbol
                NotchIcon.Data = Geometry.Parse("M 0,0 L 15,15 M 0,15 L 15,0");
            }
            else
            {
                // Hide panel and resize window
                var widthAnimation = new DoubleAnimation
                {
                    From = 320,
                    To = 20,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                widthAnimation.Completed += (s, e) => TaskPanel.Visibility = Visibility.Collapsed;
                this.BeginAnimation(WidthProperty, widthAnimation);

                var leftAnimation = new DoubleAnimation
                {
                    From = SystemParameters.WorkArea.Width - 320,
                    To = SystemParameters.WorkArea.Width - 20,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                this.BeginAnimation(LeftProperty, leftAnimation);

                // Update notch icon to show trapezoid
                NotchIcon.Data = Geometry.Parse("M 0,0 L 0,80 L 15,60 L 15,20 Z");
            }
            isPanelVisible = !isPanelVisible;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
            {
                Tasks.Add(new TaskItem { Title = NewTaskTextBox.Text });
                NewTaskTextBox.Clear();
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
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
} 