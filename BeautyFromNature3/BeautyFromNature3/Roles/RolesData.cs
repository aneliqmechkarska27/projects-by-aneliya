using BeautyFromNature3.Domain;
using Microsoft.AspNetCore.Identity;

namespace BeautyFromNature3.Roles
{
    public class RolesData
    {

        public static void Roles(UserManager<BeautyUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            BeautyRoles(roleManager);
            RolesUsers(userManager);
        }
        public static void RolesUsers(UserManager<BeautyUser> userManager)
        {
            // проверява дали има потребител или не
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new BeautyUser
                {
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "Administrator",
                    Email = "admin@localhost",
                    PhoneNumber = "0878806920"
                };

                // потребител c парола "password"
                var result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    //WAIT сложи, за да е сигурно, че админът ще бъде създаден преди потребителя
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        public static void BeautyRoles(RoleManager<IdentityRole> roleManager)
        {
            // ако ролята не съществува се създава
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}   


