package money_problem.domain;

public class MoneyUtils {

    public static Money euros(double amount) {
        return new Money(amount, Currency.EUR);
    }
}
