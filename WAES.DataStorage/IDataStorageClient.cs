using WAES.Shared;

namespace WAES.DataStorage
{
    public interface IDataStorageClient
    {
        void Upsert(int id, string data, Side side);
        bool AreBothValuesPresent(int id);
        DiffResultBase GetDiffResult(int id);
        BytesToCompare GetBytesToCompareById(int id);
        void SaveDiffResult(int id, DiffResultBase resultStatus);
    }
}