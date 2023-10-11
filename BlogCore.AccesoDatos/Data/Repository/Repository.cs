//AQUI SE IMPLEMENTAN LOS METODOS DE LA INTERFAZ

using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbset;

        //creamos el constructor
        public Repository(DbContext context) 
        {
            Context = context;
            this.dbset = context.Set<T>();
        }

        //nos trae todos los metodos de IREPOSITORY
        public void Add(T entity)//metodo generico
        {
             dbset.Add(entity);
        }

        public T Get(int id)
        {
            return dbset.Find(id); //byscara por id
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBY, string includeProperties)
        {
            IQueryable<T> query = dbset; 
            if (filter != null)
            {
                //mandamos un where
                query = query.Where(filter); 
            }
            //Include propertiesm separados por comas para traer datos relacionandos
            if (includeProperties != null)
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) //elimina todos los espacio
                {
                    query = query.Include(includeProperty);
                }
            }
            //se valida el ordenamiento
             if(orderBY != null)
            {
                return orderBY(query).ToList();
            }
            return query.ToList(); //trae la lista de los registros
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                //mandamos un where
                query = query.Where(filter);
            }
            //Include propertiesm separados por comas para traer datos relacionandos
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) //elimina todos los espacio
                {
                    query = query.Include(includeProperty);
                }
            }
            //para obtener un solo registro
            return query.FirstOrDefault();

        }
        //va a eliminar por id
        public void Remove(int id)
        {
            T entityToRemove = dbset.Find(id);
        }
        //va a eliminar por entidades

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }
    }
}
