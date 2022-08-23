package money_problem.domain;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

public class Portfolio {
    private final ArrayList<Money> moneys = new ArrayList<>();

    public void add(Money money) {
        moneys.add(money);
    }

    public Money evaluate(Bank bank, Currency toCurrency) throws MissingExchangeRatesException {

        var results = covertMoneysToList(bank, toCurrency);

        if (containsMissingExchangeRates(results)) {
            throw new MissingExchangeRatesException(getMissingExchangeRates(results));
        }

        var total = getMoneysSum(results);

        return new Money(total, toCurrency);
    }

    private List<MissingExchangeRateException> getMissingExchangeRates(List<ConversionResult> results) {
        return results.stream().map(ConversionResult::missingExchangeRateException).filter(Objects::nonNull).toList();
    }

    private double getMoneysSum(List<ConversionResult> results) {
        return results.stream().filter(ConversionResult::isSuccess).mapToDouble(ConversionResult::getAmount).sum();
    }

    private boolean containsMissingExchangeRates(List<ConversionResult> results) {
        return results
                .stream()
                .anyMatch(
                        ConversionResult::isFailure
                );
    }

    private List<ConversionResult> covertMoneysToList(Bank bank, Currency toCurrency) {
        return moneys.stream().map(money -> convertMoney(bank, money, toCurrency)).toList();
    }

    private static ConversionResult convertMoney(Bank bank, Money money, Currency toCurrency) {
        try {
            return new ConversionResult(bank.convert(money, toCurrency));
        } catch (MissingExchangeRateException missingExchangeRateException) {
            return new ConversionResult(missingExchangeRateException);
        }
    }
}