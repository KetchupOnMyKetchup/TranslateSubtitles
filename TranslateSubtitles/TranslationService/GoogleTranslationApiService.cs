using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using System;

namespace TranslateSubtitles
{
    public class GoogleTranslationApiService
    {
        public string TranslateText(string text, string targetLanguage, string sourceLanguage)
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
