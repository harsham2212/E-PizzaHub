using epizzahub.Entitites;
using epizzahub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Repositories.Implemenations
{
    public class Repository<TEntity> : IRepostiory<TEntity> where TEntity : class
    {
        protected readonly AppDBContext db;

        public Repository(AppDBContext db)
        {
            this.db = db;
        }
        public void Add(TEntity entity)
        {
           db.Set<TEntity>().Add(entity);
        }

        public void Delete(int id)
        {
            TEntity entity = db.Set<TEntity>().Find(id);
            if (entity != null)
            db.Set<TEntity>().Remove(entity);   
        }

        public IEnumerable<TEntity> GetAll()
        {
           return db.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
          db.Set<TEntity>().Update(entity);
        }
    }
}
