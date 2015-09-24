using System.Collections.Generic;

namespace AlgorithmsCore
{
    interface IDataLoader
    {
        IEnumerable<string> LoadData();
    }
}
