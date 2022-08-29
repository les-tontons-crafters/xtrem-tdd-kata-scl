namespace money_problem.Domain
{
    public sealed class Bank
    {
        private readonly Dictionary<string, double> _exchangeRates;

        private Bank(Dictionary<string, double> exchangeRates) => this._exchangeRates = exchangeRates;

        public static Bank WithExchangeRate(Currency from, Currency to, double rate)
        {
            var bank = new Bank(new Dictionary<string, double>());
            bank.AddExchangeRate(from, to, rate);
            return bank;
        }

        public void AddExchangeRate(Currency from, Currency to, double rate)
            => this._exchangeRates[KeyFor(from, to)] = rate;

        private static string KeyFor(Currency from, Currency to) => $"{from}->{to}";

        public Money Convert(Money money, Currency to) =>
            this.CanConvert(money.Currency, to)
                ? this.ConvertSafely(money, to)
                : throw new MissingExchangeRateException(money.Currency, to);

        private Money ConvertSafely(Money money, Currency to) =>
            to == money.Currency
                ? money
                : new Money(money.Amount * this._exchangeRates[KeyFor(money.Currency, to)], to);

        private bool CanConvert(Currency from, Currency to) =>
            from == to || this._exchangeRates.ContainsKey(KeyFor(from, to));
    }
}