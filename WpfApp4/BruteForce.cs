using System;
using System.Linq;
using static WpfApp4.PassSaltClass;
using static WpfApp4.AESClass;
using System.Windows;

namespace WpfApp4
{
    public class BruteForceClass
    {
        private string result = "";
        private string decResult = "";
        private bool isMatched = false;
        private int charactersToTestLength = 0;
        private long computedKeys = 0;

        private char[] charactersToTest =
        {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'
    };

        public void StartBruteForce(string encryptedText, string password)
        {
            var timeStarted = DateTime.Now;

            // The length of the array is stored permanently during runtime
            charactersToTestLength = charactersToTest.Length;

            // Since we know the password length is 4, we set it directly
            int passwordLength = 6;

            isMatched = StartBruteForceIteration(passwordLength, encryptedText, password);

            MessageBox.Show($"Start BruteForce: {timeStarted.ToString()}\n" +
                                $"Password matched: {DateTime.Now.ToString()}\n" +
                                $"Computed keys: {computedKeys}\n" +
                                $"Text Decrypted by BFA: {decResult}\n" +
                                $"Salt From Brute Force Algorithm: {result}\n" +
                                $"Time Taken: {DateTime.Now.Subtract(timeStarted).TotalSeconds:F3} seconds",
                                "Decryption Completed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
        }

        private bool StartBruteForceIteration(int keyLength, string encryptedText, string password)
        {
            var keyChars = CreateCharArray(keyLength, charactersToTest[0]);
            // The index of the last character will be stored for slight performance improvement
            var indexOfLastChar = keyLength - 1;
            return CreateNewKey(0, keyChars, keyLength, indexOfLastChar, encryptedText, password);
        }

        private char[] CreateCharArray(int length, char defaultChar)
        {
            return (from c in new char[length] select defaultChar).ToArray();
        }

        private bool CreateNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar, string encryptedText, string password)
        {
            var nextCharPosition = currentCharPosition + 1;

            for (int i = 0; i < charactersToTestLength; i++)
            {
                keyChars[currentCharPosition] = charactersToTest[i];

                if (currentCharPosition < indexOfLastChar)
                {
                    if (CreateNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar, encryptedText, password))
                    {
                        return true;
                    }
                }
                else
                {
                    computedKeys++;

                    string key = new string(keyChars);
                    string padKey = key.PadRight(8, '0');

                    if (padKey == PassSaltClass.MySalt())
                    {
                        string decryptedText = AESClass.DecryptString_Aes(encryptedText, password, padKey);

                        // Decryption succeeded, return the key and decrypted text
                        isMatched = true;
                        decResult = decryptedText;
                        result = padKey;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}