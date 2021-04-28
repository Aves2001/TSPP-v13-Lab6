using System;
using System.IO;

namespace Lab6.Task2
{
    class Program
    {
        /// <summary>
        /// Головне меню другого завдання
        /// </summary>
        /// <param name="FILE"></param>
        public void Main(string FILE)
        {
            char x = (char)ConsoleKey.Escape; // для збереження натиснутої клавіші
            do
            {
                TouringTrip trip = new TouringTrip();
                try
                {
                    if (FILE != null)
                    {
                        MusicBand.FILE_NAME = FILE;
                    }
                    do
                    {
                        Console.WriteLine("\nГарячі клавіші:" +
                                        "\t[Д] -------- додавання записів\n" +
                                      "\t\t[Р] -------- редагування записів\n" +
                                      "\t\t[Delete] --- знищення записів\n" +
                                      "\t\t[В] -------- виведення інформації з файла на екран\n" +
                                      "\t\t[М] -------- гастрольна поїздка з максимальною кількістю концертів\n" +
                                      "\t\t[C] -------- список гастрольних поїздок у певне місто\n" +
                                      "\t\t[О] -------- остання літера в прізвищі керівника\n" +
                                      "\t\t[Space] ---- сортування\n" +
                                      "\t\t[ESC] ---- вихід");

                        x = Console.ReadKey(true).KeyChar; // збереження натиснутої клавіші
                        switch (x)
                        {
                            case 'Д':
                            case 'д':
                            case 'L':
                            case 'l':
                                MusicBand.Add(); // додавання записів в кінець файла
                                break;

                            case 'Р':
                            case 'р':
                            case 'H':
                            case 'h':
                                MusicBand.Edit(); // редагування записів
                                Console.Clear();
                                break;

                            case (char)ConsoleKey.Delete:
                                MusicBand.Dell(); // знищення записів
                                Console.Clear();
                                break;

                            case 'В':
                            case 'в':
                            case 'D':
                            case 'd':
                                Console.Clear();
                                MusicBand.ViewTable(true); // виведення інформації з файла на екран
                                break;


                            case 'М':
                            case 'м':
                            case 'V':
                            case 'v':
                                Console.Clear();
                                trip.MaximumNumberConcerts();
                                break;

                            case 'С':
                            case 'с':
                            case 'C':
                            case 'c':
                                trip.CitySearch();
                                break;

                            case 'О':
                            case 'о':
                            case 'J':
                            case 'j':
                                trip.LastLetter();
                                break;

                            case (char)ConsoleKey.Spacebar:
                                MusicBand.Sort();
                                break;

                            default: break;
                        }
                    } while (x != (char)ConsoleKey.Escape); // якщо нажали ESC виходить з програми
                }
                catch (FileNotFoundException) //якщо файл відсутній
                {
                    using (StreamWriter fs = new StreamWriter(MusicBand.FILE_NAME))
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
                    Console.WriteLine(e.StackTrace);
                    _ = Console.ReadKey(true);
                    Console.Clear();
                }
            } while (x != (char)ConsoleKey.Escape);
        }
    }
}
