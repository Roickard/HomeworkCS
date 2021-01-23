using System;

namespace Homework2
{
    
    class Parser
    {
        static private string message;

        public static Int64 ParseInt(string message = null)
        {
            Parser.message = message;
            Int64 value;
            bool failedParse;

            do
            {
                string stringToParse = GetUnparsedValue();
                failedParse = !(Int64.TryParse(stringToParse, out value));
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
    class Quadratic
    {
        private static double a;
        private static double b;
        private static double c;
        private static double[] x;

        private static string answer;

        public static double[] X { get => x; }
        public static string Answer { get => answer;}

        static Quadratic()
        {
            x = new double[2];
            x[0] = 0.0f;
            x[1] = 0.0f;
            a = 0.0f;
            b = 0.0f;
            c = 0.0f;
            answer = null;
        }

        public static void SetValues()
        {
            a = Parser.ParseDouble("Set a: ");
            b = Parser.ParseDouble("Set b: ");
            c = Parser.ParseDouble("Set c: ");
        }       

        public static bool TryCalculate()
        {
            if (a != 0.0f)
            {
                return Calculate();
            }
            if (a == 0.0f)
            {
                if (b == 0.0f)
                {
                    answer = "Unsolvable equation.";
                    return false;
                }
                answer = "Non-square equation.";
                return false;
            }

            return false;
        }
        public static bool TryCalculate(double a, double b, double c)
        {
            Quadratic.a = a;
            Quadratic.b = b;
            Quadratic.c = c;

            return TryCalculate();
        }

        private static bool Calculate()
        {
            double D = Math.Pow(b, 2.0f) - 4.0f * a * c;

            if (D < 0)
            {
                answer = "The case of complex roots.";
                return false;
            }
            else if (D == 0.0f)
            {
                x[0] = (-b) / (2.0f * a);
                x[1] = x[0];
                answer = "x1 = x2 = " + x[0].ToString();
            }
            else if (D > 0.0f)
            {
                x[0] = (-b + Math.Sqrt(D)) / (2.0f * a);
                x[1] = (-b - Math.Sqrt(D)) / (2.0f * a);
                answer = "x1 = " + x[0].ToString() + "\tx2 = " + x[1].ToString();
            }

            return true;
        }

        public static string TestCase(double a, double b, double c)
        {
            Quadratic.a = a;
            Quadratic.b = b;
            Quadratic.c = c;
            return TryCalculate().ToString() + '\t' + answer;
        }
    }
    class String
    {
        private string value;

        public string Value { get => value; set => this.value = value; }
        public String()
        {
        }
        public String(string mainString)
        {
            value = mainString;
        }
        public static int operator ^(String firstString, String secondString)
        {
            return GetContainsValue(firstString.value, secondString.value);
        }
        public static implicit operator String(string mainString)
        {
            return new String(mainString);
        }
        public static int GetContainsValue(string firstString, string secondString)
        {
            if (firstString.Contains(secondString) == false || firstString.Length < secondString.Length)
            {
                return 0;
            }

            int value = 0;

            for (int i = 0; i <= firstString.Length - secondString.Length; ++i)
            {
                bool contains = true;

                for (int j = 0; j < secondString.Length; ++j)
                {
                    if (firstString[i + j] != secondString[j])
                    {
                        contains = false;
                        break;
                    }
                }
                if (contains == true)
                {
                    ++value;
                }
            }

            return value;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Clear();

            String firstString = new String("абвгабвг");
            String secondString = new String("аб");
            Console.WriteLine(firstString ^ secondString);

            firstString = "стстсап";
            secondString = "стс";
            Console.WriteLine(firstString ^ secondString);

            firstString.Value = "гещавыгавщеывав";
            secondString.Value = "ав";
            Console.WriteLine(firstString ^ secondString);

            firstString.Value = "пывземйкошоземдждмязем";
            secondString.Value = "зем";
            Console.WriteLine(firstString ^ secondString);

            firstString.Value = "фвафг";
            secondString.Value = "фпывапш";
            Console.WriteLine(firstString ^ secondString);

            Console.WriteLine(Quadratic.TestCase(2.0f, -5.0f, 2.0f));
            Console.WriteLine(Quadratic.TestCase(3.0f, 2.0f, 5.0f));
            Console.WriteLine(Quadratic.TestCase(3.0f, -12.0f, 0.0f));
            Console.WriteLine(Quadratic.TestCase(0.0f, 0.0f, 10.0f));
            Console.WriteLine(Quadratic.TestCase(0.0f, 0.0f, 0.0f));
            Console.WriteLine(Quadratic.TestCase(0.0f, 5.0f, 17.0f));
            Console.WriteLine(Quadratic.TestCase(9.0f, 0.0f, 0.0f));

            Console.ReadKey();
        }
    }
}