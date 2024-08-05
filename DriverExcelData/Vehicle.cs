using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverExcelData
{
    public  class Vehicle
    {
        public string TrafficAdmin { get; set; }
        public string TrafficUnit { get; set; }


        public string VehicleLicensePlate { get;  set; }
        public int VehicleLicenseTypeId { get;  set; }
      
        public string VehicleOwnerName { get;  set; }
        public string VehicleOwnerNationality { get;  set; }
        public string VehicleOwnerAddress { get;  set; }

        public string? Manufacturer { get;  set; }

        public string? Model { get;  set; }

        public int? MakeYear { get;  set; }

        public int? VehicleTypeId { get;  set; }
       
        public string VehicleColor { get;  set; }
        public string VehicleAttributes { get;  set; }

        public string ChassisNumber { get;  set; }
        public string MotorNumber { get;  set; }
        public string VechileVIN { get;  set; }

        public string CylendersNumber { get;  set; }
        public string CylendersCapacity { get;  set; }

        public string FuleType { get;  set; }
        public DateTime OperationStart { get;  set; }
    }
}
