using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverExcelData
{
    public  class Driver
    {
        public string TrafficAdmin { get;  set; }
        public string TrafficUnit { get;  set; }
        public string DriverLicenseNumber { get;  set; }
        public int DriverLicenseTypeId { get;  set; }
       
        public string DrvierName { get;  set; }
        public string Nationality { get;  set; }

        public string Address { get;  set; }

        public DateTime? OperationStart { get;  set; }
        public string DriverMobile { get;  set; }
        public string DriverPhoto { get;  set; }

        public DateTime? LicenseExpireDate { get;  set; }
        public DateTime? LicenseIssueDate { get;  set; }
    }
}
