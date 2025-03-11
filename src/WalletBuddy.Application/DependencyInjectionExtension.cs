using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Application.AutoMapper;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Application.Services.Expenses.Delete;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Application.Services.Expenses.GetById;
using WalletBuddy.Application.Services.Expenses.Reports.Excel;
using WalletBuddy.Application.Services.Expenses.Reports.Pdf;
using WalletBuddy.Application.Services.Expenses.Update;
using WalletBuddy.Application.Services.TokenAuthentication.Login;
using WalletBuddy.Application.Services.TokenAuthentication.Logout;
using WalletBuddy.Application.Services.TokenAuthentication.RefreshToken;
using WalletBuddy.Application.Services.Users.Create;

namespace WalletBuddy.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    { 
        AddAutoMapper(services);
        AddServices(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMap));
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ICreateExpense, CreateExpense>();
        services.AddScoped<IGetAllExpenses, GetAllExpenses>();
        services.AddScoped<IGetExpenseById, GetExpenseById>();
        services.AddScoped<IDeleteExpense, DeleteExpense>();
        services.AddScoped<IUpdateExpense, UpdateExpense>();
        services.AddScoped<IGenerateExpensesReportExcel, GenerateExpensesReportExcel>();
        services.AddScoped<IGenerateExpensesReportPdf, GenerateExpensesReportPdf>();
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<ILoginUser, LoginUser>();
        services.AddScoped<IRefreshToken, RefreshToken>();
        services.AddScoped<ILogoutUser, LogoutUser>();
    }
}
