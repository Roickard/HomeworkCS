using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework
{
    class Parser
    {
        static private string message;

        public static int ParseInt(string message = null)
        {
            Parser.message = message;
            int value;
            bool failedParse;

            do
            {
                string stringToParse = GetUnparsedValue();
                failedParse = !(int.TryParse(stringToParse, out value));
            }
            while (failedParse == true);

            return value;
        }
        public static double ParseDouble(string message = null)
        {
            Parser.message = message;
            double value;
            bool failedParse;

            do
            {
                string stringToParse = GetUnparsedValue();
                failedParse = !(double.TryParse(stringToParse, out value));
            }
            while (failedParse == true);

            return value;
        }
        public static bool ParseBool(string message = null)
        {
            Parser.message = message;
            bool value;
            bool failedParse;

            do
            {
                string stringToParse = GetUnparsedValue();
                failedParse = !(bool.TryParse(stringToParse, out value));
                if (failedParse)
                {
                    if (int.TryParse(stringToParse, out int temp) && (temp == 0 || temp == 1))
                    {
                        value = Convert.ToBoolean(temp);
                        failedParse = false;
                    }
                }
            }
            while (failedParse == true);

            return value;
        }
        private static string GetUnparsedValue()
        {
            Console.Clear();
            Console.Write(message);

            string stringForParse = Console.ReadLine();

            return stringForParse;
        }
    }
    static class CharExtention
    {
        public static int GetIndex(this char[] value, char letter)
        {
            for (int index = 0; index < value.Length; ++index)
            {
                if (value[index] == letter)
                {
                    return index;
                }
            }
            throw new Exception("The value does not constains the given letter.");
        }
        public static char GetLetter(this char[] value, int letterIndex)
        {
            if (value.Length <= letterIndex)
            {
                throw new Exception("The value does not constains the given letter.");
            }
            return value[letterIndex];
        }
    }
    class PolybiusSquare : Exception
    {
        private char[] alphabet;

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string Message { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        private string encryptedMessage;
        public string EncryptedMessage { get => encryptedMessage; }
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\EncryptedMessage.txt";
        public string Path { get => path; set => path = value; }

        public PolybiusSquare()
        {
            alphabet = new char[25];
            FillAlphabet();
        }
        private void FillAlphabet()
        {
            for (int alphabetIndex = 0, letter = Convert.ToInt32('A'); alphabetIndex < 25; ++alphabetIndex, ++letter)
            {
                if (letter == Convert.ToInt32('J'))
                {
                    --alphabetIndex;
                    continue;
                }
                alphabet[alphabetIndex] = Convert.ToChar(letter);
            }
        }
        public void TryEncryptMessage()
        {
            Message = Message.ToUpper();
            for(int index = 0; index < Message.Length; ++index)
            {
                if (Message[index] < 'A' || Message[index] > 'Z')
                {
                    throw new Exception("Incorrect value: the message contains forbidden characters.");
                }
            }
            EncryptMessage();
        }
        private void EncryptMessage()
        {
            encryptedMessage = "";
            for (int messageIndex = 0; messageIndex < Message.Length; ++messageIndex)
            {
                int letterIndex = alphabet.GetIndex(Message[messageIndex]);
                encryptedMessage += (letterIndex / 5 + 1).ToString() + (letterIndex % 5 + 1).ToString();
            }
        }
        public void WriteEncryptedMessage()
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(path))
                {
                    stream.Write(encryptedMessage);
                }
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
            
        }
        public void ReadEncryptedMessage()
        {
            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    encryptedMessage = stream.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void TryDecryptMessage()
        {
            if (encryptedMessage.Length % 2 != 0)
            {
                throw new Exception("Incorrect value: the length of encrypted message in invalid.");
            }

            for (int index = 0; index < encryptedMessage.Length; ++index)
            {
                int temp =  int.Parse(encryptedMessage[index].ToString());
                if (temp < 1 || temp > 5)
                {
                    throw new Exception("Incorrect value: the encrypted message has invalid format.");
                }
            }
            DecryptMessage();
        }
        private void DecryptMessage()
        {
            Message = "";
            for(int index = 0; index < encryptedMessage.Length; index += 2)
            {
                int temp = int.Parse(encryptedMessage[index].ToString() + encryptedMessage[index + 1].ToString());
                int letterIndex = (temp / 10 - 1) * 5 + (temp % 10 - 1);
                Message += alphabet.GetLetter(letterIndex);
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            PolybiusSquare polybiusSquare = new PolybiusSquare();

            do {
                Console.Clear();
                int modeCode = Parser.ParseInt("Chose mode.\n" +
                    "1. Read encrypted message from file.\n" +
                    "2. Write encrypted message to file.\n" +
                    "3. Set message.\n" +
                    "4. Encrypt message.\n" +
                    "5. Decrypt message.\n" +
                    "6. Print message.\n" +
                    "7. Print encrypted messgae.\n" +
                    "8. Exit.\n\n" +
                    "Enter: ");
                try
                {
                    Console.Clear();
                    switch (modeCode)
                    {
                        case 1: polybiusSquare.ReadEncryptedMessage(); Console.WriteLine("File path: " + polybiusSquare.Path); break;
                        case 2: polybiusSquare.WriteEncryptedMessage(); Console.WriteLine("File path: " + polybiusSquare.Path); break;
                        case 3: Console.Write("Enter message: "); polybiusSquare.Message = Console.ReadLine(); break;
                        case 4: polybiusSquare.TryEncryptMessage(); Console.WriteLine("The message was successfully encrypted."); break;
                        case 5: polybiusSquare.TryDecryptMessage(); Console.WriteLine("The message was successfully decrypted."); break;
                        case 6: Console.WriteLine("The message: " + polybiusSquare.Message); break;
                        case 7: Console.WriteLine("The encrypted message: " + polybiusSquare.EncryptedMessage); break;
                        case 8: return;
                        default: Console.WriteLine("Incorrect value."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } while (true);
        }
    }
}
