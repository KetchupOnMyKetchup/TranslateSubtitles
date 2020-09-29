using System.Collections.Generic;
using System.IO;

namespace TranslateSubtitles
{
    public class SrtFormatReaderWriter : ReaderWriterBase
    {
        public SrtFormatReaderWriter(string readFilePath, string writeToFilePath) : base(readFilePath, writeToFilePath) { }

        public override void Prepare()
        {
            return;
        }

        public override Subtitle ReadSubtitle()
        {
            Subtitle subtitle = new Subtitle
            {
                Number = sr.ReadLine(),
                Timing = sr.ReadLine()
            };
            string curr;
            while ((curr = sr.ReadLine()) != null && curr.Length > 0)
            {
                subtitle.Text += curr + '\n'; // change to list later and join them
            }

            if (subtitle.Text != null) subtitle.Text = subtitle.Text.Remove(subtitle.Text.Length - 1);

            return subtitle;
        }

        public override void WriteSubtitles(List<Subtitle> subs)
        {
            using FileStream fs = new FileStream(WriteToFilePath, FileMode.Append, FileAccess.Write);
            using StreamWriter sw = new StreamWriter(fs);
            foreach (var sub in subs)
            {
                sw.WriteLine(sub.Number);
                sw.WriteLine(sub.Timing);
                sw.WriteLine(sub.Text);
                sw.WriteLine();
            }
        }

        public override void AppendText(Subtitle sub, string text)
        {
            if (sub.Text == null || text == null) return;
            sub.Text = sub.Text.TrimEnd() + "\n" + text.TrimStart();
        }
    }
}
