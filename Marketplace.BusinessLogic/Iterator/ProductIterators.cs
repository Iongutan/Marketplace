using System.Collections.Generic;
using Marketplace.Domain.Entities;

namespace Marketplace.BusinessLogic.Iterator
{
    public interface IProductIterator
    {
        bool HasNext();
        object Next();
    }

    public interface IProductCollection
    {
        IProductIterator CreateIterator();
    }

    public class ProductCollection : IProductCollection
    {
        private readonly List<Product> _items = new List<Product>();

        public void AddProduct(Product item)
        {
            _items.Add(item);
        }

        public void AddRange(IEnumerable<Product> items)
        {
            _items.AddRange(items);
        }

        public int Count => _items.Count;

        public Product this[int index] => _items[index];

        public IProductIterator CreateIterator()
        {
            return new ProductIterator(this);
        }
        
        public IProductIterator CreateCategoryIterator(string category)
        {
            return new CategoryFilterIterator(this, category);
        }
    }

    public class ProductIterator : IProductIterator
    {
        private readonly ProductCollection _collection;
        private int _position = 0;

        public ProductIterator(ProductCollection collection)
        {
            _collection = collection;
        }

        public bool HasNext()
        {
            return _position < _collection.Count;
        }

        public object Next()
        {
            if (HasNext())
            {
                return _collection[_position++];
            }
            return null;
        }
    }

    public class CategoryFilterIterator : IProductIterator
    {
        private readonly ProductCollection _collection;
        private readonly string _category;
        private int _position = 0;

        public CategoryFilterIterator(ProductCollection collection, string category)
        {
            _collection = collection;
            _category = category;
        }

        public bool HasNext()
        {
            while (_position < _collection.Count)
            {
                if (_collection[_position].Category == _category)
                {
                    return true;
                }
                _position++;
            }
            return false;
        }

        public object Next()
        {
            if (HasNext())
            {
                return _collection[_position++];
            }
            return null;
        }
    }
}
