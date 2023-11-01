#region Using
using productconsole.Data;
using productconsole.Models;
#endregion

namespace productconsole.Services
{
    #region Public Class OrderService

    /// <summary>
    /// Класс для работы с данными из таблицы 
    /// </summary>
    public class OrderService
    {
        #region Public Field

        /// <summary>
        /// True в случае, если данные загрузились и false, в обратном случае
        /// </summary>
        public readonly bool LoadError;
        #endregion

        #region Private Fields

        /// <summary>
        /// Экземпляр класс для доступа к данным
        /// </summary>
        private readonly DataService dataWorker;

        /// <summary>
        /// Список товаров
        /// </summary>
        private List<Product> products;

        /// <summary>
        /// Список клиентов
        /// </summary>
        private List<Client> clients;

        /// <summary>
        /// Список заявок
        /// </summary>
        private List<Order> orders;
        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор, который принимает путь к файлу
        /// </summary>
        /// <param name="filePath"></param>
        public OrderService(string filePath)
        {
            dataWorker = new DataService(filePath);

            LoadDataFromFile();

            LoadError = ChechForEmpty();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Выводит информацию о заказах товара по наименованию товара
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public void SearchClientsByProduct(string productName)
        {
            var data = orders.Where(x => x.Product.Name.ToLower() == productName.ToLower()).OrderBy(t => t.OrderDate).ToList();

            if (data !=null && data.Count > 0)
            {
                foreach (Order item in data)
                {
                    item.WriteInfo();
                }                
            }
            else
            {
                Console.WriteLine("\nНет заказов такого продукта");
            }
        }
        
        /// <summary>
        /// Выполняет поиск золотого клиента, в зависимости от указанных параметров (только год или месяц и год)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void SearchGoldClient(int year, int month = 0)
        {
            Dictionary<Client, int> data = new Dictionary<Client, int>();

            try
            {
                if (month >= 1 && month <= 12)
                {
                    data = orders.Where(x => x.OrderDate.Month == month && x.OrderDate.Year == year)
                                 .GroupBy(cl => cl.Client)
                                 .OrderByDescending(x => x.Count())
                                 .ToDictionary(d => d.Key, d => d.Count());
                }
                else
                {
                    data = orders.Where(x => x.OrderDate.Year == year)
                                 .GroupBy(cl => cl.Client)
                                 .OrderByDescending(x => x.Count())
                                 .ToDictionary(d => d.Key, d => d.Count());
                }
            }
            catch (Exception ex) { }

            GetGoldClient(data);
        }
        
        /// <summary>
        /// Обновляет контакное лицо у организаци по id организации
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="newContactPerson"></param>
        /// <returns></returns>
        public bool UpdateClientContactPerson(int clientId, string newContactPerson)
        {
            if (clientId <= 0)
                return false;

            var result = dataWorker.UpdateClientContactPerson(clientId, newContactPerson);

            if (result)
            {
                clients.Where(x => x.Id == clientId).FirstOrDefault().ContactPerson = newContactPerson;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Вывод в консоль всех товаров из списка products
        /// </summary>
        public void ShowAllProduct()
        {
            foreach (var item in products)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Вывод в консоль всех клиентов из списка clients
        /// </summary>
        public void ShowAllClients()
        {
            foreach (var item in clients)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Вывод в консоль информации о всех заявках из списка orders
        /// </summary>
        public void ShowAllOrders()
        {
            foreach (var item in orders)
            {
                item.WriteInfo();
            }
        }
        #endregion

        #region Private Methods
        
        /// <summary>
        /// Заполнение списков из данных из файла 
        /// </summary>
        private void LoadDataFromFile()
        {
            products = dataWorker.GetProducts();
            clients = dataWorker.GetClients();
            orders = dataWorker.GetOrders();

            UpdateOrders();
        }
        
        /// <summary>
        /// Сопоставление товаров и клиентов в списке заявок
        /// </summary>
        private void UpdateOrders()
        {
            foreach (var item in orders)
            {
                Product existProduct = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                item.Product = existProduct == null ? new Product() : existProduct;

                Client existClient = clients.Where(x => x.Id == item.ClientId).FirstOrDefault();
                item.Client = existClient == null ? new Client() : existClient;
            }
        }
        
        /// <summary>
        /// Выводит в консоль информацию о золотых клиентах
        /// </summary>
        /// <param name="data"></param>
        private void GetGoldClient(Dictionary<Client, int> data)
        {
            if (data.Count > 0)
            {
                var first = data.FirstOrDefault();
                var list = data.Where(x => x.Value == first.Value).ToList();

                Console.WriteLine("\nЗолотой клиент");
                foreach (var item in list)
                {
                    Console.WriteLine(item.Key);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Нет клиентов для отображения\n");
            }
        }
        
        /// <summary>
        /// Проверка всех списков на пустоту, в случае если данные не загрузились или файл не существует
        /// </summary>
        /// <returns></returns>
        private bool ChechForEmpty()
        {
            if (products.Count == 0 && clients.Count == 0 && orders.Count == 0)
                return true;
            else
                return false;
        }
        #endregion
    }
    #endregion
}
