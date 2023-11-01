
namespace productconsole.Models
{
    #region Public Class Client

    /// <summary>
    /// Модель данных из листа Клиенты
    /// </summary>
    public class Client
    {
        #region Public Fields

        /// <summary>
        /// Код клиента
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Адрес организации
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string? ContactPerson { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Перегрузка ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Id} - {this.Name} - {this.ContactPerson}";
        }
        #endregion
    }
    #endregion
}
