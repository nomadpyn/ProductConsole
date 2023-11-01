
namespace productconsole.Models
{
    #region Public Class Product

    /// <summary>
    /// Модель данных из листа Товары
    /// </summary>
    public class Product
    {
        #region Public Fields

        /// <summary>
        /// Код товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Единица измерения товара
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal? Price { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Перегрузка ToString 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Name} {this.Unit} {this.Price}";
        }
        #endregion
    }
    #endregion
}
