using WAES.Shared;

namespace WAES.BusinessLogic
{
    public interface IByteDiffClient
    {
        void Upsert(int id, byte[] arrayBytes, Side side);
        DiffResultBase GetDiffResult(int id);
    }
}