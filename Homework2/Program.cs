using Homework2.Config;
using Homework2.Repositories;

namespace Homework2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("═══════════════════════════════════════════");
        Console.WriteLine("     Домашняя работа ADO.NET + Dapper");
        Console.WriteLine("═══════════════════════════════════════════\n");
        
        // ЗАДАНИЕ 1: Тестируем получение строк подключения
        DatabaseConfig.TestConnectionStrings();
        
        Console.WriteLine("\n═══════════════════════════════════════════\n");
        
        // Создаем репозиторий (true - использовать JSON, false - .env)
        ProductRepository productRepo = new ProductRepository(useJsonConfig: true);
        
        try
        {
            // ЗАДАНИЕ 2: Получаем общую стоимость всех продуктов
            Console.WriteLine("=== Задание 2: Общая стоимость продуктов ===");
            decimal totalPrice = productRepo.GetTotalProductsPrice();
            Console.WriteLine($"Общая стоимость: {totalPrice:C} (рублей)\n");
            
            // ЗАДАНИЕ 3: Добавляем новый продукт
            Console.WriteLine("=== Задание 3: Добавление нового продукта ===");
            Console.Write("Введите название продукта: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите цену: ");
            decimal price = decimal.Parse(Console.ReadLine());
            
            Console.Write("Введите количество: ");
            int quantity = int.Parse(Console.ReadLine());
            
            int newProductId = productRepo.AddProduct(name, price, quantity);
            Console.WriteLine($"Продукт добавлен! ID: {newProductId}\n");
            
            // ЗАДАНИЕ 4: Получаем список названий продуктов
            Console.WriteLine("=== Задание 4: Список названий продуктов ===");
            List<string> productNames = productRepo.GetAllProductNames();
            
            if (productNames.Count > 0)
            {
                Console.WriteLine($"Найдено продуктов: {productNames.Count}");
                Console.WriteLine("Список названий:");
                for (int i = 0; i < productNames.Count; i++)
                {
                    Console.WriteLine($"   {i + 1}. {productNames[i]}");
                }
            }
            else
            {
                Console.WriteLine("В таблице нет продуктов");
            }
            
            // Получаем все продукты с ценами
            Console.WriteLine("\n═══════════════════════════════════════════");
            Console.WriteLine(" Детальная информация о всех продуктах: ");
            Console.WriteLine("═══════════════════════════════════════════");
            
            var allProducts = productRepo.GetAllProducts();
            foreach (var product in allProducts)
            {
                decimal total = product.Price * product.Quantity;
                Console.WriteLine($"ID: {product.Id,-3} | {product.Name,-20} | " +
                                  $"Цена: {product.Price,8:C} | Кол-во: {product.Quantity,3} | " +
                                  $"Итого: {total,10:C}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОШИБКА: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"Детали: {ex.InnerException.Message}");
        }
        
        Console.WriteLine("\n═══════════════════════════════════════════");
        Console.WriteLine(" Нажмите любую клавишу для выхода... ");
        Console.ReadKey();
    }
}