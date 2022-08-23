namespace money_problem.Domain;

public class Portfolio
{
    private readonly ICollection<Money> moneys = new List<Money>();

    public void Add(Money money) => this.moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var results = this.moneys.Select(money => Convert(money, bank, currency));
        var exceptions = results.Where(result => result.Exception != null);

        if (exceptions.Any()) {
            throw new MissingExchangeRatesException(exceptions.Select(result => result.Exception!).ToList());
        }

        var sum = results.Sum(result => result.Money!.Amount);
        
        return new Money(sum, currency);
    }

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