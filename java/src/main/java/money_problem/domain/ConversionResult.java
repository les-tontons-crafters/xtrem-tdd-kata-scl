package money_problem.domain;

public record ConversionResult(Money money, MissingExchangeRatesException missingExchangeRateException) {
    public ConversionResult(Money money) {
        this(money, null);
    }

    public ConversionResult(MissingExchangeRatesException missingExchangeRateException) {
        this(null, missingExchangeRateException);
    }

    public boolean isFailure() {
        return missingExchangeRateException != null;
    }

    public boolean isSuccess() {
        return money != null;
    }

    public Money success() {
        return money();
    }

    public ConversionError failure() {
        return null;
    }
}
