using System.Collections.Generic;

namespace AlgorithmsCore.Contracts
{
    public interface IDataLoader
    {
        IEnumerable<string> LoadData();
    }
}
