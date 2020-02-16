using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services.WordPicker
{
    public interface IWordPicker
    {
        List<string> GetRandomSample(List<string> words, int count);

        void CacheWord(object key, string word, double minutes);

        string GetCachedWord(object key);

        void RemoveCachedWord(object key);
    }
}
