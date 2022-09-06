using Lab2CHM.Calc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SturmMethodTest
    {
        [TestMethod]
        public void ParsingTest()
        {
            var p = Polynomial.FromFunction("x^5-25x^4+35x^3-5x^2-8x+2");
            var ser = new SturmSeries(p);
            //root bounds
            double precis = 0.01;
            var meth = new SturmMethod(ser, precis);
            var bds = meth.LastResult.ToList();
            Assert.IsTrue(bds[0].Key < -0.463 && bds[0].Value >= -0.463);
            Assert.IsTrue(bds[1].Key < 0.277 && bds[1].Value >= 0.277);
            Assert.IsTrue(bds[2].Key < 0.664 && bds[2].Value >= 0.664);
            Assert.IsTrue(bds[3].Key < 1 && bds[3].Value >= 1);
            Assert.IsTrue(bds[4].Key < 23.522 && bds[4].Value >= 23.522);

            Assert.IsTrue(bds[0].Value - bds[0].Key < precis);
            Assert.IsTrue(bds[1].Value - bds[1].Key < precis);
            Assert.IsTrue(bds[2].Value - bds[2].Key < precis);
            Assert.IsTrue(bds[3].Value - bds[3].Key < precis);
            Assert.IsTrue(bds[4].Value - bds[4].Key < precis);
        }
    }
}
