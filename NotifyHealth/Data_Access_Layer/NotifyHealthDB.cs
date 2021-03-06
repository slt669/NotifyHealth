﻿using NotifyHealth.Models;
using NotifyHealth.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

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
        public String OrganizationID;
        public String Organization;
        public String Portal;
        public String Logo;
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
        private String strConnection;
        public DataSet ds;
        public DataSet ds1;
        public String MustChangePwd;
        private String StoredProcedure;

        public string CheckLogon(String UserName)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp200CheckLogon";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

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
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void GetSession(String UserName, String Password, String PageName)
        {
            try
            {
                string Encpassword = Encrypt(Password, "27012019");
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp100CreateSession";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Encpassword;
                    command.Parameters.Add("@PageName", SqlDbType.NVarChar, 100).Value = PageName;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
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
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void CheckSession(Int32 SessionId, String SessionGUID, String PageName)
        {
            ReturnValidationError = "99999";

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
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
                        SessionStatus = reader["SessionStatus"].ToString();
                        EndUserName = reader["EndUserName"].ToString();
                        MustChangePwd = reader["MustChangePwd"].ToString();
                        OrganizationID = reader["OrganizationID"] != null ? reader["OrganizationID"].ToString() : "";
                        Organization = reader["Organization"].ToString();
                        Portal = reader["Portal"].ToString();
                        Logo = reader["Logo"].ToString();
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
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public AccountSettingsViewModel GetAccountDetails(Int32 SessionId, String SessionGUID)
        {
            ReturnValidationError = "99999";

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp103GetAccountDetails";

                AccountSettingsViewModel asvm = new AccountSettingsViewModel();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@SessionId", SqlDbType.Int, 4).Value = SessionId;
                    command.Parameters.Add("@SessionGUID", SqlDbType.NVarChar, 50).Value = SessionGUID;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string Decpassword = reader["Password"].ToString();
                        //string Decpassword = Decrypt(reader["Password"].ToString(), "27012019");
                        asvm.OldPassword = Decpassword;
                        asvm.Title = reader["Title"].ToString();
                        asvm.Forename = reader["Forename"].ToString();
                        asvm.Surname = reader["Surname"].ToString();
                        asvm.LogonName = reader["Username"].ToString();
                        asvm.WorkTelephoneNo = reader["WorkTelephoneNo"].ToString();
                        asvm.JobTitle = reader["JobTitle"].ToString();
                        int x = reader.GetOrdinal("SecurityQuestionId");
                        if (!reader.IsDBNull(x)) asvm.HintQuestionID = Convert.ToInt16(reader["SecurityQuestionId"]);
                        asvm.HintAnswer = reader["SecurityAnswer"].ToString();
                        asvm.MustChangePwd = reader["MustChangePwd"].ToString();
                        asvm.UserID = Convert.ToInt32(reader["UserID"]);
                        asvm.UserLogonID = Convert.ToInt32(reader["UserLogonID"]);
                        asvm.UserRole = reader["UserRoleID"] as int? ?? default(int);
                        asvm.PhotoPath = reader["Photo"] as string;
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }

                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();

                    if (ReturnValidationError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return asvm;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetSecurityQuestions(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp107ListSecurityQuestion";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["SecurityQuestionID"].ToString();
                        sq.Text = reader["SecurityQuestion"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public string ManageAccount(Int32 SessionId, String SessionGUID, AccountSettingsViewModel asvm)
        {
            string ReturnValidationError;
            string Encpassword = null;
            string EncCheckpassword = null;
            try
            {
                if (asvm.NewPassword != null)
                {
                    Encpassword = Encrypt(asvm.NewPassword, "27012019");
                }
                if (asvm.NewPassword != null)
                {
                    EncCheckpassword = Encrypt(asvm.CheckPassword, "27012019");
                }

                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp102ManageAccount";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@SessionId", SqlDbType.Int, 4).Value = SessionId;
                    command.Parameters.Add("@SessionGUID", SqlDbType.NVarChar, 36).Value = SessionGUID;
                    command.Parameters.Add("UpdateType", SqlDbType.NVarChar, 20).Value = "EndUserUpdate";
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Encpassword;
                    command.Parameters.Add("@PasswordRepeat", SqlDbType.NVarChar, 50).Value = EncCheckpassword;
                    command.Parameters.Add("@Forename", SqlDbType.NVarChar, 50).Value = asvm.Forename;
                    command.Parameters.Add("@Surname", SqlDbType.NVarChar, 50).Value = asvm.Surname;
                    command.Parameters.Add("@WorkTelephoneNo", SqlDbType.NVarChar, 50).Value = asvm.WorkTelephoneNo;
                    command.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 50).Value = asvm.JobTitle;
                    command.Parameters.Add("@SecurityQuestionId", SqlDbType.Int, 4).Value = asvm.HintQuestionID;
                    command.Parameters.Add("@SecurityAnswer", SqlDbType.NVarChar, 50).Value = asvm.HintAnswer;
                    command.Parameters.Add("@Photo", SqlDbType.NVarChar, 500).Value = asvm.PhotoPath;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                        EndUserName = reader["EndUserName"].ToString();
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }

                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();

                    if (ReturnValidationError != "0")
                    {
                        return ReturnValidationMessage;
                    }
                }

                return "Account Settings updated successfully!";
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void LogoutSession(Int32 SessionId, String SessionGUID)
        {
            ReturnValidationError = "99999";

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp109LogoutSession";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@SessionId", SqlDbType.Int, 4).Value = SessionId;
                    command.Parameters.Add("@SessionGUID", SqlDbType.VarChar, 36).Value = SessionGUID;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();

                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
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

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Programs PD = new Programs();
                        PD.Description = reader["Description"] as string;
                        PD.Name = reader["Name"] as string;
                        PD.ProgramId = reader["Program_ID"] as int? ?? default(int);
                        PD.StatusId = reader["Status_ID"] as int? ?? default(int);
                        PD.Status = reader["Value"] as string;
                        PD.RelatedCampaigns = reader["RelatedCampaigns"] as int? ?? default(int);
                        PL.Add(PD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return PL;
        }

        public List<Campaigns> GetCampaigns(int? Organization_ID)
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

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = @Organization_ID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campaigns CD = new Campaigns();
                        CD.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        CD.Description = reader["Description"] as string;
                        CD.Name = reader["Campaign"] as string;
                        CD.ProgramId = reader["Program_ID"] as int? ?? default(int);
                        CD.Status = reader["Value"] as string;
                        CD.StatusId = reader["Status_ID"] as int? ?? default(int);
                        CD.Program = reader["Program"] as string;
                        CD.RelatedNotifications = reader["RelatedNotifications"] as int? ?? default(int);
                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();
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
                        ND.NotificationType = reader["NotificationType"] as string;
                        ND.Campaign = reader["Campaign"] as string;
                        ND.Status = reader["Value"] as string;
                        ND.StatusId = reader["Status"] as int? ?? default(int);
                        if (Convert.ToDateTime(reader["Created_When"]) != null)
                        {
                            ND.Start = Convert.ToDateTime(reader["Created_When"]);
                        }
                  
                        ND.End = reader["Edited_When"] as DateTime?;
                        NL.Add(ND);
                    }

                    reader.Close();
                    connection.Close();
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
                        CD.ParticipationReason = reader["ParticipationReason"] as string;
                        CD.ClientStatus = reader["ClientStatus"] as string;
                        CD.PhoneStatus = reader["PhoneStatus"] as string;
                        CD.AccountType = reader["AccountType"] as string;
                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CL;
        }

        public List<ClientMemberships> GetClientMemberships(int? organizationID, int? ClientID)
        {
            List<ClientMemberships> CM = new List<ClientMemberships>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp0116GetClientMemberships";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    command.Parameters.Add("@Client_ID", SqlDbType.BigInt, 4).Value = ClientID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ClientMemberships CD = new ClientMemberships();
                        CD.ClientMembershipId = reader["Client_Membership_ID"] as int? ?? default(int);
                        CD.ClientId = reader["Client_ID"] as int? ?? default(int);
                        CD.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        if (Convert.ToDateTime(reader["Start"]) != null)
                        {
                            DateTime Timestamp = Convert.ToDateTime(reader["Start"]);
                            CD.Start = Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
                            CD.SortTime = Timestamp.Ticks;
                        }
                        CD.Program = reader["Program"] as string;
                        CD.Campaign = reader["Campaign"] as string;
                        CM.Add(CD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CM;
        }

        public List<Transactions> GetTransactions(int? organizationID, int? ClientID)
        {
            List<Transactions> T = new List<Transactions>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp117GetGetTransactions";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = organizationID;
                    command.Parameters.Add("@Client_ID", SqlDbType.BigInt, 4).Value = ClientID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Transactions TD = new Transactions();
                        TD.TransactionId = reader["Transaction_ID"] as int? ?? default(int);
                        TD.ClientId = reader["Client_ID"] as int? ?? default(int);
                        TD.Notification = reader["Notification"] as string;
                        TD.Result = reader["Result"] as string;
                        if (Convert.ToDateTime(reader["Timestamp"]) != null)
                        {
                            DateTime Timestamp = Convert.ToDateTime(reader["Timestamp"]);
                            TD.Timestamp = Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
                            TD.SortTime = Timestamp.Ticks;
                        }
                    
                     
                        T.Add(TD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return T;
        }

        public void UpdatePrograms(int? OrganizationId, string Description, string Name, int? ProgramId, int Created_By, int Edited_By, int? StatusId, char Delete)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp002UpdatePrograms";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@ProgramId", SqlDbType.BigInt, 4).Value = ProgramId;
                    command.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = Description;
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 200).Value = Name;
                    command.Parameters.Add("@StatusId", SqlDbType.BigInt, 4).Value = StatusId;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    command.Parameters.Add("@Created_By", SqlDbType.BigInt, 4).Value = Created_By;
                    command.Parameters.Add("@Edited_By", SqlDbType.BigInt, 4).Value = Edited_By;
                    command.Parameters.Add("@Created_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    command.Parameters.Add("@Edited_When", SqlDbType.DateTime, 4).Value = DateTime.Now;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void UpdateCampaigns(int? OrganizationId, string Description, string Name, int? CampaignId, int? ProgramId, int Created_By, int Edited_By, int? StatusId, char Delete)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp007UpdateCampaigns";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@CampaignId", SqlDbType.BigInt, 4).Value = CampaignId;
                    command.Parameters.Add("@ProgramId", SqlDbType.BigInt, 4).Value = ProgramId;
                    command.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = Description;
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 200).Value = Name;
                    command.Parameters.Add("@StatusId", SqlDbType.BigInt, 4).Value = StatusId;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    command.Parameters.Add("@Created_By", SqlDbType.BigInt, 4).Value = Created_By;
                    command.Parameters.Add("@Edited_By", SqlDbType.BigInt, 4).Value = Edited_By;
                    command.Parameters.Add("@Created_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    command.Parameters.Add("@Edited_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void UpdateNotifications(int? OrganizationId, string Text, int Period, int NTypeId, int? NotificationId, int? CampaignId, int Created_By, int Edited_By, int? StatusId, char Delete)
        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp008UpdateNotifications";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@NotificationId", SqlDbType.BigInt, 4).Value = NotificationId;
                    command.Parameters.Add("@Text", SqlDbType.VarChar, 200).Value = Text;
                    command.Parameters.Add("@Period", SqlDbType.Int, 200).Value = Period;
                    command.Parameters.Add("@NTypeId", SqlDbType.BigInt, 4).Value = NTypeId;
                    command.Parameters.Add("@CampaignId", SqlDbType.BigInt, 4).Value = CampaignId;
                    command.Parameters.Add("@StatusId", SqlDbType.BigInt, 4).Value = StatusId;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    command.Parameters.Add("@Created_By", SqlDbType.BigInt, 4).Value = Created_By;
                    command.Parameters.Add("@Edited_By", SqlDbType.BigInt, 4).Value = Edited_By;
                    command.Parameters.Add("@Created_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    command.Parameters.Add("@Edited_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public string UpdateClients(int? OrganizationId, string FirstName, string LastName, int CStatusId, int PStatusId, int ATypeID, string PhoneNumber, string MessageAddress, int ParticipationID, string PhoneCarrier, int ClientId, int Created_By, int Edited_By, char Delete)
        {
            string Success = "";
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp009UpdateClients";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@ClientId", SqlDbType.BigInt, 4).Value = ClientId;
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 200).Value = FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 200).Value = LastName;
                    command.Parameters.Add("@CStatusID", SqlDbType.BigInt, 4).Value = CStatusId;
                    command.Parameters.Add("@PStatusID", SqlDbType.BigInt, 4).Value = PStatusId;
                    command.Parameters.Add("@ATypeID", SqlDbType.BigInt, 4).Value = ATypeID;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 12).Value = PhoneNumber;
                    command.Parameters.Add("@PhoneCarrier", SqlDbType.VarChar, 50).Value = PhoneCarrier;
                    command.Parameters.Add("@MessageAddress", SqlDbType.VarChar, 50).Value = MessageAddress;
                    command.Parameters.Add("@ParticipationID", SqlDbType.BigInt, 4).Value = ParticipationID;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    command.Parameters.Add("@Created_By", SqlDbType.BigInt, 4).Value = Created_By;
                    command.Parameters.Add("@Edited_By", SqlDbType.BigInt, 4).Value = Edited_By;
                    command.Parameters.Add("@Created_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    command.Parameters.Add("@Edited_When", SqlDbType.DateTime, 4).Value = DateTime.Now;
                    command.Parameters.Add("@IDENTITY", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();
                    if (command.Parameters["@IDENTITY"].Value is DBNull)
                    {
                    }
                    else
                    {
                        ClientId = (int)command.Parameters["@IDENTITY"].Value;
                    }

                    if (ParticipationID == 11 || ParticipationID == 8)
                    {
                        // Popup warning that given phone number CANNOT BE USED to send messages but client will be saved.

                        //  Submit button creates client account or updates user record but DO NOT INSERT into queue table
                    }
                    else if (ParticipationID == 12)
                    {
                        //Phone.Warning = "Submit button inserts new record into Queue table and adds record to clients table";
                         Success = QueueOnBoardingNotification(OrganizationId, ClientId, 1);
                    }

                    return Success;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public void UpdateClientMembership(int? OrganizationId, int CampaignId, int ClientID, DateTime start, char Delete)

        {
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp122UpdateClientMembership";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@ClientId", SqlDbType.BigInt, 4).Value = ClientID;
                    command.Parameters.Add("@CampaignId", SqlDbType.BigInt, 4).Value = CampaignId;
                    command.Parameters.Add("@Start", SqlDbType.DateTime).Value = start;
                    command.Parameters.Add("@Appointment", SqlDbType.DateTime, 12).Value = DateTime.Now;
                    command.Parameters.Add("@Delete", SqlDbType.Char, 1).Value = Delete;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetClientStatus(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp110GetClientStatus";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["C_Status_ID"].ToString();
                        sq.Text = reader["Value"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetAccountTypes(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp111GetAccountTypes";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["A_Type_ID"].ToString();
                        sq.Text = reader["Value"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetNotificationTypes(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp112GetNotificationTypes";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["N_Type_ID"].ToString();
                        sq.Text = reader["Value"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetParticipationReasons(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp113GetParticipationReasons";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["Participation_ID"].ToString();
                        sq.Text = reader["Value"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetPhoneStatus(string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp114GetPhoneStatus";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["P_Status_ID"].ToString();
                        sq.Text = reader["Value"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetProgramDDL(int OrganizationId,string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp118GetProgramDDL";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.BigInt, 4).Value = OrganizationId;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["Program_ID"].ToString();
                        sq.Text = reader["Name"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<SelectListItem> GetCampaignDDL(int? Program_ID, string selected = "")
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp119GetCampaignDDL";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@Program_ID", SqlDbType.BigInt, 4).Value = Program_ID;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SelectListItem sq = new SelectListItem();
                        sq.Value = reader["Campaign_ID"].ToString();
                        sq.Text = reader["Name"].ToString();
                        if (selected == sq.Value)
                        {
                            sq.Selected = true;
                        }

                        sqs.Add(sq);
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();

                    if (ReturnError != "0")
                    {
                        throw new ApplicationException("Error Code " + ReturnError + " returned from " + StoredProcedure);
                    }
                }
                return sqs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public List<Campaigns> GetCampaignsbyProgram(int? Organization_ID, int? ProgramId)
        {
            List<Campaigns> CL = new List<Campaigns>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp120GetCampaignsbyProgram";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = Organization_ID;
                    command.Parameters.Add("@ProgramId", SqlDbType.BigInt, 4).Value = ProgramId;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campaigns CD = new Campaigns();
                        CD.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        CD.Description = reader["Description"] as string;
                        CD.ProgramId = reader["Program_ID"] as int? ?? default(int);
                        CD.Name = reader["Name"] as string;
                        CD.Status = reader["Value"] as string;

                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CL;
        }

        public List<Notifications> GetNotificationsbyCampaigns(int? Organization_ID, int? CampaignID)
        {
            List<Notifications> CL = new List<Notifications>();
            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp121GetNotificationsbyCampaigns";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@Organization_ID", SqlDbType.BigInt, 4).Value = Organization_ID;
                    command.Parameters.Add("@Campaign_ID", SqlDbType.BigInt, 4).Value = CampaignID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Notifications CD = new Notifications();
                        CD.CampaignId = reader["Campaign_ID"] as int? ?? default(int);
                        CD.Text = reader["Text"] as string;
                        CD.Period = reader["Period"] as int? ?? default(int);
                        CD.Status = reader["Value"] as string;
                        CD.StatusId = reader["Status_ID"] as int? ?? default(int);
                        CD.NTypeId = reader["N_Type_ID"] as int? ?? default(int);
                        CD.NotificationType = reader["NotificationType"] as string;
                        CD.NotificationId = reader["Notification_ID"] as int? ?? default(int);
                        CL.Add(CD);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return CL;
        }

        public string QueueOnBoardingNotification(int? OrganizationId, int ClientId, int NotificationType)
        {
            try
            {
                var strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                var StoredProcedure = "usp115QueueOnBoardingNotification";

                List<SelectListItem> sqs = new List<SelectListItem>();

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@OrganizationId", SqlDbType.Int, 4).Value = OrganizationId;
                    command.Parameters.Add("@ClientId", SqlDbType.Int, 4).Value = ClientId;
                    command.Parameters.Add("@NotificationType", SqlDbType.Int, 4).Value = NotificationType;
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int, 4).Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add("@ValidationMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ValidationErrorNo", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }

                    reader.Close();
                    connection.Close();

                    ReturnError = command.Parameters["@ReturnValue"].Value.ToString();
                    ReturnValidationError = command.Parameters["@ValidationErrorNo"].Value.ToString();
                    ReturnValidationMessage = command.Parameters["@ValidationMessage"].Value.ToString();
                    if (ReturnValidationError != "0")
                    {
                        return ReturnValidationMessage;
                    }
                }
                return "Notification Inserted";
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
        }

        public DashboardViewModel GetDashboardDetails(int? organizationID)
        {
            DashboardViewModel DVM = new DashboardViewModel();

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp003GetDashboardDetails";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@OrganizationID", SqlDbType.BigInt, 4).Value = organizationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DVM.NewClientsLast30 = reader["NewClientsLastThirty"] as int? ?? default(int);
                        DVM.NoOfClients = reader["NoOfClients"] as int? ?? default(int);
                        DVM.NotificationsSentLast30 = reader["NotificationsSentLastThirty"] as int? ?? default(int);
                        DVM.NotificationsSentToday = reader["NotificationsSentToday"] as int? ?? default(int);
                        if (Convert.ToDateTime(reader["NoOfClientDate"]) != null)
                        {
                            DVM.NoOfClientDate = Convert.ToDateTime(reader["NoOfClientDate"]).ToShortDateString();
                        }
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return DVM;
        }

        public List<BarChart> GetBarChartDetails(int? organizationID)
        {
            List<BarChart> BAR = new List<BarChart>();

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp123GetBarChartDetails";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@OrganizationID", SqlDbType.BigInt, 4).Value = organizationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BarChart B = new BarChart();
                        B.Notifications = reader["Notifications"] as int? ?? default(int);
                        B.ReportingMonth = reader["ReportingMonth"] as string;
                        BAR.Add(B);
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return BAR;
        }

        public SMSAccount GetSMSAccount(int? organizationID)
        {
            SMSAccount SMS = new SMSAccount();

            try
            {
                strConnection = ConfigurationManager.ConnectionStrings["notifyDB"].ConnectionString;
                StoredProcedure = "usp124GetSMSAccount";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlCommand command = new SqlCommand(StoredProcedure);

                    command.CommandTimeout = 6000;

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@OrganizationID", SqlDbType.BigInt, 4).Value = organizationID;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SMS.API_Username = reader["API_Username"] as string;
                        SMS.API_Password = reader["API_Password"] as string;
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "<br /><br />Error Returned To Caller<br /><br />");
            }
            return SMS;
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public string Encrypt(string text, string pwd)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            //byte[] saltBytes = GetRandomBytes();

            // Appending salt bytes to original bytes
            //byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            //for (int i = 0; i < saltBytes.Length; i++)
            //{
            //    bytesToBeEncrypted[i] = saltBytes[i];
            //}
            //for (int i = 0; i < originalBytes.Length; i++)
            //{
            //    bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            //}

            encryptedBytes = AES_Encrypt(originalBytes, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string decryptedText, string pwd)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            //// Getting the size of salt
            //int _saltSize = 4;

            //// Removing salt bytes, retrieving original bytes
            //byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
            //for (int i = _saltSize; i < decryptedBytes.Length; i++)
            //{
            //    originalBytes[i - _saltSize] = decryptedBytes[i];
            //}

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public byte[] GetRandomBytes()
        {
            int _saltSize = 4;
            byte[] ba = new byte[_saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }
    }
}