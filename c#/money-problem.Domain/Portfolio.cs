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

    private static string ToFailureMessage(
        IEnumerable<ConversionResult<string>> results)
    {
        var missingExchangeRates = results
            .Where(result => result.IsFailure())
            .Select(result => result.Failure)
            .ToList();

        return $"Missing exchange rate(s): {GetMissingRates(missingExchangeRates)}";
    }
    
    private static string GetMissingRates(List<string> missingRates) => missingRates
        .Select(exception => $"[{exception}]")
        .Aggregate((r1, r2) => $"{r1},{r2}");
        

    private static Money ToMoney(IEnumerable<ConversionResult<string>> results, Currency currency) =>
        new(results.Sum(result => result.Success.Amount), currency);

    private static bool ContainsFailure(IEnumerable<ConversionResult<string>> results) =>
        results.Any(result => result.IsFailure());

    private List<ConversionResult<string>> GetConvertedMoneys(Bank bank, Currency currency) =>
        this.moneys
            .Select(money => ConvertMoney(bank, currency, money))
            .ToList();

    private static ConversionResult<string> ConvertMoney(Bank bank, Currency currency, Money money)
    {
        return bank.Convert(money, currency);
    }

    public Portfolio Add(Money money)
    {
        List<Money> updatedMoneys = this.moneys.ToList();
        updatedMoneys.Add(money);
        return new Portfolio(updatedMoneys);
    }

    public ConversionResult<string> Evaluate(Bank bank, Currency currency)
    {
        List<ConversionResult<string>> results = this.GetConvertedMoneys(bank, currency);
        return ContainsFailure(results)
            ? new ConversionResult<string>(ToFailureMessage(results))
            : new ConversionResult<string>(ToMoney(results, currency));
    }
}