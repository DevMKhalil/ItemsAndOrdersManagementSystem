using FluentAssertions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsAndOrdersManagementSystem.Tests
{
    public class Items_Tests
    {
        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateItem()
        {
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

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
    }
}
