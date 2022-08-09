namespace money_problem.Domain;

public class Money
{
    public Money(double amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public double Amount { get; private set; }
    public Currency Currency { get; private set; }
}

public static class MoneyCalculator
{
    public static double Add(Money money, double addedAmount) => money.Amount + addedAmount;
    public static double Times(Money money, int times) => money.Amount * times;
    public static double Divide(Money money, int divisor) => money.Amount / divisor;
}