using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Databases.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PMSDbContext>();                    // Ensure database is created
                    await context.Database.EnsureCreatedAsync();
                    //await context.Database.MigrateAsync();

                    if (!context.Security_PMSUsers.Any())
                    {
                        var passwordHasher = new PasswordHasher<SmartRxUserEntity>();
                        const string defaultPassword = "Abc@1234";

                        var usersToSeed = new List<SmartRxUserEntity>
                        {
                            new SmartRxUserEntity
                            {
                                UserName = Roles.superadmin.ToString(),
                                UserCode = "0000000001",
                                Status = (int)Status.Active,
                                MobileNo = "01786756453",
                                FirstName = "S. M.",
                                LastName = "Tamzid",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("05-20-1980"),
                                CreatedDate = DateTime.Now
                            },
                            new SmartRxUserEntity
                            {
                                UserName = Roles.admin.ToString(),
                                UserCode = "0000000002",
                                Status = (int)Status.Active,
                                MobileNo = "01786756453",
                                FirstName = "Selim",
                                LastName = "Ahmed",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("10-05-1975"),
                                CreatedDate = DateTime.Now
                            },
                            new SmartRxUserEntity
                            {
                                UserName = Roles.entryuser.ToString(),
                                UserCode = "0000000003",
                                Status = (int)Status.Active,
                                MobileNo = "01786756456",
                                FirstName = "Sharif",
                                LastName = "Uddin",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("08-15-1970"),
                                CreatedDate = DateTime.Now
                            },
                            new SmartRxUserEntity
                            {
                                UserName = Roles.recommender.ToString(),
                                UserCode = "0000000004",
                                Status = (int)Status.Active,
                                MobileNo = "01786756454",
                                FirstName = "Mamun",
                                LastName = "Ahmed",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("05-20-1980"),
                                CreatedDate = DateTime.Now
                            },
                            new SmartRxUserEntity
                            {
                                UserName = Roles.approver.ToString(),
                                UserCode = "0000000005",
                                Status = (int)Status.Active,
                                MobileNo = "01786756455",
                                FirstName = "Ali",
                                LastName = "Akbar",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("05-30-1978"),
                                CreatedDate = DateTime.Now
                            },
                            new SmartRxUserEntity
                            {
                                UserName = Roles.externaluser.ToString(),
                                UserCode = "0000000006",
                                Status = (int)Status.Active,
                                MobileNo = "01786756456",
                                FirstName = "Rakibul",
                                LastName = "Islam",
                                Gender = (int)Gender.Male,
                                AuthMethod = (int)LoginType.UserName,
                                DateOfBirth = Convert.ToDateTime("08-15-1988"),
                                CreatedDate = DateTime.Now
                            }
                        };

                        foreach (var user in usersToSeed)
                        {
                            user.Password = passwordHasher.HashPassword(user, defaultPassword);
                        }

                        await context.Security_PMSUsers.AddRangeAsync(usersToSeed);
                        await context.SaveChangesAsync();
                    }
                }

                await SeedRoleDataAsync(serviceProvider);
                await SeedDataUserRoleAsync(serviceProvider);
                await SeedDataDistrictAsync(serviceProvider);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static async Task SeedRoleDataAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PMSDbContext>();
                    // Seed Roles
                    if (!context.Security_Roles.Any())
                    {
                        await context.Security_Roles.AddRangeAsync(
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.superadmin), IsSelfService = false, Description = "Super Admin is the only one user in the system and all super access has in it and to recover the system only this user can be usable ", CreatedById = 2, CreatedDate = DateTime.Now },
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.admin), IsSelfService = false, Description = "Admin is to manipulated all general access and manage system", CreatedById = 2, CreatedDate = DateTime.Now },
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.entryuser), IsSelfService = true, Description = "An admin user who can enter all data into the system", CreatedById = 2, CreatedDate = DateTime.Now },
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.recommender), IsSelfService = true, CreatedById = 2, CreatedDate = DateTime.Now },
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.approver), IsSelfService = true, CreatedById = 2, CreatedDate = DateTime.Now },
                             new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.externaluser), IsSelfService = false, Description = "Only outside user are in this list", CreatedById = 2, CreatedDate = DateTime.Now }
                         );
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task SeedDataUserRoleAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PMSDbContext>();

                    // Ensure database is created
                    await context.Database.EnsureCreatedAsync();

                    // Seed Data for User wise Role
                    if (!context.Security_UserRoles.Any())
                    {
                        var users = await context.Security_PMSUsers.ToListAsync();
                        var roles = await context.Security_Roles.ToListAsync();
                        var adminUser = users.FirstOrDefault(u => u.UserName == Roles.admin.ToString());
                        var userRolePairs = new List<(string UserName, string RoleName)>
                    {
                        (Roles.superadmin.ToString(), Common.GetEnumDescription(Roles.superadmin)),
                        (Roles.admin.ToString(), Common.GetEnumDescription(Roles.admin)),
                        (Roles.entryuser.ToString(), Common.GetEnumDescription(Roles.externaluser)),
                        (Roles.recommender.ToString(), Common.GetEnumDescription(Roles.recommender)),
                        (Roles.approver.ToString(), Common.GetEnumDescription(Roles.approver)),
                        (Roles.externaluser.ToString(), Common.GetEnumDescription(Roles.externaluser))
                    };

                        var userRolesToAdd = userRolePairs
                             .Select(pair =>
                             {
                                 var user = users.FirstOrDefault(u => u.UserName.Equals(pair.UserName, StringComparison.OrdinalIgnoreCase));
                                 var role = roles.FirstOrDefault(r => r.Name.Equals(pair.RoleName, StringComparison.OrdinalIgnoreCase));

                                 return user != null && role != null
                                     ? new SmartRxUserRoleEntity { UserId = user.Id, RoleId = role.Id, CreatedDate = DateTime.Now, CreatedById = adminUser!.Id }
                                     : null;
                             })
                             .Where(ur => ur != null)
                             .ToList();
                        await context.Security_UserRoles.AddRangeAsync(userRolesToAdd!);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task SeedDataDistrictAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PMSDbContext>();
                    // ensure database is created
                    await context.Database.EnsureCreatedAsync();

                    if (!context.Configuration_District.Any())
                    {
                        await context.Configuration_District.AddRangeAsync(await Common.DistrictList());
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
