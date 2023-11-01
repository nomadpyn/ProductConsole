#region Using
using productconsole.Services;
#endregion

namespace productconsole
{
    #region Internal Class Program

    /// <summary>
    /// Основой класс программы
    /// </summary>
    internal class Program
    {
        #region Private Methods

        /// <summary>
        /// Точка входа в программу, в которой происходит запуска меню для выбора действий
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            MenuService menuService = new MenuService();
            menuService.Start();
        }
        #endregion
    }
    #endregion
}