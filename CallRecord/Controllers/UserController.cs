using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using AdaniCall.Entity;
using AdaniCall.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using AdaniCall.Entity.Enums;
using AdaniCall.Entity.Common;
using AdaniCall.Resources;
using AdaniCall.Business.BusinessFacade;
using AdaniCall.Utility.Common;
using AdaniCall.Entity.ViewModel;

namespace AdaniCall.Controllers
{
    //[Localisation]
    public class UserController : Controller
    {
        // GET: User
        private readonly string _module = "AdaniCall.Controllers.UserController";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private IMemoryCache _cache;

        Users objUserEntity = new Users();
        JsonMessage _jsonMessage;
        LoginVM _loginVM = null;
        Helper _helper;

        public UserController(IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _helper = new Helper(_httpContextAccessor);
            _loginVM = _helper.GetSession();
            if (_httpContextAccessor != null)
                _loginVM = _helper.GetSession();
        }

        private string GetUrl(string actionName, string controllerName)
        {
            return "/" + controllerName + "/" + actionName;
        }

        [HttpGet]
        public ActionResult SessionEnd()
        {
            return Redirect(URLDetails.GetSiteRootUrl());

        }

        [HttpGet]
        public string GetCurrentSession()
        {
            string UID = string.Empty;
            try
            {
                if (_loginVM != null)
                    UID = _loginVM.Id.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetCurrentSession()", ex.Source, ex.Message, ex);
            }
            return UID;
        }

        public IActionResult Login()
        {
            _helper = new Helper(_httpContextAccessor);
            _loginVM = _helper.GetSession();
            if (_loginVM != null && _loginVM.Id > 0)
                return RedirectToAction(AdaniCallConstants.DefaultView, AdaniCallConstants.DefaultController);

            if (AdaniCallConstants.DefaultController.ToLower() == "home")
            {
                _httpContextAccessor.HttpContext.Session.SetString(KeyEnums.SessionKeys.UserLanguage.ToString(), "EN");
                _httpContextAccessor.HttpContext.Session.SetString(KeyEnums.SessionKeys.LandingLanguage.ToString(), "EN");
            }

            return View();
        }



        public ActionResult Logout()
        {
            try
            {
                string UserName = string.Empty;
                if (_loginVM != null)
                    UserName = _loginVM.UserName;
                Int64 UserID = 0;
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                //if (HttpContext.Session[KeyEnums.SessionKeys.UserId.ToString()] != null)
                //    UserID = Convert.ToInt64(HttpContext.Session[KeyEnums.SessionKeys.UserId.ToString()]);

                if (UserID > 0)
                {
                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                    objUsers.ChangeAvailabilityStatus(UserID, "2");
                }

                UserLogOut();

                string StrUrl = string.Empty;
                StrUrl = AdaniCallConstants.AdaniCallDomain + "/User/Login";
                // StrUrl = URLDetails.GetSiteRootUrl().TrimEnd('/');
                return Redirect(StrUrl);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Logout()", ex.Source, ex.Message, ex);
                return RedirectToAction("Index", "Home");
            }
        }

        private void UserLogOut()
        {
            try
            {
                ClearCache();//Delete the user details from cache.
                _httpContextAccessor.HttpContext.Session.Clear();

                if (_httpContextAccessor.HttpContext.Request.Cookies["LoginData"] != null)
                {
                    Response.Cookies.Delete("LoginData");
                }
                if (_httpContextAccessor.HttpContext.Request.Cookies["UserLanguage"] != null)
                {
                    Response.Cookies.Delete("UserLanguage");
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "UserLogOut()", ex.Source, ex.Message, ex);
            }

            //FormsAuthentication.SignOut(); //Delete the authentication ticket and sign out.
        }

        private void ClearCache()
        {
           
            _cache.Remove("_userId" + _session.Id + "_" + "true");
            _cache.Remove("AcceptToken_" + _session.Id);
            _cache.Remove("CallToken_" + _session.Id);
        }

        private string GetRedirectUrl()
        {
            if (objUserEntity.RoleId == (byte)RoleEnums.Role.Kiosk)
                    return GetUrl("Call", "Home");
            else
            if (objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                return GetUrl("Accept", "Home");
            else
                return GetUrl(AdaniCallConstants.DefaultView, AdaniCallConstants.DefaultController);
        }

        [HttpPost]
        public JsonResult Login(string username, string password, string queryString, bool isRemember, bool autologin)
        {
            try
            {
                LoginVM loginVM = new LoginVM();

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_emailPasswordRequired, KeyEnums.JsonMessageType.ERROR);
                }
                else
                {
                    password = new Encription().Encrypt(password);
                    _jsonMessage = IsLoginValid(username, password, LoginMode.CMS);

                    if (loginVM != null)
                    {
                        if (_jsonMessage.IsSuccess)
                        {
                            objUserEntity = (Users)_jsonMessage.Data;
                            if (objUserEntity != null)
                            {
                                _helper.SetSession(objUserEntity);
                                if (objUserEntity.ID > 0)
                                {
                                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                                    objUsers.ChangeAvailabilityStatus(objUserEntity.ID, "1");
                                    if (objUserEntity.RoleId == (byte)RoleEnums.Role.Kiosk || objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                                        SetEncryptedCookieKey("AGCXWebLoginUser", username, TimeSpan.MaxValue);
                                }
                            }
                            _helper.SetUserLanguage(objUserEntity.LanguageId);
                            InsertAccessMember(objUserEntity, GetAccessMember(), "Login");
                           // CommonData _CommonData = new CommonData(_cache);

                            if (objUserEntity.RoleId == (byte)RoleEnums.Role.Kiosk)
                               _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain+ "/Home/Call";
                            else
                            if (objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                                _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/Home/Accept";
                            else 
                            if (objUserEntity.RoleId == (byte)RoleEnums.Role.SuperAdmin || objUserEntity.RoleId == (byte)RoleEnums.Role.Admin)
                               _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/Transactions/List";

                            else
                                _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/" + AdaniCallConstants.DefaultController + "/" + AdaniCallConstants.DefaultView + "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Login(username:" + username + ",password:" + password + ")", ex.Source, ex.Message, ex);
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_internalServerErrorOccurred, KeyEnums.JsonMessageType.DANGER, "", "username", string.Format("Method : Login(), Source : {0}, Message {1}", ex.Source, ex.Message));
            }
            return Json(_jsonMessage);
        }
        public AccessMember GetAccessMember()
        {
            AccessMember objAM = new AccessMember();
            try
            {
                objAM.Url = _httpContextAccessor.HttpContext.Request.Host + _httpContextAccessor.HttpContext.Request.Path + _httpContextAccessor.HttpContext.Request.QueryString.Value;
                objAM.ReferrerURL = Request.Headers["Referer"].ToString();
                objAM.port = _httpContextAccessor.HttpContext.Request.Host.Port.Value.ToString();
                objAM.Host = _httpContextAccessor.HttpContext.Request.Host.Value.ToString();
                var browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];//User-Agent

                if (_httpContextAccessor.HttpContext.GetServerVariable("REMOTE_HOST") != null)
                    objAM.REMOTE_HOST = _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_HOST");
                else
                    objAM.REMOTE_HOST = _httpContextAccessor.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_ADDR");

                objAM.REMOTE_ADDR_IP = _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_ADDR") != null ? _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_ADDR") : "";
                objAM.Useragent = (_httpContextAccessor.HttpContext.GetServerVariable("HTTP_USER_AGENT") != null ? _httpContextAccessor.HttpContext.GetServerVariable("HTTP_USER_AGENT").ToString() : "");
                //objAM.BrowserType = (browser.Type != null ? browser.Type.ToString() : "");
                //objAM.BrowserVersion = (browser.Browser != null ? browser.Browser.ToString() : "");
                //objAM.Platform = (browser.Platform != null ? browser.Platform.ToString() : "");
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetAccessMember()", ex.Source, ex.Message, ex);
            }
            return objAM;
        }
        [HttpPost]
        public JsonResult InsertAM(string UniqueCallID, string CallStatus)
        {
            Int64 UserID = 0;
            string _role = "";
            Users objUsers = new Users();
            AccessMember objAM = new AccessMember();
            Helper _helper = new Helper(_httpContextAccessor);
            objAM = GetAccessMember();

            try
            {
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                {
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                    if (_session.GetString(KeyEnums.SessionKeys.UserRole.ToString()) != null)
                    {
                        _role = _session.GetString(KeyEnums.SessionKeys.UserRole.ToString());
                        objAM.UniqueCallID = UniqueCallID;
                        if (_role == Convert.ToString((byte)RoleEnums.Role.Agent))
                        {
                            objAM.CallerID = "";
                        }
                        else if (_role == Convert.ToString((byte)RoleEnums.Role.Kiosk))
                        {
                            objAM.CallerID = "";
                            if (_session.GetString(KeyEnums.SessionKeys.KioskID.ToString()) != null && _session.GetString(KeyEnums.SessionKeys.KioskID.ToString()).ToString() != "")
                                objAM.KioskID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.KioskID.ToString()));
                        }
                    }
                }

                if (UserID > 0)
                {
                    objUsers.ID = UserID;
                    if (CallStatus.ToLower() == "connected")
                    {
                        string AMID = InsertAccessMember(objUsers, objAM, "Call");
                        return Json(AMID);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "InsertAM(UniqueCallID:" + UniqueCallID + ",UserID:" + UserID + ",CallStatus=" + CallStatus + ")", ex.Source, ex.Message);
            }
            return Json("");
        }
        private string InsertAccessMember(Users objUsers, AccessMember objAccessMember, string ClickedBy)
        {
            try
            {
                objAccessMember.UserID = objUsers.ID;
                objAccessMember.ClickedBy = ClickedBy;
                AccessMemberBusinessFacade objAccessMemberBusinessFacade = new AccessMemberBusinessFacade();
                var AMID = objAccessMemberBusinessFacade.Save(objAccessMember);
                return AMID.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "InsertAccessMember(UserID: " + objUsers.ID + ", clickedby:" + ClickedBy + ")", ex.Source, ex.Message);
            }
            return "0";
        }
        [HttpPost]
        public JsonResult InsertAMPing(string pingFrom = "")
        {
            Int64 UserID = 0;
            string _role = "";
            Users objUsers = new Users();
            AccessMember objAM = new AccessMember();
            Helper _helper = new Helper(_httpContextAccessor);
            objAM = GetAccessMember();

            try
            {
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                {
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                    if (_session.GetString(KeyEnums.SessionKeys.UserRole.ToString()) != null)
                    {
                        _role = _session.GetString(KeyEnums.SessionKeys.UserRole.ToString()).ToString();
                        objAM.UniqueCallID = "";
                        if (_role == Convert.ToString((byte)RoleEnums.Role.Agent))
                        {
                            objAM.CallerID = "";
                        }
                        else if (_role == Convert.ToString((byte)RoleEnums.Role.Kiosk))
                        {
                            objAM.CallerID = "";
                            if (_session.GetString(KeyEnums.SessionKeys.KioskID.ToString()) != null && _session.GetString(KeyEnums.SessionKeys.KioskID.ToString()).ToString() != "")
                                objAM.KioskID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.KioskID.ToString()));
                        }
                    }
                }
                string strPing = "Ping";
                if (!string.IsNullOrWhiteSpace(pingFrom))
                    strPing = pingFrom;
                if (UserID > 0 && _role == Convert.ToString((byte)RoleEnums.Role.Kiosk))
                {
                    objUsers.ID = UserID;
                    string AMID = InsertAccessMember(objUsers, objAM, strPing);
                    return Json(AMID);
                }
                else
                {
                    string AMID = InsertAccessMember(objUsers, objAM, strPing);
                    return Json(AMID);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "InsertAMPing(pingFrom:" + pingFrom + ")", ex.Source, ex.Message);
            }
            return Json("");
        }
      
        public JsonMessage IsLoginValid(string username, string password, string LoginMode = "")
        {
            Users objUserEntity = new Users();
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    _jsonMessage = new JsonMessage(false, Resource.lbl_msg_invalidEmailAddress, Resource.lbl_msg_invalidEmailAddress, KeyEnums.JsonMessageType.DANGER);
                else if (string.IsNullOrWhiteSpace(password) && LoginMode == "")
                    _jsonMessage = new JsonMessage(false, Resource.lbl_msg_invalidPassowrd, Resource.lbl_msg_invalidPassowrd, KeyEnums.JsonMessageType.DANGER);
                else
                {
                    string[] Fieldsname = new string[2];
                    string[] Values = new string[2];
                    Fieldsname[0] = username;
                    Fieldsname[1] = password;
                    Values[0] = username;
                    Values[1] = password;
                    string StrUrl = URLDetails.GetSiteRootUrl().TrimEnd('/');
                    UsersBusinessFacade objUsersBusinessFacade = new UsersBusinessFacade();
                    objUserEntity = objUsersBusinessFacade.Authenticate(username, password, LoginMode);

                    if (objUserEntity != null)
                    {
                        if (objUserEntity.StatusId == (byte)StateEnums.Statuses.Active || objUserEntity.StatusId == (byte)StateEnums.Statuses.Pending)
                        {
                            _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, Resource.lbl_msg_dataSavedSuccessfully, KeyEnums.JsonMessageType.SUCCESS, StrUrl, "true", objUserEntity);
                        }
                        else if (objUserEntity.StatusId == (byte)StateEnums.Statuses.InActive)
                        {
                            _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_accountDisabled, KeyEnums.JsonMessageType.FAILURE, "/User/Login");
                        }
                        else if (objUserEntity.StatusId == (byte)StateEnums.Statuses.Deleted)
                        {
                            _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_accountDeleted, KeyEnums.JsonMessageType.FAILURE, "/User/Login");
                        }
                        else if (objUserEntity.StatusId == (byte)StateEnums.Statuses.Active && objUserEntity.IsEmailVerified == true)
                        {
                            _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, Resource.lbl_msg_dataSavedSuccessfully, KeyEnums.JsonMessageType.SUCCESS, StrUrl, "true", objUserEntity);
                        }
                        else
                        {
                            _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_loginFailed, KeyEnums.JsonMessageType.ERROR);
                        }
                    }
                    else
                        _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_invalidEmailAddressPassword, KeyEnums.JsonMessageType.ERROR);
                }
            }
            catch (Exception ex)
            {
                _jsonMessage = new JsonMessage(false, Resource.lbl_msg_internalServerErrorOccurred, Resource.lbl_msg_internalServerErrorOccurred, KeyEnums.JsonMessageType.ERROR, ex.Message);
                Log.WriteLog(_module, "IsLoginValid(username=" + username + ", password=" + password + ")", ex.Source, ex.Message, ex);
            }

            return _jsonMessage;
        }

      

        [HttpPost]
        public JsonResult IsRefreshRequired()
        {
            string IsRefreshRequired = "0";
            JsonMessage _jsonMessage = null;
            Users objUsers = new Users();
            objUsers.ID = Convert.ToInt32(IsRefreshRequired);
            _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, "", KeyEnums.JsonMessageType.SUCCESS, objUsers);
            return Json(_jsonMessage);
        }

        [HttpPost]
        public JsonResult GetAvailableAgent()
        {

            JsonMessage _jsonMessage = null;
            UsersBusinessFacade objBF = new UsersBusinessFacade();
            Users objUsers = new Users();
            Int64 UserID = 0;
            try
            {
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));

                objUsers = objBF.GetAvailableAgent(UserID);
                _jsonMessage = new JsonMessage(true, Resources.Resource.lbl_Cap_success, "", KeyEnums.JsonMessageType.SUCCESS, objUsers);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetAvailableAgent", ex.Source, ex.Message, ex);
            }
            return Json(_jsonMessage);
        }


        [HttpPost]
        public JsonResult GetKioskDetails(string TravellerCallerID)
        {
            JsonMessage _jsonMessage = null;
            KioskMasterBusinessFacade objBF = new KioskMasterBusinessFacade();
            KioskMaster objKioskMaster = new KioskMaster();
            try
            {
                objKioskMaster = objBF.GetKioskDetails(TravellerCallerID);
                _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, "", KeyEnums.JsonMessageType.SUCCESS, objKioskMaster);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetKioskDetails(TravellerCallerID:" + TravellerCallerID + ")", ex.Source, ex.Message, ex);
            }
            return Json(_jsonMessage);
        }

        [HttpPost]
        public JsonResult ChangeAvailabilityStatus(string AvailabilityStatus, string AgentCallerID = "", string tranID = "")
        {
            Int64 UserID = 0;
            JsonMessage _jsonMessage = null;
            try
            {
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));

                if (AgentCallerID != "")
                {
                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                    _jsonMessage = objUsers.ChangeAvailabilityStatus(UserID, AvailabilityStatus, AgentCallerID);
                }
                else if (UserID > 0)
                {
                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                    _jsonMessage = objUsers.ChangeAvailabilityStatus(UserID, AvailabilityStatus, "");
                }

                if (AvailabilityStatus == "2")
                {
                    //CallTransactionsBusinessFacade objBF = new CallTransactionsBusinessFacade();
                    //objBF.UpdateCallTransactions(CallTransactionsDBFields.CallEndTime + "=getDate()," + CallTransactionsDBFields.UpdatedDate + "=getDate()", CallTransactionsDBFields.ID + "=" + tranID);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "ChangeAvailabilityStatus(UserID:" + UserID + ", strStatus:" + AvailabilityStatus + ",tranID=" + tranID + ")", ex.Source, ex.Message, ex);
            }
            return Json(_jsonMessage);
        }

        [HttpPost]
        public JsonResult MakeAgentActive()
        {
            Int64 UserID = 0;
            JsonMessage _jsonMessage = null;
            try
            {
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));

                if (UserID > 0)
                {
                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                    _jsonMessage = objUsers.ChangeAvailabilityStatus(UserID, "1");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "MakeAgentActive(UserID:" + UserID + ")", ex.Source, ex.Message, ex);
            }
            return Json(_jsonMessage);
        }
        public String GetDecryptedCookieByKey(string key)
        {
            string culture = string.Empty;
            // var cookie = HttpContext.Current.Request.Cookies[key];
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[key];
            if (cookie != null)
                culture = cookie.ToString();
            if (!string.IsNullOrWhiteSpace(culture))
            {
                var encription = new AdaniCall.Utility.Common.Encription();
                culture = encription.Decrypt(culture);
            }
            return culture;
        }
        private void SetEncryptedCookieKey(string key, string value, TimeSpan expires)
        {
            //var httpContext = _httpContextAccessor.HttpContext;
            var encription = new AdaniCall.Utility.Common.Encription();
            value = encription.Encrypt(value);

            if (_httpContextAccessor.HttpContext.Request.Cookies[key] != null)
            {
                CookieOptions cookieOld = new CookieOptions();
                cookieOld.Expires = DateTime.MaxValue;
                //cookieOld.Equals() = value;
                Response.Cookies.Append(key, value, cookieOld); ;
            }
            else
            {
                CookieOptions newCookie = new CookieOptions();
                newCookie.Expires = DateTime.MaxValue;
                Response.Cookies.Append(key, value, newCookie);
            }
        }
    }
}
