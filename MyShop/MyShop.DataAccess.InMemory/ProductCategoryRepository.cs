using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productsCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productsCategories"] = productCategories;
        }

        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void Update(ProductCategory category)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == category.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = category;
            }
            else
            {
                throw new Exception("ProductCategory not found");
            }

        }

        
        public ProductCategory Find(string Id)
        {
            ProductCategory category = productCategories.Find(p => p.Id == Id);
            if (category != null)
            {
                return category;
                // cannot implicitly convert the type ProductCategory to Product
            }
            else
            {
                throw new Exception("Category not found");
            }
        }

        public ProductCategory FindByName(string Name)
        {
            ProductCategory category = productCategories.Find(p => p.Category == Name);
            if (category != null)
            {
                return category;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory categoryToDelete = productCategories.Find(p => p.Id == Id);
            if (categoryToDelete != null)
            {
                productCategories.Remove(categoryToDelete);
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }
}


