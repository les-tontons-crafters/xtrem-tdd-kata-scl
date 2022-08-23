namespace money_problem.Domain;

public class Portfolio
{
    private readonly ICollection<Money> moneys = new List<Money>();

    public void Add(Money money) => this.moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var results = ConvertMoneys(bank, currency);
        var exceptions = GetExceptions(results);
        return exceptions.Any()
            ? throw new MissingExchangeRatesException(exceptions.Select(result => result.Exception!).ToList())
            : ToMoney(currency, results);
    }

    private static Money ToMoney(Currency currency, List<ConversionResult> results) => new Money(results.Sum(result => result.Money!.Amount), currency);

    private List<ConversionResult> ConvertMoneys(Bank bank, Currency currency) => this.moneys.Select(money => Convert(money, bank, currency)).ToList();

    private static List<ConversionResult> GetExceptions(List<ConversionResult> results) => results.Where(result => result.Exception != null).ToList();

    private ConversionResult Convert(Money money, Bank bank, Currency targetCurrency)
    {
        try
        {
            Money convertedMoney = bank.Convert(money, targetCurrency);
            return new ConversionResult(null, convertedMoney);
        }
        catch (MissingExchangeRateException exception)
        {
            return new ConversionResult(exception, null);
        }
    }
}