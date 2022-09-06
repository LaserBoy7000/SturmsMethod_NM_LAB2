using Lab2CHM.Calc;

namespace Tests
{
    [TestClass]
    public class PolynomialTest
    {
        [TestMethod]
        public void TestMultiplication()
        {
            var p1 = Polynomial.FromFunction("6x^3-4x^2+3");
            var p2 = Polynomial.FromFunction("4x-3");
            var r = p1 * p2;
            Assert.AreEqual("24,00x^4 - 34,00x^3 + 12,00x^2 + 12,00x - 9,00", r.ToString());
            var p3 = Polynomial.FromEquation("-5x^2+7 = 2x");
            var p4 = Polynomial.FromFunction("4x-3.6");
            var r2 = p3 * p4;
            Assert.AreEqual(" - 20,00x^3 + 10,00x^2 + 35,20x - 25,20", r2.ToString());
        }
        [TestMethod]
        public void TestDeviation()
        {
            var p1 = Polynomial.FromEquation("-5x^2+7 = 2x");
            var p2 = Polynomial.FromFunction("4x-3.6");
            var r = p1 / p2;
            Assert.AreEqual(" - 1,25x - 1,63", r.Item1.ToString());
            Assert.AreEqual("1,15", r.Item2.ToString());
        }
        [TestMethod]
        public void TestDifferentiation()
        {
            var p1 = Polynomial.FromEquation("3x^3 + 5x^2 - 1 = x");
            var r = p1.Differentiate();
            Assert.AreEqual("9,00x^2 + 10,00x - 1,00", r.ToString());
        }
    }
}