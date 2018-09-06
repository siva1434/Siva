using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace _360LawGroup.CostOfSalesBilling.Data
{
    public class GenericRepository<TEntity> : IDisposable where TEntity : class
    {
        internal DataEntities Context;
        internal DbSet<TEntity> DbSet;


        private bool _disposed;

        public GenericRepository(DataEntities context)
        {
            this.Context = context;
            DbSet = context.Set<TEntity>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
                
        public virtual IQueryable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if ( filter != null )
                query = query.Where(filter);

            foreach ( var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) )
                query = query.Include(includeProperty);

            if ( orderBy != null )
                return orderBy(query);
            return query;
        }

        public virtual IQueryable<TModel> GetQuery<TModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if ( filter != null )
                query = query.Where(filter);

            foreach ( var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) )
                query = query.Include(includeProperty);

            if ( orderBy != null )
                return orderBy(query).ProjectTo<TModel>();
            return query.ProjectTo<TModel>();
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual TModel GetById<TModel>(object id)
        {
            return Mapper.Map<TModel>(DbSet.Find(id));
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }
        
        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if ( Context.Entry(entityToDelete).State == EntityState.Detached )
                DbSet.Attach(entityToDelete);
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        protected virtual void Dispose(bool disposing)
        {
            if ( !_disposed )
                if ( disposing )
                    Context.Dispose();
            _disposed = true;
        }

       
    }
}