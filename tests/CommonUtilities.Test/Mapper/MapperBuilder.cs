using AutoMapper;
using WalletBuddy.Application.AutoMapper;

namespace CommonUtilities.Test.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMap());
        });
        
        return mapper.CreateMapper();
    }
}
