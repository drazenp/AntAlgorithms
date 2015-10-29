using System.Collections.Generic;

namespace AlgorithmsCore.Contracts
{
    interface IDataLoader
    {
        IEnumerable<string> LoadData();
    }
}
