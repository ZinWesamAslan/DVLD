using DVLDdataAccess.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDdataAccess.Licenses
{
    public class ClsLicensesAccess
    {
        public class ClsLicenseAccess
        {
            public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID,
                ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
                ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
            {
                bool isFound = false;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        ApplicationID = (int)reader["ApplicationID"];
                        DriverID = (int)reader["DriverID"];
                        LicenseClass = (int)reader["LicenseClass"];
                        IssueDate = (DateTime)reader["IssueDate"];
                        ExpirationDate = (DateTime)reader["ExpirationDate"];

                        // Handling Nullable Notes
                        if (reader["Notes"] != DBNull.Value)
                            Notes = (string)reader["Notes"];
                        else
                            Notes = "";

                        PaidFees = Convert.ToSingle(reader["PaidFees"]);
                        IsActive = (bool)reader["IsActive"];
                        IssueReason = (byte)reader["IssueReason"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to retrieve license information for LicenseID: {LicenseID}", ex);
                }
                finally { connection.Close(); }

                return isFound;
            }

            public static bool GetLicenseInfoByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID,
                ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
                ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
            {
                bool isFound = false;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        LicenseID = (int)reader["LicenseID"];
                        DriverID = (int)reader["DriverID"];
                        LicenseClass = (int)reader["LicenseClass"];
                        IssueDate = (DateTime)reader["IssueDate"];
                        ExpirationDate = (DateTime)reader["ExpirationDate"];

                        if (reader["Notes"] != DBNull.Value)
                            Notes = (string)reader["Notes"];
                        else
                            Notes = "";

                        PaidFees = Convert.ToSingle(reader["PaidFees"]);
                        IsActive = (bool)reader["IsActive"];
                        IssueReason = (byte)reader["IssueReason"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to retrieve license information for ApplicationID: {ApplicationID}", ex);
                }
                finally { connection.Close(); }

                return isFound;
            }

            public static int AddNewLicenseForTheFirstTime(int PersonID, int ApplicationID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees,
                bool IsActive, byte IssueReason, int CreatedByUserID)
            {
                int NewLicenseID = -1;

                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

                string query = @"
                    BEGIN TRY
                        BEGIN TRANSACTION;
                        
                            UPDATE Applications 
                            SET ApplicationStatus = 3 
                            WHERE ApplicationID = @ApplicationID;
                        
                            DECLARE @NewDriverID INT;
                        
                            IF NOT EXISTS (SELECT 1 FROM Drivers WHERE PersonID = @PersonID)
                            BEGIN
                                INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                                VALUES (@PersonID, @CreatedByUserID, GETDATE());
                        
                                SET @NewDriverID = SCOPE_IDENTITY();
                            END
                            ELSE
                            BEGIN
                                SELECT @NewDriverID = DriverID FROM Drivers WHERE PersonID = @PersonID;
                            END
                        
                            INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                            VALUES (@ApplicationID, @NewDriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                        
                            SELECT SCOPE_IDENTITY();
                        
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
                        SELECT -1; 
                    END CATCH";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@Notes", (object)Notes ?? DBNull.Value);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        NewLicenseID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    NewLicenseID = -1;
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to add new license for the first time for PersonID: {PersonID} and ApplicationID: {ApplicationID}", ex);
                }
                finally
                {
                    connection.Close();
                }

                return NewLicenseID;
            }

            public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees,
                bool IsActive, byte IssueReason, int CreatedByUserID)
            {
                int LicenseID = -1;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

                string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                             VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);

                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        LicenseID = insertedID;
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to add new license record for DriverID: {DriverID} and ApplicationID: {ApplicationID}", ex);
                }
                finally { connection.Close(); }

                return LicenseID;
            }

            public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees,
                bool IsActive, byte IssueReason, int CreatedByUserID)
            {
                int rowsAffected = 0;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

                string query = @"UPDATE Licenses 
                             SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClass = @LicenseClass, 
                                 IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, 
                                 PaidFees = @PaidFees, IsActive = @IsActive, IssueReason = @IssueReason, 
                                 CreatedByUserID = @CreatedByUserID
                             WHERE LicenseID = @LicenseID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);

                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to update database record for LicenseID: {LicenseID}", ex);
                    return false;
                }
                finally { connection.Close(); }

                return (rowsAffected > 0);
            }

            public static DataTable GetLicenseHistory(int DriverID)
            {
                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = @"
                               SELECT    Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate,licenses.ExpirationDate, Licenses.IsActive
                               FROM      Licenses INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID 
                               WHERE      Licenses.DriverID = @DriverID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) dt.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to load license history for DriverID: {DriverID}", ex);
                }
                finally { connection.Close(); }

                return dt;
            }

            public static DataTable GetAllLicenses()
            {
                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "SELECT * FROM Licenses";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) dt.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError("Failed to load all licenses table from database", ex);
                }
                finally { connection.Close(); }

                return dt;
            }

            public static bool DeleteLicense(int LicenseID)
            {
                int rowsAffected = 0;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "DELETE Licenses WHERE LicenseID = @LicenseID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Failed to delete license record for LicenseID: {LicenseID}", ex);
                }
                finally { connection.Close(); }

                return (rowsAffected > 0);
            }

            public static bool IsLicenseExist(int LicenseID)
            {
                bool isFound = false;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "SELECT Found=1 FROM Licenses WHERE LicenseID = @LicenseID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    isFound = reader.HasRows;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Error checking license existence for LicenseID: {LicenseID}", ex);
                }
                finally { connection.Close(); }

                return isFound;
            }

            public static bool IsLicenseExistsByDriverID(int DriverID, int LicenseClassID)
            {
                bool isFound = false;
                SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
                string query = "SELECT Found=1 FROM Licenses WHERE DriverID = @DriverID And LicenseClass = @LicenseClass";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    isFound = reader.HasRows;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                    // تسجيل الخطأ بداخل الـ Event Viewer
                    ClsLogger.LogError($"Error checking license existence for DriverID: {DriverID} and LicenseClassID: {LicenseClassID}", ex);
                }
                finally { connection.Close(); }

                return isFound;
            }
        }
    }
}