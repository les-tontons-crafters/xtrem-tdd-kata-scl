package money_problem.domain;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import static money_problem.domain.Currency.*;
import static org.assertj.core.api.Assertions.assertThat;

class PortfolioTest {
    private Bank bank;

    @BeforeEach
    void setup() {
        bank = Bank.withExchangeRate(EUR, USD, 1.2);
        bank.addExchangeRate(USD, KRW, 1100);
    }

    @Test
    @DisplayName("5 USD + 10 EUR = 17 USD")
    void addUSDAndEuros() {
        var portfolio = new Portfolio();

        portfolio.addMoney(5d, USD);
        portfolio.addMoney(10d, EUR);

        assertThat(portfolio.evaluate(USD, bank)).isEqualTo(17);

    }

    @Test
    @DisplayName("1 USD + 1100 KRW = 2200 KRW")
    void addUSDAndKRW() {
        var portfolio = new Portfolio();

        portfolio.addMoney(1d, USD);
        portfolio.addMoney(1100d, KRW);

        assertThat(portfolio.evaluate(KRW, bank))
                .isEqualTo(2200);
    }
}
