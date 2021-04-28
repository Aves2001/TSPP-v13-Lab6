using System;
using System.Collections.Generic;
using System.IO;

namespace Lab6.Task1
{
    public class Program
    {
        public void Main(string FILE)
        {
            try
            {
                char x; // для збереження натиснутої клавіші
                if (FILE != null)
                {
                    MeteorologicalObservations.FILE_NAME = FILE;
                }
                MeteorologicalObservations mo = new MeteorologicalObservations();
                Menu();
                do
                {
                    List<MeteorologicalObservations> metObs = mo.Input();

                    x = Console.ReadKey(true).KeyChar; // збереження натиснутої клавіші
                    switch (x)
                    {
                        case 'В':
                        case 'в':
                        case 'D':
                        case 'd':
                            Console.Clear();
                            // Сортування масива за атмосферним тиском
                            metObs.Sort((a, b) => mo.CompareAtmosphericPressure(a, b));

                            Console.WriteLine("\n       Метеорологічні спостереження\n    протягом лютого 2021р. в Чернівцях\n");
                            mo.ViewArray(metObs.ToArray()); // виведення інформації з файла на екран
                            break;


                        case 'М':
                        case 'м':
                        case 'V':
                        case 'v':
                            Console.Clear();
                            Console.WriteLine("\n  Два дні з найбільшим перепадом тиску:");

                            mo.ViewArray
                            (
                                mo.TheLargestPressureDrop(metObs.ToArray())
                            );
                            break;

                        default:
                            Menu();
                            break;
                    }
                } while (x != (char)ConsoleKey.Escape); // якщо нажали ESC виходить з програми
            }
            catch (FileNotFoundException) //якщо файл відсутній
            {
                using (StreamWriter fs = new StreamWriter(MeteorologicalObservations.FILE_NAME))
                {
                    fs.Write("");
                    fs.Close();
                }
                Console.WriteLine("Файл порожній. Для повернення в меню натисніть будьяку клавішу!");
                _ = Console.ReadKey(true);
                Console.Clear();
            }
            catch (Exception e) // виводить причину помилки в консоль
            {
                Console.WriteLine("Помилка: " + e.Message + "\n\nДля повернення в меню натисніть будьяку клавішу!");
                _ = Console.ReadKey(true);
                Console.Clear();
            }
        }
        private void Menu()
        {
            Console.WriteLine("\nГарячі клавіші:" +

                                 "\t[В] -------- виведення інформації з файла на екран\n" +
                                 "\t\t[М] -------- Два дні з найбільшим перепадом тиску\n" +
                                 "\t\t[ESC] ---- вихід");
        }
    }
}
