using AutoMapper;
using CSharpFunctionalExtensions;
using Elfie.Serialization;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem
{
    public class UpdateItemCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; set; }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<int>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public UpdateItemCommandHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            Maybe<Item> maybeItem = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (maybeItem.HasNoValue)
                return Result.Failure<int>(Messages.ItemNotFound);

            var itemDto = _mapper.Map<ItemDto>(request);

            var updateResult = maybeItem.Value.UpdateItem(itemDto);

            if (updateResult.IsFailure)
                return Result.Failure<int>(updateResult.Error);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return updateResult.Value.Id;
        }
    }
}
