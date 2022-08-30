using System.Collections.Immutable;

namespace money_problem.Domain;

public class Portfolio
{
    private readonly ICollection<Money> moneys;

    public Portfolio()
    {
        this.moneys = new List<Money>();
    }

    private Portfolio(IEnumerable<Money> moneys)
    {
        this.moneys = moneys.ToImmutableList();
    }

    public Money EvaluateWithException(Bank bank, Currency currency)
    {
        List<ConversionResult<MissingExchangeRateException>> results = this.GetConvertedMoneys(bank, currency);
        return ContainsFailure(results)
            ? throw ToException(results)
            : ToMoney(results, currency);
    }

    private static MissingExchangeRatesException ToException(IEnumerable<ConversionResult<MissingExchangeRateException>> results) =>
        new(results
            .Where(result => result.HasException())
            .Select(result => result.GetExceptionUnsafe())
            .ToList());

    private static Money ToMoney(IEnumerable<ConversionResult<MissingExchangeRateException>> results, Currency currency) =>
        new(results.Sum(result => result.GetMoneyUnsafe().Amount), currency);

    private static bool ContainsFailure(IEnumerable<ConversionResult<MissingExchangeRateException>> results) =>
        results.Any(result => result.HasException());

    private List<ConversionResult<MissingExchangeRateException>> GetConvertedMoneys(Bank bank, Currency currency) =>
        this.moneys
            .Select(money => ConvertMoney(bank, currency, money))
            .ToList();

    private static ConversionResult<MissingExchangeRateException> ConvertMoney(Bank bank, Currency currency, Money money)
    {
        try
        {
            return new ConversionResult<MissingExchangeRateException>(bank.Convert(money, currency));
        }
        catch (MissingExchangeRateException exception)
        {
            return new ConversionResult<MissingExchangeRateException>(exception);
        }
    }

    public Portfolio Add(Money money)
    {
        List<Money> updatedMoneys = this.moneys.ToList();
        updatedMoneys.Add(money);
        return new Portfolio(updatedMoneys);
    }

    public class ConversionResult<T> where T : Exception
    {
        private readonly T? exception;

        private readonly Money? money;

        public ConversionResult(Money money)
        {
            this.money = money;
        }

        public ConversionResult(T exception)
        {
            this.exception = exception;
        }

        public bool HasMoney() => this.money != null;

        public bool HasException() => this.exception != null;

        public T GetExceptionUnsafe() => this.exception!;

        public Money GetMoneyUnsafe() => this.money!;
    }

    public ConversionResult<MissingExchangeRatesException> Evaluate(Bank bank, Currency currency)
    {
        try
        {
            var money = this.EvaluateWithException(bank, currency);
            return new ConversionResult<MissingExchangeRatesException>(money);
        }
        catch (MissingExchangeRatesException exception)
        {
            return new ConversionResult<MissingExchangeRatesException>(exception);
        }
    }
}