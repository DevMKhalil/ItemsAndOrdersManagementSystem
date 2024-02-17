using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem
{
    public record CreateItemCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; set; }
    }

    public class CreateItemCommandhandler : IRequestHandler<CreateItemCommand, Result<int>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public CreateItemCommandhandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var itemDto = _mapper.Map<ItemDto>(request);

            var createResult = Item.CreateItem(itemDto);

            if (createResult.IsFailure)
                return Result.Failure<int>(createResult.Error);

            _dbContext.Items.Add(createResult.Value);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return Result.Success(createResult.Value.Id);
        }
    }
}
