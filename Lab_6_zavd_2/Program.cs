using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_6_zavd_2
{
    public interface IAverageMax
    {
        abstract void AverageOfParticipants(Meeting[] m);
        abstract void MaxParticipants(Meeting[] m);
    }
    public abstract class Conference
    {
        private string _name;
        private string _place;
        public string Name { get => _name; set => _name = value; }
        public string Place { get => _place; set => _place = value; }
        public Conference()
        {
            _name = "Не задано.";
            _place = "Не задано.";
        }
        public abstract void LengthOfName(Meeting[] c);

    }
    public class Meeting : Conference, IAverageMax
    {
        public DateTime _dateOfMeeting;
        public string _theme;
        public int _numOfParticipants;
        public DateTime DateOfMeeting { get => _dateOfMeeting; set => _dateOfMeeting = value; }
        public string Theme { get => _theme; set => _theme = value; }
        public int NumOfParticipants { get => _numOfParticipants; set => _numOfParticipants = value; }
        public Meeting()
        {
            _dateOfMeeting = new DateTime(2020, 06, 04);
            _theme = "Не задано";
            _numOfParticipants = 0;
        }
        public Meeting(string s)
        {
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            string[] str = s.Split(' ');
            Name = str[0];
            Place = str[1];
            string[] data = str[2].Split('.', '/');
            int day = int.Parse(data[0]);
            int month = int.Parse(data[1]);
            int year = int.Parse(data[2]);
            DateOfMeeting = new DateTime(year, month, day);
            Theme = str[3];
            NumOfParticipants = int.Parse(str[4]);
        }
        public override void LengthOfName(Meeting[] c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                Console.WriteLine("Довжина назви конференцiї: " + c[i].Name + ", складає " + c[i].Name.Length + " символiв.");
            }
        }
        public void AverageOfParticipants(Meeting[] m)
        {
            string n;
            int average = 0, lich = 1;
            List<string> bylo = new List<string>();
            Console.WriteLine("Середня кiлькiсть учаснiкiв на засiданнi:\n");
            for (int i = 0; i < m.Length; i++)
            {
                if (!bylo.Contains(m[i].Name))
                {
                    n = m[i].Name;
                    bylo.Add(m[i].Name);
                    average = m[i].NumOfParticipants;
                    if (i != m.Length)
                    {
                        for (int j = i + 1; j < m.Length; j++)
                        {
                            if (n == m[j].Name)
                            {
                                average += m[j].NumOfParticipants;
                                lich += 1;
                            }
                        }
                    }
                    Console.WriteLine(m[i].Name + ": " + average / lich + " учасникiв;");
                    lich = 1;
                }
            }
        }
        public void MaxParticipants(Meeting[] m)
        {
            int imax = 0, maxParticipants = m[0].NumOfParticipants;
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i].NumOfParticipants > maxParticipants)
                {
                    imax = i;
                    maxParticipants = m[i].NumOfParticipants;
                }
            }
            Console.WriteLine("Максимальна кiлькiсть учасникiв на засiданнi: " + m[imax].NumOfParticipants + ", це було на засiданнi пiд назвою: " + m[imax].Name);
        }

    }
    public class Program
    {
        public static void AddInFile(string s)
        {
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
            string[] str = s.Split(' ');
            StreamWriter file = new StreamWriter(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt", true);
            file.Write($"{str[0],-10} {str[1],15} {str[2],25} {str[3],35} {str[4],25}");
            file.Write(Environment.NewLine);
            file.Close();
        }
        public static void RemoveRecords(int n)
        {
            List<string> quotelist = File.ReadAllLines(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt").ToList();
            quotelist.RemoveAt(n - 1);
            File.WriteAllLines(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt", quotelist.ToArray());
        }
        public static Meeting[] UpdateBasa()
        {
            int k = 0, i = 0;
            StreamReader file = new StreamReader(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                k++;
            }
            file.BaseStream.Position = 0;
            Meeting[] basa = new Meeting[k];
            try
            {

                while ((line = file.ReadLine()) != null)
                {
                    basa[i] = new Meeting(line);
                    k++;
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
            }
            file.Close();
            return basa;
        }
        public static void Search(Meeting[] d)
        {
            Console.WriteLine("Введiть назву зустрiчi: ");
            string name = Console.ReadLine();
            Console.WriteLine("Ось що нам вдалося знайти: ");
            int n = 0;
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i].Name == name)
                {
                    Console.WriteLine($"{d[i].Name,-10} {d[i].Place,15} {d[i].DateOfMeeting.ToShortDateString(),25} {d[i].Theme,35} {d[i].NumOfParticipants,25}");
                    n++;
                }

            }
            if (n == 0) Console.WriteLine("Упс, за вашим запитом не вдалося нiчого знайти, перевiрте правильнiсть написання назви, або спробуйте iншу назву.");
        }
        public static void Edit(Meeting[] d)
        {
        askLine:
            Console.Write("Введiть номер рядка, в якому хочете щось змiнити: ");
            int k = (int.Parse(Console.ReadLine())) - 1;
            if (k > d.Length)
            {
                Console.WriteLine("Такого рядка не iснує. Спробуйте iнший");
                goto askLine;
            }
            Console.WriteLine("Введiть номер поля, яке хочете змiнити. Наприклад: Name(1), Place(2), Date(3), Topic(4), Number of participants(5)");
            int pole = int.Parse(Console.ReadLine());
        retry:
            Console.Write("Введiть нове значення: ");
            string val = Console.ReadLine();
            try
            {
                switch (pole)
                {
                    case 1: d[k].Name = val; break;
                    case 2: d[k].Place = val; break;
                    case 3:
                        string[] data = val.Split('.', '/');
                        int day = int.Parse(data[0]);
                        int month = int.Parse(data[1]);
                        int year = int.Parse(data[2]);
                        d[k].DateOfMeeting = new DateTime(year, month, day);
                        break;
                    case 4: d[k].Theme = val; break;
                    case 5: d[k].NumOfParticipants = int.Parse(val); break;

                    default: Console.WriteLine("Поля з таким номером не iснує!"); break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e);
                goto retry;
            }
        }
        public static void UpdateFile(Meeting[] d)
        {
            StreamWriter file = new StreamWriter(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt");
            for (int i = 0; i < d.Length; i++)
            {
                file.Write($"{d[i].Name,-10} {d[i].Place,15} {d[i].DateOfMeeting.ToShortDateString(),25} {d[i].Theme,35} {d[i].NumOfParticipants,25}");
                file.Write(Environment.NewLine);
            }
            file.Close();
        }

        public static void ShowFile()
        {
            StreamReader file = new StreamReader(@"D:\ООП\Lab_5\Lab_5_zavd_1\Conference.txt");
            string show = file.ReadToEnd();
            if (show.Length == 0)
            {
                Console.WriteLine("Упс, файл пустий.");
            }
            else Console.WriteLine(show);
            file.Close();
        }
        static void Main(string[] args)
        {
            string str;
            char check;
            int n;
            Console.WriteLine("Меню програми:\nДодавання записів - a\nРедагування записів - e\nЗнищення записів - f\nВиведення інформації з файлу на екран - s\nСередня кількість учасників на засіданнях - A\nЗасідання з найбільшою кількістю учасників - M\nДовжина назви - L\nВихід з програми - q");
            var meetTask = new Meeting();
            do
            {
            userCheck:
                Console.Write("\nВведiть команду: ");
                try
                {
                    check = char.Parse(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Неправильна команда. Спробуйте ще раз.");
                    goto userCheck;
                }
                Meeting[] meet = UpdateBasa();
                if (check == 'a')
                {
                AddZapis:
                    Console.WriteLine("Введiть новий запис до бази даних за принципом:\nНазва засiдання     Мiсце проведення      Дата проведення      Тема       Кiлькiсть учасникiв");
                    try
                    {
                        AddInFile(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Помилка: " + e.Message);
                        goto AddZapis;
                    }
                }
                else if (check == 'e')
                {
                    Edit(meet);
                    UpdateFile(meet);
                }
                else if (check == 'f')
                {
                    Console.Write("Введiть номер рядка який хочете видалити: ");
                    n = int.Parse(Console.ReadLine());
                    RemoveRecords(n);
                    meet = UpdateBasa();
                }
                else if (check == 's')
                {
                    Console.WriteLine($"{"Назва",-10} {"Місце проведення",15} {"Дата",25} {"Тема засідання",35} {"Кількість учасників",25}");
                    ShowFile();
                }
                else if (check == 'n')
                {
                    Search(meet);
                }
                else if (check == 'A')
                {
                    meetTask.AverageOfParticipants(meet);
                }
                else if (check == 'L')
                {
                    meetTask.LengthOfName(meet);
                }
                else if (check == 'M')
                {
                    meetTask.MaxParticipants(meet);
                }
                else
                {
                    if (check == 'q') Console.WriteLine("Програма завершена.");
                    else Console.WriteLine("На жаль, такої команди немає, спробуйте iншу.");
                }
            } while (check != 'q');
        }
    }
}
