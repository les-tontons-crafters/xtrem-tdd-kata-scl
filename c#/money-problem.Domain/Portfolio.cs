namespace money_problem.Domain;

public class Portfolio
{
    private readonly ICollection<Money> moneys = new List<Money>();

    public void Add(Money money) => this.moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        double convertedResult = 0;
        var missingExchangeRates = new List<MissingExchangeRateException>();
        this.moneys.ToList().ForEach(money =>
        {
            try
            {
                Money convertedMoney = bank.Convert(money, currency);
                convertedResult += convertedMoney.Amount;
            }
            catch (MissingExchangeRateException exception)
            {
                missingExchangeRates.Add(exception);
            }
        });
        
        if (missingExchangeRates.Any()) {
            throw new MissingExchangeRatesException(missingExchangeRates);
        }
        
        // Simply instantiate a new Money from here
        return new Money(convertedResult, currency);
    }

    public ConversionResult EvaluateWithResult(Bank bank, Currency currency)
    {
        double convertedResult = 0;
        var missingExchangeRates = new List<MissingExchangeRateException>();
        var results = this.moneys.Select(money => Convert(money, bank, currency));
        
        if (results.Any(result => result.Exception != null)) {
            throw new MissingExchangeRatesException(results.Where(result => result.Exception != null).Select(result => result.Exception).ToList());
        }

        var sum = results.Sum(result => result.Money.Amount);
        
        // Simply instantiate a new Money from here
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