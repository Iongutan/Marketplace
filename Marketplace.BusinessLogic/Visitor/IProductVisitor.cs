using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Visitor
{
    /// <summary>
    /// Interfața Visitor pentru paternul Visitor (Lab 7).
    /// Permite adăugarea de operații noi asupra produselor fără a modifica clasa Product.
    /// </summary>
    public interface IProductVisitor
    {
        void Visit(VisitableProduct product);
        string GetResult();
    }
}
