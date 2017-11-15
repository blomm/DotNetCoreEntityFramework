using System;
using System.Collections.Generic;
using System.Linq;
using DutchTreat.Database.Entities;
using Microsoft.EntityFrameworkCore;
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

        public void AddEntity(object order)
        {
            //throw new NotImplementedException();
            _ctx.Add(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _ctx.Orders
                    .Include(o=>o.Items)
                    .ThenInclude(i=>i.Product)
                    .ToList();
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username)
        {
            return _ctx.Orders
                .Where(o=>o.User.UserName == username)
                .Include(o=>o.Items)
                .ThenInclude(i=>i.Product)
                .ToList();        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                    .Where(x=>x.Id == id && x.User.UserName == username)
                    .Include(x=>x.Items)
                    .ThenInclude(i=>i.Product)
                    .FirstOrDefault();
        }

        public IEnumerable<OrderItem> GetOrderItems(string username, int orderid)
        {
            try
            {
                var order = _ctx.Orders
                    .Where(x=>x.Id == orderid && x.User.UserName == username)
                    .Include(x=>x.Items)
                    .FirstOrDefault();
                if(order == null){
                    _logger.LogError($"No order found with order id {orderid}");
                    return null;
                } 
                else return order.Items;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem retrieving order: {ex}");
                return null;
            }
        }

        IEnumerable<Product> IDutchRepository.GetAllProducts()
        {
            //_logger.LogInformation("info: got all the products");
            //_logger.LogDebug("debug: got all the products!!!!!!!!!!!!!!!");
            try{
                return _ctx.Products.OrderBy(p =>p.Title).Take(5).ToList();
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
