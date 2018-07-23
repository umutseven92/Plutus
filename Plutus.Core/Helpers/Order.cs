namespace Plutus.Core.Helpers
{
    public class Order
    {
        public bool Test { get; set; }

        public string Symbol { get; set; }

        public string Base { get; set; }

        public decimal Amount { get; set; }

        public decimal Price { get; set; }
        
        public decimal ProfitStop { get; set; }

        public decimal LossStop { get; set; }
        
    }
}