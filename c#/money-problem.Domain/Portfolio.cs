namespace money_problem.Domain;

public class Portfolio
{
    private readonly Dictionary<Currency, double> _exchangesRates = new();
    
    public double Evaluate(Currency currency, Bank bank)
    {
        double rtnVal = 0;
        foreach (var exchangesRate in _exchangesRates)
        {
            rtnVal += bank.Convert(exchangesRate.Value, exchangesRate.Key, currency);
        }

        return rtnVal;
    }

    public void Add(Money money)
    {
        if (_exchangesRates.ContainsKey(money.Currency))
        {
            _exchangesRates[money.Currency] += money.Amount;
        }
        else
        {
            _exchangesRates.Add(money.Currency, money.Amount);
        }
    }
}