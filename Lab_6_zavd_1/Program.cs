using System;

namespace Lab_6_zavd_1
{
    public interface IMissingProduction
    {
        int Compare(int planproductivity, int realproductivity);
    }
    class ResultsOfWorking : IMissingProduction
    {
        private string Month;
        private int PlannedProductivity;
        private int Productivity;
        public ResultsOfWorking()
        {
            Month = "May";
            PlannedProductivity = 123;
            Productivity = 100;
        }
        public ResultsOfWorking(string s)
        {
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            string[] str = s.Split(' ');
            foreach (char ch in str[0])
            {
                if (char.IsDigit(ch))
                {
                    throw new System.Exception("Неправильно введена назва місяця!");
                }
            }
            Month = str[0];
            PlannedProductivity = int.Parse(str[1]);
            Productivity = int.Parse(str[2]);
            if (PlannedProductivity < 0 || Productivity < 0) throw new FormatException();


        }
        public int Compare(int planproductivity, int realproductivity)
        {
            return (planproductivity - realproductivity);
        }
        public string month
        {
            get { return Month; }
            set { Month = value; }
        }
        public int plannedProductivity
        {
            get { return PlannedProductivity; }
            set { PlannedProductivity = value; }
        }
        public int productivity
        {
            get { return Productivity; }
            set { Productivity = value; }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ResultsOfWorking[] dbase = new ResultsOfWorking[12];
        retry:
            try
            {
                Console.WriteLine("Ввведiть таблицю про результати роботи пiдприємства протягом року у форматi:\nМiсяць    План випуску продукцiї   Фактичний випуск продукцiї");
                string str;
                int i = 0;
                while (i < 12)
                {
                    str = Console.ReadLine();
                    dbase[i] = new ResultsOfWorking(str);
                    i++;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nНедопустиме значення продуктивностi!");
                Console.WriteLine("Спробуйте ще раз!\n");
                goto retry;
            }
            catch (System.Exception)
            {
                Console.WriteLine("\nНеправильно введена назва місяця!");
                Console.WriteLine("Спробуйте ще раз!\n");
                goto retry;
            }
            int j = 0, riznycia = 0;
            Console.WriteLine("\nМiсяцi з недовиконанням плану випуску продукцiї:\nМiсяць    План випуску продукцiї   Фактичний випуск продукцiї   Недостача продукцiї");
            var results = new ResultsOfWorking();
            while (j < 12)
            {
                riznycia = results.Compare(dbase[j].plannedProductivity, dbase[j].productivity);
                if (riznycia > 0)
                {
                    Console.WriteLine($"{dbase[j].month,-10} {dbase[j].plannedProductivity,15} {dbase[j].productivity,25} {riznycia,25}");
                }
                j++;
            }
        }
    }
}
