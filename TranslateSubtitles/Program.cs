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
            
            // TODO: convert encoding of sub to UTF-8
            Console.WriteLine("Open subs in Notepad++ > Convert > Convert to UTF-8");

            // TODO: troubleshoot Google Translate issue, when batched needs newline to differentiate lines. will try to translate a special character used as a delimiter and won't send it back. 
            int batchCount = 1; // number subs to send at a time to google translate

            string existingSubFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HTTYD1.srt";
            string type = existingSubFilePath.Substring(existingSubFilePath.IndexOf('.') + 1);

            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HTTYD1-translated.sub";

            if (File.Exists(fileName)) File.Delete(fileName);

            ReaderWriterBase sfr;

            if (type == "sub") sfr = new SubFormatReaderWriter(existingSubFilePath, fileName);
            else sfr = new SrtFormatReaderWriter(existingSubFilePath, fileName);

            using (sfr)
            {
                List<Subtitle> batch;
                sfr.Prepare();

                while ((batch = sfr.ReadSubtitles(batchCount)).Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var sub in batch)
                    {
                        sb.Append(sub.Text + " @ ");
                    }
                    string untranslated = sb.ToString().Remove(sb.Length - 3);
                    string translatedLines = _translateService.TranslateText(untranslated, "en", "bg");
                    string[] translatedLineArr = translatedLines.Split('@');


                    for (int i = 0; i < batch.Count; i++)
                    {
                        if (batch[i].Text == null || batch[i].Text.Length == 0) break;

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
