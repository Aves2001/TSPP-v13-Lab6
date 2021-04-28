using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6.Task2
{
    /// <summary>
    /// Похідний клас від: 
    /// <seealso cref="MusicBand"/> 
    /// та інтерфейса: 
    /// <seealso cref="ITouringTrip"/> 
    /// </summary>
    public class TouringTrip : MusicBand, ITouringTrip
    {
        private string city, year, numberOfConcerts;
        public TouringTrip() : base()
        {
            City = "null";
            Year = "null";
            NumberOfConcerts = "null";
        }

        public TouringTrip(string id, string title, string lastNameOfTheHead, string city, string year, string numberOfConcerts) : base(id, title, lastNameOfTheHead)
        {
            City = city;
            Year = year;
            NumberOfConcerts = numberOfConcerts;
        }

        public string City
        {
            get => city;
            set
            {
                if (TestLengthIsNull(value, cL))
                {
                    city = "null";
                    return;
                }
                city = value;
            }
        }
        public string Year
        {
            get => year;
            set
            {
                if (TestLengthIsNull(value, yL))
                {
                    year = "null";
                    return;
                }
                else if (!TestIsNumber(value))
                {
                    throw new Exception("Рік має бути вказаний числом");
                }
                // перевіряє чи вказаний рік поступання не більший за теперішній і чи не менший за 1000
                if (int.Parse(value) > DateTime.Today.Year)
                {
                    throw new Exception("Рік не може бути більший за теперішній");
                }
                else if (int.Parse(value) < 1000)
                {
                    throw new Exception("Рік не може бути менший за 1000");
                }
                year = value;
            }
        }
        public string NumberOfConcerts
        {
            get => numberOfConcerts;
            set
            {
                if (TestLengthIsNull(value, yL))
                {
                    numberOfConcerts = "null";
                    return;
                }
                else if (!TestIsNumber(value))
                {
                    throw new Exception("\nКількість концертів має бути вказана числом");
                }
                if (int.Parse(value) < 0)
                {
                    throw new Exception("Вказана кількість концертів не може бути меншою нулю");
                }
                numberOfConcerts = value;
            }
        }

        public override string ToString()
        {
            string x = "\n|";
            for (int i = 0; i < tableSize; i++)
            {
                x += "=";
            }
            x += "|";
            return string.Format($"| {Id,iL} | {Title,tL} | {LastNameOfTheHead,lL} | {City,cL} | {Year,yL} | {NumberOfConcerts,nL} |" + x);
        }

        public override string LastLetter(List<TouringTrip> list = null, string str = null)
        {
            if (list == null)
            {
                list = Input();
            }
            int index;
            bool not_found = true;
            do
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    Console.Clear();
                    ViewTable(list);
                    System.Console.WriteLine("Введіть id запису в якому потрібно вивести останню літеру в прізвищі керівника, або [Enter] для виходу");
                    System.Console.Write(">");
                    str = System.Console.ReadLine();
                }
                if (str == null || str.Equals("") || str.Equals("null"))
                {
                    System.Console.Clear();
                    return null;
                }
                index = list.FindIndex(s => s.Id.Equals(str));
                not_found = index == -1;
                if (not_found)
                {
                    System.Console.WriteLine("Таке id не знайдено");
                    return null;
                }
                if (list[index].LastNameOfTheHead.Equals("null"))
                {
                    Console.WriteLine("\nІнформація не вказана!");
                    return null;
                }
            } while (not_found);
            char last = list[index].LastNameOfTheHead.Last();
            Console.WriteLine("Остання літера в прізвищі керівника: " + last);
            return last.ToString();
        }

        public void MaximumNumberConcerts()
        {
            List<TouringTrip> list = Input();
            list = list.Where(s => int.TryParse(s.NumberOfConcerts, out _)).ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Нічого не знайдено!");
                return;
            }
            string max = Convert.ToString(list.Max(s => int.Parse(s.NumberOfConcerts)));

            _ = list.RemoveAll(s => !s.NumberOfConcerts.Equals(max));
            if (list.Count != 0)
            {
                string x;
                if (list.Count == 1)
                {
                    x = "Гастрольна поїздка з максимальною кількістю концертів";
                }
                else
                {
                    x = "Гастрольні поїздки з максимальною кількістю концертів";
                }

                int y = (tableSize - x.Length) / 2;
                string z = "";
                for (int i = 0; i < y; i++) { z += " "; }
                Console.Write(z += x);
                TableCap();
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public List<TouringTrip> CitySearch(List<TouringTrip> list = null, string str = null)
        {
            bool not_exactly = false;
            if (list == null)
            {
                list = Input();
                Console.Write("Введіть місто: \n>");
                str = Console.ReadLine();
                Console.Clear();
            }
            string city = str.ToLower();
            if (city.Length == 0)
            {
                ViewTable();
                Console.WriteLine("\n\"" + str + "\"" + " не знайдено");
                return list;
            }
            list = list.Where(s => s.City
                                    .ToLower()
                                    .Equals(city))
                                    .ToList();
            if (list.Count == 0)
            {
                list = Input();
                list = list.Where(s => s.City
                                        .ToLower()
                                        .Contains(city))
                                        .ToList();
                if (list.Count > 0)
                {
                    not_exactly = true;
                }
            }
            if (list.Count > 0)
            {
                if (not_exactly)
                {
                    Console.WriteLine("\n\"" + str + "\"" + " не знайдено, можливо ви мали наувазі:");
                }
                else
                {
                    Console.WriteLine("\nРезультат для: " + str);
                }
                TableCap();
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
                return list;
            }
            else
            {
                ViewTable();
                Console.WriteLine("\n\"" + str + "\"" + " не знайдено");
                return list;
            }
        }
    }
}
