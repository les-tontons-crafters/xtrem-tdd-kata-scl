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
            .Where(result => result.IsFailure())
            .Select(result => result.GetFailureUnsafe())
            .ToList());

    private static Money ToMoney(IEnumerable<ConversionResult<MissingExchangeRateException>> results, Currency currency) =>
        new(results.Sum(result => result.GetSuccessUnsafe().Amount), currency);

    private static bool ContainsFailure(IEnumerable<ConversionResult<MissingExchangeRateException>> results) =>
        results.Any(result => result.IsFailure());

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

    public class ConversionResult<T>
    {
        private readonly T? failure;

        private readonly Money? success;

        public ConversionResult(Money success)
        {
            this.success = success;
        }

        public ConversionResult(T failure)
        {
            this.failure = failure;
        }

        public bool IsSuccess() => this.success != null;

        public bool IsFailure() => this.failure != null;

        public T GetFailureUnsafe() => this.failure!;

        public Money GetSuccessUnsafe() => this.success!;
    }

    public ConversionResult<string> Evaluate(Bank bank, Currency currency)
    {
        try
        {
            var money = this.EvaluateWithException(bank, currency);
            return new ConversionResult<string>(money);
        }
        catch (MissingExchangeRatesException exception)
        {
            return new ConversionResult<string>(exception.Message);
        }
    }
}