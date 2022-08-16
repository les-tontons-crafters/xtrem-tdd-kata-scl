namespace money_problem.Domain;

public record Money(double Amount, Currency Currency)
{
    public static Money FromEuro(int amount)
    {
        return new Money(amount, Currency.EUR);
    }
}