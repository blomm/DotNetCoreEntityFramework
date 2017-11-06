using System.Collections.Generic;
using DutchTreat.Database.Entities;

namespace DutchTreat.Database.Repos{

    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    } 

}