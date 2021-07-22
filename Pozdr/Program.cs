using System;
using System.Data;
using System.Data.SqlClient;

namespace Pozdr
{
    public class BirthList
    {
        private static string stringConnect = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\BDList.mdb;Integrated Security=True";
        private SqlConnection connect;
        private SqlDataAdapter adapter;
        private SqlCommand command;
        private DataTable datatable;

        public BirthList()
        {
            connect = new SqlConnection(stringConnect);
            datatable = new DataTable();
        }

        public DataTable GetAll()
        {
            connect.Open();
            datatable.Clear();
            adapter = new SqlDataAdapter("SELECT * FROM List ORDER BY DBirth;", connect);
            adapter.Fill(datatable);
            connect.Close();
            return datatable;
        }

        public DataTable GetToday()
        {
            connect.Open();
            datatable.Clear();
            adapter = new SqlDataAdapter($"SELECT * FROM List WHERE DBirth = {DateTime.Today.Date};", connect);
            adapter.Fill(datatable);
            connect.Close();
            return datatable;
        }

        public DataTable GetSomeNext()
        {
            connect.Open();
            datatable.Clear();
            adapter = new SqlDataAdapter($"SELECT * FROM List WHERE DBirth > {DateTime.Today.Date} AND DBirth <= {DateTime.Today.Date.AddDays(7)};", connect);
            adapter.Fill(datatable);
            connect.Close();
            return datatable;
        }

        public bool delete(int id)
        {
            connect.Open();
            command = new SqlCommand($"DELETE FROM List WHERE id = {id}", connect);
            bool ret = (command.ExecuteNonQuery()>0);
            connect.Close();
            return ret;
        }
        public bool insert(string name, string surname, string fname, DateTime date)
        {
            connect.Open();
            command = new SqlCommand($"INSERT INTO list VALUES ({name}, {surname}, {fname}, {date}", connect);
            bool ret = (command.ExecuteNonQuery() > 0);
            connect.Close();
            return ret;
        }

        public bool update(int id, string name, string surname, string fname, DateTime date)
        {
            connect.Open();
            command = new SqlCommand($"UPDATE List SET Name_ = {name}, Surname_ = {surname}, FName = {fname}, DBirth = {date} WHERE id = {id}", connect);
            bool ret = (command.ExecuteNonQuery() > 0);
            connect.Close();
            return ret;
        }

        public bool find(int id)
        {
            connect.Open();
            command = new SqlCommand($"SELECT * List id = {id}", connect);
            bool ret = (command.ExecuteNonQuery() > 0);
            connect.Close();
            return ret;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            void menu(BirthList list)
            {
                Console.WriteLine("----------\n1) Вывести весь список ДР\n2) Вывести сегодняшние и ближайшие ДР\n3) Добавить запись\n4) Редактировать запись (по id)\n5) Удалить запись (по id)\n");
                int ch;
                try
                {
                    ch = Convert.ToInt32(Console.ReadLine());
                    if (ch < 1 || ch > 5) throw new Exception();
                    switch (ch)
                    {
                        case 1:
                            Console.WriteLine("\nID    Имя     Фамилия     Отчество     Дата рождения\n");
                            foreach (DataRow r in list.GetAll().Rows)
                            {
                                Console.WriteLine($"{r[0]}  {r[1]}  {r[2]}  {r[3]}");
                            }
                            break;
                        case 2:
                            Console.WriteLine($"\nСегодня ({DateTime.Today.ToString()}) ДР празднуют:");
                            DataTable datatable = list.GetToday();
                            Console.WriteLine("\nID    Имя     Фамилия     Отчество\n");
                            foreach (DataRow r in datatable.Rows)
                            {
                                Console.WriteLine($"{r[0]}  {r[1]}  {r[2]}  {r[3]}");
                            }
                            Console.WriteLine("\nБлижайшие ДР (7 дней):");
                            Console.WriteLine("\nID    Имя     Фамилия     Отчество     Дата рождения");
                            foreach (DataRow r in datatable.Rows)
                            {
                                Console.WriteLine($"{r[0]}  {r[1]}  {r[2]}  {r[3]}  {r[4]}");
                            }
                            break;
                        case 3:
                            string n, s, f;
                            DateTime d;
                            try
                            {
                                Console.WriteLine("Введите имя");
                                n = Console.ReadLine();
                                Console.WriteLine("Введите фамилию");
                                s = Console.ReadLine();
                                Console.WriteLine("Введите отчество (при отсутствии введите \"-\"");
                                f = Console.ReadLine();
                                Console.WriteLine("Введите дату рождения (в формате ДД.ММ.ГГГГ)");
                                d = Convert.ToDateTime(Console.ReadLine(), System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU"));
                                if (!list.insert(n, s, f, d)) Console.WriteLine("Ошибка: запись не добавлена");
                                else Console.WriteLine("Запись добавлена");
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Неверный ввод!");
                                break;
                            }
                        case 4:
                            Console.WriteLine("Введите id удаляемой записи");
                            try
                            {
                                if (!list.delete(Convert.ToInt32(Console.ReadLine()))) Console.WriteLine("Запись удалена");
                                else Console.WriteLine("Ошибка: запись не удалена");
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Записи с таким id не существует!");
                                break;
                            }
                        case 5:
                            Console.WriteLine("Введите id редактируемой записи");
                            try
                            {
                                int id = Convert.ToInt32(Console.ReadLine());
                                if (list.find(id))
                                {
                                    try
                                    {
                                        Console.WriteLine("Введите имя");
                                        n = Console.ReadLine();
                                        Console.WriteLine("Введите фамилию");
                                        s = Console.ReadLine();
                                        Console.WriteLine("Введите отчество (при отсутствии введите \"-\"");
                                        f = Console.ReadLine();
                                        Console.WriteLine("Введите дату рождения (в формате ДД.ММ.ГГГГ)");
                                        d = Convert.ToDateTime(Console.ReadLine(), System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU"));
                                        if (!list.insert(n, s, f, d)) Console.WriteLine("Ошибка: запись не добавлена");
                                        else Console.WriteLine("Запись добавлена");
                                        break;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Неверный ввод!");
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Записи с таким id не существует");
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Неверный ввод!");
                                break;
                            }
                    }
                    menu(list);
                }
                catch (Exception)
                {
                    Console.WriteLine("Неверный ввод!");
                    menu(list);
                }
            }

            BirthList list = new BirthList();
            Console.WriteLine($"\nСегодня ({DateTime.Today.ToString()}) ДР празднуют:");
            DataTable datatable = list.GetToday();
            Console.WriteLine("\nID    Имя     Фамилия     Отчество\n");
            foreach (DataRow r in datatable.Rows)
            {
                Console.WriteLine($"{r[0]}  {r[1]}  {r[2]}  {r[3]}");
            }
            Console.WriteLine("\nБлижайшие ДР (7 дней):");
            Console.WriteLine("\nID    Имя     Фамилия     Отчество     Дата рождения");
            foreach (DataRow r in datatable.Rows)
            {
                Console.WriteLine($"{r[0]}  {r[1]}  {r[2]}  {r[3]}  {r[4]}");
            }
            menu(list);
        }
    }
}
