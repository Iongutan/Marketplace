using Marketplace.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Marketplace.BusinessLogic.Visitor
{
    /// <summary>
    /// Interfața pentru elementele vizitabile — paternul Visitor (Lab 7).
    /// </summary>
    public interface IVisitable
    {
        void Accept(IProductVisitor visitor);
    }

    /// <summary>
    /// Wrapper vizitabil pentru Product — permite aplicarea vizitatorilor fără a modifica clasa Product.
    /// </summary>
    public class VisitableProduct : IVisitable
    {
        public Product Product { get; }

        public VisitableProduct(Product product)
        {
            Product = product;
        }

        public void Accept(IProductVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // ── Vizitator 1: Export HTML ────────────────────────────────────────────────
    /// <summary>
    /// Vizitator care exportă produsele ca tabel HTML stilizat.
    /// </summary>
    public class HtmlExportVisitor : IProductVisitor
    {
        private readonly StringBuilder _sb = new();
        private int _count = 0;

        public HtmlExportVisitor()
        {
            _sb.AppendLine("<table class='table table-striped table-hover'>");
            _sb.AppendLine("<thead class='table-dark'><tr><th>#</th><th>Produs</th><th>Categorie</th><th>Brand</th><th>Preț (MDL)</th><th>Stoc</th><th>Tip</th></tr></thead>");
            _sb.AppendLine("<tbody>");
        }

        public void Visit(VisitableProduct vp)
        {
            _count++;
            var p = vp.Product;
            _sb.AppendLine($"<tr><td>{_count}</td><td><strong>{p.Name}</strong></td><td>{p.Category}</td><td>{p.Brand}</td><td>{p.Price:N0}</td><td>{p.Stock}</td><td>{(p.IsDigital == true ? "Digital" : "Fizic")}</td></tr>");
        }

        public string GetResult()
        {
            _sb.AppendLine("</tbody></table>");
            return _sb.ToString();
        }
    }

    // ── Vizitator 2: Export CSV ─────────────────────────────────────────────────
    /// <summary>
    /// Vizitator care exportă produsele în format CSV.
    /// </summary>
    public class CsvExportVisitor : IProductVisitor
    {
        private readonly StringBuilder _sb = new();

        public CsvExportVisitor()
        {
            _sb.AppendLine("Id,Nume,Categorie,Brand,Pret,Stoc,TipDigital");
        }

        public void Visit(VisitableProduct vp)
        {
            var p = vp.Product;
            _sb.AppendLine($"{p.Id},\"{p.Name}\",\"{p.Category}\",\"{p.Brand}\",{p.Price},{p.Stock},{(p.IsDigital == true ? "Da" : "Nu")}");
        }

        public string GetResult() => _sb.ToString();
    }

    // ── Vizitator 3: Export JSON ────────────────────────────────────────────────
    /// <summary>
    /// Vizitator care exportă produsele în format JSON.
    /// </summary>
    public class JsonExportVisitor : IProductVisitor
    {
        private readonly List<object> _items = new();

        public void Visit(VisitableProduct vp)
        {
            var p = vp.Product;
            _items.Add(new
            {
                id = p.Id,
                name = p.Name,
                category = p.Category,
                brand = p.Brand,
                price = p.Price,
                stock = p.Stock,
                isDigital = p.IsDigital ?? false
            });
        }

        public string GetResult()
        {
            return JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
