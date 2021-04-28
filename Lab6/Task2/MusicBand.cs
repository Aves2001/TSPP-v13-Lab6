using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6.Task2
{
    /// <summary>
    /// Батьківський (базовий) клас: 
    /// <see langword="Музичний гурт"/>
    /// </summary>
    public abstract class MusicBand
    {
        private static string file_name = "dbase.txt";
        private string id, title, lastNameOfTheHead;
        public const int iL = -4, tL = -25, lL = -25, cL = -15, yL = -4, nL = -20;
        public static int tableSize = (-iL - tL - lL - cL - yL - nL) + (6 * 3) - 1;

        public MusicBand()
        {
            Id = "null";
            Title = "null";
            LastNameOfTheHead = "null";
        }
        public MusicBand(string id, string title, string lastNameOfTheHead)
        {
            Id = id;
            Title = title;
            LastNameOfTheHead = lastNameOfTheHead;
        }

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
        public string Id
        {
            get => id;
            set
            {
                if (value.Contains("bad_id"))
                {
                    throw new Exception("Запись з таким ID уже є в базі");
                }
                if (TestLengthIsNull(value, iL))
                {
                    try
                    {
                        List<TouringTrip> list = Input();
                        int[] a = list.Select(s => int.Parse(s.Id)).ToArray();
                        Array.Sort(a);
                        for (int i = 1; i <= list.Count; i++)
                        {
                            if (i != a[i - 1])
                            {
                                id = Convert.ToString(i);
                                return;
                            }
                        }
                        id = Convert.ToString(list.Count + 1);
                        return;
                    }
                    catch (Exception)
                    {
                        id = "1";
                        return;
                    }
                }
                if (!int.TryParse(value, out _))
                {
                    throw new Exception("ID не число");
                }
                else if (int.Parse(value) < 0)
                {
                    throw new Exception("ID не може бути відємним числом");
                }
                id = value;
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (TestLengthIsNull(value, tL))
                {
                    title = "null";
                    return;
                }
                title = value;
            }
        }
        public string LastNameOfTheHead
        {
            get => lastNameOfTheHead;
            set
            {
                if (value.Length != value.ToCharArray().Where(s => char.IsLetter(s)).Count())
                {
                    throw new Exception("У прізвищі використані заборонені символи");
                }
                else if (TestLengthIsNull(value, lL))
                {
                    lastNameOfTheHead = "null";
                    return;
                }

                lastNameOfTheHead = value;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Запис у файл колекцію гастрольних поїздок
        /// </summary>
        /// <param name="x"> Колекція гастрольних поїздок </param>
        public static void Output(List<TouringTrip> x)
        {
            using StreamWriter f = new StreamWriter(FILE_NAME);
            foreach (TouringTrip item in x)
            {
                f.Write(item.Id + "; ");
                f.Write(item.Title + "; ");
                f.Write(item.LastNameOfTheHead + "; ");
                f.Write(item.City + "; ");
                f.Write(item.Year + "; ");
                f.WriteLine(item.NumberOfConcerts);
            }
            f.Close(); // закриття файла
        }
        public static void Output(TouringTrip x)
        {
            using StreamWriter f = new StreamWriter(FILE_NAME, true);
            f.Write(x.Id + "; ");
            f.Write(x.Title + "; ");
            f.Write(x.LastNameOfTheHead + "; ");
            f.Write(x.City + "; ");
            f.Write(x.Year + "; ");
            f.WriteLine(x.NumberOfConcerts);
            f.Close(); // закриття файла
        }

        public static List<TouringTrip> Input()
        {
            string s; // для збереження рядка з бази даних
            IsFile();
            List<TouringTrip> list = new List<TouringTrip>();
            bool save_edit = false;//якщо були зміни запитує чи потрібно їх бререгти
            bool printFile = false;// якщо є одинакові ID виводить вміст файла 
            using (StreamReader f = new StreamReader(FILE_NAME))
            {
                List<string> bad_id = new List<string>();
                List<string[]> lineFile = new List<string[]>();
                int i = 1;
                bool bad;
                using (StreamReader a = new StreamReader(FILE_NAME))
                {
                    string ss;
                    while ((ss = a.ReadLine()) != null) // запис рядка з бази в масив
                    {
                        lineFile.Add(ss.Split(';').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray());
                    }
                    a.Close();
                }
                while ((s = f.ReadLine()) != null) // запис рядка з бази в масив
                {
                    string[] x = s.Split(';').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    if (x.Length != 0)
                    {
                        bad_id.Add(x[0]);
                    }
                    if (s.Length != 0)
                    {
                        do
                        {
                            bad = false;
                            int index = -1;
                            try
                            {
                                if (bad_id.Where(q => x[0].Equals(q)).Count() > 1) // якщо є дублікат
                                {
                                    if (!printFile) // якщо список ще не робився
                                    {
                                        printFile = true;
                                    }
                                    x[0] += "__bad_id";
                                }
                                list.Add(new TouringTrip(x[0], x[1], x[2], x[3], x[4], x[5]));
                            }
                            catch (IndexOutOfRangeException)
                            {
                                bad = false;
                                save_edit = true;
                                Console.WriteLine("\nПомилка в рядку під номером {0}: \n{1}", i, s);
                                Console.WriteLine("Він буде видалений, для продовження натисніть будьяку клавішу, або [ESC] щоб вийти");
                                if ((char)ConsoleKey.Escape == Console.ReadKey(true).KeyChar)
                                {
                                    list.Clear();
                                    return list;
                                }
                                lineFile.RemoveAt(i - 1);
                                i--;
                            }
                            catch (Exception e)
                            {
                                save_edit = true;
                                bad = true;
                                string[] y = { "Id",
                                               "Title",
                                               "LastNameOfTheHead",
                                               "City",
                                               "Year",
                                               "NumberOfConcerts"
                                };
                                string[] y_ua = {
                                                "ID",
                                                "Назва музичного гурту",
                                                "Прізвище керівника",
                                                "Місто",
                                                "Рік",
                                                "Кількість концертів"
                                };

                                Console.Clear();
                                if (printFile)
                                {
                                    int z = 1;
                                    Console.WriteLine("\tВміст бази даних:\n");
                                    foreach (var item in lineFile)
                                    {
                                        Console.Write("№_{0}  ", z++);
                                        foreach (var item2 in item)
                                        {
                                            Console.Write("{0}; ", item2);
                                        }
                                        Console.WriteLine();
                                    }
                                }
                                Console.WriteLine("\nПомилка в рядку під номером {0}:\n", i);
                                foreach (var item in y_ua)
                                {
                                    Console.Write(item + "; ");
                                }
                                Console.WriteLine("\n\n--> {0}", s);
                                Console.WriteLine("\nПричина помилки: " + e.Message);
                                if (index == -1)
                                {
                                    string errors = e.StackTrace.Split('.', '(').Where(d => d.Contains("set_")).ToArray()[0].Substring(4);
                                    for (int j = 0; j < x.Length; j++)
                                    {
                                        if (y[j].Equals(errors))
                                        {
                                            index = j;
                                            break;
                                        }
                                    }
                                }
                                Console.WriteLine("\n\nПричина помилки в полі: " + y_ua[index]);
                                Console.WriteLine(e.Message);
                                Console.WriteLine("\nДля редагування натисніть [Enter], або будьяку іншу клавішу, щоб не зберігати цей запис");
                                char ch = Console.ReadKey(true).KeyChar;
                                if (ch == (char)ConsoleKey.Enter)
                                {
                                    Console.WriteLine("\n" + y_ua[index]);
                                    Console.Write(">");
                                    x[index] = Console.ReadLine();
                                    if (index == 0)
                                    {
                                        bad_id.Insert(i - 1, x[0]);
                                    }
                                }
                                else
                                {
                                    bad = false;
                                    lineFile.RemoveAt(i - 1);
                                    i--;
                                }
                            }
                        } while (bad);
                    }
                    i++;
                }
                f.Close(); // закриття файла
            }
            if (save_edit)
            {
                Console.Clear();
                Console.WriteLine("Попередній перегляд:");
                ViewTable(list);
                Console.WriteLine("Для збереження змін натисніть [Enter], або будьяку іншу клавішу, для скасування");
                if ((char)ConsoleKey.Enter == Console.ReadKey(true).KeyChar)
                {
                    Output(list);
                }
                else
                {
                    return Input();
                }
            }
            return list;
        }

        public static void TableCap(bool clear = false)
        {
            if (clear)
            {
                Console.Clear();
            }
            string x = "\n|";
            for (int i = 0; i < tableSize; i++)
            {
                x += "=";
            }
            x += "|";

            Console.WriteLine(string.Format(x + $"\n| {"ID",iL} | {"Назва музичного гурту",tL} | {"Прізвище керівника",lL} | {"Місто",cL} | {"Рік",yL} | {"Кількість концертів",nL} |" + x));
        }

        public static void ViewTable(TouringTrip x)
        {
            TableCap();
            Console.WriteLine(x);
        }

        public static void ViewTable(bool clear = false)
        {
            List<TouringTrip> arr = Input();
            if (clear)
            {
                Console.Clear();
            }
            if (arr.Count != 0)
            {
                TableCap();
                foreach (TouringTrip item in arr)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void ViewTable(List<TouringTrip> arr)
        {
            TableCap();
            foreach (TouringTrip item in arr)
            {
                Console.WriteLine(item);
            }
        }
        public static void Add()
        {
            List<TouringTrip> add = new List<TouringTrip>
            {
                new TouringTrip()
            };
            char x;
            do
            {
                bool bad;
                add[0].Id = "null";
                do
                {
                    bad = false;
                    try
                    {
                        add[0].Title = Read(tL, "Назва музичного гурту:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);

                do
                {
                    bad = false;
                    try
                    {
                        add[0].LastNameOfTheHead = Read(lL, "Прізвище керівника:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);

                do
                {
                    bad = false;
                    try
                    {
                        add[0].City = Read(cL, "Місто:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);

                do
                {
                    bad = false;
                    try
                    {
                        add[0].Year = Read(yL, "Рік:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);

                do
                {
                    bad = false;
                    try
                    {
                        add[0].NumberOfConcerts = Read(nL, "Кількість:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);


                Console.Clear();
                ViewTable(add[0]);
                Console.WriteLine("Для збереження натисніть [Enter]");
                Console.WriteLine("Для редагування натисніть будьяку клавішу");
                Console.WriteLine("Для повернення в головне меню натисніть ESC");

                x = Console.ReadKey(true).KeyChar;
                if (x == (char)ConsoleKey.Escape) { return; }
                if (x != (char)ConsoleKey.Enter)
                {
                    Edit(add);
                    return;
                }
            } while (x != (char)ConsoleKey.Enter);
            Output(add[0]);
        }

        public static void Edit(List<TouringTrip> list = null)
        {
            bool add = true;
            int index = 0;
            if (list == null)
            {
                add = false;
                list = new List<TouringTrip>();
                list = Input();
                Console.Clear();
                ViewTable(list);
                if (list.Count != 1)
                {
                    do
                    {
                        Console.Write("Введіть ID рядка який потрібно редагувати: \n>");
                        string id = Console.ReadLine();
                        index = list.FindIndex(s => s.Id.Equals(id));
                    } while (index == -1);
                }
            }
            char x = 'z';
            do
            {
                Console.Clear();
                if (x == '0')
                {
                    Console.WriteLine("<= Такого пункта немає =>\n");
                }
                ViewTable(list[index]);
                Console.WriteLine("Виберіть що саме потрібно редагувати:");
                Console.WriteLine("[1] Назва музичного гурту:");
                Console.WriteLine("[2] Прізвище керівника: ");
                Console.WriteLine("[3] Місто:");
                Console.WriteLine("[4] Рік:");
                Console.WriteLine("[5] Кількість:");
                Console.WriteLine("[ESC] Вихід");
                x = Console.ReadKey(true).KeyChar;
                bool bad;

                do
                {
                    bad = false;
                    try
                    {

                        switch (x)
                        {
                            case (char)ConsoleKey.Escape:
                                break;

                            case '1':
                                list[index].Title = Read(tL, "Назва музичного гурту:");
                                break;

                            case '2':
                                list[index].LastNameOfTheHead = Read(lL, "Прізвище керівника:");
                                break;

                            case '3':
                                list[index].City = Read(cL, "Місто:");
                                break;

                            case '4':
                                list[index].Year = Read(yL, "Рік:");
                                break;

                            case '5':
                                list[index].NumberOfConcerts = Read(nL, "Кількість:");
                                break;

                            default:
                                x = '0';
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n" + e.Message);
                        Console.WriteLine("Введіть ще раз:");
                        bad = true;
                    }
                } while (bad);
            } while (x != (char)ConsoleKey.Escape);
            Console.WriteLine("Для збереження результату натисніть [Enter]");
            if (Console.ReadKey(true).KeyChar == (char)ConsoleKey.Enter)
            {
                if (add)
                {
                    Output(list[0]);
                }
                else
                {
                    Output(list);
                }
            }
        }

        public static void Dell()
        {
            List<TouringTrip> list = Input();
            Console.Clear();
            ViewTable(list);
            int index;
            int x = 0;
            string[] id = null;
            do
            {
                do
                {
                    if (x == 0)
                    {
                        Console.Write("Введіть ID запису (або декілька через пробіл) який потрібно видалити, або [Enter] для виходу: \n>");
                        id = Console.ReadLine().Split(' ')
                                               .Where(s => !string.IsNullOrWhiteSpace(s))
                                               .Select(s => s.Trim())
                                               .ToArray();
                    }
                    if (id == null || id.Length == 0)
                    {
                        return;
                    }
                    index = list.FindIndex(s => s.Id.Equals(id[x]));
                    if (index == -1)
                    {
                        Console.Clear();
                        Console.Write("\nID {0} немає в базі, для продовження натисніть будьяку клавішу", id[x]);
                        _ = Console.ReadKey(true);
                        index = -1;
                    }
                    x++;
                } while (x < id.Length && index == -1);
                if (index != -1)
                {
                    Console.Clear();
                    ViewTable(list[index]);
                    Console.WriteLine("Для видалення натисніть [Enter]");
                    if (Console.ReadKey(true).KeyChar == (char)ConsoleKey.Enter)
                    {
                        list.RemoveAt(index);
                        Output(list);
                    }
                }
            } while (x < id.Length && index != -1);
        }
        public virtual string LastLetter(List<TouringTrip> list = null, string str = null)
        {
            Console.WriteLine("MusicBand");
            return "MusicBand";
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        private static void IsFile()
        {
            using StreamReader f = new StreamReader(FILE_NAME);
            bool empty = true;
            while ((_ = f.ReadLine()) != null)
            {
                empty = false;
                break;
            }
            f.Close(); // закриття файла
            if (empty)
            {
                throw new FileNotFoundException();
            }
        }
        protected static bool TestIsNumber(string str)
        {
            if (!double.TryParse(str, out _))
            {
                return false;
            }
            return true;
        }

        protected static bool TestLengthIsNull(string value, int size)
        {
            size = -size;
            if (value == null || value == "" || value.Equals("null"))
            {
                return true;
            }
            else if (value.Length > size)
            {
                throw new Exception("\nМаксимальна допустима довжина: " + size);
            }
            return false;
        }

        protected static string Read(int len, string value = null)
        {
            if (value == null)
            {
                value = "";
            }
            else
            {
                value = "\n" + value;
            }
            string str;
            bool err;
            do
            {
                err = false;
                Console.Write(value + "\n>");
                str = Console.ReadLine();
                if (str.Length > -len)
                {
                    Console.WriteLine("\nМаксимальна довжина: " + -len);
                    Console.WriteLine("Введіть ще раз:");
                    err = true;
                }
            } while (err);
            return str;
        }
        public static void Sort(char x = '0')
        {
            List<TouringTrip> list = Input();
            string pole = "null";
            do
            {
                if (x == '0')
                {

                    Console.Clear();
                    Console.WriteLine("Виберіть за яким полем виконати сортування:\n");

                    Console.WriteLine("[1] --- ID");
                    Console.WriteLine("[2] --- Назва музичного гурту");
                    Console.WriteLine("[3] --- Прізвище керівника");
                    Console.WriteLine("[4] --- Місто");
                    Console.WriteLine("[5] --- Рік");
                    Console.WriteLine("[6] --- Кількість концертів");

                    Console.WriteLine("\n[ESC] --- Вихід\n");

                    Console.Write(">");
                    x = Console.ReadKey(true).KeyChar;
                }
                switch (x)
                {
                    case (char)ConsoleKey.Escape:
                        break;

                    case '1':
                        list.Sort((a, b) => a.Id.CompareTo(b.Id));
                        pole = "ID";
                        break;

                    case '2':
                        list.Sort((a, b) => a.Title.CompareTo(b.Title));
                        pole = "Назва музичного гурту";
                        break;

                    case '3':
                        list.Sort((a, b) => a.LastNameOfTheHead.CompareTo(b.LastNameOfTheHead));
                        pole = "Прізвище керівника";
                        break;

                    case '4':
                        list.Sort((a, b) => a.City.CompareTo(b.City));
                        pole = "Місто";
                        break;

                    case '5':
                        list.Sort((a, b) => a.Year.CompareTo(b.Year));
                        pole = "Рік";
                        break;

                    case '6':
                        list.Sort((a, b) => a.NumberOfConcerts.CompareTo(b.NumberOfConcerts));
                        pole = "Кількість концертів";
                        break;

                    default:
                        x = '0';
                        Console.WriteLine("\nПомилка: Такого пункта немає\n\n");
                        break;
                }
            } while (x == '0');
            if (!pole.Equals("null"))
            {
                Console.WriteLine("База відcортована за полем: " + pole);
            }
            Output(list);
        }
    }
}
