using System.Linq;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{

    [TestClass]
    public class AntSystemTests
    {
        [TestMethod]
        public void Treil_AllOnesSet_Success()
        {
            var antSystem = new AntSystem(3, 7);

            var sumOfTreil = 0M;
            foreach (var value in antSystem.Treil)
            {
                sumOfTreil += value;
            }

            Assert.AreEqual(21,sumOfTreil);
        }

        [TestMethod]
        public void SumOfWeight_AllZerosSet_Success()
        {
            var antSystem = new AntSystem(3, 7);

            var sumOfWeights = antSystem.SumOfRegionWeight.Sum();

            Assert.AreEqual(3, sumOfWeights);
        }
    }
}
