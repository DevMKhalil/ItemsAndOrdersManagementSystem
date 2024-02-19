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
using Microsoft.Extensions.DependencyInjection;
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
    [Collection("TestCollection")]
    public class Orders_Test : IClassFixture<TestFixture>
    {
        private readonly TestFixture _testFixture;

        public Orders_Test(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateOrder()
        {
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

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
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

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
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

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
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

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
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

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
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = GetHttpContextAccessor("InvalidUserId");

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
    }

    [CollectionDefinition("TestCollection")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
