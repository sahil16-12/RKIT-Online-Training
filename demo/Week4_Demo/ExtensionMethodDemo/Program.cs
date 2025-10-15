using ExtensionMethodDemo.Extensions;

namespace ExtensionMethodDemo
{
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Order> orders = new List<Order>
            {
                new Order { Id = 1, Customer = "Sahil", Amount = 1200m },
                new Order { Id = 2, Customer = "Hakim", Amount = 450m },
                new Order { Id = 3, Customer = "Danish", Amount = 2300m },
                new Order { Id = 4, Customer = "Sameed", Amount = 800m }
            };

            // Use extension method: TotalAmount
            decimal total = orders.TotalAmount();
            Console.WriteLine($"Total orders amount: ₹{total:N2}");

            // Use extension: Average
            decimal avg = orders.AverageAmount();
            Console.WriteLine($"Average order amount: ₹{avg:N2}");

            // Use extension: filter high-value orders
            var highOrders = orders.OrdersAbove(1000m);
            Console.WriteLine("High-value orders (>1000):");
            foreach (var o in highOrders)
            {
                Console.WriteLine($"  Order #{o.Id}, Customer: {o.Customer}, Amount: ₹{o.Amount:N2}");
            }

            // Extend single order with IsHighValue
            Order ord = orders[2];
            if (ord.IsHighValue(2000m))
                Console.WriteLine($"Order {ord.Id} is high value (threshold 2000).");
            else
                Console.WriteLine($"Order {ord.Id} is not high value (threshold 2000).");
        }
    }
}
