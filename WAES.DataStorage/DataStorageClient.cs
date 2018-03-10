using System;
using System.Collections.Generic;
using System.Linq;
using WAES.Shared;

namespace WAES.DataStorage
{
    public class DataStorageClient : IDataStorageClient
    {
        private readonly Dictionary<int, BytesToCompare> _storedge;

        public DataStorageClient()
        {
            _storedge = new Dictionary<int, BytesToCompare>();
        }

        //Inserting or updating values in datastoredge
        public void Upsert(int id, string data, Side side)
        {
            if (!_storedge.ContainsKey(id))
            {
                var toCompare = new BytesToCompare();
                switch (side)
                {
                    case Side.Left:
                        toCompare.Left = data;
                        break;
                    case Side.Right:
                        toCompare.Right = data;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(side), side, null);
                }
                _storedge.Add(id, toCompare);
            }
            else
            {
                var stringsToCompare = GetBytesToCompareById(id);
                switch (side)
                {
                    case Side.Left:
                        stringsToCompare.Left = data;
                        break;
                    case Side.Right:
                        stringsToCompare.Right = data;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(side), side, null);
                }
            }
        }

        public bool IsReadyForDiff(int id)
        {
            var stringsToCompare = GetBytesToCompareById(id);
            return stringsToCompare.Left != null && stringsToCompare.Right != null;
        }

        public DiffResultBase GetDiffResult(int id)
        {
            var firstOrDefault = GetBytesToCompareById(id) ?? throw new ArgumentNullException("GetBytesToCompareById(id)");
            return firstOrDefault.DiffResult;
        }


        public BytesToCompare GetBytesToCompareById(int id)
        {
            return _storedge.FirstOrDefault(x => x.Key.Equals(id)).Value;
        }

        public void SaveDiffResult(int id, DiffResultBase resultStatus)
        {
            var stringsToCompareById = GetBytesToCompareById(id);
            stringsToCompareById.DiffResult = resultStatus;
        }
    }
}