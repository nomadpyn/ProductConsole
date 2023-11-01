
namespace productconsole.Services
{
    #region Public Class MenuService

    /// <summary>
    /// Класс для вывода информации пользователю в консоль
    /// </summary>
    public class MenuService
    {
        #region Private Fields

        /// <summary>
        /// Экземпляр класса OrderService 
        /// </summary>
        private OrderService? orderService;
        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MenuService() { }
        #endregion

        #region Public Methods

        /// <summary>
        /// Начальный вывод данных в консоль
        /// </summary>
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
        #endregion

        #region Private Methods

        /// <summary>
        /// Уровень загрузки данных из файла
        /// </summary>
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

        /// <summary>
        /// Уровень выбора функции приложения
        /// </summary>
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
                            ChangeContactPersonLevel();
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            SearchGoldClientLevel();
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

        /// <summary>
        /// Уровень поиска заявок по наименовании продукта
        /// </summary>
        private void SearchClientsLevel()
        {

            Console.WriteLine("Список всех продуктов");
            orderService?.ShowAllProduct();

            Console.WriteLine("\nВведите наименование продукта");

            string searchedProductName = Console.ReadLine();

            orderService?.SearchClientsByProduct(searchedProductName);
                 
            GoToWorkChoiseLevel();
        }

        /// <summary>
        /// Уровень изменения контактного лица у клиента
        /// </summary>
        private void ChangeContactPersonLevel()
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

            GoToWorkChoiseLevel();
        }

        /// <summary>
        /// Уровень выбора признака поиска золотого клиента
        /// </summary>
        private void SearchGoldClientLevel()
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
                            SearchGoldClientByYearLevel();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            SearchGoldClientByMonthAndYearLevel();
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

        /// <summary>
        /// Уровень поиска золотого клиента по году
        /// </summary>
        private void SearchGoldClientByYearLevel()
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
            GoToWorkChoiseLevel();
        }

        /// <summary>
        /// Уровень поиска золотого клиета по месяцу и году
        /// </summary>
        private void SearchGoldClientByMonthAndYearLevel()
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
            GoToWorkChoiseLevel();
        }
        
        /// <summary>
        /// Метод выбирает куда вернуться, ну уровень выбора функции или в начало программы, для возможности работы с другим файлом
        /// </summary>
        private void GoToWorkChoiseLevel()
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
        #endregion
    }
    #endregion
}


