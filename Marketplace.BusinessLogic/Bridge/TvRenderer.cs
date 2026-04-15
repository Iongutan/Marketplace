namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE — Implementare concretă: redare pe televizor (Smart TV).
    /// Calitate maximă, sunet surround, rezoluție 4K.
    /// </summary>
    public class TvRenderer : IMediaRenderer
    {
        public string DeviceName => "Smart TV";

        public string RenderAudio(string trackName, int durationSeconds)
            => $"[📺 Smart TV] ▶ Audio: \"{trackName}\" ({durationSeconds}s) — Dolby Surround 5.1, lossless";

        public string RenderVideo(string videoTitle, string resolution)
            => $"[📺 Smart TV] ▶ Video: \"{videoTitle}\" @ 4K UHD (upscaled din {resolution}) — HDR10+, Dolby Vision";
    }
}
