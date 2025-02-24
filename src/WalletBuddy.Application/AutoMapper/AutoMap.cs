using AutoMapper;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Application.AutoMapper;

public class AutoMap : Profile
{
    public AutoMap()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestExpenseCreateJson, Expense>();
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseExpenseCreatedJson>();
    }
}
