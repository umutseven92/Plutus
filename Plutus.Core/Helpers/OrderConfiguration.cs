namespace Plutus.Core.Helpers
{
    public class OrderConfiguration 
    {
        /// <summary>
        /// What to buy with
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// What to buy
        /// </summary>
        public string Symbol { get; set; }
        
        /// <summary>
        /// When to sell for profit in PLSymbol
        /// </summary>
        public decimal ProfitStop { get; set; }

        /// <summary>
        /// When to sell for loss in PLSymbol
        /// </summary>
        public decimal LossStop { get; set; }
        
        /// <summary>
        /// Buy amount in PLSymbol
        /// </summary>
        public decimal BuyAmount { get; set; }
    }
}