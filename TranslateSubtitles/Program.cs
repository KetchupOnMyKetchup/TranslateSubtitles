using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;

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

            Console.WriteLine("Open subs in Notepad++ > Convert > Convert to UTF-8");
            string existingSubFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Ratatouille_BG_original.sub";
            // convert encoding of sub to UTF-8

            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Ratatouille_BG_translated.sub";

            if (File.Exists(fileName)) File.Delete(fileName);

            using (StreamWriter sw = File.CreateText(fileName))
            {
                int count = 0;
                StringBuilder sb = new StringBuilder();

                using (StreamReader sr = File.OpenText(existingSubFilePath))
                {
                    string currLine = string.Empty;
                    sw.WriteLine(sr.ReadLine());
                    List<string> list = new List<string>();

                    while ((currLine = sr.ReadLine()) != null)
                    {
                        if (count <= 3)
                        {
                            list.Add(currLine);
                            string lineToTranslate = currLine.Remove(0, currLine.LastIndexOf('}') + 1);
                            sb.Append(lineToTranslate + "\n");
                            count++;
                        }
                        else 
                        {
                            string untranslated = sb.ToString().Remove(sb.Length - 1);
                            string translatedLines = TranslateText(untranslated, "en", "bg");
                            string[] translatedLineArr = translatedLines.Split("\n");

                            for(int i = 0; i < list.Count; i++)
                            {
                                sw.WriteLine(list[i] + "|" + translatedLineArr[i]);
                            }

                            sb.Clear();
                            list.Clear();
                            count = 0;
                        }
                    }
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Success! Done.");
        }

        public static string TranslateText(string text, string targetLanguage, string sourceLanguage)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(text);
            var credential = GoogleCredential.FromFile(@"C:\Keys\BgSubtitlesAddEn-58905f3982e4.json");
            TranslationClient client = TranslationClient.Create(credential);
            var response = client.TranslateText(
                text: text,
                targetLanguage: targetLanguage,
                sourceLanguage: sourceLanguage);
            Console.WriteLine(response.TranslatedText);
            return response.TranslatedText;
        }
    }
}
