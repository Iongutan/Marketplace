namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE — Implementare concretă: redare pe telefon mobil.
    /// Rezoluție redusă, optimizat pentru ecran mic și date mobile.
    /// </summary>
    public class PhoneRenderer : IMediaRenderer
    {
        public string DeviceName => "Phone";

        public string RenderAudio(string trackName, int durationSeconds)
            => $"[📱 Phone] ▶ Audio: \"{trackName}\" ({durationSeconds}s) — mono, 128kbps";

        public string RenderVideo(string videoTitle, string resolution)
            => $"[📱 Phone] ▶ Video: \"{videoTitle}\" @ 480p (adaptat din {resolution}) — date mobile optimizate";
    }
}
