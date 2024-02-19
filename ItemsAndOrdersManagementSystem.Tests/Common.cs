using AutoMapper;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItemsAndOrdersManagementSystem.Tests
{
    public class Common
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            return config.CreateMapper();
        }

        public static TestDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                    .UseInMemoryDatabase(databaseName: "ItemsAndOrdersManagementSystem")
                    .Options;

            return new TestDbContext(options);
        }

        public static IHttpContextAccessor GetHttpContextAccessor(string loggedUserId = "adminUserId")
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
            var httpContextMock = new Mock<HttpContext>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedUserId)
            }));
            httpContextMock.Setup(c => c.User).Returns(user);
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            return httpContextAccessorMock.Object;
        }

        public async static Task CreateUsers(TestDbContext context)
        {
            var isUsersCreated = await context.ApplicationUsers.AnyAsync(x => x.Id == "adminUserId");

            if (isUsersCreated)
                return;

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var email_Admin = "admin@admin.com";
            var email_User = "user@user.com";

            var addedUserList = new List<ApplicationUser>();

            if (await userManagerMock.Object.FindByNameAsync(email_Admin) == null)
            {
                var user_Admin = new ApplicationUser()
                {
                    Id = "adminUserId",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_Admin,
                    Email = email_Admin
                };
                await userManagerMock.Object.CreateAsync(user_Admin, "123");
                addedUserList.Add(user_Admin);
            }

            if (await userManagerMock.Object.FindByNameAsync(email_User) == null)
            {
                var user_User = new ApplicationUser()
                {
                    Id = "userUserId",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_User,
                    Email = email_User
                };
                await userManagerMock.Object.CreateAsync(user_User, "123");
                addedUserList.Add(user_User);
            }

            if (addedUserList.Count > default(int))
            {
                context.ApplicationUsers.AddRange(addedUserList);
                await context.SaveChangesAsync();
            }
        }
    }
}
