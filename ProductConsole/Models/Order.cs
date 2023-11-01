using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace productconsole.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int OrderNumber { get; set; }
        public int ProductCount { get; set; }
        public DateTime OrderDate { get; set; }

        public void WriteInfo()
        {
            try
            {
                Console.WriteLine(this.Client.Name + " " + this.Client.ContactPerson);
                Console.WriteLine(this.OrderDate.ToShortDateString() + " " + this.Product.Price + "р " + this.ProductCount + "шт");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка вывода информации о товаре");
            }
        }
    }
}
