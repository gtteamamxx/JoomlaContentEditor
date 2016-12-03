using JoomlaContentEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoomlaContentEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow gui;

        public MainWindow()
        {
            InitializeComponent();

            gui = this;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(await Network.IsInternetAvailable()))
            {
                txt_HeaderTitle.Text = "Potrzebujesz połączenia z internetem, by móc edytować zmiany do planu zajęc";
                txt_ConnectToDatabase.Text = "Spróbuj ponownie";
            }
        }

        private async void btn_ConnectToDatabase_Click(object sender, RoutedEventArgs e)
        {
            txt_HeaderTitle.Text = "Trwa sprawdzanie połączenia...";
            pgr_Loading.Visibility = Visibility.Visible;

            btn_ConnectToDatabase.IsEnabled = false;

            if (await Network.IsInternetAvailable())
            {
                if (!(await ConnectToDatabase()))
                {
                    txt_HeaderTitle.Text = "Wystąpił problem podczas łączenia z serwerem";
                    pgr_Loading.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pgr_Loading.Visibility = Visibility.Collapsed;
                    await Task.Delay(TimeSpan.FromSeconds(2.0)); // wait 1.5 sec before change page

                    txt_HeaderTitle.Visibility = Visibility.Collapsed;
                    btn_ConnectToDatabase.Visibility = Visibility.Collapsed;

                    f_PageNavigator.NavigationService.Navigate(new View.CreateChanges());
                }
            }
            else
            {
                txt_HeaderTitle.Text = "Dalej nie posiadasz połączenia z internetem";
                pgr_Loading.Visibility = Visibility.Collapsed;
            }

            btn_ConnectToDatabase.IsEnabled = true;
        }

        private async Task<bool> ConnectToDatabase()
        {
            txt_HeaderTitle.Text = "Trwa łączenie z serwerem ...";

            if (await SQL.ConnectToDatabase())
            {
                txt_HeaderTitle.Text = "Połączono z serwerem bazy danych!";
                return true;
            };

            return false;
        }
    }
}
