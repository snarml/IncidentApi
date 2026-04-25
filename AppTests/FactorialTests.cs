using IncidentApiRimel.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTests
{
    public class FactorialTests
    {
        [Theory]
        [InlineData(5, 120)]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        public void Factorial_ValidInputs_ReturnsExpectedResult(int input, int expected)
        {
            var mathematics = new Mathematics();
            var result = mathematics.Factorial(input);

            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(-3)]
        [InlineData(-10)]
        public void Factorial_NegativeInputs_ThrowsArgumentException(int input)
        {
            var mathematics = new Mathematics();
            Assert.Throws<ArgumentException>(() => mathematics.Factorial(input));
        }
    }
}
