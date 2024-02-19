using AutoMapper;
using FluentAssertions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.DeleteCommand;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder;
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

            // Detach entities from the context
            DetachEntities(context);

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

            // Detach entities from the context
            DetachEntities(context);

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

            // Detach entities from the context
            DetachEntities(context);

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

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value,999 } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);

            // Act 
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);

            // Detach entities from the context
            DetachEntities(context);

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

            // Detach entities from the context
            DetachEntities(context);

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

            // Detach entities from the context
            DetachEntities(context);

            // Assert
            createOrderResult.IsFailure.Should().BeTrue();
            createOrderResult.Error.Should().Contain(Messages.userIsNotAuthenticated);
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateOrderAndDeleteTheOrder()
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
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var deleteOrderCommand = new DeleteOrderCommand { Id = createOrderResult.Value };
            var deleteOrderCommandHandler = new DeleteOrderCommandHandler(context);

            // Act
            var deleteOrderCommandResult = await deleteOrderCommandHandler.Handle(deleteOrderCommand, default);

            // Detach entities from the context
            DetachEntities(context);

            // Assert
            deleteOrderCommandResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateOrderAndUpdateTheOrderAddNewItem()
        {
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

            var createFirstItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createFirstItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createFirstItemResult = await createFirstItemCommandHandler.Handle(createFirstItemCommand, default);
            createFirstItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createFirstItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var createSecondItemCommand = new CreateItemCommand { Name = "test2", Description = "test desc2", Price = 1.5m };
            var createSecondItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createSecondItemResult = await createSecondItemCommandHandler.Handle(createSecondItemCommand, default);
            createSecondItemResult.IsSuccess.Should().BeTrue();

            var updateOrderCommand = new UpdateOrderCommand { id = createOrderResult.Value,UserId = "adminUserId",OrderItemsDtoList = new() { createFirstItemResult.Value, createSecondItemResult.Value } };
            var updateOrderCommandHandler = new UpdateOrderCommandHandler(context,mapper,httpContextAccessor);

            // Act
            var updateOrderCommandResult = await updateOrderCommandHandler.Handle(updateOrderCommand, default);

            // Detach entities from the context
            DetachEntities(context);

            // Assert
            updateOrderCommandResult.IsSuccess.Should().BeTrue();
            updateOrderCommandResult.Value.Should().Be(createOrderResult.Value);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenCreateOrderAndUpdateTheOrderRemoveItems()
        {
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

            var createFirstItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createFirstItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createFirstItemResult = await createFirstItemCommandHandler.Handle(createFirstItemCommand, default);
            createFirstItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createFirstItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var updateOrderCommand = new UpdateOrderCommand { id = createOrderResult.Value, UserId = "adminUserId", OrderItemsDtoList = new() { } };
            var updateOrderCommandHandler = new UpdateOrderCommandHandler(context, mapper, httpContextAccessor);

            // Act
            var updateOrderCommandResult = await updateOrderCommandHandler.Handle(updateOrderCommand, default);

            // Detach entities from the context
            DetachEntities(context);

            // Assert
            updateOrderCommandResult.IsFailure.Should().BeTrue();
            updateOrderCommandResult.Error.Should().Contain(Messages.ItemsListIsEmpty);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenCreateOrderAndUpdateTheOrderAddNewItemFromOtherUser()
        {
            // Arrange
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

            var createFirstItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createFirstItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createFirstItemResult = await createFirstItemCommandHandler.Handle(createFirstItemCommand, default);
            createFirstItemResult.IsSuccess.Should().BeTrue();

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createFirstItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var createSecondItemCommand = new CreateItemCommand { Name = "test2", Description = "test desc2", Price = 1.5m };
            var createSecondItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createSecondItemResult = await createSecondItemCommandHandler.Handle(createSecondItemCommand, default);
            createSecondItemResult.IsSuccess.Should().BeTrue();

            var updateOrderCommand = new UpdateOrderCommand { id = createOrderResult.Value, UserId = "userUserId", OrderItemsDtoList = new() { createFirstItemResult.Value, createSecondItemResult.Value } };
            var updateOrderCommandHandler = new UpdateOrderCommandHandler(context, mapper, httpContextAccessor);

            // Act
            var updateOrderCommandResult = await updateOrderCommandHandler.Handle(updateOrderCommand, default);

            // Detach entities from the context
            DetachEntities(context);

            // Assert
            updateOrderCommandResult.IsFailure.Should().BeTrue();
            updateOrderCommandResult.Error.Should().Contain(Messages.CanOnlyUpdateTheOrderByTheOwnerUser);
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

        private void DetachEntities(TestDbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
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
