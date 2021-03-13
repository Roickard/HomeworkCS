using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayException
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
        public static DateTime ParseDate(string message = null)
        {
            DateTime value;
            bool failedParse;

            do
            {
                int year = Parser.ParseInt(message + "\n\nBirthday year: ");
                int month = Parser.ParseInt(message + "\n\nBirthday month: ");
                int day = Parser.ParseInt(message + "\n\nBirthday day: ");

                string tempYear = "";
                for(int i = 3; i >= 0; --i)
                {
                    if (Convert.ToInt32((year / Math.Pow(10, i))) == 0)
                    {
                        tempYear += "0";
                    }
                    else
                    {
                        tempYear += Convert.ToInt32((Convert.ToInt32(year / Math.Pow(10, i)) % 10)).ToString();
                    }
                }
                string date = day.ToString() + "." + month.ToString() + "." + tempYear;

                failedParse = !(DateTime.TryParse(date, out value));
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
    class Birthday
    {
        private DateTime birthdayDate;

        public Birthday()
        {
            birthdayDate = new DateTime();
        }

        public void SetBithdayDate()
        {
            birthdayDate = Parser.ParseDate("Birthday date.");            
        }
        
        public int GetAge()
        {
            DateTime currentDate = DateTime.Today;

            if (currentDate < birthdayDate) throw new Exception("Current year less than birthday year.");

            DateTime age = new DateTime();
            age = currentDate;

            age = age.AddDays(-birthdayDate.Day);
            age = age.AddMonths(-birthdayDate.Month);
            age = age.AddYears(-birthdayDate.Year);

            return age.Year;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            Birthday birthday = new Birthday();
            birthday.SetBithdayDate();

            Console.Clear();
            try
            {
                Console.WriteLine("Age is {0}.", birthday.GetAge());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Write("Press any key...");
            Console.ReadKey();
        }
    }
}