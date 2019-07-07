using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Contracts
{
   public  interface IRepository<T>
    {
        void Commit();
        void Insert(T t);
        void Update(T t);
        T Find(string Id);

        //public T FindByName(string name)
        //{
        //    T product = items.Find(p => p.Name == name);
        //    if (product != null)
        //    {
        //        return product;
        //    }
        //    else
        //    {
        //        throw new Exception("Product not found");
        //    }
        //}

        IQueryable<T> Collection();
        void Delete(string Id);
        
    }
}
