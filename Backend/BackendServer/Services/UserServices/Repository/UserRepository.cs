using BackendServer.Data;
using BackendServer.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendServer.Services.UserServices.Repository;

public class UserRepository(UserManager<User> userManager, ApiDbContext context/*, ITokenService tokenService*/) : IUserRepository
{
    public async Task CreateUser(User user, string password, string role)
    {
        var result = await userManager.CreateAsync(user, password);
        if (result != IdentityResult.Success) throw new Exception(result.ToString());
        
        await userManager.AddToRoleAsync(user, "User");
    }

    public Task<bool> AreCredentialsTaken(string email, string username)
    {
        return context.Users.AnyAsync(u => u.Email == email || u.UserName == username);
    }

    public async Task<string> LoginUser(string email, string password)
    {
        var managedUser = await userManager.FindByEmailAsync(email);
        if (managedUser == null) throw new Exception("Invalid credentials");
        var isPasswordCorrect = await userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordCorrect) throw new Exception("Invalid credentials");
        var roles = await userManager.GetRolesAsync(managedUser);
        //var token = tokenService.CreateToken(managedUser, roles[0]);
        //managedUser.SessionToken = token;
        await userManager.UpdateAsync(managedUser);
        return token;
    }

    public bool IsUserLoggedIn(string username)
    {
        return (context.Users.FirstOrDefault(u => u.UserName == username) ?? 
               throw new Exception("user could not be found")).SessionToken != string.Empty;
    }

    public async Task LogoutUser(string username)
    {
        var managedUser = await userManager.FindByNameAsync(username) ??
                          throw new Exception("user could not be found");
        managedUser.SessionToken = string.Empty;
        await userManager.UpdateAsync(managedUser);
    }

    public async ValueTask<User?> GetUserById(string id)
    {
        return await context.Users
            .Include(u => u.Problems)
            //.Include(u => u.Fixes)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateKarma(User user, int karma)
    {
        user.Karma += karma;
        await userManager.UpdateAsync(user);
    }

    public async ValueTask<User?> GetUserByUsername(string username)
    {
        return await context.Users
            .Include(u => u.Problems)
            //.Include(u => u.Fixes)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async ValueTask<User?> GetUserOnlyProblems(string username)
    {
        return await context.Users
            .Include(u => u.Problems)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
}