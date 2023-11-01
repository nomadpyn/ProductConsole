
using productconsole.Data;
using productconsole.Models;

namespace productconsole.Services
{
    public class OrderService
    {
        private readonly DataService dataWorker;

        private List<Product> products;
        private List<Client> clients;
        private List<Order> orders;

        public readonly bool LoadError;

        public OrderService(string filePath)
        {
            dataWorker = new DataService(filePath);

            LoadDataFromFile();

            LoadError = ChechForEmpty();
        }

        public List<Order> SearchClientsByProduct(string productName)
        {
            var data = orders.Where(x => x.Product.Name.ToLower() == productName.ToLower()).OrderBy(t => t.OrderDate).ToList();

            return data == null ? new List<Order>() : data;
        }

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

        public void ShowAllProduct()
        {
            foreach (var item in products)
            {
                Console.WriteLine(item);
            }
        }

        public void ShowAllClients()
        {
            foreach (var item in clients)
            {
                Console.WriteLine(item);
            }
        }

        public void ShowAllOrders()
        {
            foreach (var item in orders)
            {
                item.WriteInfo();
            }
        }
        private void LoadDataFromFile()
        {
            products = dataWorker.GetProducts();
            clients = dataWorker.GetClients();
            orders = dataWorker.GetOrders();

            UpdateOrders();
        }
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

        private bool ChechForEmpty()
        {
            if (products.Count == 0 && clients.Count == 0 && orders.Count == 0)
                return true;
            else
                return false;
        }


    }
}
