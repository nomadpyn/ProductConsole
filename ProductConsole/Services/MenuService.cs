using productconsole.Models;

namespace productconsole.Services
{
    public class MenuService
    {
        private OrderService? orderService;

        public MenuService() { }

        public void Start()
        {
            Console.WriteLine("Программа по работе с данными из файла Excel");
            Console.WriteLine("Что вы хотите сделать:");
            ConsoleKeyInfo choise;
            do
            {
                Console.WriteLine("1- Загрузить файл, 0 - Выход");
                choise = Console.ReadKey();
                Console.Clear();
                switch (choise.Key)
                {
                    case ConsoleKey.D1:
                        {
                            Console.Clear();
                            LoadLevel();
                            break;
                        }
                    case ConsoleKey.D0:
                        {
                            Console.WriteLine("До свидания!");
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Вы не сделали выбор");
                            break;
                        }
                }
            }
            while (choise.Key != ConsoleKey.D1 && choise.Key != ConsoleKey.D0);
        }

        private void LoadLevel()
        {
            do
            {
                Console.WriteLine("Введите путь к файлу");

                string? filePath = Console.ReadLine();

                orderService = new OrderService(filePath);

                switch (orderService.LoadError)
                {
                    case false:
                        {
                            WorkChoiseLevel();
                            break;
                        }
                    case true:
                        {
                            Console.WriteLine("Попробуйте еще раз, возможно файл занят или введен не верный путь");
                            break;
                        }
                }

            } while (orderService.LoadError);
        }

        private void WorkChoiseLevel()
        {
            Console.Clear();
            ConsoleKeyInfo choise;
            do
            {
                Console.WriteLine("1 - Показать покупателей по имени товара и информацию о покупке");
                Console.WriteLine("2 - Изменить контакное лицо у организации");
                Console.WriteLine("3 - Найти Золотого клиента за месяц/год");
                Console.WriteLine("0 - В начало");

                choise = Console.ReadKey();
                Console.Clear();
                switch (choise.Key)
                {
                    case ConsoleKey.D1:
                        {
                            SearchClientsLevel();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            ChangeContactPerson();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            SearchGoldClient();
                            break;
                        }
                    case ConsoleKey.D0:
                        {
                            this.Start();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Вы не сделали выбор");
                            break;
                        }
                }
            }
            while (choise.Key != ConsoleKey.D1 && choise.Key != ConsoleKey.D0 && choise.Key != ConsoleKey.D2 && choise.Key != ConsoleKey.D3);
        }

        private void SearchClientsLevel()
        {

            Console.WriteLine("Список всех продуктов");
            orderService?.ShowAllProduct();

            Console.WriteLine("\nВведите наименование продукта");

            string searchedProductName = Console.ReadLine();

            var data = orderService?.SearchClientsByProduct(searchedProductName);

            if (data.Count == 0)
            {
                Console.WriteLine("\nНет заказов такого продукта");
            }
            else
            {
                foreach (Order item in data)
                {
                    item.WriteInfo();
                }
            }

            GoToUpLevel();
        }

        private void ChangeContactPerson()
        {
            Console.WriteLine("Список всех организации");

            orderService?.ShowAllClients();

            Console.WriteLine("Введите id клиента, которого надо изменить");

            if (Int32.TryParse(Console.ReadLine(), out int clientId))
            {
                Console.WriteLine("Введите новое наименование контактного лица");
                string newContact = Console.ReadLine();

                bool result = orderService.UpdateClientContactPerson(clientId, newContact);

                string message = result == false ? "Не удалось записать данные" : "Данные успешно обновлены";

                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Вы некорректно ввели id клиента");
            }

            GoToUpLevel();
        }

        private void SearchGoldClient()
        {
            Console.Clear();
            ConsoleKeyInfo choise;
            do
            {
                Console.WriteLine("1 - Найти золотых клиентов за год");
                Console.WriteLine("2 - Найти золотых клиентов за месяц и год");

                choise = Console.ReadKey();
                Console.Clear();
                switch (choise.Key)
                {
                    case ConsoleKey.D1:
                        {
                            SearchGoldClientByYear();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            SearchGoldClientByMonthAndYear();
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Вы не сделали выбор");
                            break;
                        }
                }
            }
            while (choise.Key != ConsoleKey.D1 && choise.Key != ConsoleKey.D2);
        }

        private void SearchGoldClientByYear()
        {
            Console.WriteLine("Введите год");

            if (Int32.TryParse(Console.ReadLine(), out int year))
            {
                orderService.SearchGoldClient(year);
            }
            else
            {
                Console.WriteLine("Вы не корректно ввели год");
            }
            GoToUpLevel();
        }

        private void SearchGoldClientByMonthAndYear()
        {
            Console.WriteLine("Введите год");

            if (Int32.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Введите месяц");
                if (Int32.TryParse(Console.ReadLine(), out int month) && (month >= 1 && month <= 12))
                {
                    orderService.SearchGoldClient(year, month);
                }
                else
                {
                    Console.WriteLine("Вы не корректно ввели месяц");
                }

            }
            else
            {
                Console.WriteLine("Вы не корректно ввели год");
            }
            GoToUpLevel();
        }
        private void GoToUpLevel()
        {
            ConsoleKeyInfo choise;

            do
            {
                Console.WriteLine("1 - К выбору функции");
                Console.WriteLine("0 - В начало");

                choise = Console.ReadKey();
                Console.Clear();
                switch (choise.Key)
                {
                    case ConsoleKey.D1:
                        {
                            WorkChoiseLevel();
                            break;
                        }

                    case ConsoleKey.D0:
                        {
                            this.Start();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Вы не сделали выбор\n");
                            break;
                        }
                }
            }
            while (choise.Key != ConsoleKey.D1 && choise.Key != ConsoleKey.D0);
        }
    }
}


