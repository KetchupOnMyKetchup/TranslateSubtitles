using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TranslateSubtitles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GoogleTranslationApiService _translateService = new GoogleTranslationApiService();

            Console.WriteLine("Open subs in Notepad++ > Convert > Convert to UTF-8");
            //string existingSubFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Ratatouille_BG_original.sub";
            string existingSubFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Frozen-Bulgarian.srt";
            //string type = "sub";
            string type = "srt";
            // convert encoding of sub to UTF-8

            //string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Ratatouille_BG_translated.sub";
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Frozen-Bulgarian-translated.srt";

            if (File.Exists(fileName)) File.Delete(fileName);

            ReaderWriterBase sfr;

            if (type == "sub") sfr = new SubFormatReaderWriter(existingSubFilePath, fileName);
            else sfr = new SrtFormatReaderWriter(existingSubFilePath, fileName);

            using (sfr)
            {
                List<Subtitle> batch;
                sfr.Prepare();

                while ((batch = sfr.ReadSubtitles(2)).Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var sub in batch)
                    {
                        sb.Append(sub.Text + '@');
                    }
                    string untranslated = sb.ToString().Remove(sb.Length - 1);
                    string translatedLines = _translateService.TranslateText(untranslated, "en", "bg");
                    string[] translatedLineArr = translatedLines.Split("@");

                    for (int i = 0; i < batch.Count; i++)
                    {
                        sfr.AppendText(batch[i], translatedLineArr[i]);
                    }

                    sfr.WriteSubtitles(batch);
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Success! Done.");
        }
    }
}
