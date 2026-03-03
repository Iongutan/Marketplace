using System.Collections.Generic;

namespace Marketplace.BusinessLogic.Composite
{
    /// <summary>
    /// Componenta de bază (Component) pentru Composite Pattern.
    /// Atât categoriile (Composite) cât și produsele individuale (Leaf)
    /// implementează această interfață, permițând tratarea lor uniformă.
    /// </summary>
    public interface ICatalogComponent
    {
        string Name { get; }
        string? Description { get; }

        /// <summary>Afișează componenta, indentată la nivelul specificat.</summary>
        void Display(int depth = 0);

        /// <summary>
        /// Returnează numărul total de produse individuale din sub-arbore.
        /// Pentru Leaf = 1, pentru Composite = suma tuturor copiilor.
        /// </summary>
        int GetProductCount();

        /// <summary>Returnează toate produsele individuale din sub-arbore (recursive).</summary>
        IEnumerable<CatalogProduct> GetAllProducts();
    }
}
