using client.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Send.xaml
    /// </summary>
    public partial class Send : Window
    {
        public string SERVICE_URL = "http://localhost:9063/";

        private string userName;
        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Server GetWebClient(string uri)
        {
            //_log.Info(uri);
            var client = new Server(new Uri(uri), new BasicAuthenticationCredentials());
            return client;
        }
        public Send(string userName="")
        {
            InitializeComponent();
            this.userName = userName;
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            var client = GetWebClient(SERVICE_URL);

            var key = client.ServerOperations.GetPubKey(TextBoxTo.Text);

            using (var rsa = new RSACryptoServiceProvider(512))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(key);
                var pubKey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                rsa.FromXmlString(pubKey);

                var byteConverter = new UnicodeEncoding();
                var byteText = byteConverter.GetBytes(TextBoxMessage.Text);
                var encrypted = rsa.Encrypt(byteText, false);

                var message = new SingleMessage()
                {
                    Time = DateTime.Now,
                    FromProperty = userName,
                    To = TextBoxTo.Text,
                    Title = TextBoxTitle.Text,
                    Message = encrypted,
                };

                client.ServerOperations.PostUpload(message);
            }

            this.Close();

        }
    }
}
