using System.Collections.Generic;
using System.Linq;
using Marketplace.Data.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Data.Implementations
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BusinessContext _context;
        private DbSet<T> table;

        public GenericRepository(BusinessContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(int id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            _context.SaveChanges();
        }
    }
}
