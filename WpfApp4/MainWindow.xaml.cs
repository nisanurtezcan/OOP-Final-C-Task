using System;
using System.IO;
using System.Windows;
using static WpfApp4.AESClass;
using static WpfApp4.BruteForceClass;
using static WpfApp4.PassSaltClass;

namespace WpfApp4
{
    public partial class MainWindow : Window
    {
        private string encryptedText;
        private string originalText;
        private DateTime startTime;
        private string password = PassSaltClass.MyPassword();
        private string salt = PassSaltClass.MySalt();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            originalText = InputTextBox.Text;
            startTime = DateTime.Now;

            if (!string.IsNullOrEmpty(originalText))
            {
                encryptedText = AESClass.EncryptString_Aes(originalText, password, salt);
                EncryptionStatusTextBlock.Text = encryptedText;
                File.WriteAllText("enc_text.txt", encryptedText);
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            string viewTxt = "";
            viewTxt = File.ReadAllText("enc_text.txt");

            EncryptedTextBox.Text = viewTxt;

            EncryptedPasswordLabel.Visibility = Visibility.Visible;
            EncryptedTextBox.Visibility = Visibility.Visible;

        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            var decryptWindow = new DecryptWindow();
            decryptWindow.Show();
        }
    }
}

