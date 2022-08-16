package money_problem.domain;

import static money_problem.domain.Currency.*;

public class MoneyUtils {

    public static Money euros(double amount) {
        return new Money(amount, EUR);
    }

    public static Money dollars(double amount) {
        return new Money(amount, USD);
    }

    public static Money koreanWons(double amount) {
        return new Money(amount, KRW);
    }

}
