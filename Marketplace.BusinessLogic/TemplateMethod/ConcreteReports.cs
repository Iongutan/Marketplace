using Marketplace.Domain.Entities;
using System.Linq;
using System.Text;

namespace Marketplace.BusinessLogic.TemplateMethod
{
    /// <summary>
    /// Raport Vânzări Produse — personalizează pașii Template Method.
    /// </summary>
    public class ProductSalesReport : ReportGenerator
    {
        public override string ReportTitle => "📊 Raport Vânzări Produse";

        protected override string PrepareData()
        {
            // Sortare după preț descrescător
            Products = Products.OrderByDescending(p => p.Price).ToList();
            return string.Empty;
        }

        protected override string RenderBody()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class='table-responsive'><table class='table table-hover align-middle'>");
            sb.AppendLine("<thead class='table-dark'><tr><th>#</th><th>Produs</th><th>Brand</th><th>Categorie</th><th>Preț (MDL)</th><th>Stoc</th><th>Status</th></tr></thead><tbody>");

            int i = 1;
            foreach (var p in Products)
            {
                string badge = p.Stock > 10 ? "success" : p.Stock > 0 ? "warning" : "danger";
                string stockLabel = p.Stock > 10 ? "Disponibil" : p.Stock > 0 ? "Stoc redus" : "Epuizat";
                sb.AppendLine($"<tr><td>{i++}</td><td><strong>{p.Name}</strong></td><td>{p.Brand}</td><td><span class='badge bg-secondary'>{p.Category}</span></td><td class='fw-bold text-success'>{p.Price:N0}</td><td>{p.Stock}</td><td><span class='badge bg-{badge}'>{stockLabel}</span></td></tr>");
            }

            sb.AppendLine("</tbody></table></div>");
            return sb.ToString();
        }

        protected override string RenderSummary()
        {
            decimal total = Products.Sum(p => p.Price ?? 0);
            decimal avg = Products.Any() ? Products.Average(p => p.Price ?? 0) : 0;
            int outOfStock = Products.Count(p => p.Stock == 0);

            return $@"
            <div class='row g-3 mt-2'>
                <div class='col-md-4'><div class='card border-success text-center p-3'><div class='fs-4 fw-bold text-success'>{total:N0} MDL</div><small class='text-muted'>Valoare totală inventar</small></div></div>
                <div class='col-md-4'><div class='card border-primary text-center p-3'><div class='fs-4 fw-bold text-primary'>{avg:N0} MDL</div><small class='text-muted'>Preț mediu</small></div></div>
                <div class='col-md-4'><div class='card border-danger text-center p-3'><div class='fs-4 fw-bold text-danger'>{outOfStock}</div><small class='text-muted'>Produse epuizate</small></div></div>
            </div>";
        }
    }

    /// <summary>
    /// Raport Activitate Utilizatori — personalizează pașii Template Method.
    /// </summary>
    public class UserActivityReport : ReportGenerator
    {
        public override string ReportTitle => "👥 Raport Activitate Utilizatori";

        protected override string PrepareData()
        {
            // Grupare după UserId
            Products = Products.OrderBy(p => p.UserId).ToList();
            return string.Empty;
        }

        protected override string RenderBody()
        {
            var sb = new StringBuilder();
            var grouped = Products.GroupBy(p => p.UserId ?? 0);

            sb.AppendLine("<div class='row g-3'>");
            foreach (var group in grouped)
            {
                int userId = group.Key;
                int count = group.Count();
                decimal total = group.Sum(p => p.Price ?? 0);
                string[] categories = group.Select(p => p.Category ?? "N/A").Distinct().ToArray();

                sb.AppendLine($@"
                <div class='col-md-6'>
                    <div class='card h-100 shadow-sm'>
                        <div class='card-header bg-dark text-white d-flex justify-content-between align-items-center'>
                            <span><i class='bi bi-person-circle me-2'></i>Utilizator #{userId}</span>
                            <span class='badge bg-warning text-dark'>{count} produse</span>
                        </div>
                        <div class='card-body'>
                            <p class='mb-1'><strong>Valoare listată:</strong> <span class='text-success'>{total:N0} MDL</span></p>
                            <p class='mb-0'><strong>Categorii:</strong> {string.Join(", ", categories)}</p>
                        </div>
                    </div>
                </div>");
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        protected override string RenderSummary()
        {
            int activeUsers = Products.Select(p => p.UserId).Distinct().Count();
            int totalProducts = Products.Count;

            return $@"
            <div class='alert alert-info mt-3'>
                <i class='bi bi-info-circle me-2'></i>
                <strong>{activeUsers}</strong> vânzători activi au listat în total <strong>{totalProducts}</strong> produse pe platformă.
            </div>";
        }
    }

    /// <summary>
    /// Raport Stocuri Inventar — personalizează pașii Template Method.
    /// </summary>
    public class InventoryReport : ReportGenerator
    {
        public override string ReportTitle => "📦 Raport Stocuri Inventar";

        protected override string PrepareData()
        {
            Products = Products.OrderBy(p => p.Stock).ToList();
            return string.Empty;
        }

        protected override string RenderBody()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class='row g-3'>");

            foreach (var p in Products)
            {
                int stock = p.Stock ?? 0;
                string color = stock == 0 ? "danger" : stock < 5 ? "warning" : "success";
                int barWidth = stock == 0 ? 0 : stock > 50 ? 100 : stock * 2;

                sb.AppendLine($@"
                <div class='col-md-6'>
                    <div class='card border-{color} mb-1'>
                        <div class='card-body py-2 px-3'>
                            <div class='d-flex justify-content-between mb-1'>
                                <span class='fw-bold'>{p.Name}</span>
                                <span class='badge bg-{color}'>{stock} buc.</span>
                            </div>
                            <div class='progress' style='height:8px;'>
                                <div class='progress-bar bg-{color}' style='width:{barWidth}%'></div>
                            </div>
                        </div>
                    </div>
                </div>");
            }

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        protected override string RenderSummary()
        {
            int outOfStock = Products.Count(p => p.Stock == 0);
            int lowStock = Products.Count(p => p.Stock > 0 && p.Stock < 5);
            int healthy = Products.Count(p => p.Stock >= 5);

            return $@"
            <div class='row g-3 mt-2'>
                <div class='col-md-4'><div class='card border-success text-center p-3'><div class='fs-4 fw-bold text-success'>{healthy}</div><small class='text-muted'>Stoc sănătos (≥5)</small></div></div>
                <div class='col-md-4'><div class='card border-warning text-center p-3'><div class='fs-4 fw-bold text-warning'>{lowStock}</div><small class='text-muted'>Stoc redus (&lt;5)</small></div></div>
                <div class='col-md-4'><div class='card border-danger text-center p-3'><div class='fs-4 fw-bold text-danger'>{outOfStock}</div><small class='text-muted'>Epuizate</small></div></div>
            </div>";
        }
    }
}
