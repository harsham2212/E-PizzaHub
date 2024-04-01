using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Implementation
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected IRepostiory<TEntity> _repo;
        
        public Service(IRepostiory<TEntity> repo)
        {
            _repo = repo;
        }
        public void Add(TEntity entity)
        {
           _repo.Add(entity);
            _repo.SaveChanges();
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
            _repo.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repo.GetAll();
        }

        public TEntity GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void Update(TEntity entity)
        {
            _repo .Update(entity);
            _repo.SaveChanges();
        }
    }
}
