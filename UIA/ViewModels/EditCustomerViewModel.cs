using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIA.ViewModels //view model can be used to store infos of two tables, or more, like this view model, consists of properties (loginid, email, password, etc) of two tables, customer and login 
{
    public class EditCustomerViewModel
    {
        public int login_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int cust_id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string addresss { get; set; }
        public Nullable<int> phone { get; set; }
    }
}