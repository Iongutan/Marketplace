using Marketplace.Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.BusinessLogic.TemplateMethod
{
    /// <summary>
    /// Clasa abstractă ReportGenerator — paternul Template Method (Lab 7).
    /// Definește algoritmul general de generare a raportului.
    /// Subclasele suprascriu pașii specifici (PrepareData, RenderBody).
    /// </summary>
    public abstract class ReportGenerator
    {
        protected List<Product> Products { get; set; } = new();

        // ── Template Method ──────────────────────────────────────────────────────
        /// <summary>
        /// Algoritmul general fix — ordinea pașilor nu se modifică.
        /// </summary>
        public string GenerateReport(IEnumerable<Product> products)
        {
            Products = new List<Product>(products);
            var sb = new StringBuilder();

            sb.Append(RenderHeader());
            sb.Append(PrepareData());
            sb.Append(RenderBody());
            sb.Append(RenderSummary());
            sb.Append(RenderFooter());

            return sb.ToString();
        }

        // ── Pași comuni (implementați în clasa de bază) ──────────────────────────
        protected virtual string RenderHeader()
        {
            return $@"
            <div class='report-header p-4 mb-3 rounded-3' style='background:linear-gradient(135deg,#1a1a2e,#16213e);color:#fff;'>
                <h3 class='mb-1'><i class='bi bi-file-earmark-bar-graph me-2'></i>{ReportTitle}</h3>
                <small class='opacity-75'>Generat la: {System.DateTime.Now:dd MMM yyyy, HH:mm} &bull; Total produse analizate: {Products.Count}</small>
            </div>";
        }

        protected virtual string RenderFooter()
        {
            return $@"
            <div class='text-center text-muted mt-4 pt-3 border-top'>
                <small><i class='bi bi-shield-check me-1'></i>Raport generat automat de <strong>Marketplace Platform</strong> &bull; Patern: <em>Template Method</em></small>
            </div>";
        }

        // ── Pași abstracți (implementați de subclase) ────────────────────────────
        public abstract string ReportTitle { get; }
        protected abstract string PrepareData();
        protected abstract string RenderBody();
        protected abstract string RenderSummary();
    }
}
