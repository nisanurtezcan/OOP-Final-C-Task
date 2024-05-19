using System;
using System.IO;
using System.Windows;

namespace WpfApp4
{
    public partial class DecryptWindow : Window
    {
        private static readonly string password = PassSaltClass.MyPassword();
        private static readonly string salt = PassSaltClass.MySalt();

        public DecryptWindow()
        {
            InitializeComponent();
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            var encryptedInput = DecryptTextBox.Text;
            var startTime = DateTime.Now;

            BruteForceClass bruteForce = new BruteForceClass();
            bruteForce.StartBruteForce(encryptedInput, password);
        }

        private void SaveToFile(string encryptedText, string salt, string decryptedText)
        {
            string filePath = "decryption_log.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Encrypted Data: {encryptedText}");
                writer.WriteLine($"Salt: {salt}");
                writer.WriteLine($"Decrypted Data: {decryptedText}");
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine(new string('-', 50));
            }
        }
    }
}