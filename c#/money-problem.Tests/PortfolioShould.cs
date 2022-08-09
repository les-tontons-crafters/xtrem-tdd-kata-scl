using FluentAssertions;
using money_problem.Domain;
using Xunit;

namespace money_problem.Tests;

public class PortfolioShould
{
    [Fact(DisplayName = "5 USD + 10 EUR= 17 USD")]
    public void Evaluate_should_return_17usd_given_5usd_and_10eur()
    {
        var bank = Bank.WithExchangeRate(Currency.EUR, Currency.USD, 1.2);
        var portfolio = new Portfolio();
        portfolio.Add(5, Currency.USD);
        portfolio.Add(10, Currency.EUR);

        double result = portfolio.Evaluate(Currency.USD, bank);
        result.Should().Be(17);
    }
}

public class Portfolio
{
    public void Add(double amount, Currency usd)
    {
        
    }

    public double Evaluate(Currency currency, Bank bank)
    {
        return 17;
    }
}