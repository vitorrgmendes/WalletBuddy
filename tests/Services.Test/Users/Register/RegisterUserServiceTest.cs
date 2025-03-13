using CommonUtilities.Test.Cryptography;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using CommonUtilities.Test.Token;
using NUnit.Framework;
using WalletBuddy.Application.Services.Users.Register;
using Assert = NUnit.Framework.Assert;

namespace Services.Test.Users.Register;

public class RegisterUserServiceTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var service = CreateService();

        var result = await service.Execute(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(request.Name));
        Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
    }

    private RegisterUser CreateService()
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRepository = new UserRepositoryBuilder().Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new RegisterUser(mapper, passwordEncripter, userRepository, unitOfWork, accessTokenGenerator);
    }
}
