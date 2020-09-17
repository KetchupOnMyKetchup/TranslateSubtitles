using System.Collections.Generic;
using System.IO;

namespace TranslateSubtitles
{
    public class SrtFormatReaderWriter : ReaderWriterBase
    {
        public SrtFormatReaderWriter(string readFilePath, string writeToFilePath) : base(readFilePath, writeToFilePath) { }

        public override void Prepare()
        {
            using FileStream fs = new FileStream(WriteToFilePath, FileMode.Append, FileAccess.Write);
            using StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sr.ReadLine());
        }

        public override Subtitle ReadSubtitle()
        {
            string currLine = sr.ReadLine();
            if (currLine == null)
                return null;

            int index = currLine.LastIndexOf('}') + 1;

            return new Subtitle { Timing = currLine.Substring(index), Text = currLine.Substring(0, index) };
        }

        public override void WriteSubtitles(List<Subtitle> subs)
        {
            using FileStream fs = new FileStream(WriteToFilePath, FileMode.Append, FileAccess.Write);
            using StreamWriter sw = new StreamWriter(fs);
            foreach (var sub in subs)
            {
                sw.WriteLine(sub.Number + sub.Text);
            }
        }

        public override void AppendText(Subtitle sub, string text)
        {
            sub.Text += "|" + text;
        }
    }
}
