﻿using Gpm.Common.Util;
using System.Text;

namespace Gpm.CacheStorage
{
    public struct GpmCacheResult
    {
        public CacheInfo Info
        {
            get;
            private set;
        }

        public byte[] Data
        {
            get;
            private set;
        }

        public string Text
        {
            get
            {
                return GetTextData();
            }
        }

        public GpmCacheResult(CacheInfo info = null, byte[] data = null)
        {
            this.Info = info;
            this.Data = data;
        }

        public bool IsSuccess()
        {
            return (Data != null);
        }

        public string GetTextData()
        {
            return GetTextData(Encoding.UTF8);
        }

        public string GetTextData(Encoding encoding)
        {
            return encoding.GetString(Data);
        }

        public T GetJsonData<T>()
        {
            return GetJsonData<T>(Encoding.UTF8);
        }

        public T GetJsonData<T>(Encoding encoding)
        {
            string text = GetTextData(encoding);

            return GpmJsonMapper.ToObject<T>(text);
        }
    }
}