using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace NotifyHealth.Utils
{
    public class UserManager
    {
        private NotifyHealthDB dbc = new NotifyHealthDB();

        public string SessionId;
        public string SessionGUID;
        public string OrganizationID;
        public string Organization;
        public string adminLogonId;
        public string strReturnValidationError;
        public string strReturnValidationMessage;

        //public List<AccessRights> ar = new List<AccessRights>();
        public AccountSettingsViewModel accset = new AccountSettingsViewModel();

        //public DataAccessRights dar = new DataAccessRights();
        //public List<Tenant> ltn = new List<Tenant>();
        //public List<Users> usr = new List<Users>();

        public bool IsValid(string email, string password)
        {
            bool retval = false;

            try
            {
                dbc.GetSession(email, password, "LogonActivateAccount");
            }
            catch (Exception ex)
            {
                var LoginError = ex.Message;
            }

            SessionId = dbc.SessionId;
            SessionGUID = dbc.SessionGUID;
            strReturnValidationError = dbc.ReturnValidationError;
            strReturnValidationMessage = dbc.ReturnValidationMessage;

            try
            {
                dbc.CheckSession(Convert.ToInt32(SessionId), SessionGUID, "LogonActivateAccount");
            }
            catch (Exception ex)
            {
                var LoginError = ex.Message;
            }

            OrganizationID = dbc.OrganizationID;
            Organization = dbc.Organization;

            if (strReturnValidationError == "0")
            {
                //load pages for menu

                accset = dbc.GetAccountDetails(Convert.ToInt32(dbc.SessionId), SessionGUID);
                //ar = dbc.GetContentAccessRights(Convert.ToInt32(dbc.SessionId), accset.UserLogonID.ToString());
                //dar = dbc.GetDataAccessRights(Convert.ToInt32(SessionId));
                //ltn = dbc.GetTenant(Convert.ToInt32(SessionId));

                //if (accset.UserRole == 1 || accset.UserRole == 2)
                //{
                //    usr = dbc.GetUserDropdown(Convert.ToInt32(SessionId), Convert.ToInt32(ltn.First().TenantID), accset.CompanyId, accset.UserLogonID);
                //}
                //if (ltn.Count() == 1) { TenantId = ltn.FirstOrDefault().TenantID; }

                retval = true;
            }

            return retval;
        }

        //    public List<Users> GetUserDropdown(int SessionId, int TenantID, int CompanyId, int UserLogonID)
        //    {
        //        try
        //        {
        //            //usr = dbc.GetUserDropdown(Convert.ToInt32(SessionId), Convert.ToInt32(TenantID), Convert.ToInt32(CompanyId), Convert.ToInt32(UserLogonID));
        //        }
        //        catch (Exception ex)
        //        {
        //            var LoginError = ex.Message;
        //        }

        //        //return usr;
        //    }

        //    public bool ChangeSesssion(string user, string session, string sessionGUID, int? adminUser, string tenantID)
        //    {
        //    //    bool retval = false;

        //    //    try
        //    //    {
        //    //        dbc.UpdateSession(Convert.ToInt32(session), user, adminUser);
        //    //        //Session["AdminLoginId"] = dbc.SessionId;
        //    //        //Session["UserLogonId"] = User;
        //    //        //Session["UserFullName"] = name;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        var LoginError = ex.Message;
        //    //    }

        //    //    TenantId = tenantID;
        //    //    CompanyID = dbc.CompanyID;
        //    //    SessionId = session;
        //    //    SessionGUID = sessionGUID;
        //    //    strReturnValidationError = dbc.ReturnValidationError;
        //    //    strReturnValidationMessage = dbc.ReturnValidationMessage;

        //    //    if (strReturnValidationError == "0")
        //    //    {
        //    //        //load pages for menu
        //    //        ar = dbc.GetContentAccessRights(Convert.ToInt32(SessionId), user);
        //    //        accset = dbc.GetAccountDetails(Convert.ToInt32(SessionId), SessionGUID);
        //    //        dar = dbc.GetDataAccessRights(Convert.ToInt32(SessionId));
        //    //        ltn = dbc.GetTenant(Convert.ToInt32(SessionId));
        //    //        if (ltn.Count() == 1) { TenantId = ltn.FirstOrDefault().TenantID; }
        //    //        usr = dbc.GetUserDropdown(Convert.ToInt32(SessionId), Convert.ToInt32(tenantID), accset.CompanyId, Convert.ToInt32(user));

        //    //        retval = true;
        //    //    }

        //    //    return retval;
        //    //}

        //}

        public class AccessRights
        {
            public string ContentName { get; set; }
            public string ContentLabel { get; set; }
            public string ContentDescription { get; set; }
            public string MenuItem { get; set; }
            public int DisplayOrder { get; set; }
        }

        public class MenuModel
        {
            public List<AccessRights> menu { get; set; }

            //public List<Tenant> tenants { get; set; }
            public DataAccessRights dataAccessRights { get; set; }

            public List<Users> user { get; set; }

            //public Tenant defaultTenant { get; set; }
            public string tenantId { get; set; }
        }

        public class DataAccessRights
        {
            public string AllQuotes { get; set; }
            public string PersonalQuotes { get; set; }
            public string SalesCost { get; set; }
            public string CarrierCost { get; set; }
            public string UpdateCost { get; set; }
            public string UpdateCarrier { get; set; }
            public string UpdateOwner { get; set; }
            public string UpdateSalesCost { get; set; }
            public string Payback { get; set; }
        }

        public class UserUnapprovedQuotes
        {
            public string Unapproved { get; set; }
            public string Approver { get; set; }
        }

        public class Users
        {
            public int UserId { get; set; }
            public string Forename { get; set; }
            public string Surname { get; set; }
            public string UserStatus { get; set; }
            public string EmailAddress { get; set; }
            public int UserLogonID { get; set; }
            public string Username { get; set; }
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string CustomerType { get; set; }
        }
    }

    public class Companies
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}