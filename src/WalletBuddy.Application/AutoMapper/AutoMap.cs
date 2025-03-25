using AutoMapper;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Expenses;
using WalletBuddy.Communication.Responses.Users;
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
        CreateMap<RequestExpenseJson, Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(entity => entity.Password, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseExpenseCreatedJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
