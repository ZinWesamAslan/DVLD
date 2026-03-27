using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVLDdataAccess
{
    public  class ClsPeopleAccess
    {
        public static bool GetPersonInfoByID( int PersonID, ref string NationalNO, ref string FirstName,
                                              ref string SecondName , ref string ThirdName , ref string LastName,
                                              ref string Gender , ref string Email, ref string Phone,
                                              ref string Address, ref DateTime DateOfBirth, ref int CountryID, 
                                              ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    NationalNO = ((string)reader["NationalNO"]).Trim();
                    FirstName = ((string)reader["FirstName"]).Trim();
                    SecondName = ((string)reader["SecondName"]).Trim();
                    ThirdName = ((string)reader["ThirdName"]).Trim();
                    LastName = ((string)reader["LastName"]).Trim();

                    if (((string)reader["Gender"]).ToUpper() == "F".ToUpper())
                    {
                        Gender = "FEMALE".ToUpper();
                    }
                    else
                    {
                        Gender = "MALE".ToUpper();
                    }

                    Email = ((string)reader["Email"]).Trim();
                    Phone = ((string)reader["Phone"]).Trim();
                    Address = ((string)reader["Address"]).Trim();
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    CountryID = (int)reader["CountryID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = ((string)reader["ImagePath"]).Trim();
                    }
                    else
                    {
                        ImagePath = "";
                    }

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
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNO( string NationalNO,ref int PersonID, ref string FirstName,
                                              ref string SecondName, ref string ThirdName, ref string LastName,
                                              ref string Gender, ref string Email, ref string Phone,
                                              ref string Address, ref DateTime DateOfBirth, ref int CountryID,
                                              ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNO = @NationalNO";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNO", NationalNO);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    PersonID = ((int)reader["PersonID"]);
                    FirstName = ((string)reader["FirstName"]).Trim();
                    SecondName = ((string)reader["SecondName"]).Trim();
                    ThirdName = ((string)reader["ThirdName"]).Trim();
                    LastName = ((string)reader["LastName"]).Trim();

                    if (((string)reader["Gender"]).ToUpper() == "F".ToUpper())
                    {
                        Gender = "FEMALE".ToUpper();
                    }
                    else
                    {
                        Gender = "MALE".ToUpper();
                    }

                    Email = ((string)reader["Email"]).Trim();
                    Phone = ((string)reader["Phone"]).Trim();
                    Address = ((string)reader["Address"]).Trim();
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    CountryID = (int)reader["CountryID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = ((string)reader["ImagePath"]).Trim();
                    }
                    else
                    {
                        ImagePath = "";
                    }

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
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewPerson( string NationalNo ,string FirstName, string SecondName , 
                                        string ThirdName ,string LastName,string Gender,
                                        string Email, string Phone, string Address,
                                        DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int ID = -1;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO People (NationalNo,FirstName,SecondName,ThirdName,LastName,Gender, Email, Phone, Address,DateOfBirth, CountryID,ImagePath)
                             VALUES (@NationalNo,@FirstName,@SecondName,@ThirdName ,@LastName,@Gender, @Email, @Phone, @Address,@DateOfBirth, @CountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo" , NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            if (Gender == "FEMALE")
            {
                command.Parameters.AddWithValue("@Gender", 'F');
            }
            else
            {
                command.Parameters.AddWithValue("@Gender", 'M');
            }
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    ID = insertedID;
                
            }

            catch (Exception ex) 
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                connection.Close();
            }


            return ID;
        }

        public static bool UpdatePerson( int PersonID, string NationalNo , string FirstName, 
                                         string SecondName,string ThirdName ,string LastName,
                                         string Gender ,string Email, string Phone,
                                         string Address, DateTime DateOfBirth, int CountryID,
                                         string ImagePath)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = @"Update  People  
                            set NationalNo = @NationalNo ,
                                FirstName = @FirstName, 
                                SecondName = @SecondName, 
                                ThirdName = @ThirdName, 
                                LastName = @LastName, 
                                Gender = @Gender, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryID = @CountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            if (Gender == "FEMALE")
            {
                command.Parameters.AddWithValue("@Gender", 'F');
            }
            else
            {
                command.Parameters.AddWithValue("@Gender", 'M');
            }
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);
            
             string query =
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName, 
              People.Email, People.Phone,People.Address,
			  People.DateOfBirth,
				  CASE
                  WHEN People.Gender = 'm' THEN 'Male'

                  ELSE 'Female'

                  END as Gender,
			   
              People.CountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.CountryID = Countries.CountryID
                ORDER BY People.FirstName";

             
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = @"Delete People 
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNO)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNO = @NationalNO";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNO", NationalNO);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }
}
