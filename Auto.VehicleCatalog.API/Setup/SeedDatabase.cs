using System.Collections.Generic;
using System.IO;
using System.Linq;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Auto.VehicleCatalog.API.Setup
{
    // Just seeding the database with some fake data for testing purposes
    public class SeedDatabase
    {
        public static void SeedDomain(DataContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            // If there aren't any vehicles in the database
            if (!context.Vehicles.Any())
            {
                var vehiclesData = File.ReadAllText("Setup/Vehicles.json");
                var vehiclesObjs = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesData);

                var brandsAndModelsData = File.ReadAllText("Setup/BrandsAndModels.json");
                var brandsAndModelsObjs = JsonConvert.DeserializeObject<List<Brand>>(brandsAndModelsData);

                int j = 0;
                foreach (var item in brandsAndModelsObjs)
                {
                    context.Brands.Add(item);

                    for (int i = 0; i < 4; i++)
                    {
                        vehiclesObjs[j].Brand = item;
                        vehiclesObjs[j].Model = item.Models.First();

                        context.Vehicles.Add(vehiclesObjs[j]);

                        j++;
                    }
                }

                context.SaveChanges();
            }

            // If there aren't any users in the database
            if (!userManager.Users.Any())
            {
                var usersData = File.ReadAllText("Setup/Users.json");
                var usersObjs = JsonConvert.DeserializeObject<List<User>>(usersData);

                // We gonna create the admin and public roles
                var roles = new List<Role>
                {
                    new Role { Name = "admin" },
                    new Role { Name = "guest" }
                };
                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                int i = 0;
                // Create users with their roles
                foreach (var user in usersObjs)
                {
                    userManager.CreateAsync(user, "1234").Wait();

                    if (i % 2 == 0) // if even then is admin ;-)
                    {
                        userManager.AddToRoleAsync(user, "admin").Wait();
                    }
                    else
                    {
                        userManager.AddToRoleAsync(user, "guest").Wait();
                    }

                    i++;
                }
            }
        }
    }
}
