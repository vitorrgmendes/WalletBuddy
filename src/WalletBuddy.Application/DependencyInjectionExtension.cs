using Microsoft.Extensions.DependencyInjection;
using WalletBuddy.Application.AutoMapper;
using WalletBuddy.Application.Services.Auth.Login;
using WalletBuddy.Application.Services.Auth.Logout;
using WalletBuddy.Application.Services.Auth.RefreshToken;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Application.Services.Expenses.Delete;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Application.Services.Expenses.GetById;
using WalletBuddy.Application.Services.Expenses.Reports.Excel;
using WalletBuddy.Application.Services.Expenses.Reports.Pdf;
using WalletBuddy.Application.Services.Expenses.Update;
using WalletBuddy.Application.Services.Users.ChangePassword;
using WalletBuddy.Application.Services.Users.GetProfile;
using WalletBuddy.Application.Services.Users.Register;
using WalletBuddy.Application.Services.Users.Restore;
using WalletBuddy.Application.Services.Users.SoftDelete;
using WalletBuddy.Application.Services.Users.Update;

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
        AddAuthServices(services);
        AddUsersServices(services);
        AddExpensesServices(services);        
    }    

    private static void AddUsersServices(IServiceCollection services)
    {
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<IGetUserProfile, GetUserProfile>();
        services.AddScoped<IUpdateUser, UpdateUser>();
        services.AddScoped<IChangePassword, ChangePasswordService>();
        services.AddScoped<ISoftDeleteUser, SoftDeleteUser>();
        services.AddScoped<IRestoreUser, RestoreUser>();
    }

    private static void AddAuthServices(IServiceCollection services)
    {
        services.AddScoped<ILoginUser, LoginUser>();
        services.AddScoped<IRefreshToken, RefreshToken>();
        services.AddScoped<ILogoutUser, LogoutUser>();
    }

    private static void AddExpensesServices(IServiceCollection services)
    {
        services.AddScoped<ICreateExpense, CreateExpense>();
        services.AddScoped<IGetAllExpenses, GetAllExpenses>();
        services.AddScoped<IGetExpenseById, GetExpenseById>();
        services.AddScoped<IDeleteExpense, DeleteExpense>();
        services.AddScoped<IUpdateExpense, UpdateExpense>();
        services.AddScoped<IGenerateExpensesReportExcel, GenerateExpensesReportExcel>();
        services.AddScoped<IGenerateExpensesReportPdf, GenerateExpensesReportPdf>();
    }
}
