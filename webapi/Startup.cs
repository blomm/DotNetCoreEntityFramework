using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Database;
using Api.Database.Entities;
using DutchTreat.Database;
using DutchTreat.Database.Entities;
using DutchTreat.Database.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        private IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder.WithOrigins("http://localhost:5050"));
                    options.AddPolicy("AllowReactBoiler3000",
                        builder => builder.WithOrigins("http://localhost:3000"));
                });

            // services.AddIdentity<CampUser, IdentityRole>()
            //    .AddEntityFrameworkStores<CampContext>();
            
            // services.AddDbContext<CampContext>(options =>
            //     options.UseSqlServer(_config.GetConnectionString("CampConnection")));
            services.AddIdentity<DutchUser, IdentityRole>(cfg=>cfg.User.RequireUniqueEmail=true)
               .AddEntityFrameworkStores<DutchContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };

                });


            services.AddDbContext<DutchContext>(options => 
                options.UseSqlServer(_config.GetConnectionString("DutchTreatConnection")));
            
            // how to seed: https://forums.asp.net/t/2122687.aspx?How+to+Seed+Data+in+Core+
            //services.AddTransient<CampDbInitializer>();
            //services.AddTransient<CampIdentityInitializer>();
            //seed the dutch-treat db
            services.AddTransient<DutchSeeder>();

            //services.AddScoped<ICampRepository, CampRepository>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            //Testing would looke like:
            //services.AddScoped<IDutchRepository, MockDutchRepository>();

            
            
            services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                IApplicationBuilder app, 
                IHostingEnvironment env
                //CampDbInitializer campSeeder,
                //CampIdentityInitializer campIdSeeder
                //ILoggerFactory loggerFactory
                //DutchSeeder _dutchSeeder
            )
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("we in dev");
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddDebug();

            app.UseCors("AllowSpecificOrigin");
            app.UseCors("AllowReactBoiler3000");
            app.UseAuthentication();
            app.UseMvc();
            //campSeeder.Seed().Wait();
            //campIdSeeder.Seed().Wait();

            //this is async
            //_dutchSeeder.Seed();
            using(var scope = app.ApplicationServices.CreateScope()){
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed().Wait();
            }
        }
    }
}
