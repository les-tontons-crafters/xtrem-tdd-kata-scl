package money_problem.domain;

import io.vavr.Function2;
import io.vavr.collection.HashMap;
import io.vavr.collection.Map;
import io.vavr.control.Either;

import static io.vavr.control.Either.left;
import static io.vavr.control.Either.right;

public final class Bank {
    private final Map<String, Double> exchangeRates;
    private final Currency pivotCurrency;

    private Bank(Currency pivotCurrency, Map<String, Double> exchangeRates) {
        this.pivotCurrency = pivotCurrency;
        this.exchangeRates = exchangeRates;
    }

    private Bank(Currency pivotCurrency) {
        this(pivotCurrency, HashMap.empty());
    }

    public static Bank withPivotCurrency(Currency pivotCurrency) {
        return new Bank(pivotCurrency);
    }

    private static String keyFor(Currency from, Currency to) {
        return from + "->" + to;
    }

    public Either<String, Bank> addExchangeRate(Currency currency, double rate) {
        return checkPivotCurrency(
                currency,
                rate,
                this::parseAmount);
    }

    private Either<String, Bank> checkPivotCurrency(
            Currency currency,
            double rate,
            Function2<Currency, Double, Either<String, Bank>> onSuccess) {
        return isSameCurrency(currency, pivotCurrency)
                ? left("Can not add an exchange rate for the pivot currency")
                : onSuccess.apply(currency, rate);
    }

    private Either<String, Bank> parseAmount(Currency currency, Double rate) {
        return isPositive(rate)
                ? right(addMultiplierAndDividerExchangeRate(currency, rate))
                : left("Exchange rate should be greater than 0");
    }

    private boolean isPositive(double rate) {
        return rate > 0;
    }

    private Bank addMultiplierAndDividerExchangeRate(Currency to, double rate) {
        return new Bank(pivotCurrency,
                exchangeRates.put(keyFor(pivotCurrency, to), rate)
                        .put(keyFor(to, pivotCurrency), 1 / rate));
    }

    public Either<String, Money> convert(Money money, Currency toCurrency) {
        return canConvert(money, toCurrency)
                ? right(convertSafely(money, toCurrency))
                : left(String.format("%s->%s", money.currency(), toCurrency));
    }

    private boolean canConvert(Money money, Currency to) {
        return isSameCurrency(money.currency(), to)
                || canConvertDirectly(money.currency(), to)
                || canConvertThroughPivotCurrency(money.currency(), to);
    }

    private boolean isSameCurrency(Currency money, Currency to) {
        return money == to;
    }

    private boolean canConvertDirectly(Currency money, Currency to) {
        return exchangeRates.containsKey(keyFor(money, to));
    }

    private boolean canConvertThroughPivotCurrency(Currency from, Currency to) {
        return exchangeRates.containsKey(keyFor(pivotCurrency, from))
                && exchangeRates.containsKey(keyFor(pivotCurrency, to));
    }

    private Money convertSafely(Money money, Currency to) {
        if (isSameCurrency(money.currency(), to)) {
            return money;
        }
        return canConvertDirectly(money.currency(), to)
                ? convertDirectly(money, to)
                : convertThroughPivotCurrency(money, to);
    }

    private Money convertDirectly(Money money, Currency to) {
        return new Money(money.amount() * exchangeRates.getOrElse(keyFor(money.currency(), to), 0d), to);
    }

    private Money convertThroughPivotCurrency(Money money, Currency to) {
        return convertDirectly(convertDirectly(money, pivotCurrency), to);
    }
}