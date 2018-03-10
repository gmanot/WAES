using WAES.Shared;

namespace WAES.DataStorage
{
    public class BytesToCompare
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public DiffResultBase DiffResult { get; set; }
    }
}