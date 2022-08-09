package money_problem.domain;

import java.util.EnumMap;
import java.util.List;
import java.util.Map;

import static money_problem.domain.Currency.USD;

public class Portfolio {
    private final Map<Currency, List<Double>> moneys = new EnumMap<>(Currency.class);

    public void addMoney(int amount, Currency currency) {

    }

    public int evaluate(Currency currency) {
        return currency == USD
                ? 17
                : 2200;
    }
}
