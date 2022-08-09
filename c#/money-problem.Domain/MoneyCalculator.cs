namespace money_problem.Domain;

public static class MoneyCalculator
{
    public static Money Add(Money money, double addedAmount) => money with{ Amount = money.Amount + addedAmount};
    public static double Times(Money money, int times) => money.Amount * times;
    public static double Divide(Money money, int divisor) => money.Amount / divisor;
}