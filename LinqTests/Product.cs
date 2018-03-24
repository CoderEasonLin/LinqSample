namespace LinqTests
{
    internal class Product
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public string Supplier { get; set; }

        public bool IsTopSale()
        {
            return Price >= 200 && Price <= 500 && Cost >= 30;
        }
    }
}