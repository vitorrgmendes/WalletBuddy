using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Application.AutoMapper;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Application.Services.Expenses.Delete;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Application.Services.Expenses.GetById;
using WalletBuddy.Application.Services.Expenses.Reports.Excel;
using WalletBuddy.Application.Services.Expenses.Update;

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
    }
}
