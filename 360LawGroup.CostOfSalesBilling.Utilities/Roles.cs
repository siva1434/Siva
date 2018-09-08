using System;
using System.Linq;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        Consultant,
        ClientUser
    }

    public static class RoleExtension
    {
        public const string SuperAdmin = "Super Admin";
        public const string Admin = "Admin";
        public const string Consultant = "Consultant";
        public const string ClientUser = "Client User";

        public static bool IsValidRole(string loggedInRole, string role)
        {
            switch (loggedInRole)
            {
                case SuperAdmin:
                    return (new string[] { SuperAdmin, Admin, Consultant, ClientUser }).Contains(role);
                case Admin:
                    return (new string[] { Admin, ClientUser }).Contains(role);
                case Consultant:
                    return (new string[] { Consultant }).Contains(role);
                case ClientUser:
                    return (new string[] { ClientUser }).Contains(role);
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public static string[] GetRoleAccessList(string role)
        {
            switch (role)
            {
                case SuperAdmin:
                    return (new string[] { Admin, Consultant, ClientUser });
                case Admin:
                    return (new string[] {ClientUser});
                case Consultant:
                    return (new string[] { ClientUser});
                case ClientUser:
                    return (new string[] { });
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public static string ToName(this Roles role)
        {
            switch (role)
            {
                case Roles.SuperAdmin:
                    return SuperAdmin;
                case Roles.Admin:
                    return Admin;
                case Roles.Consultant:
                    return Consultant;
                case Roles.ClientUser:
                    return ClientUser;

                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }
        /*
        public static string GetPrefix(string roleName)
        {
            switch (roleName)
            {
                case SuperAdmin:
                    return "SA";
                case BranchManager:
                    return "BM";
                case HumanResource:
                    return "HR";
                case SalesTeam:
                    return "ST";
                case Member:
                    return "MB";
                case Dietitian:
                    return "DN";
                case RegionalBranchManager:
                    return "RM";
                case Jr_GeneralTrainer:
                    return "JGTR";
                case Sr_GeneralTrainer:
                    return "SGTR";
                case SilverPersonalTrainer:
                    return "SPTR";
                case GoldPersonalTrainer:
                    return "GPTR";
                case PlatinumPersonalTrainer:
                    return "PPTR";
                case TrainerHead:
                    return "TR";

                default:
                    throw new ArgumentOutOfRangeException(nameof(roleName), roleName, null);
            }
        }
        */
    }
}
