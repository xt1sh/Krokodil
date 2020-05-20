using Microsoft.AspNetCore.Hosting;
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
        private readonly IMemoryCache _cache;
        private string WordsFilePath { get; }

        public WordPicker(IMemoryCache memoryCache,
            IWebHostEnvironment env)
        {
            _cache = memoryCache;
            WordsFilePath = Path.Combine(env.ContentRootPath, "words.csv");
        }
        
        public List<string> GetRandomWords(int count)
        {
            var words = new List<string>();
            using (var reader = new StreamReader(WordsFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';').ToList<string>();
                    words.AddRange(values);
                }
            }

            return GetRandomSample(words, count);
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
