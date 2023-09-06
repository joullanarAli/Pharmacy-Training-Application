using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDB.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task <T> GetById(int id);   
        Task<IEnumerable<T>> GetAll();  
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
