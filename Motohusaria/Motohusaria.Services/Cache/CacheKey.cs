using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services
{
    public class CacheKey
    {
        /// <summary>
        /// Obiekt przechowujący informacje o kluczu do cache'owana danych.
        /// </summary>
        /// <param name="keyName">Nazwa klucza pod podawanego do cache</param>
        /// <param name="cacheTime">Na ile minut cacheowane są dane</param>
        public CacheKey(string keyName, int cacheTime)
        {
            KeyName = keyName;
            CacheTime = cacheTime;
        }

        /// <summary>
        /// Nazwa klucza pod podawanego do cache
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Na ile minut cacheowane są dane
        /// </summary>
        public int CacheTime { get; set; }
    }
}
