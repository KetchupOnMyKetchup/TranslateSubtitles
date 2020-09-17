using System;
using System.Collections.Generic;
using System.IO;

namespace TranslateSubtitles
{
    public abstract class ReaderWriterBase : IDisposable
    {
        protected StreamReader sr;
        protected string WriteToFilePath;

        public ReaderWriterBase(string readFilePath, string writeToFilePath)
        {
            sr = File.OpenText(readFilePath);
            WriteToFilePath = writeToFilePath;
        }

        public abstract void Prepare();


        public abstract Subtitle ReadSubtitle();

        public List<Subtitle> ReadSubtitles(int count)
        {
            var list = new List<Subtitle>();
            Subtitle curr;
            while (list.Count < count && (curr = ReadSubtitle()) != null)
            {
                list.Add(curr);
            }

            return list;
        }

        public abstract void WriteSubtitles(List<Subtitle> subs);

        public abstract void AppendText(Subtitle sub, string text);

        public void Dispose()
        {
            sr.Dispose();
            sr = null;
        }
    }
}
