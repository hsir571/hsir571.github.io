
using MusicPlayerApp.MVVM.ViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MusicPlayerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        
        
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Loaded += MainWindow_LoadedAsync; 


        }



        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainViewModel)DataContext;
            await viewModel.GetNewReleases();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            TextBlock textBlock = sender as TextBlock;

            if(textBlock != null)
            {
                if(textBlock.IsMouseDirectlyOver)
                {
                    textBlock.Background = new SolidColorBrush(Color.FromArgb(255, 85, 0, 255));
                }
                else
                {
                    textBlock.Background = (Brush)new BrushConverter().ConvertFromString("#00b3b3");
                }
            }

            
            
        }

        private void Button_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
