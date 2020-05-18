using client.Models;
using client.Models.Messages;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MessageEntry = client.Models.Messages.MessageEntry;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        private string privateKey;
        private string publicKey;
        private string userName;
        private string password;

        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow(UserEntry user)
        {
            InitializeComponent();
            privateKey= user.PrivateKey;
            publicKey = user.PubKey;
            userName = user.Name;
            password = user.Pass;

            SaveNewMessages();
            RefreshListOfEntires();

            ButtonUserSetings.Content = userName;
        }

        private void clear()
        {
            privateKey = "";
            publicKey = "";
            userName = "";
        }

        private void SaveNewMessages()
        {
            var webClient = RestComunator.GetWebClient();

            var result = webClient.ServerOperations.PostDownloadReceived(userName);
            webClient.ServerOperations.ConfirmDownloadOfMessages(result.Select(message => message.Id).ToList());

            using (var r = new MessagesRepository())
            {
                using (var rsa = new RSACryptoServiceProvider(512))
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(privateKey);
                    var privateKeyDecoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    rsa.FromXmlString(privateKeyDecoded);

                    var byteConverter = new UnicodeEncoding();

                    string messageStr = "";

                    foreach (var m in result)
                    {
                        var aesKey = rsa.Decrypt(m.Key, false);
                        messageStr = DecryptStringFromBytes_Aes(m.Message, aesKey.Take(32).ToArray(), aesKey.Skip(32).Take(16).ToArray());

                        r.Add(new MessageEntry()
                        {
                            Time = m.Time.GetValueOrDefault(),
                            From = m.FromProperty,
                            To = m.To,
                            Message = messageStr,
                        });
                    }
                }
            }
        }

        private void RefreshListOfEntires(string filter = "All")
        {
            using (var r = new MessagesRepository())
            {
                var messages = r.GetRecieved(userName, false);
                messages = messages.OrderByDescending(x => x.Time);

                ListOfEntires.Items.Clear();
                foreach (var m in messages)
                {
                    ListOfEntires.Items.Add(m);
                }
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            var w = new Send();
            w.ShowDialog();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SaveNewMessages();
            RefreshListOfEntires();
        }

        private void MenuPublicKey_click(object sender, RoutedEventArgs e)
        {
            var w = new PopUpWindow("Your public key is", publicKey);
            w.ShowDialog();
        }

        private void MenuPrivateKey_click(object sender, RoutedEventArgs e)
        {
            var w = new PopUpWindow("Your private key is", privateKey);
            w.ShowDialog();
        }

        private void MenuLogOut_click(object sender, RoutedEventArgs e)
        {
            var log = new LogWindow();
            log.Show();
            clear();
            this.Close();
        }
        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
