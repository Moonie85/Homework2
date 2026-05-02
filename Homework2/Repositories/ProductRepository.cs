using Dapper;
using Npgsql;
using Homework2.Config;
using Homework2.Models;

namespace Homework2.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    // Конструктор: true - использовать JSON, false - использовать .env
    public ProductRepository(bool useJsonConfig = true)
    {
        if (useJsonConfig)
            _connectionString = DatabaseConfig.GetConnectionStringFromJson();
        else
            _connectionString = DatabaseConfig.GetConnectionStringFromEnv();
    }

    // Задание 2: Получение общей стоимости всех продуктов
    // Метод для получения данных из БД
    public decimal GetTotalProductsPrice()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        // SQL запрос: считаем сумму (цена * количество)
        string sql = "SELECT SUM(price * quantity) FROM table_products";
        
        // получаем первое значение или null
        decimal? totalPrice = connection.QueryFirstOrDefault<decimal?>(sql);
        
        // Если нет записей, возвращаем 0
        return totalPrice ?? 0;
    }

    // Задание 3: Добавление нового продукта (без user_id)
    // Метод для изменения данных
    public int AddProduct(string name, decimal price, int quantity)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        // SQL запрос: вставляем новый продукт и возвращаем его ID
        string sql = @"
            INSERT INTO table_products (name, price, quantity) 
            VALUES (@Name, @Price, @Quantity) 
            RETURNING Id
        ";
        
        // Создаем объект с параметрами
        var parameters = new
        {
            Name = name,
            Price = price,
            Quantity = quantity
        };
        
        // выполняем запрос и получаем одно значение (ID)
        int newId = connection.QuerySingle<int>(sql, parameters);
        
        return newId;
    }

    // Задание 4: Получение списка названий продуктов
    // Метод для получения списка данных
    public List<string> GetAllProductNames()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        // SQL запрос: получаем только названия, сортируем по алфавиту
        string sql = "SELECT name FROM table_products ORDER BY name";
        
        // получаем список строк
        List<string> productNames = connection.Query<string>(sql).ToList();
        
        return productNames;
    }

    // Показать все продукты (для проверки)
    public List<Product> GetAllProducts()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        string sql = "SELECT * FROM table_products ORDER BY id";
        
        return connection.Query<Product>(sql).ToList();
    }
}