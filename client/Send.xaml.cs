using client.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.IO;
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


                byte[] encryptedMessage;
                byte[] encryptedKey;

                using (Aes aesAlg = Aes.Create())
                {
                    encryptedMessage = EncryptStringToBytes_Aes(TextBoxMessage.Text, aesAlg.Key, aesAlg.IV);   

                    var keyVi = new Byte[0].Concat(aesAlg.Key).Concat(aesAlg.IV).ToArray();
                    encryptedKey = rsa.Encrypt(keyVi, false);
                }

                var message = new SingleMessage()
                {
                    Time = DateTime.Now,
                    FromProperty = userName,
                    To = TextBoxTo.Text,
                    Title = TextBoxTitle.Text,
                    Message = encryptedMessage,
                    Key = encryptedKey
                };

                client.ServerOperations.PostUpload(message);
            }

            this.Close();

        }

        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
