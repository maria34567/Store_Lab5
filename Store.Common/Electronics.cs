namespace Store.Common
{
    public class Electronics : Product
    {
        public int WarrantyMonths { get; set; }
        public string Brand { get; set; } = string.Empty;

        public Electronics() { }

        public static Electronics CreateNew()
        {
            string[] names = { "Ноутбук", "Смартфон", "Планшет", "Монітор", "Навушники" };
            string[] brands = { "ASUS", "Samsung", "Lenovo", "Acer", "Sony" };
            return new Electronics(names[Random.Shared.Next(names.Length)],
                Random.Shared.Next(1000, 80001), brands[Random.Shared.Next(brands.Length)],
                Random.Shared.Next(6, 37));
        }

        public Electronics(string name, decimal price, string brand, int warranty) 
            : base(name, price)
        {
            Brand = brand;
            WarrantyMonths = warranty;
        }

        public override string GetInfo()
        {
            return $"[Електроніка] ID: {Id}, Назва: {Name}, Бренд: {Brand}, Гарантія: {WarrantyMonths} міс., Ціна: {Price:C}";
        }
    }
}