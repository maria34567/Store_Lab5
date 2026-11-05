using System;

namespace Store.Common
{
    public abstract class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public static int TotalProductsCreated;

        static Product()
        {
            TotalProductsCreated = 0;
        }

        public Product()
        {
            System.Threading.Interlocked.Increment(ref TotalProductsCreated);
        }

        public Product(string name, decimal price) : this()
        {
            Name = name;
            Price = price;
        }

        public virtual string GetInfo()
        {
            return $"[Товар] ID: {Id}, Назва: {Name}, Ціна: {Price:C}";
        }

        public static void DisplayTotalCount()
        {
            Console.WriteLine($"Всього створено об'єктів Product: {TotalProductsCreated}");
        }
    }
}