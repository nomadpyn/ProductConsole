
namespace productconsole.Models
{
    #region Public Class Order

    /// <summary>
    /// Модель данных из листа Заявки
    /// </summary>
    public class Order
    {
        #region Public Fields

        /// <summary>
        /// Код заявки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код продукта
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Продукт, соответствующий ProductId
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Клиент, соответствующий ClientId
        /// </summary>
        public Client? Client { get; set; }

        /// <summary>
        /// Номер заявки
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Количество товара в заявке
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public DateTime OrderDate { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Вывод в консоль информации о заказе
        /// </summary>
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
                Console.WriteLine("Ошибка вывода информации о заказе");
            }
        }
        #endregion
    }
    #endregion
}
