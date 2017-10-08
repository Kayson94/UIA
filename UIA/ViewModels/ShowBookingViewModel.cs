using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UIA.ViewModels
{
    public class ShowBookingViewModel
    {
        public int booking_id { get; set; }
        public int cust_id { get; set; }
        public String name { get; set; }
        public int flight_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime flight_date { get; set; }
        public TimeSpan flight_time { get; set; }
        public String duration { get; set; }
        public String destination { get; set; }
        public String plane_company { get; set; }
        public String plane_name { get; set; }
    }
}