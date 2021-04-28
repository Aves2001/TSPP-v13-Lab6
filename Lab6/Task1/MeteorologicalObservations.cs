using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6.Task1
{
    public class MeteorologicalObservations : IMeteorologicalObservations
    {
        public static string file_name = "Meteorological_Observations.txt";  // для збереження імені файла з яким потрібно працювати
        private string date;
        private string temperature;
        private string atmosphericPressure;
        public static string FILE_NAME
        {
            get => file_name;
            set
            {
                if (value.Length > 150)
                {
                    throw new Exception("Кількість символів в назві файла не може перевищювати 150 символів.\nВказана назва файла: " + value);
                }
                file_name = value;
            }
        }
        public string Date
        {
            get => date;
            set
            {
                TestIsDate(value);
                date = value;
            }
        }
        public string Temperature
        {
            get => temperature;
            set 
            {
                TestIsNumber(value, "Невірно введено температуру, використано заборонені символи");
                temperature = value; 
            }
        }
        public string AtmosphericPressure
        {
            get => atmosphericPressure;
            set
            {
                TestIsNumber(value, "Невірно введено атмосферний тиск, використано заборонені символи");
                atmosphericPressure = value;
            }
        }

        public MeteorologicalObservations()
        {
            Date = "01.01.0001";
            Temperature = "0";
            AtmosphericPressure = "0";
        }

        public MeteorologicalObservations(string[] x)
        {
            Date = x[0];
            Temperature = x[1];
            AtmosphericPressure = x[2];
        }

        public List<MeteorologicalObservations> Input()
        {
            List<MeteorologicalObservations> metObs = new List<MeteorologicalObservations>();
            string s; // для збереження рядка з бази даних
            int i = 0; // індекс для масива
            using (StreamReader f = new StreamReader(FILE_NAME))
            {
                while ((s = f.ReadLine()) != null) // запис рядка з бази в масив
                {
                    // Створення масива "х" в якому зберігаються дані студента
                    string[] x = s.Split(';').Select(tag => tag.Trim()).ToArray();

                    if (x.Length != 3) // Якщо вказані не всі дані, або їх забагато, повідомляє як має бути оформлена база
                    {
                        throw new Exception("Дані в базі мають бути в такому форматі: \n" +
                        "         дата, температура, атмосферний тиск \n");
                    }

                    metObs.Add(new MeteorologicalObservations(x));
                    ++i;
                }
                f.Close(); // закриття файла
            }
            return metObs;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            const int size = -12;
            return string.Format($"{"| "}{Date,size} {"| "}{Temperature + "°",size} {"| "}{AtmosphericPressure + "мм",size}|\n" +
                                 "=============================================");
        }

        // порівняння дати
        public int CompareDate(MeteorologicalObservations a, MeteorologicalObservations b)
        {
            return a.Date.CompareTo(b.Date);
        }

        // порівняння атмосферного тиску
        public int CompareAtmosphericPressure(MeteorologicalObservations a, MeteorologicalObservations b)
        {
            return a.AtmosphericPressure.CompareTo(b.AtmosphericPressure);
        }

        public void ViewArray(MeteorologicalObservations[] metObs)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("|     Дата     |  Температура |     Тиск    |");
            Console.WriteLine("=============================================");
            foreach (MeteorologicalObservations item in metObs)
            {
                Console.WriteLine(item);
            }
        }

        // Повертає два дні з найбільшим перепадом тиску
        public MeteorologicalObservations[] TheLargestPressureDrop(MeteorologicalObservations[] metObs)
        {
            // Сортування масива по даті
            Array.Sort(metObs, CompareDate);

            // Max - для збереження двох днів з найбільшим перепадом тиску
            MeteorologicalObservations[] Max = new MeteorologicalObservations[2];

            // х - масив в якому зберігаються дані про перепади тиску на кожні два дні
            double[] x = new double[metObs.Length - 1];

            // цикл в якому тиск сьогодні віднімається від тиску завтра і так далі, результат зберігається з додатнім знаком
            for (int i = 0; i < x.Length; i++)
            {
                double tmp = double.Parse(metObs[i].AtmosphericPressure) - double.Parse(metObs[1 + i].AtmosphericPressure);
                if (tmp < 0)
                {
                    x[i] = -tmp;
                }
                else
                {
                    x[i] = tmp;
                }
            }
            //for (int i = 0; i < x.Length; i++)
            //{
            //    Console.WriteLine("{0,2} = {1}", 01 + i, x[i]);
            //}
            double max = x.Max(); // знаходження максимального перепаду тиску (це буде перший день)
            int index = Array.IndexOf(x, max);// збереження індекса де він знаходиться в масиві (x)
            Max[0] = metObs[index];      // збереження першого
            Max[1] = metObs[1 + index]; // та другого дня, з найбільшим перепадом тиску

            return Max;
        }

        public void TestIsDate(string s)
        {
            if (!DateTime.TryParseExact(s, "dd.mm.yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                Console.WriteLine("Причина помилки: " + s);
                throw new Exception("Невірний формат дати\n" +
                                    "Дата має бути у форматі: \"dd.mm.yyyy\"");
            }
        }

        public void TestIsNumber(string str, string ErrorMessage)
        {
            // Перевірка чи str є числом, і якщо не є то створює виключення з текстом ErrorMessage
            if (!double.TryParse(str, out _))
            {
                Console.WriteLine("Причина помилки: \"{0}\"", str);
                throw new Exception(ErrorMessage);
            }
        }
    }
}
