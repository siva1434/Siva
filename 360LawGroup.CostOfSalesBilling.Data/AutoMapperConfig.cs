using System;
using System.Linq;
using AutoMapper;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System.Collections.Generic;
using System.Data.Entity;

namespace _360LawGroup.CostOfSalesBilling.Data
{
    public static class AutoMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AspNetUser, UserViewModel>(MemberList.None);
                cfg.CreateMap<UserViewModel, AspNetUser>(MemberList.None);

                cfg.CreateMap<ApplicationUser, UserViewModel>(MemberList.None);
                cfg.CreateMap<UserViewModel, ApplicationUser>(MemberList.None);

                cfg.CreateMap<ApplicationUser, RegisterAdminBindingModel>(MemberList.None);
                cfg.CreateMap<RegisterAdminBindingModel, ApplicationUser>(MemberList.None);

                cfg.CreateMap<ApplicationUser, RegisterPublisherBindingModel>(MemberList.None);
                cfg.CreateMap<RegisterPublisherBindingModel, ApplicationUser>(MemberList.None);

                cfg.CreateMap<AspNetUser, ApplicationUser>(MemberList.None);
                cfg.CreateMap<ApplicationUser, AspNetUser>(MemberList.None);

                cfg.CreateMap<AdminSetting, AdminSettingsViewModel>(MemberList.None);
                cfg.CreateMap<AdminSettingsViewModel, AdminSetting>(MemberList.None);

                cfg.CreateMap<WorkRate, WorkRateViewModel>(MemberList.None);
                cfg.CreateMap<WorkRateViewModel, WorkRate>(MemberList.None);

                cfg.CreateMap<ClientFile, ClientFileViewModel>(MemberList.None);
                cfg.CreateMap<ClientFileViewModel, ClientFile>(MemberList.None);

                cfg.CreateMap<ResetNewMonth, ResetNewMonthViewModel>(MemberList.None);
                cfg.CreateMap<ResetNewMonthViewModel, ResetNewMonth>(MemberList.None);

                cfg.CreateMap<ClientIncome, ClientIncomeViewModel>(MemberList.None);
                cfg.CreateMap<ClientIncomeViewModel, ClientIncome>(MemberList.None);

                cfg.CreateMap<Consultant, ConsultantViewModel>(MemberList.None);
                cfg.CreateMap<ConsultantViewModel, Consultant>(MemberList.None);

                cfg.CreateMap<ConsultantCost, ConsultantCostViewModel>(MemberList.None);
                cfg.CreateMap<ConsultantCostViewModel, ConsultantCost>(MemberList.None);

                cfg.CreateMap<Client, ClientViewModel>(MemberList.None)
                .ForMember(x => x.MatterWorkHours, y => y.MapFrom(z => z.Matters.Sum(i => i.ConsultantHours.Sum(c => c.WorkHours))))
                .ForMember(x => x.SubscriptionCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => ((i.WorkRateId == 1) ? (i.WorkHours * i.AspNetUser2.SubscriptionHourlyRate) : 0))))
                .ForMember(x => x.MemberHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.MemberHours)))
                .ForMember(x => x.MemberCosts, y => y.MapFrom(z => z.ConsultantHours.Sum(i => ((i.WorkRateId == 2) ? (i.WorkHours * i.AspNetUser2.MemberHourlyRate) : 0))))
                .ForMember(x => x.PrivateClientHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.PrivateClientHours)))
                .ForMember(x => x.PrivateClientCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 3 ? i.WorkHours : 0))))
                .ForMember(x => x.LitigationHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.LitigationHours)))
                .ForMember(x => x.LitigationCosts, y => y.MapFrom(z => z.ConsultantHours.Sum(i => ((i.WorkRateId == 4) ? (i.WorkHours * i.AspNetUser2.LitigationHourlyRate) : 0))))
                .ForMember(x => x.RegulatedHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.RegulatedHours)))
                .ForMember(x => x.RegulatedCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => ((i.WorkRateId == 5) ? (i.WorkHours * i.AspNetUser2.RegulatedHourlyRate) : 0))))
                .ForMember(x => x.OverseasHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.OverseasHours)))
                .ForMember(x => x.OverseasCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => ((i.WorkRateId == 6) ? (i.WorkHours * i.AspNetUser2.OverseasHourlyRate) : 0))))
                .ForMember(x => x.OverseasCharge, y => y.MapFrom(z => z.Matters.Sum(i => (i.WorkRate.RateId == 6 ? (i.ConsultantHours.Sum(c => c.WorkHours) * i.OverseasChargeRate) : 0))))
                .ForMember(x => x.Matters_CurrentMonth, y => y.MapFrom(z => z.Matters.Where(i => i.CurrentMonth).Count()))
                .ForMember(x => x.Matters_Total, y => y.MapFrom(z => z.Matters.Count()))
                .ForMember(x => x.SubscriptionFeeMinusCosts, y => y.MapFrom(z => (z.MonthlySubscription - (z.ConsultantHours.Sum(i => ((i.WorkRateId == 1) ? (i.WorkHours * i.AspNetUser2.SubscriptionHourlyRate) : 0))))))
                .ForMember(x => x.MemberCharge, y => y.MapFrom(z => (z.ConsultantHours.Sum(i => i.MemberHours + z.MemberChargeRate))))
                .ForMember(x => x.PrivateClientCharge, y => y.MapFrom(z => (z.ConsultantHours.Sum(i => i.PrivateClientHours) + z.PrivateClientChargeRate)))
                .ForMember(x => x.LitigationCharge, y => y.MapFrom(z => (z.LitigationChargeRate) * (z.ConsultantHours.Sum(i => i.LitigationHours))))
                .ForMember(x => x.RegulatedCharge, y => y.MapFrom(z => (z.RegulatedChargeRate) * (z.ConsultantHours.Sum(i => i.RegulatedHours))));
                cfg.CreateMap<ClientViewModel, Client>(MemberList.None);

                cfg.CreateMap<ConsultantHour, ConsultantHourViewModel>(MemberList.None)
                .ForMember(x => x.MatterWorkPeriod, y => y.MapFrom(z => z.Matter.MatterName + "-" + z.Month + " " + z.Year))
                .ForMember(x => x.Matter_Consultant_WorkPeriod, y => y.MapFrom(z => z.Matter.MatterName + "[" + z.AspNetUser2.ConsultantName + "]-" + z.Month + " " + z.Year))
                //.ForMember(x => x.WorkRateText, y => y.MapFrom(z => z.Matter.WorkRate))
                .ForMember(x => x.SubscriptionCost, y => y.MapFrom(z => (z.WorkRateId == 1) ? (z.WorkHours * z.AspNetUser2.SubscriptionHourlyRate) : 0))
                .ForMember(x => x.MemberCost, y => y.MapFrom(z => (z.WorkRateId == 2) ? (z.WorkHours * z.AspNetUser2.MemberHourlyRate) : 0))
                .ForMember(x => x.PrivateClientCost, y => y.MapFrom(z => z.WorkRateId == 3 ? z.WorkHours : 0))
                .ForMember(x => x.LitigationCost, y => y.MapFrom(z => (z.WorkRateId == 4) ? (z.WorkHours * z.AspNetUser2.LitigationHourlyRate) : 0))
                .ForMember(x => x.RegulatedCost, y => y.MapFrom(z => (z.WorkRateId == 5) ? (z.WorkHours * z.AspNetUser2.RegulatedHourlyRate) : 0))
                .ForMember(x => x.OverseasCost, y => y.MapFrom(z => (z.WorkRateId == 6) ? (z.WorkHours * z.AspNetUser2.OverseasHourlyRate) : 0))
                .ForMember(x => x.TotalCost_inclSub, y => y.MapFrom(z => (((z.WorkRateId == 1) ? (z.WorkHours * z.AspNetUser2.SubscriptionHourlyRate) : 0) +
                             ((z.WorkRateId == 2) ? (z.WorkHours * z.AspNetUser2.MemberHourlyRate) : 0) +
                             ((z.WorkRateId == 3) ? (z.WorkHours) : 0) +
                             ((z.WorkRateId == 4) ? (z.WorkHours * z.AspNetUser2.LitigationHourlyRate) : 0) +
                             ((z.WorkRateId == 5) ? (z.WorkHours * z.AspNetUser2.RegulatedHourlyRate) : 0) +
                             ((z.WorkRateId == 6) ? (z.WorkHours * z.AspNetUser2.OverseasHourlyRate) : 0))))
                .ForMember(x => x.TotalCost_exclSub, y => y.MapFrom(z => ((z.WorkRateId == 2) ? (z.WorkHours * z.AspNetUser2.MemberHourlyRate) : 0) +
                             ((z.WorkRateId == 3) ? z.WorkHours : 0) +
                             ((z.WorkRateId == 4) ? (z.WorkHours * z.AspNetUser2.LitigationHourlyRate) : 0) +
                             ((z.WorkRateId == 5) ? (z.WorkHours * z.AspNetUser2.RegulatedHourlyRate) : 0) +
                             ((z.WorkRateId == 6) ? (z.WorkHours * z.AspNetUser2.OverseasHourlyRate) : 0)))
                .ForMember(x => x.ClientWorkPeriod, y => y.MapFrom(z => z.Matter.Client.Company + "-" + z.Month + " " + z.Year))
                .ForMember(x => x.WorkRateText, y => y.MapFrom(z => z.Matter.WorkRate.RateType));
                cfg.CreateMap<ConsultantHourViewModel, ConsultantHour>(MemberList.None);

                cfg.CreateMap<Matter, MatterViewModel>(MemberList.None)
                .ForMember(x => x.Client_Matter, y => y.MapFrom(z => z.Client.Company + " - " + z.MatterName))
                .ForMember(x => x.WorkRateText, y => y.MapFrom(z => z.WorkRate.RateType))
                .ForMember(x => x.ClientText, y => y.MapFrom(z => z.Client.Company))
                .ForMember(x => x.WorkHours, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.WorkHours)))
                .ForMember(x => x.SubscriptionCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 1) ? (i.WorkHours * i.AspNetUser2.SubscriptionHourlyRate) : 0)))
                .ForMember(x => x.MemberCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 2) ? (i.WorkHours * i.AspNetUser2.MemberHourlyRate) : 0)))
                .ForMember(x => x.PrivateClientCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 3 ? i.WorkHours : 0))))
                .ForMember(x => x.PrivateClientCharge, y => y.MapFrom(z => z.WorkRate.RateId == 3 ? (z.ConsultantHours.Sum(i => i.WorkHours) * z.Client.PrivateClientChargeRate) : 0))
                .ForMember(x => x.LitigationCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 4) ? (i.WorkHours * i.AspNetUser2.LitigationHourlyRate) : 0)))
                .ForMember(x => x.LitigationCharge, y => y.MapFrom(z => z.WorkRate.RateId == 4 ? (z.ConsultantHours.Sum(i => i.WorkHours) * z.Client.LitigationChargeRate) : 0))
                .ForMember(x => x.RegulatedCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 5) ? (i.WorkHours * i.AspNetUser2.RegulatedHourlyRate) : 0)))
                .ForMember(x => x.RegulatedCharge, y => y.MapFrom(z => z.WorkRate.RateId == 5 ? (z.ConsultantHours.Sum(i => i.WorkHours) * z.Client.RegulatedChargeRate) : 0))
                .ForMember(x => x.OverseasCost, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (i.WorkRateId == 6) ? (i.WorkHours * i.AspNetUser2.OverseasHourlyRate) : 0)))
                .ForMember(x => x.OverseasCharge, y => y.MapFrom(z => z.WorkRate.RateId == 6 ? (z.ConsultantHours.Sum(i => i.WorkHours) * z.OverseasChargeRate) : 0))
                .ForMember(x => x.CheckCost_inclSub, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (((i.WorkRateId == 1) ? (i.WorkHours * i.AspNetUser2.SubscriptionHourlyRate) : 0) +
                                    ((i.WorkRateId == 2) ? (i.WorkHours * i.AspNetUser2.MemberHourlyRate) : 0) +
                                    ((i.WorkRateId == 3) ? (i.WorkHours) : 0) +
                                    ((i.WorkRateId == 4) ? (i.WorkHours * i.AspNetUser2.LitigationHourlyRate) : 0) +
                                    ((i.WorkRateId == 5) ? (i.WorkHours * i.AspNetUser2.RegulatedHourlyRate) : 0) +
                                    ((i.WorkRateId == 6) ? (i.WorkHours * i.AspNetUser2.OverseasHourlyRate) : 0)))))
                .ForMember(x => x.CheckCost_exclSub, y => y.MapFrom(z => z.ConsultantHours.Sum(i => (((i.WorkRateId == 2) ? (i.WorkHours * i.AspNetUser2.MemberHourlyRate) : 0) +
                                    ((i.WorkRateId == 3) ? i.WorkHours : 0) +
                                    ((i.WorkRateId == 4) ? (i.WorkHours * i.AspNetUser2.LitigationHourlyRate) : 0) +
                                    ((i.WorkRateId == 5) ? (i.WorkHours * i.AspNetUser2.RegulatedHourlyRate) : 0) +
                                    ((i.WorkRateId == 6) ? (i.WorkHours * i.AspNetUser2.OverseasHourlyRate) : 0)))))
                .ForMember(x => x.DisbursementAmount, y => y.MapFrom(z => z.ConsultantHours.Sum(i => i.DisbursementAmount)));
                cfg.CreateMap<MatterViewModel, Matter>(MemberList.None);
            });
            Mapper.AssertConfigurationIsValid();
        }

        public static TD To<TD>(this object source, int timeInterval)
        {
            return Mapper.Map<TD>(source).NullToEmpty(timeInterval);
        }

        public static List<TD> ToListMap<TD>(this IEnumerable<object> source, int timeInterval)
        {
            var newList = new List<TD>();
            foreach (var item in source)
            {
                newList.Add(Mapper.Map<TD>(item).NullToEmpty(timeInterval));
            }
            return newList;
        }

        public static TD To<TS, TD>(this TS source, TD dest, int timeInterval)
        {
            return Mapper.Map<TS, TD>(source, dest).NullToEmpty(timeInterval);
        }

        public static TObject NullToEmpty<TObject>(this TObject obj, int timeInterval)
        {
            if (obj == null) return obj;
            var allPropList = obj.GetType().GetProperties();
            var propList = allPropList.Where(x => x.PropertyType == typeof(string) && x.GetValue(obj, null) == null);
            foreach (var p in propList)
            {
                p.SetValue(obj, string.Empty);
            }
            propList = allPropList.Where(x => x.PropertyType == typeof(DateTime));
            foreach (var p in propList)
            {
                var value = (DateTime)p.GetValue(obj, null);
                if (value == DateTime.MinValue)
                    continue;
                p.SetValue(obj, value.AddMinutes(timeInterval));
            }

            propList = allPropList.Where(x => x.PropertyType == typeof(DateTime?) && x.GetValue(obj, null) != null);
            foreach (var p in propList)
            {
                var value = (DateTime?)p.GetValue(obj, null);
                if (value.Value == DateTime.MinValue)
                    continue;
                p.SetValue(obj, value.Value.AddMinutes(timeInterval));
            }

            propList = allPropList.Where(x => x.PropertyType.GetInterface(typeof(ICollection<>).FullName) != null);
            foreach (var p in propList)
            {
                var value = p.GetValue(obj, null);
                if (value != null)
                {
                    var list = (System.Collections.ICollection)value;
                    foreach (var item in list)
                    {
                        NullToEmpty(item, timeInterval);
                    }
                }
            }
            return obj;
        }
    }
}