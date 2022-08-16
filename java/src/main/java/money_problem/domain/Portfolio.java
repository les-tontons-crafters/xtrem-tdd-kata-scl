package money_problem.domain;

import java.util.ArrayList;
import java.util.EnumMap;
import java.util.List;
import java.util.Map;

public class Portfolio {
    private final ArrayList<Money> moneys = new ArrayList<>();

    public void add(Money money) {
        moneys.add(money);
    }

    public Money evaluate(Bank bank, Currency toCurrency) throws MissingExchangeRatesException {
        var convertedResult = 0d;
        var missingExchangeRates = new ArrayList<MissingExchangeRateException>();

        for (var money : moneys) {
            try {
                var convertedAmount = bank.convert(money.amount(), money.currency(), toCurrency);
                convertedResult += convertedAmount;
            } catch (MissingExchangeRateException missingExchangeRateException) {
                missingExchangeRates.add(missingExchangeRateException);
            }
        }

        if (!missingExchangeRates.isEmpty()) {
            throw new MissingExchangeRatesException(missingExchangeRates);
        }
        return new Money(convertedResult, toCurrency);
    }
}