namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE — Abstractizare rafinată (Refined Abstraction): Conținut Audio.
    /// Reprezintă un produs digital de tip audio (muzică, podcast, audiobook)
    /// din catalogul marketplace. Redarea se face prin bridge-ul IMediaRenderer.
    /// </summary>
    public class AudioContent : MediaContent
    {
        public int DurationSeconds { get; }

        public AudioContent(string title, string fileFormat,
                            IMediaRenderer renderer, int durationSeconds)
            : base(title, fileFormat, renderer)
        {
            DurationSeconds = durationSeconds;
        }

        public override string Play()
            => _renderer.RenderAudio(Title, DurationSeconds);
    }
}
