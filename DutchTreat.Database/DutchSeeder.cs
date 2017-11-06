using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DutchTreat.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

//using DutchTreat.Database;


namespace DutchTreat.Database
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _env;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment env)
        {
            _ctx = ctx;   
            _env = env; 
            //_hosting = hosting;
        }

        public void Seed()
        {
            var path = Path.Combine(_env.ContentRootPath, "seed/art.json");
            var json = File.ReadAllText(path);
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

            _ctx.Products.AddRange(products);

            var order = new Order(){
                OrderDate=DateTime.Now,
                OrderNumber = "12",
                Items = new List<OrderItem>(){
                    new OrderItem(){
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price
                    }
                }          
            };

            _ctx.Orders.Add(order);
            _ctx.SaveChanges();
        }
    }
    
}