using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DutchTreat.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

//using DutchTreat.Database;


namespace DutchTreat.Database
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _env;

        private readonly UserManager<DutchUser> _usrMgr;

        public DutchSeeder(DutchContext ctx, 
            IHostingEnvironment env
            ,UserManager<DutchUser> usrMgr
            )
        {
            _ctx = ctx;   
            _env = env; 
            _usrMgr = usrMgr;
        }

        public async Task Seed()
        //public void Seed()
        {
            var myuser = await _usrMgr.FindByEmailAsync("mike.blom@outlook.com");
            if(myuser==null)
            {
                myuser = new DutchUser()
                {
                    FirstName="Michael",
                    LastName="Blom",
                    Email="mike.blom@outlook.com",
                    UserName = "mike.blom@outlook.com"
                };

                var res = await _usrMgr.CreateAsync(myuser, "P@ssw0rd!");
                if(res!=IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create new user");
                }
            }
            
            var path = Path.Combine(_env.ContentRootPath, "seed/art.json");
            var json = File.ReadAllText(path);
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

            _ctx.Products.AddRange(products);

            var order = new Order(){
                OrderDate=DateTime.Now,
                OrderNumber = "12",
                User = myuser,
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