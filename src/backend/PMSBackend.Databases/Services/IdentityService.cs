using Microsoft.AspNetCore.Identity;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Databases.Services
{
    public class IdentityService //: IIdentityService
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly UserManager<PMSUser> _userManager;
        //private readonly SignInManager<PMSUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(IServiceProvider serviceProvider, UserManager<SmartRxUserEntity> userManager, SignInManager<SmartRxUserEntity> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _serviceProvider = serviceProvider;
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
        }

        //public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }

        //        var result = await userManager.AddToRolesAsync(user, roles);
        //        return result.Succeeded;
        //    }
        //}

        //public async Task<bool> CreateRoleAsync(string roleName)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
        //        if (!result.Succeeded)
        //        {
        //            throw new ValidationException(result.Errors);
        //        }
        //        return result.Succeeded;
        //    }
        //}


        //// Return multiple value
        //public async Task<(bool isSucceed, long userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var user = new PMSUser()
        //        {
        //            UserCode = userName,
        //            Password = password,
        //            // FullName = fullName,
        //            UserName = userName
        //            //   Email = email
        //        };
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var result = await userManager.CreateAsync(user, password);

        //        if (!result.Succeeded)
        //        {
        //            throw new ValidationException(result.Errors);
        //        }

        //        var addUserRole = await userManager.AddToRolesAsync(user, roles);
        //        if (!addUserRole.Succeeded)
        //        {
        //            throw new ValidationException(addUserRole.Errors);
        //        }
        //        return (result.Succeeded, user.Id);
        //    }
        //}

        //public async Task<bool> DeleteRoleAsync(string roleId)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var roleDetails = await roleManager.FindByIdAsync(roleId);
        //        if (roleDetails == null)
        //        {
        //            throw new NotFoundException("Role not found");
        //        }

        //        if (roleDetails.Name == "Administrator")
        //        {
        //            throw new BadRequestException("You can not delete Administrator Role");
        //        }
        //        var result = await roleManager.DeleteAsync(roleDetails);
        //        if (!result.Succeeded)
        //        {
        //            throw new ValidationException(result.Errors);
        //        }
        //        return result.Succeeded;
        //    }
        //}

        //public async Task<bool> DeleteUserAsync(string userId)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //            //throw new Exception("User not found");
        //        }

        //        if (user.UserName == "system" || user.UserName == "admin")
        //        {
        //            throw new Exception("You can not delete system or admin user");
        //            //throw new BadRequestException("You can not delete system or admin user");
        //        }
        //        var result = await userManager.DeleteAsync(user);
        //        return result.Succeeded;
        //    }
        //}

        //public async Task<List<(string id, string fullName, string userName, string email)>> GetAllUsersAsync()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var users = await userManager.Users.Select(x => new
        //        {
        //            x.Id,
        //            x.FullName,
        //            x.UserName,
        //            x.Email
        //        }).ToListAsync();

        //        return users.Select(user => (user.Id, user.FullName, user.UserName, user.Email)).ToList();
        //    }
        //}

        //public Task<List<(string id, string userName, string email, IList<string> roles)>> GetAllUsersDetailsAsync()
        //{
        //    throw new NotImplementedException();

        //    //var roles = await _userManager.GetRolesAsync(user);
        //    //return (user.Id, user.UserName, user.Email, roles);

        //    //var users = _userManager.Users.ToListAsync();
        //}

        //public async Task<List<(string id, string roleName)>> GetRolesAsync()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var roles = await roleManager.Roles.Select(x => new
        //        {
        //            x.Id,
        //            x.Name
        //        }).ToListAsync();

        //        return roles.Select(role => (role.Id, role.Name)).ToList();
        //    }
        //}

        //public async Task<(string userId, string fullName, string UserName, string email, IList<string> roles)> GetUserDetailsAsync(string userId)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        var roles = await userManager.GetRolesAsync(user);
        //        return (user.Id, user.FullName, user.UserName, user.Email, roles);
        //    }
        //}

        //public async Task<(string userId, string fullName, string UserName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        var roles = await userManager.GetRolesAsync(user);
        //        return (user.Id, user.FullName, user.UserName, user.Email, roles);
        //    }
        //}

        //public async Task<string> GetUserIdAsync(string userName)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //            //throw new Exception("User not found");
        //        }
        //        return await userManager.GetUserIdAsync(user);
        //    }
        //}

        //public async Task<string> GetUserNameAsync(string userId)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //            //throw new Exception("User not found");
        //        }
        //        return await userManager.GetUserNameAsync(user);
        //    }
        //}

        //public async Task<List<string>> GetUserRolesAsync(string userId)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        var roles = await userManager.GetRolesAsync(user);
        //        return roles.ToList();
        //    }
        //}

        //public async Task<bool> IsInRoleAsync(string userId, string role)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        return await userManager.IsInRoleAsync(user, role);
        //    }
        //}

        //public async Task<bool> IsUniqueUserName(string userName)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        return await userManager.FindByNameAsync(userName) == null;
        //    }
        //}

        //public async Task<bool> SigninUserAsync(string userName, string password)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<PMSUser>>();
        //        var result = await signInManager.PasswordSignInAsync(userName, password, true, false);
        //        return result.Succeeded;
        //    }
        //}

        //public async Task<bool> UpdateUserProfile(string id, string fullName, string email, IList<string> roles)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.FindByIdAsync(id);
        //        user.FullName = fullName;
        //        user.Email = email;
        //        var result = await userManager.UpdateAsync(user);

        //        return result.Succeeded;
        //    }
        //}

        //public async Task<(string id, string roleName)> GetRoleByIdAsync(string id)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var role = await roleManager.FindByIdAsync(id);
        //        return (role.Id, role.Name);
        //    }
        //}

        //public async Task<bool> UpdateRole(string id, string roleName)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        if (roleName != null)
        //        {
        //            var role = await roleManager.FindByIdAsync(id);
        //            role.Name = roleName;
        //            var result = await roleManager.UpdateAsync(role);
        //            return result.Succeeded;
        //        }
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PMSUser>>();
        //        var user = await userManager.FindByNameAsync(userName);
        //        var existingRoles = await userManager.GetRolesAsync(user);
        //        var result = await userManager.RemoveFromRolesAsync(user, existingRoles);
        //        result = await userManager.AddToRolesAsync(user, usersRole);

        //        return result.Succeeded;
        //    }
        //}
    }
}
