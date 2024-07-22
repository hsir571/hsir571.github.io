
using MusicPlayerApp.MVVM.ViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MusicPlayerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public String testing = "Im testing this out";
        
        
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
        public String Test
        {
            get => testing;
            set
            {
                if (value == testing)
                {
                    return;
                }
                testing = value;
                OnPropertyChanged();
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test = "wow i made it work";
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
