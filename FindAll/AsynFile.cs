using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FindAll
{
    public class AsynFile
    {
        public event Action<SortedSet<string>> AccessCompleted;
        private SortedSet<string> asynResult;
        private Dictionary<string, bool> asynSource;
        private object readLock = new object();

        public void FindFiles(string path, string pattern, string word)
        {
            asynSource = new Dictionary<string, bool>();
            asynResult = new SortedSet<string>();

            var fs = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
            foreach (var file in fs)
            {
                asynSource.Add(file, false);
            }

            var files = asynSource.Keys.ToList();
            foreach (var file in files)
            {
                AccessFileAsyn(file, word);
            }
        }

        private void AccessFileAsyn(string file, string word)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.ReadAllText(file).Contains(word))
                    {
                        asynResult.Add(file);
                    }

                    lock (readLock)
                    {
                        asynSource[file] = true;

                        var s = asynSource.Values;
                        if (s.All((p) => p.Equals(true)))
                        {
                            var local = AccessCompleted;
                            if (local != null)
                            {
                                local(asynResult);
                            }
                        }
                    }
                }
                catch
                {
                }
            });
        }
    }
}
