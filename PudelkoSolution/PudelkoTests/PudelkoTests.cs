using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLib;

namespace PudelkoTests
{
    [TestClass]
    public class PudelkoUnitTests
    {
        [TestMethod]
        public void TestObjetosc()
        {
            var p = new Pudelko(2, 3, 4);
            Assert.AreEqual(24.0, p.Objetosc);
        }

        [TestMethod]
        public void TestPole()
        {
            var p = new Pudelko(2, 3, 4);
            Assert.AreEqual(52.000000, p.Pole);
        }

        [TestMethod]
        public void TestEqualsOperator()
        {
            var p1 = new Pudelko(1, 2, 3);
            var p2 = new Pudelko(2, 1, 3);
            Assert.IsTrue(p1 == p2);
        }

        [TestMethod]
        public void TestAddingPudelka()
        {
            var p1 = new Pudelko(2, 2, 2);
            var p2 = new Pudelko(1, 1, 1);
            var p3 = p1 + p2;
            Assert.IsTrue(p3.Objetosc >= p1.Objetosc + p2.Objetosc);
        }
    }
}
