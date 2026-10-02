using MyCalculator;

namespace Shop.Test
{
    public class UnitTest1
    {
        [Fact]
        public void SumTest()
        {
            int a = 10, b = 20;
            Calculator calculator = new Calculator();
            int result = calculator.Sum(a, b);

            Assert.Equal(30, result);
        }

        [Fact]
        public void SubtractTest()
        {
            Calculator calculator = new Calculator();

            int result = calculator.Subtract(20, 10);

            Assert.Equal(10, result);
        }

        [Fact]
        public void MultiplyTest()
        {
            Calculator calculator = new Calculator();

            int result = calculator.Multiply(5, 4);

            Assert.Equal(20, result);
        }
    }
}