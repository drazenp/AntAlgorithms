using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void ReadVerticesWeights_CorrectFormat_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");

        }
    }
}