using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System.Collections.Generic;
using _360LawGroup.CostOfSalesBilling.Data;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Web;
using System.Text.RegularExpressions;
using _360LawGroup.CostOfSalesBilling.Web.Helper;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers
{
    [ApiAuth]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        [HttpGet, Route("apiui")]
        [ApiAuth(RoleExtension.SuperAdmin/*,RoleExtension.Consultant,RoleExtension.ClientUser*/)]
        public DefaultResponse SwaggerUi()
        {
            var status = new DefaultResponse();
            try
            {
                var token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                using (var myHttpClient = new HttpClient())
                {
                    myHttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["API_URL"]);
                    if (!string.IsNullOrEmpty(token))
                        myHttpClient.DefaultRequestHeaders.Add("Authorization", token);
                    status.StatusCode = HttpStatusCode.OK;
                    status.Data = myHttpClient.GetStringAsync("swagger/ui/index").Result;
                }
            }
            catch (Exception exe)
            {
                var msg = exe.Message + "\n" + exe.StackTrace;
                if (exe.InnerException != null)
                    msg += "\n" + exe.InnerException.Message + "\n" + exe.InnerException.StackTrace;
                status.StatusCode = HttpStatusCode.InternalServerError;
                status.Data = msg;
            }
            return status;
        }
        /*
        [Route("getallcounter"), HttpGet]
        public GenericResponse<CounterViewModel> GetAllCounter()
        {
            var status = new GenericResponse<CounterViewModel>();
            var invoice = Uow.InvoiceRepository.GetQuery(x => !x.IsDeleted && x.IsActive && x.LocationId == CurrentLocation);
            if (UserIsInRole(RoleExtension.SalesTeam))
            {
                invoice = invoice.Where(x => x.CreatedBy == LoggedInUser.Id);
            }
            var appoinment = Uow.AppointmentRepository.GetQuery(x => !x.IsDeleted && x.LocationId == CurrentLocation);
            if (UserIsInRole(RoleExtension.SalesTeam))
            {
                appoinment = appoinment.Where(x => x.CreatedBy == LoggedInUser.Id);
            }
            var enquryhndlby = Uow.InquiryRepository.GetQuery(x => x.IsActive && !x.IsDeleted && x.LocationId == CurrentLocation);
            if (UserIsInRole(RoleExtension.SalesTeam))
            {
                enquryhndlby = enquryhndlby.Where(x => x.SalesPersonId == LoggedInUser.Id);
            }
            var clientrepres = Uow.MemberInfoRepository.GetQuery(x => x.IsActive && !x.IsDeleted &&
             x.AspNetUser3 != null && x.AspNetUser3.Locations2.Any(y => y.Id == CurrentLocation));
            if (UserIsInRole(RoleExtension.SalesTeam))
            {
                clientrepres = clientrepres.Where(x => x.ClientRepresentativeUserId == LoggedInUser.Id);
            }
            status.StatusCode = HttpStatusCode.OK;
            status.Data = new CounterViewModel
            {
                TotalAppoinment = appoinment.Count(),
                TotalClientRepr = clientrepres.Count(),
                TotalEnquiryHandledBy = enquryhndlby.Count(),
                TotalInvoice = invoice.Count(),
                TotalAmount = invoice.Any() ? invoice.Sum(x => x.TotalAmount) : 0,
                TotalMember = enquryhndlby.Where(a => a.AspNetUser2.MemberType == MemberTypes.Member).Count()
            };
            return status;
        }

        [HttpPost, Route("getupcomingbdays")]
        public GridData<BirthdayViewModel> GetUpcomingBirthdays(SearchModel model)
        {
            int total;
            model.offset = 0;
            model.limit = int.MaxValue;
            var startdate = DateTime.UtcNow.Date;
            var enddate = startdate.AddDays(6);
            var memberInfo = Uow.MemberInfoRepository.GetQuery<MemberInfoViewModel>(x => x.IsActive && !x.IsDeleted);
            var newquery = memberInfo.GroupBy(x => new { x.DOB.Value.Month, x.DOB.Value.Day })
                .Where(x => x.Key.Day >= startdate.Day && x.Key.Day <= enddate.Day &&
                    x.Key.Month >= startdate.Month && x.Key.Month <= enddate.Month)
               .Select(x => new BirthdayViewModel
               {
                   Day = x.Key.Day,
                   Month = x.Key.Month,
                   Members = x.ToList()
               });

            var list = newquery.ApplyFilter(model, out total);
            for (var i = 0; i <= 6; i++)
            {
                var dt = startdate.AddDays(i);
                var data = list.FirstOrDefault(y => y.Day == dt.Day && y.Month == dt.Month);
                if (data == null)
                {
                    list.Add(new BirthdayViewModel { Day = dt.Day, Month = dt.Month, DayName = dt.ToString("ddd"), Members = new List<MemberInfoViewModel>() });
                }
                else
                {
                    data.DayName = dt.ToString("ddd");
                }
            }
            list = list.OrderBy(x => x.Month).ThenBy(x => x.Day).ToList();
            var gridData = new GridData<BirthdayViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }
        */
    }
}