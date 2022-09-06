using Lab2CHM.Calc;

namespace Tests
{
    [TestClass]
    public class SturmSeriesTest
    {
        [TestMethod]
        public void CreationAndGenerationTest()
        {
            var p = Polynomial.FromFunction("x^4+x^3-x-1");
            var st = new SturmSeries(p);
            Assert.AreEqual(p.ToString(), st.Series[0].ToString());
            Assert.AreEqual("4,00x^3 + 3,00x^2 - 1,00", st.Series[1].ToString());
            Assert.AreEqual("0,19x^2 + 0,75x + 0,94", st.Series[2].ToString());
            Assert.AreEqual(" - 32,00x - 64,00", st.Series[3].ToString());
            Assert.AreEqual(" - 0,19", st.Series[4].ToString());
        }
    }
}
