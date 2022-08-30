package money_problem.domain;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import java.util.Arrays;

import static money_problem.domain.Currency.*;
import static money_problem.domain.MoneyUtils.*;
import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.assertThatThrownBy;

class PortfolioTest {
    private Bank bank;

    @BeforeEach
    void setup() {
        bank = Bank.withExchangeRate(EUR, USD, 1.2)
                .addExchangeRate(USD, KRW, 1100);
    }

    @Test
    @DisplayName("5 USD + 10 USD = 15 USD")
    void shouldAddMoneyInTheSameCurrency() throws MissingExchangeRatesException {
        var portfolio = portfolioWith(
                dollars(5),
                dollars(10)
        );

        assertThat(portfolio.evaluate(bank, USD).success())
                .isEqualTo(dollars(15));
    }

    @Test
    @DisplayName("5 USD + 10 EUR = 17 USD")
    void shouldAddMoneyInDollarsAndEuros() throws MissingExchangeRatesException {
        var portfolio = portfolioWith(
                dollars(5),
                euros(10)
        );

        assertThat(portfolio.evaluateWithException(bank, USD))
                .isEqualTo(dollars(17));
    }

    @Test
    @DisplayName("1 USD + 1100 KRW = 2200 KRW")
    void shouldAddMoneyInDollarsAndKoreanWons() throws MissingExchangeRatesException {
        var portfolio = portfolioWith(
                dollars(1),
                koreanWons(1100)
        );

        assertThat(portfolio.evaluateWithException(bank, KRW))
                .isEqualTo(koreanWons(2200));
    }

    @Test
    @DisplayName("5 USD + 10 EUR + 4 EUR = 21.8 USD")
    void shouldAddMoneyInDollarsAndMultipleAmountInEuros() throws MissingExchangeRatesException {
        var portfolio = portfolioWith(
                dollars(5),
                euros(10),
                euros(4)
        );

        assertThat(portfolio.evaluateWithException(bank, USD))
                .isEqualTo(dollars(21.8));
    }

    @Test
    @DisplayName("Throws a MissingExchangeRatesException in case of missing exchange rates")
    void shouldThrowAMissingExchangeRatesException() {
        var portfolio = portfolioWith(
                euros(1),
                dollars(1),
                koreanWons(1)
        );


        assertThatThrownBy(() -> portfolio.evaluateWithException(bank, EUR))
                .isInstanceOf(MissingExchangeRatesException.class)
                .hasMessage("Missing exchange rate(s): [USD->EUR],[KRW->EUR]");
    }

    private Portfolio portfolioWith(Money... moneys) {
        return Arrays.stream(moneys)
                .reduce(new Portfolio(), Portfolio::add, (previousPortfolio, newPortfolio) -> newPortfolio);
    }
}

