using WalletBuddy.Domain.Enums;
using WalletBuddy.Domain.Reports;

namespace WalletBuddy.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => ResourceReportMessages.CASH,
            PaymentType.CreditCard => ResourceReportMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportMessages.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourceReportMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
