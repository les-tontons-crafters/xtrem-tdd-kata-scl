namespace money_problem.Domain;

public class Portfolio
{
    private readonly Dictionary<Currency, double> _exchangesRates = new();
    
    public void Add(double amount, Currency currency)
    {
        if (_exchangesRates.ContainsKey(currency))
        {
            _exchangesRates[currency] += amount;
        }
        else
        {
            _exchangesRates.Add(currency, amount);
        }
    }

    public double Evaluate(Currency currency, Bank bank)
    {
        double rtnVal = 0;
        foreach (var exchangesRate in _exchangesRates)
        {
            rtnVal += bank.Convert(exchangesRate.Value, exchangesRate.Key, currency);
        }

        return rtnVal;
    }
}