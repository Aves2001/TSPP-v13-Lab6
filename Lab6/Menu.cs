using System;

namespace Lab6
{
    class Menu
    {
        public static void Main(string[] args)
        {
            string FILE; // для збереження імя файла з яким потрібно працювати
            if (args.Length != 1) // якщо передали більше одного, або жодного файла, працює з стандартними іменами файлів
            {
                FILE = null;
            }
            else // інакше працює з іншим файлом
            {
                FILE = args[0];
            }

            char task; // для збереження натиснутої клавіші
            do
            {
                Console.WriteLine("Виберіть завдання для запуску:\n");

                Console.WriteLine("[1] --- Завдання 1");
                Console.WriteLine("[2] --- Завдання 2");

                Console.WriteLine("\n[ESC] --- Вихід\n");

                Console.Write(">");
                task = Console.ReadKey(true).KeyChar; // присвоєння натиснутої клавіші
                Console.Clear(); // очистка консолі
                switch (task)
                {
                    case (char)ConsoleKey.Escape: // якщо нажали ESC виходить з програми
                        break;

                    case '1': // якщо нажали 1 запускає перше завдання, та по завершенню його роботи очищає консоль
                        new Task1.Program().Main(FILE);
                        Console.Clear();
                        break;

                    case '2': // якщо нажали 2 -||-
                        new Task2.Program().Main(FILE);
                        Console.Clear();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("\nПомилка: Такого пункта немає\n\n");
                        break;
                }
            } while (task != (char)ConsoleKey.Escape); // якщо нажали ESC виходить з програми
        }
    }
}
