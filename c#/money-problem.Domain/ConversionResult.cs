namespace money_problem.Domain;

public record ConversionResult(MissingExchangeRateException? Exception, Money? Money);