﻿using client.Models;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public string SERVICE_URL = "http://localhost:9063/";
        private string pubKey;
        private string privateKey;

        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Server GetWebClient(string uri)
        {
            //_log.Info(uri);
            var client = new Server(new Uri(uri), new BasicAuthenticationCredentials());
            return client;
        }

        public void clear()
        {
            pubKey = null;
            privateKey = null;
        }
        public Register()
        {
            InitializeComponent();
        }

        private void ButtonSingUp_Click(object sender, RoutedEventArgs e)
        {
            var webClient = GetWebClient(SERVICE_URL);

            if (String.IsNullOrEmpty(privateKey) || String.IsNullOrEmpty(pubKey))
            {
                using (var rsa = new RSACryptoServiceProvider(512))
                {
                    pubKey = rsa.ToXmlString(false);
                    privateKey = rsa.ToXmlString(true);
                }
            }

            var user = new UserEntry() 
            {
                Name = TextBoxLogin.Text.ToString(),
                Pass = TextBoxPassword.Text.ToString(),
                PubKey = pubKey,
            };

            try
            {
                
                if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Pass))
                {
                    throw new Exception("invalid username or password");
                }
                webClient.ServerOperations.PostRegister(user);
                this.Close();
            }
            catch(HttpOperationException ex)
            {
                LabelInfo.Content = "User name already registered";
            }
            catch(Exception ex)
            {
                LabelInfo.Content = ex.Message;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}