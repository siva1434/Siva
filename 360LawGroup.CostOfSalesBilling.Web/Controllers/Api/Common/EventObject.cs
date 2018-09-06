﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers.Common
{
    public class EventObject<T> where T : class
    {
        // GET: EventObject
        public string id { get; set; }
        public bool allDay { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public T otherInfo { get; set; }
    }
}