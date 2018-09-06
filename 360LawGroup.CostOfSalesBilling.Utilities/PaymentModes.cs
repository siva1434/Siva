using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public enum PaymentModes
    {
        Cheque, Cash, CreditCard, DebitCard, RTGS, Other
    }

    public static class PaymentModesExt
    {
        public const string Cheque = "Cheque";
        public const string Cash = "Cash";
        public const string CreditCard = "Credit Card";
        public const string DebitCard = "Debit Card";
        public const string RTGS = "RTGS";
        public const string Other = "Other";

        public static string ToName(this PaymentModes mode)
        {
            switch (mode)
            {
                case PaymentModes.Cash:
                    return Cash;
                case PaymentModes.Cheque:
                    return Cheque;
                case PaymentModes.CreditCard:
                    return CreditCard;
                case PaymentModes.DebitCard:
                    return DebitCard;
                case PaymentModes.RTGS:
                    return RTGS;
                case PaymentModes.Other:
                    return Other;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static bool IsOther(string value)
        {
            return !(new[] { Cheque, Cash, CreditCard, DebitCard, RTGS }.Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
