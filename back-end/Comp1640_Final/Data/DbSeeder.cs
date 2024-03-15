using Comp1640_Final.Models;
using Microsoft.AspNetCore.Identity;

namespace Comp1640_Final.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<ApplicationUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole("Admin"));
            await roleMgr.CreateAsync(new IdentityRole("Manager"));
            await roleMgr.CreateAsync(new IdentityRole("Guest"));
            await roleMgr.CreateAsync(new IdentityRole("Student"));
            await roleMgr.CreateAsync(new IdentityRole("Coordinator"));

            // create admin user

            var admin = new ApplicationUser
            {
                FirstName = "Quoc",
                LastName = "Viet",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FacultyId = 1,
                EmailConfirmed = true
                
            };

            var manager = new ApplicationUser
            {
                FirstName = "Tho",
                LastName = "Khiem",
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                FacultyId = 2,
                EmailConfirmed = true

            };

            var coordinator = new ApplicationUser
            {
                FirstName = "Nam",
                LastName = "Khanh",
                UserName = "coordinator@gmail.com",
                Email = "coordinator@gmail.com",
                FacultyId = 2,
                EmailConfirmed = true

            };

            var student = new ApplicationUser
            {
                FirstName = "Long",
                LastName = "Hoang",
                UserName = "student@gmail.com",
                Email = "student@gmail.com",
                FacultyId = 3,
                EmailConfirmed = true

            };
            var guest = new ApplicationUser
            {
                FirstName = "Tuyen",
                LastName = "Do",
                UserName = "guest@gmail.com",
                Email = "guest@gmail.com",
                FacultyId = 2,
                EmailConfirmed = true

            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, "Admin");

                await userMgr.CreateAsync(manager, "Manager@123");
                await userMgr.AddToRoleAsync(manager, "Manager");

                await userMgr.CreateAsync( coordinator, "Coordinator@123");
                await userMgr.AddToRoleAsync(coordinator, "Coordinator");

                await userMgr.CreateAsync( student, "Student@123");
                await userMgr.AddToRoleAsync(student, "Student");

                await userMgr.CreateAsync( guest, "Guest@123");
                await userMgr.AddToRoleAsync(guest, "Guest");

            }




        }

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProjectDbContext>();
                context.Database.EnsureCreated();
                if (!context.Faculties.Any())
                {
                    context.Faculties.AddRange(new List<Faculty>()
                    {
                        new Faculty()
                        {
                            Name = "It",

                        },
                        new Faculty()
                        {
                            Name = "Design",

                        },
                        new Faculty()
                        {
                            Name = "Marketing",

                        }
                    });
                    context.SaveChanges();

                }

                //if (!context.Articles.Any())
                //{
                //    context.Articles.AddRange(new List<Article>() {
                //        new Article()
                //        {
                //            ArticleId = Guid.NewGuid(),
                //            Title = "Quoc Viet dep trai",
                //            Description = "Dep trai nhat the gioi",
                //            UploadDate = DateTime.Now,
                //            FacultyId = 1,
                //            PublishStatusId = (int)EPublishStatus.Approval,
                //            studentId = "8228e5c9-9386-43f8-bfbf-224206679b39"
                //        },

                //        new Article()
                //        {
                //            ArticleId = Guid.NewGuid(),
                //            Title = "mot hai ba bon",
                //            Description = "bon ba hai mot",
                //            UploadDate = DateTime.Now,
                //            FacultyId = 2,
                //            PublishStatusId = (int)EPublishStatus.NotApproval,
                //            studentId = "8228e5c9-9386-43f8-bfbf-224206679b39"

                //        },
                //        new Article()
                //        {
                //            ArticleId = Guid.NewGuid(),
                //            Title = "tho khiem ngu",
                //            Description = "tho khiem dumbass",
                //            UploadDate = DateTime.Now,
                //            FacultyId = 3,
                //            PublishStatusId = (int)EPublishStatus.Browsing,
                //            studentId = "8228e5c9-9386-43f8-bfbf-224206679b39"
                //        }
                //    });
                //    context.SaveChanges();
                //}
            }
        }
    }
}
