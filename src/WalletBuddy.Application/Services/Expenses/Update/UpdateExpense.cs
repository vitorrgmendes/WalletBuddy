using AutoMapper;
using WalletBuddy.Communication.Enums;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories;
using WalletBuddy.Domain.Repositories.Expenses;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Application.Services.Expenses.Update;

public class UpdateExpense : IUpdateExpense
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpense(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetByIdForChanges(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);
        expense.UpdatedAt = DateTime.UtcNow;

        SyncTags(expense, request.Tags);

        _repository.Update(expense);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

    private void SyncTags(Expense expense, IEnumerable<TagEnum> tags)
    {
        var requestTags = tags.Distinct().ToList();

        // Tags To Delete
        var tagsToDelete = expense.Tags
            .Where(tag => !requestTags.Contains((TagEnum)tag.Value))
            .ToList();

        foreach (var tag in tagsToDelete)
            expense.Tags.Remove(tag);

        // Tags To Add
        var currentTags = expense.Tags.Select(t => t.Value).ToHashSet();
        var tagsToAdd = requestTags
            .Where(tagValue => !currentTags.Contains((Domain.Enums.TagEnum)tagValue))
            .Select(tagValue => new Tag
            {
                Value = (Domain.Enums.TagEnum)tagValue,
                ExpenseId = expense.Id
            });

        foreach (var tag in tagsToAdd)
            expense.Tags.Add(tag);
    }
}
