using System;
using System.Collections.Generic;
using System.Linq;
using WAES.Shared;

namespace WAES.DataStorage
{
    public class DataStorageClient : IDataStorageClient
    {
        //this Dictionary is public for unit testing purpose only.
        public readonly Dictionary<int, BytesToCompare> Storage;

        public DataStorageClient()
        {
            Storage = new Dictionary<int, BytesToCompare>();
        }

        /// <summary>
        /// Inserting or updating values in datastoredge 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="side"></param>
        public void Upsert(int id, string data, Side side)
        {
            if (!Storage.ContainsKey(id))
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
                Storage.Add(id, toCompare);
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

        /// <summary>
        /// Checking are record in storage are data ready to be diffed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsReadyForDiff(int id)
        {
            var stringsToCompare = GetBytesToCompareById(id);
            return stringsToCompare.Left != null && stringsToCompare.Right != null;
        }

        /// <summary>
        /// Returnig result from storage.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DiffResultBase GetDiffResult(int id)
        {
            var firstOrDefault = GetBytesToCompareById(id) ?? throw new ArgumentNullException("GetBytesToCompareById(id)");
            return firstOrDefault.DiffResult;
        }

        /// <summary>
        /// Retrieve BytesToCompare from storage by id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BytesToCompare GetBytesToCompareById(int id)
        {
            return Storage.FirstOrDefault(x => x.Key.Equals(id)).Value;
        }

        /// <summary>
        /// Saving result into the storage.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resultStatus"></param>
        public void SaveDiffResult(int id, DiffResultBase resultStatus)
        {
            var stringsToCompareById = GetBytesToCompareById(id);
            stringsToCompareById.DiffResult = resultStatus;
        }
    }
}