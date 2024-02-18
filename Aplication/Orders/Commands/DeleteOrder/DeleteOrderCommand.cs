using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.DeleteCommand
{
    public class DeleteOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand,Result>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteOrderCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            string err = string.Empty;
            Maybe<Order> maybeOrder = await _dbContext.Orders
                                                .Include(x => x.Items)
                                                .FirstOrDefaultAsync(x => x.id == request.Id);

            if (maybeOrder.HasNoValue)
                return Result.Failure(err.ErrorAppendMessage(Messages.OrderNotFound));

            _dbContext.Orders.Remove(maybeOrder.Value);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return Result.Success();
        }
    }
}
