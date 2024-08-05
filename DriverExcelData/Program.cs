using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using DriverExcelData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

static class Program
{
    static void Main(string[] args)
    {
        string filePath = @"../../../../Fleet Data 2.xlsx";
        string connectionString = "Server=DESKTOP-U5TKJ2H;Database=FleetDB;Integrated Security=true;TrustServerCertificate=true;";

        IWorkbook workbook = ReadExcelFile(filePath);
        ISheet DriverSheet = workbook.GetSheetAt(1);
        int rowCountDriver = DriverSheet.PhysicalNumberOfRows;

        Dictionary<string, int> licenseType = new Dictionary<string, int>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            for (int row = 2; row < rowCountDriver; row++)
            {
                Driver driver = new Driver();
                try
                {
                    IRow excelRow = DriverSheet.GetRow(row);
                    ProcessExcelRow(excelRow, connection, licenseType, driver);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing data in row {row + 1}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error in row {row + 1}: {ex.Message}");
                }
            }

            connection.Close();
        }

        Console.WriteLine("Data processing completed.");
    }

    static IWorkbook ReadExcelFile(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return new XSSFWorkbook(fileStream);
        }
    }

    static int GetDriverLicenseTypeId(string driverLicenseType, SqlConnection connection, Dictionary<string, int> licenseType)
    {
        if (!licenseType.ContainsKey(driverLicenseType))
        {
            string query = "INSERT INTO DriverLicenseTypes (LocaleName, LatinName, SystemCode, DeleteDisabled, IsHidden) " +
                           "OUTPUT INSERTED.Id VALUES (@LocaleName, @LatinName, NEWID(), 0, 0)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocaleName", driverLicenseType ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LatinName", driverLicenseType ?? (object)DBNull.Value);
                int driverLicenseTypeId = (int)command.ExecuteScalar();
                licenseType.Add(driverLicenseType, driverLicenseTypeId);
                Console.WriteLine($"Inserted new DriverLicenseType: {driverLicenseType} with Id: {driverLicenseTypeId}");
                return driverLicenseTypeId;
            }
        }
        else
        {
            return licenseType[driverLicenseType];
        }
    }

    static void InsertDriverRecord(SqlConnection connection, Driver driver)
    {
        string query = "INSERT INTO Drivers (TrafficAdmin, TrafficUnit, DriverLicenseNumber, DriverLicenseTypeId, DrvierName, Nationality, Address, OperationStart, DriverMobile, DriverPhoto, LicenseExpireDate, LicenseIssueDate, SystemCode, DeleteDisabled, IsHidden) VALUES " +
                       "(@TrafficAdmin, @TrafficUnit, @DriverLicenseNumber, @DriverLicenseTypeId, @DrvierName, @Nationality, @Address, @OperationStart, @DriverMobile, -1, @LicenseExpireDate, @LicenseIssueDate, NEWID(), 0, 0)";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@TrafficAdmin", driver.TrafficAdmin ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TrafficUnit", driver.TrafficUnit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DriverLicenseNumber", driver.DriverLicenseNumber);
            command.Parameters.AddWithValue("@DriverLicenseTypeId", driver.DriverLicenseTypeId);
            command.Parameters.AddWithValue("@DrvierName", driver.DrvierName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Nationality", driver.Nationality ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Address", driver.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@OperationStart", (object)driver.OperationStart ?? DBNull.Value);
            command.Parameters.AddWithValue("@DriverMobile", driver.DriverMobile ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DriverPhoto", driver.DriverPhoto ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LicenseExpireDate", (object)driver.LicenseExpireDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@LicenseIssueDate", (object)driver.LicenseIssueDate ?? DBNull.Value);

            command.ExecuteNonQuery();
        }
    }

    static void ProcessExcelRow(IRow excelRow, SqlConnection connection, Dictionary<string, int> licenseType, Driver driver)
    {
        driver.TrafficAdmin = excelRow.GetCell(1)?.ToString();
        driver.TrafficUnit = excelRow.GetCell(2)?.ToString();
        driver.DriverLicenseNumber = excelRow.GetCell(3)?.ToString();
        string driverLicenseType = excelRow.GetCell(4)?.ToString().Trim();

        int driverLicenseTypeId = GetDriverLicenseTypeId(driverLicenseType, connection, licenseType);
        driver.DriverLicenseTypeId = driverLicenseTypeId;

        driver.DrvierName = excelRow.GetCell(5)?.ToString();
        driver.Nationality = excelRow.GetCell(6)?.ToString();
        driver.Address = excelRow.GetCell(7)?.ToString();

        driver.OperationStart = ParseDate(excelRow, 16, 17, 18);
        driver.LicenseIssueDate = ParseDate(excelRow, 13, 14, 15);
        driver.LicenseExpireDate = ParseDate(excelRow, 10, 11, 12);

        driver.DriverMobile = excelRow.GetCell(19)?.ToString();
        driver.DriverPhoto = excelRow.GetCell(20)?.ToString();

        InsertDriverRecord(connection, driver);
    }

    static DateTime? ParseDate(IRow row, int dayIndex, int monthIndex, int yearIndex)
    {
        try
        {
            int? day = int.TryParse(row.GetCell(dayIndex)?.ToString(), out int d) ? (int?)d : null;
            int? month = int.TryParse(row.GetCell(monthIndex)?.ToString(), out int m) ? (int?)m : null;
            int? year = int.TryParse(row.GetCell(yearIndex)?.ToString(), out int y) ? (int?)y : null;

            if (day.HasValue && month.HasValue && year.HasValue)
            {
                return new DateTime(year.Value, month.Value, day.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing date components: {ex.Message}");
        }

        return null;
    }
}


