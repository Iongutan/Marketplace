namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE — Abstractizare rafinată (Refined Abstraction): Conținut Video.
    /// Reprezintă un produs digital de tip video (curs online, film, tutorial)
    /// din catalogul marketplace. Redarea se face prin bridge-ul IMediaRenderer.
    /// </summary>
    public class VideoContent : MediaContent
    {
        public string NativeResolution { get; }

        public VideoContent(string title, string fileFormat,
                            IMediaRenderer renderer, string nativeResolution = "1080p")
            : base(title, fileFormat, renderer)
        {
            NativeResolution = nativeResolution;
        }

        public override string Play()
            => _renderer.RenderVideo(Title, NativeResolution);
    }
}
