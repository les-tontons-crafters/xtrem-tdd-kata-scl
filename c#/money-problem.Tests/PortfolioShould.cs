using System.Collections.Generic;
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

        portfolio.Evaluate(Currency.USD, bank).Should().Be(17);
    }

    [Fact(DisplayName = "1usd + 1100 krw = 2200 krw")]
    public void Evaluate_Should_Return_2200Krw_Given_1Usd_And_1100Krw()
    {
        var bank = Bank.WithExchangeRate(Currency.USD, Currency.KRW, 1100);
        var portfolio = new Portfolio();
        portfolio.Add(1, Currency.USD);
        portfolio.Add(1100, Currency.KRW);
        
        portfolio.Evaluate(Currency.KRW, bank).Should().Be(2200);
    }

    [Fact(DisplayName = "1usd + 1100 usd = 1101 usd")]
    public void Evaluate_Should_Return_1101Krw_Given_1Usd_And_1100Usd()
    {
        var bank = Bank.WithExchangeRate(Currency.USD, Currency.USD, 1);
        var portfolio = new Portfolio();
        portfolio.Add(1, Currency.USD);
        portfolio.Add(1100, Currency.USD);

        portfolio.Evaluate(Currency.USD, bank).Should().Be(1101);
    }
}

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