package money_problem.domain;

public record ConversionResult(Money money, MissingExchangeRateException missingExchangeRateException) {

    ConversionResult(Money money) {
        this(money, null);
    }

    ConversionResult(MissingExchangeRateException missingExchangeRateException) {
        this(null, missingExchangeRateException);
    }


}
