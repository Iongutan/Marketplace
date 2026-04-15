namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE — Implementare concretă: redare pe tabletă.
    /// Rezoluție medie, echilibru între calitate și performanță.
    /// </summary>
    public class TabletRenderer : IMediaRenderer
    {
        public string DeviceName => "Tablet";

        public string RenderAudio(string trackName, int durationSeconds)
            => $"[📟 Tablet] ▶ Audio: \"{trackName}\" ({durationSeconds}s) — stereo, 256kbps";

        public string RenderVideo(string videoTitle, string resolution)
            => $"[📟 Tablet] ▶ Video: \"{videoTitle}\" @ 1080p (adaptat din {resolution}) — Wi-Fi streaming";
    }
}
