using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;

namespace Last
{
    class Base
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public int Experience { get; set; }
        public string Password { get; set; }
        public string Hash { get; set; }

        public Base()
        {
            Index = -1;
            Name = "0";
            Surname = "0";
            Age = 0;
            Position = "0";
            Experience = 0;
            Password = "0";
            Hash = "0";
        }
        public Base(int index, string name, string surname, int age, string position, int experience, string password)
        {
            Index = index;
            Name = name;
            Surname = surname;
            Age = age;
            Position = position;
            Experience = experience;
            Password = password;
            Hash = CalcHash();
        }
        public string CalcHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Password}";
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
    class Database
    {
        protected List<Base> list;

        public Database()
        {
            list = new List<Base>();
            InitBlockGenesis();
        }
        public void InitBlockGenesis()
        {
            list.Add(new Base(0, "", "", 0, "", 0, ""));
        }
        public Base getLast()
        {
            return list[list.Count - 1];
        }
        public List<Base> GetList()
        {
            return list;
        }
        public void darkPass(Base bs)
        {
            string str = new string('*', bs.Password.Length);
            Console.WriteLine(str);
        }
        public void AddBlock(Base bs)
        {
            Base temp = getLast();
            bs = new Base(
                temp.Index + 1,
                temp.Name,
                temp.Surname,
                temp.Age,
                temp.Position,
                temp.Experience,
                temp.Password
            );
            bs.Hash = bs.CalcHash();
            list.Add(bs);
        }
        public void Input()         // (1) Заполнить базу данных
        {
            DeleteFile();
            Console.Write("Все введеное автоматически сохраняется в файл!\nСколько хотите создать блоков: ");
            int n = Convert.ToInt32(Console.ReadLine());
            Base lastBlock = getLast();
            for (int i = 0; i < n; i++)
            {
                Base bs = new Base();
                bs.Index = lastBlock.Index + 1;
                Console.Write("Введите имя: ");
                bs.Name = Console.ReadLine();
                Console.Write("Введите фамилию: ");
                bs.Surname = Console.ReadLine();
                Console.Write("Введите возраст: ");
                bs.Age = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите должность: ");
                bs.Position = Console.ReadLine();
                Console.Write("Введите опыт: ");
                bs.Experience = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите пароль: ");
                bs.Password = Console.ReadLine();
                bs.Hash = bs.CalcHash();
                list.Add(bs);
                Console.WriteLine();
                string file = "File.txt";
                ToFilleStandart(bs, file);
            }
        }
        public void Print()         // (2) Вывести все 
        {
            for (int i = 1; i < list.Count; i++)
            {
                Base item = list[i];
                Console.WriteLine($"Номер ячейки: {item.Index}");
                Console.WriteLine($"Имя: {item.Name}");
                Console.WriteLine($"Фамилия: {item.Surname}");
                Console.WriteLine($"Возраст: {item.Age}");
                Console.WriteLine($"Должность: {item.Position}");
                Console.WriteLine($"Опыт: {item.Experience}");
                Console.WriteLine($"Пароль: {item.Password}");
                Console.WriteLine($"Хэш: {item.Hash}");
                Console.WriteLine();
            }
        }
        public void plusBlock()     // (3) Добавить новую ячейку
        {
            Base lastBlock = getLast();
            Base bs = new Base
            {
                Index = lastBlock.Index + 1
            };
            Console.Write("Введите имя: ");
            bs.Name = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            bs.Surname = Console.ReadLine();
            Console.Write("Введите возраст: ");
            bs.Age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите должность: ");
            bs.Position = Console.ReadLine();
            Console.Write("Введите опыт: ");
            bs.Experience = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите пароль: ");
            bs.Password = Console.ReadLine();
            bs.Hash = bs.CalcHash();
            list.Add(bs);
            string file = "File.txt";
            ToFilleStandart(bs, file);
        }
        public void changeBlock(int index)       // (4) Изменить N ячейку
        {
            if (index >= 0 && index < list.Count)
            {
                Base block = list[index];
                Console.WriteLine($"Редактирование ячейки {index}:");
                Console.Write("Введите имя: ");
                block.Name = Console.ReadLine();
                Console.Write("Введите фамилию: ");
                block.Surname = Console.ReadLine();
                Console.Write("Введите возраст: ");
                block.Age = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите должность: ");
                block.Position = Console.ReadLine();
                Console.Write("Введите опыт: ");
                block.Experience = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите пароль: ");
                block.Password = Console.ReadLine();
                block.Hash = block.CalcHash();
                Console.WriteLine();
                Console.WriteLine("Ячейка успешно обновлена.");
            }
            else
            {
                Console.WriteLine("Неверно введеный индекс ячейки!");
            }
        }
        public void removeBlock(int index)       // (5) Удалить N ячейку
        {
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                Console.WriteLine($"Ячейка {index} успешно удалена.");
            }
            else
            {
                Console.WriteLine("Неверно введеный индекс ячейки!");
            }
        }
        public void sortExperience()        // (6) Сортировка
        {
            Console.Clear();
            Console.WriteLine("Выбрана сортировка по опыту работу. Все данные автоматично обновятся в файлах!");
            list.Sort((x, y) => x.Experience.CompareTo(y.Experience));
            Console.WriteLine("\nСортировка успешно завершена!");
        }
        public void sortSurname()           // (6) Сортировка 
        {
            Console.Clear();
            Console.WriteLine("Выбрана сортировка по фамилии. Все данные автоматично обновятся в файлах!");
            list.Sort((x, y) => string.Compare(x.Surname, y.Surname));
            Console.WriteLine("\nСортировка успешно завершена!");
        }
        public void sortAge()           // (6) Сортировка
        {
            Console.Clear();
            Console.WriteLine("Выбрана сортировка по возрасту. Все данные автоматично обновятся в файлах!");
            list.Sort((x, y) => x.Age.CompareTo(y.Age));
            Console.WriteLine("\nСортировка успешно завершена!");

        }
        public void Clearr()        // (7) Очисть полностью всю базу данных
        {
            list.Clear();
            Console.WriteLine("Все успешно очищенно!");
        }
        public void ToFilleStandart(Base bs, string fileName)       // (8) Запись в файл
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                //writer.WriteLine($"Номер ячейки: {bs.Index}");
                writer.WriteLine(bs.Name);
                writer.WriteLine(bs.Surname);
                writer.WriteLine(bs.Age);
                writer.WriteLine(bs.Position);
                writer.WriteLine(bs.Experience);
                writer.WriteLine(bs.Password);
                writer.WriteLine(bs.Hash);
                writer.WriteLine();
            }
        }
        public void ToFileUser()        // (8) Запись в файл
        {
            Console.Write("Введите название файла без расширения: ");
            string fileName = Console.ReadLine();
            fileName = string.Concat(fileName, ".txt");
            for (int i = 0; i < list.Count; i++)
            {
                Base bs = list[i];
                bs.Index = i + 1;
                ToFilleStandart(bs, fileName);
            }
        }

        public void DeleteFile()        // Удаление файла 
        {
            string file = "File.txt";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        public void ReadFromFile(string fileName)           // (9) Чтение из файла
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("Файл не существует.");
            }
        }
        public void WithFilleStandart()         // (9) Чтение из файла
        {
            string file = "File.txt";
            ReadFromFile(file);
            //Print();
        }
        public void WithFilleUser()             // (9) Чтение из файла
        {
            Console.Write("Введите название файла без расширения: ");
            string fileName = Console.ReadLine();
            fileName = string.Concat(fileName, ".txt");
            ReadFromFile(fileName);
            //Print();
        }
    }
    class Menu
    {
        public int mn()
        {
            Console.Clear();
            Console.WriteLine("^^) Выберите что делаем?"); 
            Console.WriteLine("(1) Заполнить базу данных."); //
            Console.WriteLine("(2) Показать всю базу данных."); //
            Console.WriteLine("(3) Добавить новую ячейку."); //
            Console.WriteLine("(4) Изменить N ячейку."); //
            Console.WriteLine("(5) Удалить N ячейку."); //
            Console.WriteLine("(6) Сортировки."); //
            Console.WriteLine("(7) Очисть полностью всю базу данных."); //
            Console.WriteLine("(8) Запись в файл."); //
            Console.WriteLine("(9) Чтение из файла."); //
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(0) Выход."); //
            Console.ResetColor();
            Console.Write("^^) Ваш выбор: ");
            return Convert.ToInt32(Console.ReadLine());
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            Database db = new Database();
            Base bs = new Base(0, "", "", 0, "", 0, "");
            int chooise;
            int cycle = 1;
            chooise = menu.mn();

            while(cycle == 1)
            {
                switch (chooise)
                {
                    case 0:
                        Console.Clear();
                        Console.Write("Точно желаете выйти (0 / 1): ");
                        cycle = Convert.ToInt32(Console.ReadLine());
                        if (cycle == 0)
                            cycle = 0;
                        else
                        {
                            chooise = menu.mn();
                        }
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали заполнить базу данных.");
                        db.Input();
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали показать всю базу данных.\n");
                        db.Print();
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали добавить новую ячейку.");
                        db.plusBlock();
                        Console.WriteLine("Ячейка успешно добавлена в базу данных.");
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали изменить N ячейку.");
                        int update;
                        Console.Write("Выберите какую ячейку вы хотите изменить: ");
                        update = Convert.ToInt32(Console.ReadLine());
                        db.changeBlock(update);
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали удалить N ячейку.");
                        int remove;
                        Console.Write("Выберите какую ячейку вы хотите удалить: ");
                        remove = Convert.ToInt32(Console.ReadLine());
                        db.removeBlock(remove);
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("^^) Вы выбрали сортировки.");
                        Console.WriteLine("^^) Выберите по какому критерию сделать сортировку.");
                        Console.WriteLine("(1) По опыту работы.");
                        Console.WriteLine("(2) По фамилии.");
                        Console.WriteLine("(3) По возрасту.");
                        Console.Write("^^) Ваш выбор: ");
                        int sort = Convert.ToInt32(Console.ReadLine());
                        switch (sort)
                        {
                            case 1:
                                Console.Clear();
                                db.sortExperience();
                                Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                                Console.ReadLine();
                                Console.Clear();
                                chooise = menu.mn();
                                break;
                            case 2:
                                Console.Clear();
                                db.sortSurname();
                                Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                                Console.ReadLine();
                                Console.Clear();
                                chooise = menu.mn();
                                break;
                            case 3:
                                Console.Clear();
                                db.sortAge();
                                Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                                Console.ReadLine();
                                Console.Clear();
                                chooise = menu.mn();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Вы ввели неверно, попробуйте еще раз!");
                                Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                                Console.ReadLine();
                                Console.Clear();
                                chooise = menu.mn();
                                break;
                        }
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали очисть полностью всю базу данных.");
                        db.Clearr();
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 8:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали запись в файл.");
                        db.ToFileUser();
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    case 9:
                        Console.Clear();
                        Console.WriteLine("Вы выбрали чтение из файла.");
                        Console.Write("Вы хотите воспользоваться файлом созданным автоматически или созданный вами (0 / 1): ");
                        int f = Convert.ToInt32(Console.ReadLine());
                        switch (f)
                        {
                            case 0:
                                db.WithFilleStandart();
                                break;
                            case 1:
                                db.WithFilleUser();
                                break;
                            default:
                                Console.WriteLine("Вы ввели не правильно, попробуйте еще раз!");
                                Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                                Console.ReadLine();
                                Console.Clear();
                                chooise = menu.mn();
                                break;
                        }
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Вы ввели не верно, попробуйте еще раз!");
                        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
                        Console.ReadLine();
                        Console.Clear();
                        chooise = menu.mn();
                        break;

                }
            }
        }
    }
}
