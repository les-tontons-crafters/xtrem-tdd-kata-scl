namespace money_problem.Domain;

public class Portfolio
{
    private readonly ICollection<Money> _moneys = new List<Money>();

    private static ConversionResult Convert(Money money, Bank bank, Currency targetCurrency)
    {
        try
        {
            var convertedMoney = bank.Convert(money, targetCurrency);
            return ConversionResult.Success(convertedMoney);
        }
        catch (MissingExchangeRateException exception)
        {
            return ConversionResult.Failure(exception);
        }
    }

    private static IEnumerable<ConversionResult> GetExceptions(IEnumerable<ConversionResult> results) =>
        results.Where(result => result.HasException()).ToList();

    private static MissingExchangeRatesException ToException(IEnumerable<ConversionResult> results) =>
        new(GetExceptions(results).Select(result => result.GetExceptionUnsafe()));

    private static Money ToMoney(Currency currency, IEnumerable<ConversionResult> results) =>
        new(results.Sum(result => result.GetMoneyUnsafe().Amount), currency);

    private IEnumerable<ConversionResult> ConvertMoneys(Bank bank, Currency currency) =>
        this._moneys.Select(money => Convert(money, bank, currency));

    public void Add(Money money) => this._moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var results = this.ConvertMoneys(bank, currency).ToList();
        return HasMissingExchangeRates(results)
            ? throw ToException(results)
            : ToMoney(currency, results);
    }

    private static bool HasMissingExchangeRates(IEnumerable<ConversionResult> results) => GetExceptions(results).Any();
}