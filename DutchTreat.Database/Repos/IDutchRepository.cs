using System.Collections.Generic;
using DutchTreat.Database.Entities;

namespace DutchTreat.Database.Repos{

    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(string username, int id);
        IEnumerable<Order> GetAllOrdersByUser(string username);
        IEnumerable<OrderItem> GetOrderItems(string username, int orderid);
        //void AddNewOrder(Order order);
        void AddEntity(object order);
    } 

}