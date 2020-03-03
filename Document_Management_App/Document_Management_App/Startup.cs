using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Document_Management_App.DataAcessesLayer;
using Document_Management_App.DBContext;
using Document_Management_App.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Document_Management_App
{
public class Startup
        {


            public IConfiguration Configuration { get; }
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }


            readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {

                services.AddCors(options =>
                {
                    options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44303").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
                });


                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                services.AddOptions();

                // Configure ConnectionStrings using config
                services.Configure<ConnectionStrings>(Configuration.GetSection("Data"));


                services.AddSingleton<EmployeeInterface, logic>();
                services.AddSingleton<AdminInterface, AdminLogic>();
           

        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseHsts();
                }
                app.UseCors(MyAllowSpecificOrigins);
                app.UseHttpsRedirection();
                app.UseMvc();
            }
        }
    }

