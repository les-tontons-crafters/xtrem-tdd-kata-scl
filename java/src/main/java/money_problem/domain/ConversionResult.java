package money_problem.domain;

public record ConversionResult(Money money, MissingExchangeRateException missingExchangeRateException) {

    ConversionResult(Money money) {
        this(money, null);
    }

    ConversionResult(MissingExchangeRateException missingExchangeRateException) {
        this(null, missingExchangeRateException);
    }

    public Boolean isFailure() {
        return missingExchangeRateException != null;
    }

    public Boolean isSuccess() {
        return money != null;
    }

    public double getAmount() {
        return money.amount();
    }


}
