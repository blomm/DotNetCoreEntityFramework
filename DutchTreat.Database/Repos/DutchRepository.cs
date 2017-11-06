using System;
using System.Collections.Generic;
using System.Linq;
using DutchTreat.Database.Entities;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Database.Repos{
    public class DutchRepository:IDutchRepository{

        private DutchContext _ctx;
        private ILogger<DutchRepository> _logger;
        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx=ctx;
            _logger = logger;
            //_logger.LogInformation("into the repo!!!!!!!!!");

        }

        IEnumerable<Product> IDutchRepository.GetAllProducts()
        {
            //_logger.LogInformation("info: got all the products");
            //_logger.LogDebug("debug: got all the products!!!!!!!!!!!!!!!");
            try{
                return _ctx.Products.OrderBy(p =>p.Title).ToList();
            }
            catch (Exception ex){
                _logger.LogError($"error getting products: {ex}");
                return null;
            }
        }

        IEnumerable<Product> IDutchRepository.GetProductsByCategory(string category)
        {
            //_logger.LogInformation("info: got some of the products by category filtering");
            //_logger.LogDebug("debug: got some of the products by category filtering");
            try
            {
                return _ctx.Products.Where(p=>p.Category==category).ToList();

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"error getting results by category: {ex}");
                return null;
            }
        }

        bool IDutchRepository.SaveAll()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
                
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"error saving changes: {ex}");
                return false;
            }
        }
    }
}
