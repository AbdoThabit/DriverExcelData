using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverExcelData
{
    public  class VehiclesData
    {
        static void InsertVechileRecord(SqlConnection connection, string trafficAdmin, string trafficUnit, string driverLicenseNumber,
                                  int driverLicenseTypeId, string driverName, string nationality, string address,
                                  DateTime? operationStartDate, string driverMobile, string driverPhoto,
                                  DateTime? licenseExpireDate, DateTime? licenseIssueDate)
        {
            string query = "INSERT INTO Drivers (TrafficAdmin, TrafficUnit, DriverLicenseNumber, DriverLicenseTypeId, DrvierName, Nationality, Address, OperationStart, DriverMobile, DriverPhoto, LicenseExpireDate, LicenseIssueDate, SystemCode, DeleteDisabled, IsHidden) VALUES " +
                           "(@TrafficAdmin, @TrafficUnit, @DriverLicenseNumber, @DriverLicenseTypeId, @DrvierName, @Nationality, @Address, @OperationStart, @DriverMobile, -1, @LicenseExpireDate, @LicenseIssueDate, NEWID(), 0, 0)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrafficAdmin", trafficAdmin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TrafficUnit", trafficUnit ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DriverLicenseNumber", driverLicenseNumber);
                command.Parameters.AddWithValue("@DriverLicenseTypeId", driverLicenseTypeId);
                command.Parameters.AddWithValue("@DrvierName", driverName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Nationality", nationality ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@OperationStart", (object)operationStartDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@DriverMobile", driverMobile ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DriverPhoto", driverPhoto ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LicenseExpireDate", (object)licenseExpireDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@LicenseIssueDate", (object)licenseIssueDate ?? DBNull.Value);

                command.ExecuteScalar();
            }
        }
    }
}
