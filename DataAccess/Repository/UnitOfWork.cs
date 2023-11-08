using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    //    Unit Of Work : Veritabanı ile ilgili tüm işlemlerin tek kanaldan 
    //    yapılmasını sağlayan ve yapılan tüm işlemlerin hafızada tutularak 
    //    toplu halde gerçekleştirilmesini sağlayan bir tasarım desenidir.
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IBookRepository Book { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Book = new BookRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
