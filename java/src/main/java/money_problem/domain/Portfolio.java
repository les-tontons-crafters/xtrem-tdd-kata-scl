package money_problem.domain;

import java.util.ArrayList;
import java.util.EnumMap;
import java.util.List;
import java.util.Map;

public class Portfolio {
    private final Map<Currency, List<Double>> moneys = new EnumMap<>(Currency.class);


    public void addMoney(Double amount, Currency currency) {
        List<Double> amounts = new ArrayList<>();
        amounts.add(amount);
        moneys.put(currency, amounts);
    }

    public Double evaluate(Currency currency, Bank bank) {
        Double res = 0d;

        for (var money : moneys.entrySet()) {
            if (money.getKey() == currency) {
                res += money.getValue().stream().mapToDouble(x -> x).sum();
            } else {
                res += money.getValue().stream().mapToDouble(x -> {
                    try {
                        return bank.convert(x, money.getKey(), currency);
                    } catch (MissingExchangeRateException e) {
                        throw new RuntimeException(e);
                    }
                }).sum();
            }

        }
        return res;
    }
}
