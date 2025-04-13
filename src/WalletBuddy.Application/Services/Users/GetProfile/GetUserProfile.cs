using AutoMapper;
using WalletBuddy.Communication.Responses.Users;
using WalletBuddy.Domain.Services.LoggedUser;

namespace WalletBuddy.Application.Services.Users.GetProfile;

public class GetUserProfile : IGetUserProfile
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetUserProfile(IMapper mapper, ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
