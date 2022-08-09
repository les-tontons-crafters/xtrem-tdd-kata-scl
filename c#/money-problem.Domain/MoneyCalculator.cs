namespace money_problem.Domain;

public static class MoneyCalculator
{
    public static double Add(Money money, double addedAmount) => money.Amount + addedAmount;
    public static double Times(Money money, int times) => money.Amount * times;
    public static double Divide(Money money, int divisor) => money.Amount / divisor;
}