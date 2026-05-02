using Microsoft.Extensions.Configuration;
using DotNetEnv;
using System;
using System.IO;

namespace Homework2.Config;

public static class DatabaseConfig
{
    private static IConfigurationRoot _configuration;

    static DatabaseConfig()
    {
        // Загружаем переменные из .env файла
        Env.Load();
        
        // Получаем текущую директорию 
        string currentDirectory = Directory.GetCurrentDirectory();
        
        // Строим конфигурацию из JSON файла
        var builder = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        _configuration = builder.Build();
    }

    // Задание 1: Получение строки подключения из JSON
    public static string GetConnectionStringFromJson()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Строка подключения не найдена в appsettings.json");
        
        return connectionString;
    }

    // Задание 1: Получение строки подключения из .env
    public static string GetConnectionStringFromEnv()
    {
        string host = Env.GetString("DB_HOST");
        string port = Env.GetString("DB_PORT");
        string database = Env.GetString("DB_NAME");
        string username = Env.GetString("DB_USER");
        string password = Env.GetString("DB_PASSWORD");
        
        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database))
            throw new Exception("Данные для подключения не найдены в .env файле");
        
        return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }

    // Метод для тестирования (Задание 1)
    public static void TestConnectionStrings()
    {
        Console.WriteLine("=== Задание 1: Тестирование строк подключения ===\n");
        
        try
        {
            string jsonConnection = GetConnectionStringFromJson();
            Console.WriteLine("Строка из appsettings.json:");
            Console.WriteLine($"   {jsonConnection}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении из JSON: {ex.Message}\n");
        }
        
        try
        {
            string envConnection = GetConnectionStringFromEnv();
            Console.WriteLine("Строка из .env:");
            Console.WriteLine($"   {envConnection}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении из .env: {ex.Message}\n");
        }
    }
}