namespace money_problem.Domain;

public static class MoneyCalculator
{
    public static Money Add(Money money, double addedAmount) => money with { Amount = money.Amount + addedAmount };
    public static Money Times(Money money, int times) => money with { Amount = money.Amount * times };
    public static Money Divide(Money money, int divisor) => money with { Amount = money.Amount / divisor };
}