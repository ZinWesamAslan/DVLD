using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDdataAccess.Logger; // استدعاء مساحة الأسماء الخاصة باللّوغر

namespace DVLDdataAccess
{
    public class ClsCountryAccess
    {
        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    CountryName = ((string)reader["CountryName"]).Trim();
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
                // تسجيل الخطأ بداخل الـ Event Viewer مع تمرير المعامل
                ClsLogger.LogError($"Failed to retrieve country information for CountryID: {CountryID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetCountryInfoByName(ref int CountryID, string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    CountryID = (int)reader["CountryID"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
                // تسجيل الخطأ بداخل الـ Event Viewer مع تمرير اسم الدولة المعني
                ClsLogger.LogError($"Failed to retrieve country information for CountryName: {CountryName}", ex);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}