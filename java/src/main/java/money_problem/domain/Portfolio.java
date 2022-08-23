package money_problem.domain;

import java.util.ArrayList;
import java.util.Objects;
import java.util.function.Function;

public class Portfolio {
    private final ArrayList<Money> moneys = new ArrayList<>();

    public void add(Money money) {
        moneys.add(money);
    }

    public Money evaluate(Bank bank, Currency toCurrency) throws MissingExchangeRatesException {

        var results = moneys.stream().map(money -> convertMoney(bank, money, toCurrency)).toList();

        if (results.stream().anyMatch(money -> money.missingExchangeRateException() != null)) {
            throw new MissingExchangeRatesException(results.stream().map(ConversionResult::missingExchangeRateException).filter(Objects::nonNull).toList());
        }

        var total = results.stream().filter(result -> result.money() != null).mapToDouble(result -> result.money().amount()).sum();

        return new Money(total, toCurrency);
    }

    private static ConversionResult convertMoney(Bank bank, Money money, Currency toCurrency) {
        try {
            return new ConversionResult(bank.convert(money, toCurrency));
        } catch (MissingExchangeRateException missingExchangeRateException) {
            return new ConversionResult(missingExchangeRateException);
        }
    }
}