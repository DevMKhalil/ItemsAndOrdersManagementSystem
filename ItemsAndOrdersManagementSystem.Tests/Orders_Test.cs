using AutoMapper;
using FluentAssertions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ItemsAndOrdersManagementSystem.Tests
{
    public partial class Orders_Test
    {

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateOrder()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor();
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            await CreateUsers(context);


            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenUserIsNotCreated()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor();
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            await CreateUsers(context);

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "RemovedUserId", OrderItemsDtoList = new() { createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.UserNotFound);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenItemListIsEmpty()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor();
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            await CreateUsers(context);

            // Arrange

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.ItemsListIsEmpty);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenItemNotFound()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor();
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            await CreateUsers(context);

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value,2 } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.OrderItemNotFound);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenItemIsDublicated()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor();
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            await CreateUsers(context);

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value, createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.TheItemIsDublicated);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenLoggedUserIsNotInUsers()
        {
            //var mapper = Common.GetMapper();
            //var context = Common.GetApplicationDbContext();
            //var httpContextAccessor = Common.GetHttpContextAccessor("invalidUserId");
            //await Common.CreateUsers(context);

            var mapper = GetMapper();
            var context = GetApplicationDbContext();
            var httpContextAccessor = GetHttpContextAccessor("InvalidUserId");
            await CreateUsers(context);

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.userIsNotAuthenticated);
        }

        [Before]
        public void Setup()
        {
            // Code to run before every test
        }

        public IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            return config.CreateMapper();
        }

        public TestDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            var res = new TestDbContext(options);

            res.Database.EnsureDeleted();

            return res;
        }

        public IHttpContextAccessor GetHttpContextAccessor(string loggedUserId = "adminUserId")
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

        public async Task CreateUsers(TestDbContext context)
        {
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
