using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Application.Services.Expenses.Create;

namespace WalletBuddy.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    { 
        services.AddScoped<ICreateExpense, CreateExpense>();
    }
}
