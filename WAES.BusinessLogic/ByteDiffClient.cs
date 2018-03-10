﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WAES.DataStorage;
using WAES.Shared;

namespace WAES.BusinessLogic
{
    public class ByteDiffClient : IByteDiffClient
    {
        private readonly IDataStorageClient _dataStorageClient;

        public ByteDiffClient(IDataStorageClient dataStorageClient)
        {
            _dataStorageClient = dataStorageClient;
        }

        public void Upsert(int id, byte[] arrayBytes, Side side)
         {
             var decodedString = Encoding.UTF8.GetString(arrayBytes);
            _dataStorageClient.Upsert(id, decodedString, side);
            if (_dataStorageClient.IsReadyForDiff(id))
            {
                PerformDiff(id);
            }
        }

        public DiffResultBase GetDiffResult(int id)
        {
            return _dataStorageClient.GetDiffResult(id);
        }

        private void PerformDiff(int id)
        {
            var bytesToCompare = _dataStorageClient.GetBytesToCompareById(id);
            if (bytesToCompare.Left.Equals(bytesToCompare.Right))
            {
                _dataStorageClient.SaveDiffResult(id, new DiffResultBase(){ResultStatusString = ConstantStatusStrings.Equal});
            }
            else if (!bytesToCompare.Left.Length.Equals(bytesToCompare.Right.Length))
            {
                _dataStorageClient.SaveDiffResult(id, new DiffResultBase(){ ResultStatusString = ConstantStatusStrings.NotEqual });
            }
            else if (bytesToCompare.Left.Length.Equals(bytesToCompare.Right.Length))
            {
                var diffResult = new DiffResult() { ResultStatusString = ConstantStatusStrings.SameSizeWithOffset, ByteArrayLenght = bytesToCompare.Left.Length, OffsetIndexes = new List<int>() };
                for (var i = 0; i < bytesToCompare.Left.Length; i++)
                {
                    if (bytesToCompare.Left[i] != bytesToCompare.Right[i])
                    {
                        diffResult.OffsetIndexes.Add(i);
                    }
                }
                _dataStorageClient.SaveDiffResult(id, diffResult);
            }
        }
    }
}