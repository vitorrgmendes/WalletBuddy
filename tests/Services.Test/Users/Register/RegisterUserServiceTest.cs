using CommonUtilities.Test.Cryptography;
using CommonUtilities.Test.InlineData;
using CommonUtilities.Test.Mapper;
using CommonUtilities.Test.Repositories;
using CommonUtilities.Test.Requests;
using CommonUtilities.Test.Token;
using NUnit.Framework;
using WalletBuddy.Application.Services.Users.Register;
using WalletBuddy.Exception;
using WalletBuddy.Exception.Exception;
using _NUnit = NUnit.Framework;

namespace Services.Test.Users.Register;

public class RegisterUserServiceTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var service = CreateService();

        var result = await service.Execute(request);

        _NUnit.Assert.That(result, Is.Not.Null);
        _NUnit.Assert.That(result.Name, Is.EqualTo(request.Name));
        _NUnit.Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        _NUnit.Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
    }

    [Fact]
    public void Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var service = CreateService(request.Email);

        var exception = _NUnit.Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        _NUnit.Assert.That(exception!.GetErrors(), Has.Count.EqualTo(1));
        _NUnit.Assert.That(exception!.GetErrors(), Contains.Item(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    [Xunit.Theory]
    [ClassData(typeof(EmptyStringInLineDataTest))]
    public void Error_Invalid_Name(string name)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var service = CreateService();

        var exception = _NUnit.Assert.ThrowsAsync<ErrorOnValidationException>(async () => await service.Execute(request));

        _NUnit.Assert.That(exception!.GetErrors(), Has.Count.EqualTo(1));
        _NUnit.Assert.That(exception!.GetErrors(), Contains.Item(ResourceErrorMessages.NAME_REQUIRED));
    }

    private RegisterUser CreateService(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRepository = new UserRepositoryBuilder();
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            userRepository.ExistActiveUserWithEmail(email!);

        return new RegisterUser(mapper, passwordEncrypter, userRepository.Build(), unitOfWork, accessTokenGenerator);
    }
}
