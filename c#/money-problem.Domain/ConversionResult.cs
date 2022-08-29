namespace money_problem.Domain;

public record ConversionResult(MissingExchangeRateException? Exception, Money? Money)
{
    public static ConversionResult Success(Money money) => new(null, money);

    public static ConversionResult Failure(MissingExchangeRateException exception) => new(exception, null);

    public bool HasException() => this.Exception != null;

    public MissingExchangeRateException GetExceptionUnsafe() => this.Exception!;

    public Money GetMoneyUnsafe() => this.Money!;
}