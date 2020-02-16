using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services.WordPicker
{
    public class WordPicker : IWordPicker
    {
        private IMemoryCache _cache;

        public WordPicker(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public List<string> GetRandomSample(List<string> words, int count)
        {
            var rnd = new Random();

            return words.OrderBy(x => rnd.Next(words.Count)).Take(count).ToList();
        }

        public void CacheWord(object key, string word, double minutes)
        {
            _cache.Set(key, word, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes)
            });
        }

        public string GetCachedWord(object key)
        {
            string word;

            if (!_cache.TryGetValue(key, out word))
                return null;

            return word;
        }   

        public void RemoveCachedWord(object key)
        {
            _cache.Remove(key);
        }
    }
}
