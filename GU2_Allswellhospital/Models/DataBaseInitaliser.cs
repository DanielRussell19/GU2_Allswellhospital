using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 04/05/2019

    /// <summary>
    /// Defines how the database will be initalized and how it will be initally seeded
    /// </summary>
    public class DataBaseInitaliser : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            if (!context.Users.Any())
            {

                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("StaffAdmin"))
                {
                    roleManager.Create(new IdentityRole("StaffAdmin"));
                }
                if (!roleManager.RoleExists("MedicalRecordsStaff"))
                {
                    roleManager.Create(new IdentityRole("MedicalRecordsStaff"));
                }
                if (!roleManager.RoleExists("Nurse"))
                {
                    roleManager.Create(new IdentityRole("Nurse"));
                }
                if (!roleManager.RoleExists("StaffNurse"))
                {
                    roleManager.Create(new IdentityRole("StaffNurse"));
                }
                if (!roleManager.RoleExists("WardSister"))
                {
                    roleManager.Create(new IdentityRole("WardSister"));
                }
                if (!roleManager.RoleExists("Doctor"))
                {
                    roleManager.Create(new IdentityRole("Doctor"));
                }
                if (!roleManager.RoleExists("Consultant"))
                {
                    roleManager.Create(new IdentityRole("Consultant"));
                }

                context.SaveChanges();
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            /*
            //Create an staff
            if (userManager.FindByName("staff@test.com") == null)
            {
                var staff = new Staff
                {
                    UserName = "staff@test.com",
                    Email = "staff@test.com",
                    RegisteredAt = DateTime.Now,
                    EmailConfirmed = true,
                    IsSuspended = false,
                    SuspensionReason = "",
                    SuspensionTime = null,
                    Forename = "sam",
                    Surname = "samson",
                };
                userManager.Create(staff, "staff123");
                userManager.AddToRole(staff.Id, "Staff");
            }

            //creatse moderator

            if (userManager.FindByName("mod@test.com") == null)
            {
                var moderator = new Staff
                {
                    UserName = "mod@test.com",
                    Email = "mod@test.com",
                    RegisteredAt = DateTime.Now,
                    EmailConfirmed = true,
                    IsSuspended = false,
                    SuspensionReason = "",
                    SuspensionTime = null,
                    Forename = "jill",
                    Surname = "jelly",
                };
                userManager.Create(moderator, "mod123");
                userManager.AddToRole(moderator.Id, "Moderator");
            }

            ///create admin

            if (userManager.FindByName("admin@test.com") == null)
            {
                var admin = new Staff
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    RegisteredAt = DateTime.Now,
                    EmailConfirmed = true,
                    IsSuspended = false,
                    SuspensionReason = "",
                    SuspensionTime = null,
                    Forename = "tucker",
                    Surname = "trucker",
                };
                userManager.Create(admin, "admin123");
                userManager.AddToRole(admin.Id, "Admin");
            }

            //create member

            if (userManager.FindByName("member@test.com") == null)
            {
                var member = new Member
                {
                    UserName = "member@test.com",
                    Email = "member@test.com",
                    RegisteredAt = DateTime.Now,
                    EmailConfirmed = true,
                    IsSuspended = false,
                    SuspensionReason = "",
                    SuspensionTime = null,
                    Forename = "generic",
                    Surname = "guest",
                };
                userManager.Create(member, "member123");
                userManager.AddToRole(member.Id, "Member");
            }

            //creates suspended

            if (userManager.FindByName("suspended@test.com") == null)
            {
                var suspended = new Member
                {
                    UserName = "suspended@test.com",
                    Email = "suspended@test.com",
                    RegisteredAt = DateTime.Now,
                    EmailConfirmed = true,
                    IsSuspended = true,
                    SuspensionReason = "Bad Language",
                    SuspensionTime = DateTime.Now.AddDays(16),
                    Forename = "bad",
                    Surname = "mans",
                };
                userManager.Create(suspended, "suspended123");
                userManager.AddToRole(suspended.Id, "Suspended");
            }

            //create categories

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { CategoryName = "Announcement" });
                context.Categories.Add(new Category { CategoryName = "Update" });
                context.Categories.Add(new Category { CategoryName = "Debut" });
                context.Categories.Add(new Category { CategoryName = "Merchandise" });

                context.SaveChanges();
            }

            //create posts
            if (!context.Posts.Any())
            {
                context.Posts.Add(new Post { Header = "Top 10 showings", PostText = "Donec iaculis dolor vel leo elementum euismod. Morbi at sapien vel felis blandit lacinia. Maecenas vel eros id felis ornare tincidunt vitae a justo. Sed interdum congue ante. Nullam et diam porta, facilisis diam id, dignissim lacus. Maecenas ornare ultricies feugiat. Ut congue orci eget tincidunt auctor. Nullam eleifend urna ac urna consectetur lobortis.", isApproved = true, DateOfPublish = DateTime.Now, DateOfEdit = null, CategoryID = 1, Id = context.Users.Local.First().Id });
                context.Posts.Add(new Post { Header = "Sweetness of tears coming soon", PostText = "Praesent non enim et lorem fermentum tincidunt quis vel ligula. Aenean id odio cursus, sagittis ante non, pretium orci. Ut in diam vitae sem porta commodo quis vel lectus. Donec ut velit eget justo pellentesque porttitor eget sed justo. Nulla sit amet sollicitudin elit. Sed quis fringilla lorem. Cras et posuere magna, id commodo enim. Nulla rutrum at nulla non pellentesque. Curabitur porttitor et tellus et vulputate. Nam vitae neque quis orci sagittis congue. Vestibulum lectus urna, lobortis non mattis in, aliquet vel magna. Integer libero augue, egestas at bibendum ornare, malesuada sollicitudin libero.", isApproved = false, DateOfPublish = DateTime.Now, DateOfEdit = null, CategoryID = 3, Id = context.Users.Local.First().Id });

                context.SaveChanges();
            }

            //creates comments

            if (!context.Comments.Any())
            {
                context.Comments.Add(new Comment { PostID = context.Posts.Local.First().PostID, CommentText = "Test", DateOfComment = DateTime.Now, isApproved = true, ID = context.Users.Local.First().Id });
                context.Comments.Add(new Comment { PostID = context.Posts.Local.First().PostID, CommentText = "Test", DateOfComment = DateTime.Now, isApproved = false, ID = context.Users.Local.First().Id });

                context.SaveChanges();
            }
            */

        }


}
}