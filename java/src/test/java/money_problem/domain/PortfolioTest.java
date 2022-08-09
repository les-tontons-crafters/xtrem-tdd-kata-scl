package money_problem.domain;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

class PortfolioTest {

    @Test
    @DisplayName("5 USD + 10 EUR = 17 USD")
    void addUSDAndEuros() {

        var portfolio = new Portfolio();

        portfolio.addMoney(5, Currency.USD);
        portfolio.addMoney(10, Currency.EUR);

        assertThat(portfolio.evaluate(Currency.USD)).isEqualTo(17);

    }

    @Test
    @DisplayName("1 USD + 1100 KRW = 2200 KRW")
    void addUSDAndKRW() {
        var portfolio = new Portfolio();

        portfolio.addMoney(1, Currency.USD);
        portfolio.addMoney(1100, Currency.KRW);

        assertThat(portfolio.evaluate(Currency.KRW))
                .isEqualTo(2200);
    }
}
