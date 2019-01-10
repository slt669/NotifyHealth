using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotifyHealth.Models;
namespace NotifyHealth.Data_Access_Layer
{
    public class NotifyHealthDB
    {
        public String ReturnValidationError;
        public String ReturnValidationMessage;
        public String StartDate;
        public String EndDate;
        public String ReturnError;
        public String EndUserName;
        public String SessionStatus;
        public String SessionId;
        public String SessionGUID;
        public String TenantId;
        public int CompanyID;
        public String PageName;
        public String Password;
        public String Title;
        public String Forename;
        public String Surname;
        public String EmailAddress;
        public String MobileTelephoneNo;
        public String WorkTelephoneNo;
        public String HomeTelephoneNo;
        public String FaxTelephoneNo;
        public Int32 SecurityQuestionId;
        public String SecurityQuestion;
        public String SecurityAnswer;
        public String DataViewSystem;
        public String DataViewId;
        public String DataViewQueryId;
        //public SqlDataSource OutputDataSet;
        //private String StringId;
        private String strConnection;
        public DataSet ds;
        public DataSet ds1;
        public String MustChangePwd;
        //private SqlCommand sqlComm;
        //private SqlConnection sqlConn;
        //private SqlDataAdapter da;
        //private SqlConnection sqlcon;
        private String StoredProcedure;


        public string CheckLogon(String UserName)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["CustomerWebControlDB"].ConnectionString;
                StoredProcedure = "usp100CheckLogon";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) ReturnValidationMessage = "ok";
                    else ReturnValidationMessage = "Username not recognized";

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return ReturnValidationMessage;
            }
            catch (Exception ex)
            {
                //Global.gExceptionMessage = "DataControl.cs - " + ex.Message;
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }



        public void GetSession(String UserName, String Password, String PageName)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["CustomerWebControlDB"].ConnectionString;
                StoredProcedure = "usp100CreateSession";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;
                    command.Parameters.Add("@PageName", SqlDbType.NVarChar, 100).Value = PageName;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //Console.WriteLine(reader[0].ToString());
                        SessionId = reader["UserSessionId"].ToString();
                        SessionGUID = reader["UserSessionGUID"].ToString();

                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
            }
            catch (Exception ex)
            {
                //Global.gExceptionMessage = "DataControl.cs - " + ex.Message;
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }


        public void CheckSession(Int32 SessionId, String SessionGUID, String PageName)
        {
            ReturnValidationError = "99999";

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["CustomerWebControlDB"].ConnectionString;
                StoredProcedure = "usp101CheckSession";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@SessionId", SqlDbType.Int, 4).Value = SessionId;
                    command.Parameters.Add("@SessionGUID", SqlDbType.NVarChar, 50).Value = SessionGUID;
                    command.Parameters.Add("@PageName", SqlDbType.NVarChar, 100).Value = PageName;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //Console.WriteLine(reader[0].ToString());
                        SessionStatus = reader["SessionStatus"].ToString();
                        EndUserName = reader["EndUserName"].ToString();
                        MustChangePwd = reader["MustChangePwd"].ToString();
                        TenantId = reader["TenantID"] != null ? reader["TenantID"].ToString() : "";
                        CompanyID = reader["CompanyID"] as int? ?? default(int);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0" & ReturnError != "10015")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }

                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();

                    if (ReturnValidationError != "0")
                    {
                        //Global.gStatusMessage = ReturnValidationMessage;
                    }
                    else
                    {
                        //Global.gStatusMessage = SessionStatus;
                        //Global.gUserLogonName = EndUserName;
                    }
                }
            }
            catch (Exception ex)
            {
                //Global.gExceptionMessage = "DataControl.cs - " + ex.Message;
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///   /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///     /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Programs> GetPrograms(int? organizationID)
        {

            List<Programs> PL = new List<Programs>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp001GetPrograms";



                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    //command.Parameters.Add("@SpeedDial", SqlDbType.BigInt,4).Value = SpeedDial;

                    //command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Programs PD = new Programs();
                        PD.Description = reader["Description"] as string;
                        PD.Name = reader["Name"] as string;
                        PD.ProgramId = reader["Program_ID"] as int? ?? default(int);
                        PD.StatusId = reader["Status_ID"] as int? ?? default(int);
                        PL.Add(PD);
                    }

                    reader.Close();
                    connection.Close();

                    //ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    //ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    //ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();



                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return PL;
        }
        public List<Campaigns> GetCampaigns(int? organizationID)
        {

            List<Campaigns> CL = new List<Campaigns>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp005GetCampaigns";



                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    //command.Parameters.Add("@SpeedDial", SqlDbType.BigInt,4).Value = SpeedDial;

                    //command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campaigns CD = new Campaigns();
                        CD.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        CD.Description = reader["Description"] as string;
                        CD.Name = reader["Name"] as string;
                        CD.ProgramId = reader["Program_ID"] as int? ?? default(int);
                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();

                    //ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    //ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    //ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();



                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CL;
        }
        public List<Notifications> GetNotifications(int? organizationID)
        {

            List<Notifications> NL = new List<Notifications>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp006GetNotifications";



                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    //command.Parameters.Add("@SpeedDial", SqlDbType.BigInt,4).Value = SpeedDial;

                    //command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Notifications ND = new Notifications();
                        ND.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        ND.NotificationId = reader["Notification_ID"] as int? ?? default(int);
                        ND.NTypeId = reader["N_Type_ID"] as int? ?? default(int);
                        ND.Period = reader["Period"] as int? ?? default(int);
                        ND.StatusId = reader["Status_ID"] as int? ?? default(int);
                        ND.Text = reader["Text"] as string;
                        NL.Add(ND);
                    }

                    reader.Close();
                    connection.Close();

                    //ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    //ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    //ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();



                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return NL;
        }
        public List<Clients> GetClients(int? organizationID)
        {

            List<Clients> CL = new List<Clients>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp004GetClients";



                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    //command.Parameters.Add("@SpeedDial", SqlDbType.BigInt,4).Value = SpeedDial;

                    //command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    //command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    // Open the connection and execute the insert command. 

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Clients CD = new Clients();
                        CD.ATypeId = reader["A_Type_ID"] as int? ?? default(int);
                        CD.ClientId = reader["Client_ID"] as int? ?? default(int);
                        CD.FirstName = reader["First_Name"] as string;
                        CD.LastName = reader["Last_Name"] as string;
                        CD.MessageAddress = reader["Message_Address"] as string;
                        CD.ParticipationId = reader["Participation_ID"] as int? ?? default(int);
                        CD.PhoneCarrier = reader["Phone_Carrier"] as string;
                        CD.PhoneNumber = reader["Phone_Number"] as string;
                        CD.PStatusId = reader["P_Status_ID"] as int? ?? default(int);
                        CD.CStatusId = reader["C_Status_ID"] as int? ?? default(int);
                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();

                    //ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    //ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    //ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();



                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CL;
        }

        public void UpdatePrograms(int? OrganizationId,string Description, string Name, int? ProgramId,char Delete)
        {
            //ReturnValidationError = "99999";

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp002UpdatePrograms";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {

                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = 1;
                    command.Parameters.Add("@ProgramId", SqlDbType.BigInt, 4).Value = ProgramId;
                    command.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = Description;
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 200).Value = Name;
                    command.Parameters.Add("@StatusId", SqlDbType.BigInt, 4).Value = 1;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();


                    //ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    //ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                //Global.gExceptionMessage = "DataControl.cs - " + ex.Message;
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }
    }
}