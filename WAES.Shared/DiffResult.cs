using System.Collections.Generic;

namespace WAES.Shared
{
    public class DiffResult : DiffResultBase
    {
        public int ByteArrayLength { get; set; }
        public IList<int> OffsetIndexes { get; set; }
    }
}