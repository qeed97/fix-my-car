﻿using BackendServer.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace BackendServer.Services.AuthenticationServices.AuthenticationSeeder;

public class AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
{
    public void SeedRoles()
    {
        var tAdminRole = CreateAdminRole();
        tAdminRole.Wait();
        var tUserRole = CreateUserRole();
        tUserRole.Wait();
        CreateAdminIfNotExists().Wait();
    }

    private async Task CreateAdminRole()
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    private async Task CreateUserRole()
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@adnim.com");
        if (adminInDb == null)
        {
            var admin = new User
            {
                UserName = "admin", Email = "admin@adnim.com", Karma = 1000000,
            };
            var adminCreated = await userManager.CreateAsync(admin, "admin123");

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}