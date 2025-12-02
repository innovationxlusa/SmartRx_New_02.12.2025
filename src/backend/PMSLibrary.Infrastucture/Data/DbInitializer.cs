using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.Entities;
using PMSBackend.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Infrastructure.Data
{
    public static class DbInitializer
    {       
        public static async Task<bool> SeedData(PMSDbContext _context)
        {
            try
            {               
                // Ensure database is created
                await _context.Database.EnsureCreatedAsync();

                // Seed Roles
                if (!_context.Roles.Any())
                {
                    await _context.Roles.AddRangeAsync(
                        new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.superadmin), IsSelfService = false, Description = "Super Admin is the only one user in the system and all super access has in it and to recover the system only this user can be usable ", CreatedDate=DateTime.Now },
                         new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.admin), IsSelfService = false, Description = "Admin is to manipulated all general access and manage system", CreatedDate = DateTime.Now },
                         new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.entryuser), IsSelfService = true, Description = "An admin user who can enter all data into the system", CreatedDate = DateTime.Now },
                         new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.recommender), IsSelfService = true, CreatedDate = DateTime.Now },
                         new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.approver), IsSelfService = true, CreatedDate = DateTime.Now },
                         new SmartRxRoleEntity { Name = Common.GetEnumDescription(Roles.externaluser), IsSelfService = false, Description = "Only outside user are in this list", CreatedDate = DateTime.Now }
                     );
                }

                if (!_context.PMSUsers.Any())
                {
                    await _context.PMSUsers.AddRangeAsync(
                        new SmartRxUserEntity
                        {
                            UserName = Roles.superadmin.ToString(),
                            UserCode = "0000000001",
                            Status = (int)Status.Active,
                            MobileNo = "01786756453",
                            FirstName = "S. M.",
                            LastName = "Tamzid",
                            Password = "1234",
                            Gender = (int)Gender.Male,
                            AuthMethod = (int)LoginType.UserName,
                            DateOfBirth = Convert.ToDateTime("05-20-1980"),
                            CreatedDate=DateTime.Now
                        },
                         new SmartRxUserEntity
                         {
                             UserName = Roles.admin.ToString(),
                             UserCode = "0000000002",
                             Status = (int)Status.Active,
                             MobileNo = "01786756453",
                             FirstName = "Selim",
                             LastName = "Ahmed",
                             Password = "1234",
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
                              Password = "1234",
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
                             Password = "1234",
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
                             Password = "1234",
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
                             Password = "1234",
                             Gender = (int)Gender.Male,
                             AuthMethod = (int)LoginType.UserName,
                             DateOfBirth = Convert.ToDateTime("08-15-1988"),
                             CreatedDate = DateTime.Now
                         }
                        );
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }           
        }
        public static async Task<bool> SeedDataDependent(PMSDbContext _context)
        {
            try
            {
                // Ensure database is created
                await _context.Database.EnsureCreatedAsync();

                // Seed Data for User wise Role
                if (!_context.UserRoles.Any())
                {
                    var users = await _context.PMSUsers.ToListAsync();
                    var roles = await _context.Roles.ToListAsync();
                    var adminUser=users.FirstOrDefault(u=>u.UserName== Roles.admin.ToString());
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
                                 ? new UserRoleEntity { UserId = user.Id, RoleId = role.Id,CreatedDate=DateTime.Now, CreatedBy= adminUser!.Id }
                                 : null;
                         })
                         .Where(ur => ur != null)
                         .ToList();
                    await _context.UserRoles.AddRangeAsync(userRolesToAdd!);
                }
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}
