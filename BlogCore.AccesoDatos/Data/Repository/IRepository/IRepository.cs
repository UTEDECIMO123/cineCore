//INTERFAZ DONDE SE DEFINEN LOS METODOS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        //METODO QUE NOS A VA A PERMITIR A TRAER TODOS LOS DATOS DE LA BD
        //la T representa generico
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBY = null,
            String includeProperties = null
            );

        //metodo que nos permite traer un valor unico por su valor
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            String includeProperties = null  
            );
        //metodo que no retornan nada
        void Add( T item );
        void Remove( int id );
        void Remove( T entity );
    }

}
