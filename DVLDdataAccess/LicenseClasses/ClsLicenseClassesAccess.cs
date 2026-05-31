 // استدعاء مساحة الأسماء الخاصة باللوغر المشترك
using DVLDdataAccess.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDdataAccess.LicenseClasses
{
    public class ClsLicenseClassesAccess
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription,
                                               ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
                // تسجيل الخطأ بداخل الـ Event Viewer
                ClsLogger.LogError($"Failed to retrieve license class information for LicenseClassID: {LicenseClassID}", ex);
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO LicenseClasses (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
                         VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees);
                         SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    ID = insertedID;
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ بداخل الـ Event Viewer
                ClsLogger.LogError($"Failed to add new license class: {ClassName}", ex);
            }
            finally { connection.Close(); }

            return ID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = @"UPDATE LicenseClasses 
                         SET ClassName = @ClassName, 
                             ClassDescription = @ClassDescription, 
                             MinimumAllowedAge = @MinimumAllowedAge, 
                             DefaultValidityLength = @DefaultValidityLength, 
                             ClassFees = @ClassFees
                         WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ بداخل الـ Event Viewer
                ClsLogger.LogError($"Failed to update license class for LicenseClassID: {LicenseClassID}", ex);
                return false;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM LicenseClasses ORDER BY LicenseClassID";
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
                ClsLogger.LogError("Failed to retrieve all license classes table from database", ex);
            }
            finally { connection.Close(); }

            return dt;
        }

        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = "DELETE FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ بداخل الـ Event Viewer
                ClsLogger.LogError($"Failed to delete license class record for LicenseClassID: {LicenseClassID}", ex);
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool IsLicenseClassExist(int LicenseClassID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            string query = "SELECT Found=1 FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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
                ClsLogger.LogError($"Error checking existence for LicenseClassID: {LicenseClassID}", ex);
            }
            finally { connection.Close(); }

            return isFound;
        }
    }
}