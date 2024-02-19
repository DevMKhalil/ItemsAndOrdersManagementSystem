using FluentAssertions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.DeleteCommand;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsAndOrdersManagementSystem.Tests
{
    [Collection("TestCollection")]
    public class Items_Tests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _testFixture;

        public Items_Tests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateItem()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test" ,Description = "test desc", Price = 1};
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsSuccess.Should().BeTrue();
            createItemResult.Value.Should().BeGreaterThan(default);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenNameIsNull()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = null, Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemName);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenNameIsEmpty()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = string.Empty, Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemName);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenDescriptionIsEmpty()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = string.Empty, Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemDesc);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenDescriptionIsNull()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = null, Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemDesc);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenPriceIsZero()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = default(decimal) };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemPrice);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenPriceIsNegative()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = -1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemPrice);
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenNoEnteredValues()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);

            // Act 
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);

            // Assert
            createItemResult.IsFailure.Should().BeTrue();
            createItemResult.Error.Should().Contain(Messages.InsertItemName);
            createItemResult.Error.Should().Contain(Messages.InsertItemDesc);
            createItemResult.Error.Should().Contain(Messages.InsertItemPrice);
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenDeleteItemNotRelatedToOrder()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();
            createItemResult.Value.Should().BeGreaterThan(default);

            var deleteItemCommand = new DeleteItemCommand { ItemId = createItemResult.Value };
            var deleteItemCommandHandler = new DeleteItemCommandHandler(context);

            // Act 
            var deleteResult = await deleteItemCommandHandler.Handle(deleteItemCommand, default);

            // Assert
            deleteResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handel_Should_ReturnFailResult_WhenDeleteItemRelatedToOrder()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();
            createItemResult.Value.Should().BeGreaterThan(default);

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var deleteItemCommand = new DeleteItemCommand { ItemId = createItemResult.Value };
            var deleteItemCommandHandler = new DeleteItemCommandHandler(context);

            // Act 
            var deleteResult = await deleteItemCommandHandler.Handle(deleteItemCommand, default);

            // Assert
            deleteResult.IsFailure.Should().BeTrue();
            deleteResult.Error.Should().Contain(Messages.ItemRelatedToOrders);
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenUpdateItemNotRelatedToOrder()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();
            createItemResult.Value.Should().BeGreaterThan(default);

            var updateItemCommand = new UpdateItemCommand { Id = createItemResult.Value,Name = "test2", Description = "test desc2", Price = 1.5m };
            var updateItemCommandHandler = new UpdateItemCommandHandler(context,mapper);

            // Act 
            var updateResult = await updateItemCommandHandler.Handle(updateItemCommand, default);

            // Assert
            updateResult.IsSuccess.Should().BeTrue();
            updateResult.Value.Should().Be(createItemResult.Value);
        }

        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenUpdateItemRelatedToOrder()
        {
            var mapper = _testFixture.Mapper;
            var context = _testFixture.Context;
            var httpContextAccessor = _testFixture.HttpContextAccessor;

            // Arrange
            var createItemCommand = new CreateItemCommand { Name = "test", Description = "test desc", Price = 1 };
            var createItemCommandHandler = new CreateItemCommandhandler(context, mapper);
            var createItemResult = await createItemCommandHandler.Handle(createItemCommand, default);
            createItemResult.IsSuccess.Should().BeTrue();
            createItemResult.Value.Should().BeGreaterThan(default);

            var createOrderCommand = new CreateOrderCommand { UserId = "adminUserId", OrderItemsDtoList = new() { createItemResult.Value } };
            var createOrderCommandHandler = new CreateOrderCommandHandler(context, httpContextAccessor);
            var createOrderResult = await createOrderCommandHandler.Handle(createOrderCommand, default);
            createOrderResult.IsSuccess.Should().BeTrue();
            createOrderResult.Value.Should().BeGreaterThan(default);

            var updateItemCommand = new UpdateItemCommand { Id = createItemResult.Value, Name = "test2", Description = "test desc2", Price = 1.5m };
            var updateItemCommandHandler = new UpdateItemCommandHandler(context, mapper);

            // Act 
            var updateResult = await updateItemCommandHandler.Handle(updateItemCommand, default);

            // Assert
            updateResult.IsSuccess.Should().BeTrue();
            updateResult.Value.Should().Be(createItemResult.Value);
        }
    }
}
