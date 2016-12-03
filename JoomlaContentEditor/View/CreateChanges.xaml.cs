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

namespace JoomlaContentEditor.View
{
    /// <summary>
    /// Interaction logic for CreateChanges.xaml
    /// </summary>
    public partial class CreateChanges : Page
    {
        public CreateChanges()
        {
            InitializeComponent();

            MainWindow.gui.Width = 800.0;
            MainWindow.gui.Height = 500.0;

            this.Loaded += CreateChanges_Loaded;
        }

        private async void CreateChanges_Loaded(object sender, RoutedEventArgs e)
        {
            string actualChangesHtml = await Model.SQL.GetPlainHTMLOfChanges();

            if (actualChangesHtml == null)
            {
                txt_Loading.Text = "Wystąpił problem podczas pobierania aktualnych zmian.";
            }
            else
            {
                Editor.ContentHtml = actualChangesHtml;
                pgr_Loading.Visibility = Visibility.Collapsed;
                txt_Loading.Text = "Pomyślnie wczytano aktualne zmiany!";

                await Task.Delay(TimeSpan.FromSeconds(4));

                txt_Loading.Text = "";
            }
        }

        private async void btn_SendButton_Click(object sender, RoutedEventArgs e)
        {
            pgr_Loading.Visibility = Visibility.Visible;
            txt_Loading.Text = "Trwa aktualizowanie zmian....";

            btn_SendButton.IsEnabled = false;

            if (await Model.Network.IsInternetAvailable())
            {
                if (await Model.SQL.SendPlainHTMLOfChanges(Editor.ContentHtml))
                {
                    pgr_Loading.Visibility = Visibility.Collapsed;
                    txt_Loading.Text = "Pomyślnie zaaktualizowano zmiany";

                    await Task.Delay(TimeSpan.FromSeconds(4));

                    txt_Loading.Text = "";
                }
                else
                {
                    pgr_Loading.Visibility = Visibility.Collapsed;
                    txt_Loading.Text = "Wystąpił błąd podczas aktualizowania";
                    btn_SendButton.Content = "Spróbuj ponownie";
                }
            }
            else
            {
                pgr_Loading.Visibility = Visibility.Collapsed;
                txt_Loading.Text = "Nie posiadasz połączenia z internetem!";
                btn_SendButton.Content = "Spróbuj ponownie";
            }

            btn_SendButton.IsEnabled = true;
        }
    }
}
