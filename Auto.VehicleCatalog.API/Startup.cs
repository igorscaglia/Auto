using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Auto.VehicleCatalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            void AddIdentityCore()
            {
                // Enable ASP.NET Core Identity
                IdentityBuilder identityBuilder = services.AddIdentityCore<User>(opt =>
                {
                    // For brevity, we are setting minimum security issues. In production, this settings should be revised.
                    opt.Password.RequireDigit = false;
                    opt.Password.RequiredLength = 4;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                });

                identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(Role), identityBuilder.Services);
                identityBuilder.AddEntityFrameworkStores<DataContext>();
                // Enable roles in ASP.NET Core Identity
                identityBuilder.AddRoleValidator<RoleValidator<Role>>();
                identityBuilder.AddRoleManager<RoleManager<Role>>();
                identityBuilder.AddSignInManager<SignInManager<User>>();

                // Enable JWT authentication scheme
                services.AddAuthentication(options =>
               {
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

               }).AddJwtBearer(options =>
               {
                   var authConfigurationSection = Configuration.GetSection("AuthConfiguration");

                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                           .GetBytes(authConfigurationSection.GetValue<string>("TokenValidateSecurityKey"))),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
            }

            // Enable ASP.NET Core Identity with JWT authentication scheme
            AddIdentityCore();

            // Enable AutoMapper
            services.AddAutoMapper(typeof(VehicleCatalogRepository).Assembly);

            // Enable lower case urls
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddControllers(options =>
            {
                // Enable authorization by default only for administrators
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(new [] { "admin" })
                    .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson(options => 
            {
                // Enable NewtonsoftJson features
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Enable CORS
            services.AddCors();

            // Add DataContext based on environment variables passed by CLI
            void AddDataContext(IServiceCollection services)
            {
                var dbHost = Configuration["DBHOST"];
                string connString, dbVer = String.Empty;

                // If is null or empty then we'll use appsettings connection string
                // In theory, this means that we aren't using Docker
                if (!String.IsNullOrEmpty(dbHost))
                {
                    var dbPort = Configuration["DBPORT"];
                    var dbPass = Configuration["DBPASS"];
                    dbVer = Configuration["DBVER"];
                    connString = $"server={dbHost};user=root;password={dbPass};port={dbPort};database=VehicleCatalog;";
                }
                else
                {
                    dbVer = this.Configuration.GetValue<string>("MySqlDatabaseVersion");
                    connString = this.Configuration.GetConnectionString("MySQLConnectionString");
                }

                services.AddDbContext<DataContext>(x =>
                {
                    x.UseLazyLoadingProxies();
                    x.UseMySql(connString, options =>
                    {
                        options.ServerVersion(new ServerVersion(new Version(dbVer)));
                    });
                });
            }

            // Enable EF DbContext
            AddDataContext(services);

            // Enable Repositories
            services.AddScoped<IVehicleCatalogRepository, VehicleCatalogRepository>();

            // Register the Swagger generator, defining 1 or more Swagger documents, and support for JWT token
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auto Vehicle Catalog API", Version = "v1" });

                // Config swagger support for JWT token

                // First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer", //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            }, new List<string>()
                        }
                    }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure our DataContext
            void ConfigureDataContext()
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    try
                    {
                        // Get all by DI
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                        // Apply any migration on database otherwise create the database
                        context.Database.Migrate();

                        // Just seeding if we are in development mode
                        if (env.IsDevelopment())
                        {
                            Setup.SeedDatabase.SeedDomain(context, userManager, roleManager);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Error configuring DataContext");
                    }
                }
            }

            ConfigureDataContext();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auto Vehicle Catalog API V1");

                // To serve the Swagger UI at the app's root (http://localhost:<port>/)
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
