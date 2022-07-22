package money_problem.domain;

import org.assertj.core.api.Assertions;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.stream.Stream;

import static money_problem.domain.Currency.*;
import static money_problem.domain.DomainUtility.*;
import static org.assertj.core.api.Assertions.offset;
import static org.assertj.vavr.api.VavrAssertions.assertThat;

class BankTest {
    public static final Currency PIVOT_CURRENCY = EUR;
    private final Bank bank = Bank.withPivotCurrency(PIVOT_CURRENCY);

    @Nested
    class ConvertShould {
        @Nested
        class Fail {
            @Test
            void whenMissingExchangeRate() {
                assertThat(bank.convert(euros(10), KRW))
                        .containsOnLeft("EUR->KRW");
            }

            @Test
            void whenCannotConvertThroughPivotCurrency() {
                assertThat(bank.addExchangeRate(USD, 1.2)
                        .flatMap(newBank -> newBank.convert(koreanWons(10), USD)))
                        .containsOnLeft("KRW->USD");
            }
        }

        @Nested
        class Succeed {
            @Test
            @DisplayName("10 EUR -> USD = 12 USD")
            void whenEuroToUsd() {
                assertThat(bank.addExchangeRate(USD, 1.2)
                        .flatMap(newBank -> newBank.convert(euros(10), USD)))
                        .containsOnRight(dollars(12));
            }

            @Test
            @DisplayName("10 EUR -> EUR = 10 EUR")
            void whenInSameCurrency() {
                assertThat(bank.convert(euros(10), EUR))
                        .containsOnRight(euros(10));
            }

            @Test
            @DisplayName("10 USD -> KRW = 3300 KRW")
            void whenMoneyInAnotherCurrencyThanPivot() {
                assertThat(bank.addExchangeRate(USD, 1.2)
                        .flatMap(b -> b.addExchangeRate(KRW, 1344))
                        .flatMap(b -> b.convert(dollars(10), KRW)))
                        .containsOnRight(koreanWons(11200));
            }

            @Test
            @DisplayName("-2 USD -> EUR = -1.667 EUR")
            void whenNegativeAmount() {
                assertThat(bank.addExchangeRate(USD, 1.2)
                        .flatMap(b -> b.convert(dollars(-2), EUR)))
                        .hasRightValueSatisfying(right ->
                                Assertions.assertThat(right.amount())
                                        .isCloseTo(-1.667, offset(0.001)));
            }

            @Test
            @DisplayName("0 USD -> EUR = 0 EUR")
            void whenZero() {
                assertThat(bank.addExchangeRate(USD, 1.2)
                        .flatMap(b -> b.convert(dollars(0), EUR)))
                        .containsOnRight(euros(0));
            }

            @Test
            @DisplayName("Conversion with different exchange rates EUR to USD")
            void whenExchangeRatesUpdated() {
                var bankWithRate = bank.addExchangeRate(USD, 1.2);

                assertThat(bankWithRate
                        .flatMap(b -> b.convert(euros(10), USD)))
                        .containsOnRight(dollars(12));

                assertThat(bankWithRate
                        .flatMap(b -> b.addExchangeRate(USD, 1.3))
                        .flatMap(b -> b.convert(euros(10), USD)))
                        .containsOnRight(dollars(13));
            }
        }
    }

    @Nested
    class AddShould {
        @Nested
        class Fail {
            private static Stream<Arguments> failingExchangeRates() {
                return Stream.of(
                        Arguments.of(KRW, -1),
                        Arguments.of(KRW, 0),
                        Arguments.of(USD, 0),
                        Arguments.of(USD, -2022)
                );
            }

            @Test
            void whenCurrencyIsPivotCurrency() {
                assertThat(bank.addExchangeRate(PIVOT_CURRENCY, 9))
                        .containsOnLeft("Can not add an exchange rate for the pivot currency");
            }

            @ParameterizedTest
            @MethodSource("failingExchangeRates")
            void whenExchangeRateIsLessOrEqualZero(Currency currency, double rate) {
                assertThat(bank.addExchangeRate(currency, rate))
                        .containsOnLeft("Exchange rate should be greater than 0");
            }
        }

        @Nested
        class Succeed {
            private static Stream<Arguments> successExchangeRates() {
                return Stream.of(
                        Arguments.of(KRW, 1.298989888),
                        Arguments.of(KRW, 345.090988),
                        Arguments.of(USD, 0.00000001455)
                );
            }

            @ParameterizedTest
            @MethodSource("successExchangeRates")
            void whenRateGreaterThan0(Currency currency, double rate) {
                assertThat(bank.addExchangeRate(currency, rate))
                        .isRight();
            }
        }
    }
}