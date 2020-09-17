namespace TranslateSubtitles
{
    /// <summary>
    /// One subtitle at a certain timestamp out of many subtitles in one movie
    /// </summary>
    public class Subtitle
    {
        public int Number { get; set; }
        public string Timing { get; set; }
        public string Text { get; set; }
    }
}
