using Microsoft.EntityFrameworkCore;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        protected readonly TeamWeeklyStatusContext _context;
        private DbSet<T> _entities;

        public Repository(TeamWeeklyStatusContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public T GetById(int id) => _entities.Find(id);

        public IEnumerable<T> GetAll() => _entities.AsEnumerable();

        public void Add(T entity) => _entities.Add(entity);

        public void Update(T entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity) => _entities.Remove(entity);

        public void SaveChanges() => _context.SaveChanges();
    }
}
