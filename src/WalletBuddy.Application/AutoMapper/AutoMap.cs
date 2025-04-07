using AutoMapper;
using WalletBuddy.Communication.Enums;
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
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(entity => entity.Password, config => config.Ignore());

        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(entity => entity.Tags, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseExpenseCreatedJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(response => response.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)));

        CreateMap<User, ResponseUserProfileJson>();
    }
}
