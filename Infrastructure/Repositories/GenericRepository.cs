using Application.Interfaces.RepositoryInterfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly RealDatabase _realDatabase;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(RealDatabase realDatabase)
        {
            _realDatabase = realDatabase;
            _dbSet = _realDatabase.Set<T>();
        }

        public T GetById(Guid id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the entity with ID {id}.", ex);
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all entities.", ex);
            }
        }

        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _realDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the entity.", ex);
            }
        }

        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _realDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the entity.", ex);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _realDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the entity.", ex);
            }
        }
    }


}
