using FluentAssertions;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests
{
    public class MoneyShould
    {
        [Fact(DisplayName = "5 USD + 10 USD = 15 USD")]
        public void AddInUsd()
        {
            Money result = MoneyCalculator.Add(new Money(5, USD), 10);
            result.Should()
                .Be(new Money(15, USD));
        }
        
        [Fact(DisplayName = "10 EUR x 2 = 20 EUR")]
        public void MultiplyInEuros()
        {
            MoneyCalculator
                .Times(new Money(10, EUR), 2)
                .Should()
                .Be(new Money(20, EUR));
        }

        [Fact(DisplayName = "4002 KRW / 4 = 1000.5 KRW")]
        public void DivideInKoreanWons()
        {
            MoneyCalculator
                .Divide(new Money(4002, KRW), 4)
                .Should()
                .Be(new Money(1000.5d, KRW));
        }
    }
}