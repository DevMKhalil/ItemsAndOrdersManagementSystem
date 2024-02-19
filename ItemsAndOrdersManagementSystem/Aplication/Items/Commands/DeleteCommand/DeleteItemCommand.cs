using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Commands.DeleteCommand
{
    public class DeleteItemCommand : IRequest<Result>
    {
        public int ItemId { get; set; }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteItemCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            string err = string.Empty;

            var item = await _dbContext.Items.FindAsync(request.ItemId);

            if (item is null)
                return Result.Failure(err.ErrorAppendMessage(Messages.ItemNotFound));

            var isRelatedToOrders = await _dbContext.Orders.AnyAsync(x => x.Items.Select(z => z.ItemId).Contains(request.ItemId));

            if (isRelatedToOrders)
                return Result.Failure(err.ErrorAppendMessage(Messages.ItemRelatedToOrders));

            _dbContext.Items.Remove(item);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return Result.Success();
        }
    }
}
