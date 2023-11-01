using productconsole.Services;

namespace productconsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuService menuService = new MenuService();
            menuService.Start();
        }
    }
}