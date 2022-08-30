package money_problem.domain;

public record ConversionResult<T extends Exception>(Money money, T failureException) {
    public ConversionResult(Money money) {
        this(money, null);
    }

    public ConversionResult(T failure) {
        this(null, failure);
    }

    public boolean isFailure() {
        return failureException != null;
    }

    public boolean isSuccess() {
        return money != null;
    }

    public Money success() {
        return money();
    }

    public ConversionError failure() {
        return new ConversionError(failureException.getMessage());
    }
}
