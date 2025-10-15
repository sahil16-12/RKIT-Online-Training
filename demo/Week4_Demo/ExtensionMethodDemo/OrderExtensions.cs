
namespace ExtensionMethodDemo.Extensions
{
    public static class OrderExtensions
    {
        // Extension on List<Order> to compute total
        public static decimal TotalAmount(this List<Order> orders)
        {
            decimal sum = 0;
            foreach (var ord in orders)
                sum += ord.Amount;
            return sum;
        }

        // Extension on IEnumerable<Order> (so it works on List or others)
        public static decimal AverageAmount(this IEnumerable<Order> orders)
        {
            decimal sum = 0;
            long count = 0;
            foreach (var o in orders)
            {
                sum += o.Amount;
                count++;
            }
            return (count > 0) ? (sum / count) : 0;
        }

        // Extension to filter orders above a threshold
        public static IEnumerable<Order> OrdersAbove(this IEnumerable<Order> orders, decimal threshold)
        {
            foreach (var o in orders)
                if (o.Amount > threshold)
                    yield return o;
        }

        // Extension on Order to check if it's above a given amount
        public static bool IsHighValue(this Order order, decimal minValue)
        {
            return order.Amount >= minValue;
        }
    }
}
