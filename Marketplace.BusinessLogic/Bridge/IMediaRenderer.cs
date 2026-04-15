namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE PATTERN — Interfața de Implementare (Implementor).
    /// Separă "cum se redă" de "ce se redă".
    /// Implementările concrete: PhoneRenderer, TabletRenderer, TvRenderer.
    /// Abstractizările (MediaContent, AudioContent, VideoContent) dețin
    /// o referință la această interfață — aceasta este "puntea" (bridge-ul).
    /// </summary>
    public interface IMediaRenderer
    {
        string DeviceName { get; }

        /// <summary>Redă conținut audio pe dispozitivul concret.</summary>
        string RenderAudio(string trackName, int durationSeconds);

        /// <summary>Redă conținut video pe dispozitivul concret.</summary>
        string RenderVideo(string videoTitle, string resolution);
    }
}
