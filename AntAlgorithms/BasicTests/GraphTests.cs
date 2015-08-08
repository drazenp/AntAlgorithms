using System.IO;
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

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(@"tc
                                4
                                3
                                8
                                7
                                5
                                3
                                6
                                7
                                2");
                writer.Flush();
                stream.Position = 0;
                
                graph.ReadVerticesWeights(stream);
            }

            Assert.AreEqual(9, graph.VerticesWeights.Count);
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}