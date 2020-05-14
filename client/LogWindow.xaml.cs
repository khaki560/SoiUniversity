using client.Models;
using Microsoft.Rest;
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
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public string SERVICE_URL = "http://localhost:9063/";

        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Server GetWebClient(string uri)
        {
            //_log.Info(uri);
            var client = new Server(new Uri(uri), new BasicAuthenticationCredentials());
            return client;
        }

        public LogWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            LabelInfo.Content = "";
            var webClient = GetWebClient(SERVICE_URL);
            try
            {
                var result = webClient.ServerOperations.PostlogIn(new Credentials(TextBoxLogin.Text.ToString(), TextBoxPassword.Text.ToString()));

                if(result == null)
                {
                    LabelInfo.Content = "Inccorect login or password";
                }
                else 
                {
                    var a = new MainWindow(result);
                    a.Show();
                    this.Close();
                }

            }
            catch (Microsoft.Rest.HttpOperationException ex)
            {
                if(ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    LabelInfo.Content = "Inccorect login or password";
                }
                else 
                {
                    throw new Exception("unknown error code");
                }
            }
        }

        private void ButtonSingUp_Click(object sender, RoutedEventArgs e)
        {
            LabelInfo.Content = "";
            var a = new Register();
            a.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
