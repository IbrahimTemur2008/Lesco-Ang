using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static RestService.DataObjects.eMeterObjects;

namespace RestService
{
    using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static RestService.DataObjects.eMeterObjects;

namespace RestService
{
    
    public class eMeterAPI : IeMeterAPI
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        public ResponseMessage SignIn(UserCredentials credentials)
        {
            ResponseMessage response = new ResponseMessage();            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT userID FROM Registration WHERE userName = @userName AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", credentials.UserName);
                command.Parameters.AddWithValue("@password", credentials.Password);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    response.Code = 0; // Success
                    response.Message = "Sign in successful.";
                }
                else
                {
                    response.Code = 1; // Error
                    response.Message = "Invalid username or password.";
                }
            }

            return response;
        }

        public ResponseMessage SignOut(UserCredentials credentials)
        {            
            ResponseMessage response = new ResponseMessage
            {
                Code = 0, // Success
                Message = "Sign out successful."
            };
            return response;
        }

        public ResponseMessage ChangePassword(UserCredentials request)
        {
            ResponseMessage response = new ResponseMessage();
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Registration SET password = @newPassword WHERE userName = @userName AND password = @oldPassword";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", request.UserName);
                command.Parameters.AddWithValue("@oldPassword", request.OldPassword);
                command.Parameters.AddWithValue("@newPassword", request.NewPassword);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    response.Code = 0; // Success
                    response.Message = "Password changed successfully.";
                }
                else
                {
                    response.Code = 1; // Error
                    response.Message = "Invalid username or password.";
                }
            }

            return response;
        }
        public ResponseMessage RecordReading(RecordReadingRequest request)
        {
            var billMain = request.BillMain;
            var billDetailsList = request.BillDetailsList;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into BillMain
                        string insertBillMainQuery = @"INSERT INTO BillMain (meterID, customerID, issueDate, dueDate, statusID, billMonth, readingDate, unitsConsumed, createdBy, createdAt) 
                                                   OUTPUT INSERTED.billID
                                                   VALUES (@MeterID, @CustomerID, @IssueDate, @DueDate, @StatusID, @BillMonth, @ReadingDate, @UnitsConsumed, @CreatedBy, @CreatedAt)";
                        SqlCommand cmdBillMain = new SqlCommand(insertBillMainQuery, connection, transaction);
                        cmdBillMain.Parameters.AddWithValue("@MeterID", billMain.MeterID);
                        cmdBillMain.Parameters.AddWithValue("@CustomerID", billMain.CustomerID);
                        cmdBillMain.Parameters.AddWithValue("@IssueDate", billMain.IssueDate);
                        cmdBillMain.Parameters.AddWithValue("@DueDate", billMain.DueDate);
                        cmdBillMain.Parameters.AddWithValue("@StatusID", billMain.StatusID);
                        cmdBillMain.Parameters.AddWithValue("@BillMonth", billMain.BillMonth);
                        cmdBillMain.Parameters.AddWithValue("@ReadingDate", billMain.ReadingDate);
                        cmdBillMain.Parameters.AddWithValue("@UnitsConsumed", billMain.UnitsConsumed);
                        cmdBillMain.Parameters.AddWithValue("@CreatedBy", billMain.CreatedBy);
                        cmdBillMain.Parameters.AddWithValue("@CreatedAt", billMain.CreatedAt);

                        int billID = (int)cmdBillMain.ExecuteScalar();

                        // Insert into BillDetails
                        foreach (var billDetail in billDetailsList)
                        {
                            string insertBillDetailsQuery = @"INSERT INTO BillDetails (billID, slabID, slabRate, unitsApplied, slabAmount) 
                                                          VALUES (@BillID, @SlabID, @SlabRate, @UnitsApplied, @SlabAmount)";
                            SqlCommand cmdBillDetails = new SqlCommand(insertBillDetailsQuery, connection, transaction);
                            cmdBillDetails.Parameters.AddWithValue("@BillID", billID);
                            cmdBillDetails.Parameters.AddWithValue("@SlabID", billDetail.SlabID);
                            cmdBillDetails.Parameters.AddWithValue("@SlabRate", billDetail.SlabRate);
                            cmdBillDetails.Parameters.AddWithValue("@UnitsApplied", billDetail.UnitsApplied);
                            cmdBillDetails.Parameters.AddWithValue("@SlabAmount", billDetail.SlabAmount);

                            cmdBillDetails.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();

                        return new ResponseMessage
                        {
                            Code = 200,
                            Message = "Record reading successful."
                        };
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction
                        transaction.Rollback();

                        return new ResponseMessage
                        {
                            Code = 500,
                            Message = "An error occurred: " + ex.Message
                        };
                    }
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CustomerID, FullName, EmailAddress, MobileNo, CnicNo, FullAddress, CityName, ActiveInd, CreatedBy, CreatedAt FROM Customers";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        CustomerID = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        EmailAddress = reader.GetString(2),
                        MobileNo = reader.GetString(3),
                        CnicNo = reader.GetString(4),
                        FullAddress = reader.GetString(5),
                        CityName = reader.GetString(6),
                        ActiveInd = reader.GetBoolean(7),
                        CreatedBy = reader.GetString(8),
                        CreatedAt = reader.GetDateTime(9)
                    };
                    users.Add(user);
                }
            }

            return users;
        }

    

    public ResponseMessage RegisterCustomer(User user)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customers (FullName, EmailAddress, MobileNo, CnicNo, FullAddress, CityName, ActiveInd, CreatedBy, CreatedAt) VALUES (@FullName, @EmailAddress, @MobileNo, @CnicNo, @FullAddress, @CityName, @ActiveInd, @CreatedBy, @CreatedAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                command.Parameters.AddWithValue("@MobileNo", user.MobileNo);
                command.Parameters.AddWithValue("@CnicNo", user.CnicNo);
                command.Parameters.AddWithValue("@FullAddress", user.FullAddress);
                command.Parameters.AddWithValue("@CityName", user.CityName);
                command.Parameters.AddWithValue("@ActiveInd", user.ActiveInd);
                command.Parameters.AddWithValue("@CreatedBy", user.CreatedBy);
                command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "User registered successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage CreateCredentials(Credentials credentials)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Registration (userName, password, customerID, activeInd, activeDate, createdBy, createdAt) VALUES (@userName, @password, @customerID, @activeInd, @activeDate, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", credentials.UserName);
                command.Parameters.AddWithValue("@password", credentials.Password);
                command.Parameters.AddWithValue("@customerID", credentials.CustomerID);
                command.Parameters.AddWithValue("@activeInd", credentials.ActiveInd);
                command.Parameters.AddWithValue("@activeDate", credentials.ActiveDate);
                command.Parameters.AddWithValue("@createdBy", credentials.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", credentials.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Credentials created successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage AddMeter(MeterType meter)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO MeterType (typeID, typeName, activeInd, createdBy, createdAt) VALUES (@typeID, @typeName, @activeInd, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@typeID", meter.TypeID);
                command.Parameters.AddWithValue("@typeName", meter.TypeName);
                command.Parameters.AddWithValue("@activeInd", meter.ActiveInd);
                command.Parameters.AddWithValue("@createdBy", meter.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", meter.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Meter added successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage RegisterMeterWithCustomer(MeterDetails meterDetails)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO MeterDetails (meterID, customerID, meterNo, refNo, oldRefNo, connectionDate, statusID, meterLoad, activeInd, createdBy, createdAt) " +
                               "VALUES (@meterID, @customerID, @meterNo, @refNo, @oldRefNo, @connectionDate, @statusID, @meterLoad, @activeInd, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@meterID", meterDetails.MeterID);
                command.Parameters.AddWithValue("@customerID", meterDetails.CustomerID);
                command.Parameters.AddWithValue("@meterNo", meterDetails.MeterNo);
                command.Parameters.AddWithValue("@refNo", meterDetails.RefNo);
                command.Parameters.AddWithValue("@oldRefNo", meterDetails.OldRefNo);
                command.Parameters.AddWithValue("@connectionDate", meterDetails.ConnectionDate);
                command.Parameters.AddWithValue("@statusID", meterDetails.StatusID);
                command.Parameters.AddWithValue("@meterLoad", meterDetails.MeterLoad);
                command.Parameters.AddWithValue("@activeInd", meterDetails.ActiveInd);
                command.Parameters.AddWithValue("@createdBy", meterDetails.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", meterDetails.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Meter registered with customer successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

    public User GetUser(string userId)
    {
        User user = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          connection.Open();
          string query = "SELECT CustomerID, FullName, EmailAddress, MobileNo, CnicNo, FullAddress, CityName, ActiveInd, CreatedBy, CreatedAt FROM Customers WHERE CustomerID = @UserId";
          SqlCommand command = new SqlCommand(query, connection);
          command.Parameters.AddWithValue("@UserId", userId);
          SqlDataReader reader = command.ExecuteReader();

          while (reader.Read())
          {
            user = new User
            {
              CustomerID = reader.GetInt32(0),
              FullName = reader.GetString(1),
              EmailAddress = reader.GetString(2),
              MobileNo = reader.GetString(3),
              CnicNo = reader.GetString(4),
              FullAddress = reader.GetString(5),
              CityName = reader.GetString(6),
              ActiveInd = reader.GetBoolean(7),
              CreatedBy = reader.GetString(8),
              CreatedAt = reader.GetDateTime(9)
            };
          }
        }

        return user;
      
    }

    public UserCredentials GetLogin(string userId)
    {
      UserCredentials login = null;

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        string query = "SELECT UserName, Password FROM Customers WHERE UserID = @UserId";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
          login = new UserCredentials
          {            
            UserName = reader.GetString(0),
            Password = reader.GetString(1)
          };
        }
      }

      return login;
    }

    public MeterDetails GetMeter(string userID)
    {
      MeterDetails meter = null;

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        string query = "SELECT MeterID, CustomerID, MeterNo, RefNo, OldRefNo, ConnectionDate, StatusID, MeterLoad, ActiveInd, CreatedBy, CreatedAt FROM Meters WHERE customerID = @userID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", userID);
        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
          meter = new MeterDetails
          {
            MeterID = reader.GetInt32(0),
            CustomerID = reader.GetInt32(1),
            MeterNo = reader.GetString(2),
            RefNo = reader.GetString(3),
            OldRefNo = reader.GetString(4),
            ConnectionDate = reader.GetDateTime(5),
            StatusID = reader.GetInt32(6),
            MeterLoad = reader.GetInt32(7),
            ActiveInd = reader.GetBoolean(8),
            CreatedBy = reader.GetString(9),
            CreatedAt = reader.GetDateTime(10)
          };
        }
      }

      return meter;
    }

    public RecordReadingRequest GetDashboardData(string userID)
    {
      RecordReadingRequest dashboardData = new RecordReadingRequest();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();

        // Query to retrieve BillMain data
        string query = "SELECT MeterID, CustomerID, IssueDate, DueDate, StatusID, BillMonth, ReadingDate, UnitsConsumed, CreatedBy, CreatedAt FROM BillMain WHERE CustomerID = @CustomerID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CustomerID", userID);
        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
          dashboardData.BillMain = new BillMain
          {
            MeterID = reader.GetInt32(0),
            CustomerID = reader.GetInt32(1),
            IssueDate = reader.GetDateTime(2),
            DueDate = reader.GetDateTime(3),
            StatusID = reader.GetInt32(4),
            BillMonth = reader.GetDateTime(5),
            ReadingDate = reader.GetDateTime(6),
            UnitsConsumed = reader.GetInt32(7),
            CreatedBy = reader.GetString(8),
            CreatedAt = reader.GetDateTime(9)
          };
        }

        // Query to retrieve BillDetails data
        query = "SELECT BillID, SlabID, SlabRate, UnitsApplied, SlabAmount FROM BillDetails WHERE BillID = @BillID";
        command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@BillID", dashboardData.BillMain.MeterID);
        reader = command.ExecuteReader();

        dashboardData.BillDetailsList = new List<BillDetails>();
        while (reader.Read())
        {
          dashboardData.BillDetailsList.Add(new BillDetails
          {
            BillID = reader.GetInt32(0),
            SlabID = reader.GetInt32(1),
            SlabRate = reader.GetDecimal(2),
            UnitsApplied = reader.GetInt32(3),
            SlabAmount = reader.GetDecimal(4)
          });
        }
      }

      return dashboardData;
    }
  }
}

    public class eMeterAPI : IeMeterAPI
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        public ResponseMessage SignIn(UserCredentials credentials)
        {
            ResponseMessage response = new ResponseMessage();            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT userID FROM Registration WHERE userName = @userName AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", credentials.UserName);
                command.Parameters.AddWithValue("@password", credentials.Password);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    response.Code = 0; // Success
                    response.Message = "Sign in successful.";
                }
                else
                {
                    response.Code = 1; // Error
                    response.Message = "Invalid username or password.";
                }
            }

            return response;
        }

        public ResponseMessage SignOut(UserCredentials credentials)
        {            
            ResponseMessage response = new ResponseMessage
            {
                Code = 0, // Success
                Message = "Sign out successful."
            };
            return response;
        }

        public ResponseMessage ChangePassword(UserCredentials request)
        {
            ResponseMessage response = new ResponseMessage();
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Registration SET password = @newPassword WHERE userName = @userName AND password = @oldPassword";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", request.UserName);
                command.Parameters.AddWithValue("@oldPassword", request.OldPassword);
                command.Parameters.AddWithValue("@newPassword", request.NewPassword);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    response.Code = 0; // Success
                    response.Message = "Password changed successfully.";
                }
                else
                {
                    response.Code = 1; // Error
                    response.Message = "Invalid username or password.";
                }
            }

            return response;
        }
        public ResponseMessage RecordReading(RecordReadingRequest request)
        {
            var billMain = request.BillMain;
            var billDetailsList = request.BillDetailsList;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into BillMain
                        string insertBillMainQuery = @"INSERT INTO BillMain (meterID, customerID, issueDate, dueDate, statusID, billMonth, readingDate, unitsConsumed, createdBy, createdAt) 
                                                   OUTPUT INSERTED.billID
                                                   VALUES (@MeterID, @CustomerID, @IssueDate, @DueDate, @StatusID, @BillMonth, @ReadingDate, @UnitsConsumed, @CreatedBy, @CreatedAt)";
                        SqlCommand cmdBillMain = new SqlCommand(insertBillMainQuery, connection, transaction);
                        cmdBillMain.Parameters.AddWithValue("@MeterID", billMain.MeterID);
                        cmdBillMain.Parameters.AddWithValue("@CustomerID", billMain.CustomerID);
                        cmdBillMain.Parameters.AddWithValue("@IssueDate", billMain.IssueDate);
                        cmdBillMain.Parameters.AddWithValue("@DueDate", billMain.DueDate);
                        cmdBillMain.Parameters.AddWithValue("@StatusID", billMain.StatusID);
                        cmdBillMain.Parameters.AddWithValue("@BillMonth", billMain.BillMonth);
                        cmdBillMain.Parameters.AddWithValue("@ReadingDate", billMain.ReadingDate);
                        cmdBillMain.Parameters.AddWithValue("@UnitsConsumed", billMain.UnitsConsumed);
                        cmdBillMain.Parameters.AddWithValue("@CreatedBy", billMain.CreatedBy);
                        cmdBillMain.Parameters.AddWithValue("@CreatedAt", billMain.CreatedAt);

                        int billID = (int)cmdBillMain.ExecuteScalar();

                        // Insert into BillDetails
                        foreach (var billDetail in billDetailsList)
                        {
                            string insertBillDetailsQuery = @"INSERT INTO BillDetails (billID, slabID, slabRate, unitsApplied, slabAmount) 
                                                          VALUES (@BillID, @SlabID, @SlabRate, @UnitsApplied, @SlabAmount)";
                            SqlCommand cmdBillDetails = new SqlCommand(insertBillDetailsQuery, connection, transaction);
                            cmdBillDetails.Parameters.AddWithValue("@BillID", billID);
                            cmdBillDetails.Parameters.AddWithValue("@SlabID", billDetail.SlabID);
                            cmdBillDetails.Parameters.AddWithValue("@SlabRate", billDetail.SlabRate);
                            cmdBillDetails.Parameters.AddWithValue("@UnitsApplied", billDetail.UnitsApplied);
                            cmdBillDetails.Parameters.AddWithValue("@SlabAmount", billDetail.SlabAmount);

                            cmdBillDetails.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();

                        return new ResponseMessage
                        {
                            Code = 200,
                            Message = "Record reading successful."
                        };
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction
                        transaction.Rollback();

                        return new ResponseMessage
                        {
                            Code = 500,
                            Message = "An error occurred: " + ex.Message
                        };
                    }
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CustomerID, FullName, EmailAddress, MobileNo, CnicNo, FullAddress, CityName, ActiveInd, CreatedBy, CreatedAt FROM Customers";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        CustomerID = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        EmailAddress = reader.GetString(2),
                        MobileNo = reader.GetString(3),
                        CnicNo = reader.GetString(4),
                        FullAddress = reader.GetString(5),
                        CityName = reader.GetString(6),
                        ActiveInd = reader.GetBoolean(7),
                        CreatedBy = reader.GetString(8),
                        CreatedAt = reader.GetDateTime(9)
                    };
                    users.Add(user);
                }
            }

            return users;
        }

        public ResponseMessage RegisterCustomer(User user)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customers (FullName, EmailAddress, MobileNo, CnicNo, FullAddress, CityName, ActiveInd, CreatedBy, CreatedAt) VALUES (@FullName, @EmailAddress, @MobileNo, @CnicNo, @FullAddress, @CityName, @ActiveInd, @CreatedBy, @CreatedAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                command.Parameters.AddWithValue("@MobileNo", user.MobileNo);
                command.Parameters.AddWithValue("@CnicNo", user.CnicNo);
                command.Parameters.AddWithValue("@FullAddress", user.FullAddress);
                command.Parameters.AddWithValue("@CityName", user.CityName);
                command.Parameters.AddWithValue("@ActiveInd", user.ActiveInd);
                command.Parameters.AddWithValue("@CreatedBy", user.CreatedBy);
                command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "User registered successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage CreateCredentials(Credentials credentials)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Registration (userName, password, customerID, activeInd, activeDate, createdBy, createdAt) VALUES (@userName, @password, @customerID, @activeInd, @activeDate, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", credentials.UserName);
                command.Parameters.AddWithValue("@password", credentials.Password);
                command.Parameters.AddWithValue("@customerID", credentials.CustomerID);
                command.Parameters.AddWithValue("@activeInd", credentials.ActiveInd);
                command.Parameters.AddWithValue("@activeDate", credentials.ActiveDate);
                command.Parameters.AddWithValue("@createdBy", credentials.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", credentials.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Credentials created successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage AddMeter(MeterType meter)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO MeterType (typeID, typeName, activeInd, createdBy, createdAt) VALUES (@typeID, @typeName, @activeInd, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@typeID", meter.TypeID);
                command.Parameters.AddWithValue("@typeName", meter.TypeName);
                command.Parameters.AddWithValue("@activeInd", meter.ActiveInd);
                command.Parameters.AddWithValue("@createdBy", meter.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", meter.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Meter added successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

        public ResponseMessage RegisterMeterWithCustomer(MeterDetails meterDetails)
        {
            ResponseMessage response = new ResponseMessage();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO MeterDetails (meterID, customerID, meterNo, refNo, oldRefNo, connectionDate, statusID, meterLoad, activeInd, createdBy, createdAt) " +
                               "VALUES (@meterID, @customerID, @meterNo, @refNo, @oldRefNo, @connectionDate, @statusID, @meterLoad, @activeInd, @createdBy, @createdAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@meterID", meterDetails.MeterID);
                command.Parameters.AddWithValue("@customerID", meterDetails.CustomerID);
                command.Parameters.AddWithValue("@meterNo", meterDetails.MeterNo);
                command.Parameters.AddWithValue("@refNo", meterDetails.RefNo);
                command.Parameters.AddWithValue("@oldRefNo", meterDetails.OldRefNo);
                command.Parameters.AddWithValue("@connectionDate", meterDetails.ConnectionDate);
                command.Parameters.AddWithValue("@statusID", meterDetails.StatusID);
                command.Parameters.AddWithValue("@meterLoad", meterDetails.MeterLoad);
                command.Parameters.AddWithValue("@activeInd", meterDetails.ActiveInd);
                command.Parameters.AddWithValue("@createdBy", meterDetails.CreatedBy);
                command.Parameters.AddWithValue("@createdAt", meterDetails.CreatedAt);

                try
                {
                    command.ExecuteNonQuery();
                    response.Code = 0; // Success
                    response.Message = "Meter registered with customer successfully.";
                }
                catch (Exception ex)
                {
                    response.Code = 1; // Error
                    response.Message = "Error: " + ex.Message;
                }
            }

            return response;
        }

    }
}
