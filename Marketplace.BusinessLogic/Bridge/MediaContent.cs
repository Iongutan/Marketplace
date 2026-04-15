namespace Marketplace.BusinessLogic.Bridge
{
    /// <summary>
    /// BRIDGE PATTERN — Abstractizarea de bază (Abstraction).
    /// Conține o referință la IMediaRenderer (implementarea) — aceasta este "puntea".
    /// Subclasele (AudioContent, VideoContent) sunt "Refined Abstractions".
    ///
    /// Avantaj: Putem adăuga un nou tip de media (PodcastContent, LiveStreamContent)
    /// sau un nou dispozitiv (SmartWatchRenderer) INDEPENDENT, fără a combina toate variantele.
    /// Fără Bridge: N tipuri × M dispozitive = N×M clase.
    /// Cu Bridge:    N tipuri + M dispozitive = N+M clase.
    /// </summary>
    public abstract class MediaContent
    {
        /// <summary>Referința la implementare — puntea spre dispozitiv.</summary>
        protected readonly IMediaRenderer _renderer;

        public string Title { get; }
        public string FileFormat { get; }

        protected MediaContent(string title, string fileFormat, IMediaRenderer renderer)
        {
            Title = title;
            FileFormat = fileFormat;
            _renderer = renderer;
        }

        /// <summary>Redă conținutul folosind implementarea curentă.</summary>
        public abstract string Play();

        /// <summary>Returnează informații despre conținut și dispozitiv.</summary>
        public string GetInfo()
            => $"\"{Title}\" ({FileFormat}) → pe {_renderer.DeviceName}";
    }
}
