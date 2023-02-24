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
using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;
using static System.Collections.Specialized.BitVector32;

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

        private readonly string _licenseKey;
        private readonly string _RSAPubKey;
        private readonly string _auth;
        private readonly string _ProductId;

        Users objUserEntity = new Users();
        JsonMessage _jsonMessage;
        LoginVM _loginVM = null;
        Helper _helper;

        public UserController(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _helper = new Helper(_httpContextAccessor);
            _loginVM = _helper.GetSession();
            _licenseKey = configuration["licenseKey"].ToString();
            _RSAPubKey = configuration["RSAPubKey"].ToString();
            _auth = configuration["auth"].ToString();
            _ProductId = configuration["ProductId"].ToString();
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

                if (UserID > 0)
                {
                    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                    objUsers.ChangeAvailabilityStatus(UserID, "2");
                }

                UserLogOut();

                //string StrUrl = string.Empty;
                //StrUrl = URLDetails.GetSiteRootUrl().TrimEnd('/');
                //return Redirect(StrUrl);
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Logout()", ex.Source, ex.Message, ex);
                return RedirectToAction("Login", "User");
            }
        }
        private void UserLogOut()
        {
            try
            {
                ClearCache();//Delete the user details from cache.
                             //Session.Abandon();
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
            // FormsAuthentication.SignOut(); //Delete the authentication ticket and sign out.

            // CookieStore.RemoveCookie("AGCXWebLoginUser");
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

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime > 0)
                option.Expires = DateTime.Now.AddDays(double.Parse(expireTime.ToString()));
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }

        [HttpPost]
        public JsonResult Login(string username, string password, string queryString, bool isRemember, bool autologin)
        {
            try
            {

                //HttpCookie isRemembercookie = new HttpCookie("isRemembercookie");
                //HttpCookie cookie = new HttpCookie("LoginData");
                SetCookie("RememberMe", isRemember.ToString().ToLower(), Convert.ToInt32(AdaniCallConstants.LoginCookie));

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
                            if (isRemember)
                            {
                                //cookie.Values.Add("UserName", username);
                                //cookie.Values.Add("ReturnURL", _jsonMessage.ReturnUrl);
                                //cookie.HttpOnly = true;
                                //int cookieDays = Convert.ToInt32(AdaniCallConstants.LoginCookie);
                                //cookie.Expires = DateTime.Now.AddDays(cookieDays);
                                //Response.Cookies.Add(cookie);
                                SetCookie("UserName", username, Convert.ToInt32(AdaniCallConstants.LoginCookie));
                                SetCookie("ReturnURL", _jsonMessage.ReturnUrl, Convert.ToInt32(AdaniCallConstants.LoginCookie));
                            }
                            objUserEntity = (Users)_jsonMessage.Data;
                            if (isRemember)
                            {
                                SetCookie("UserRoleID", objUserEntity.RoleId.ToString(), Convert.ToInt32(AdaniCallConstants.LoginCookie));
                            }

                            if (objUserEntity != null)
                            {
                                _helper.SetSession(objUserEntity);
                                //if (objUserEntity.ID > 0)
                                //{
                                //    UsersBusinessFacade objUsers = new UsersBusinessFacade();
                                //    objUsers.ChangeAvailabilityStatus(objUserEntity.ID, "1");
                                //    if (objUserEntity.RoleId == (byte)RoleEnums.Role.Kiosk || objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                                //        CookieStore.SetEncryptedCookieKey("AGCXWebLoginUser", username, TimeSpan.MaxValue);
                                //}
                            }

                            _helper.SetUserLanguage(objUserEntity.LanguageId);
                            InsertAccessMember(objUserEntity.ID, "Login", getAccessMember());
                            //CommonData _CommonData = new CommonData();

                            //if (objUserEntity.RoleId == (byte)RoleEnums.Role.Kiosk)
                            //    _jsonMessage.ReturnUrl = URLDetails.GetSiteRootUrl().TrimEnd('/') + "/Home/Call";
                            //else if (objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                            //    _jsonMessage.ReturnUrl = URLDetails.GetSiteRootUrl().TrimEnd('/') + "/Home/Accept";
                            //else 
                            if (objUserEntity.RoleId == (byte)RoleEnums.Role.SuperAdmin || objUserEntity.RoleId == (byte)RoleEnums.Role.Admin || objUserEntity.RoleId == (byte)RoleEnums.Role.LocationAdmin)
                                _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/Transactions/List";
                            else if (objUserEntity.RoleId == (byte)RoleEnums.Role.Agent)
                                _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/Home/Accept";
                            else
                                _jsonMessage.ReturnUrl = AdaniCallConstants.AdaniCallDomain + "/" + AdaniCallConstants.DefaultController + "/" + AdaniCallConstants.DefaultView + "";
                        }
                        else
                        {
                            _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_invalidEmailAddressPassword, KeyEnums.JsonMessageType.ERROR);
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

        public AccessMember getAccessMember()
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
                Log.WriteLog(_module, "getAccessMember()", ex.Source, ex.Message, ex);
            }
            return objAM;
        }

        public decimal InsertAccessMember(Int64 ID, string clickedby, AccessMember objAccessMember)
        {
            try
            {
                AccessMember objAM = new AccessMember();
                objAM.UserID = Convert.ToInt64(ID);
                objAM.Url = objAccessMember.Url;
                objAM.ReferrerURL = objAccessMember.ReferrerURL;
                objAM.port = objAccessMember.port;
                objAM.Host = objAccessMember.Host;
                objAM.REMOTE_ADDR_IP = objAccessMember.REMOTE_ADDR_IP;
                objAM.Useragent = objAccessMember.Useragent;
                objAM.BrowserType = objAccessMember.BrowserType;
                objAM.BrowserVersion = objAccessMember.BrowserType;
                objAM.Platform = objAccessMember.Platform;
                if (!string.IsNullOrWhiteSpace(objAccessMember.ClickedBy))
                    objAM.ClickedBy = objAccessMember.ClickedBy;
                else
                    objAM.ClickedBy = clickedby;
                objAM.DeviceName = objAccessMember.DeviceName;
                objAM.DeviceType = objAccessMember.DeviceType;
                objAM.OperatingSystem = objAccessMember.OperatingSystem;
                objAM.DeviceModel = objAccessMember.DeviceModel;
                objAM.Build = objAccessMember.Build;
                objAM.Version = objAccessMember.Version;

                AccessMemberBusinessFacade objAccessMemberBusinessFacade = new AccessMemberBusinessFacade();
                objAccessMemberBusinessFacade.Save(objAM);
                return objAM.ID;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "InsertAccessMember(ID: " + ID + ", clickedby: " + clickedby + ")", ex.Source, ex.Message);
            }
            return 0;
        }
        public JsonMessage LicenseKeys(string licenseKey, string RSAPubKey, string auth)
        {
            //var licenseKey = "DRBYF-PRVMW-UATHM-CUGVH";
            //var RSAPubKey = "<RSAKeyValue><Modulus>wYdMz6oMaEUf/zjTnZvquNdlbKR2fXh/xcsGlqHFwN4YWJrEWPfiThxpBAHRIWdjWFMgN/aKPcvwlO14JIrO093fckLZ3WA84/cfOStnwS8pbZPjkMi+1GpK20R5OwVirtDYwZYShxCD6I7iYtaViBi4BWIaKJqC1FcjS+UnfyDNOAHDDFMnvGjjNZeDV1GCwfsU8PQ3m6ljAOlpYQxU/fMVh51t/o0bm3RiJhCfe7OSdsQ3Y/Pp6aKv24a4gRciINw4HIKTfdUhOhMOK1EAlo8AkymVS71i8JFhzAyRksxq7pXiJmqUcNRQJZd6Wtlg81WaOVp3RIx9u2uVO79jHQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string resultMessage = "";
            bool license = false;
            //var auth = "WyIzOTA0MzE3NiIsIjZlSGxlRDVKd3pNV0N0SnZ0UmEwNTQ5NTNBaHlHRXdFZWJSeEpoVTciXQ==";
            var result = Key.Activate(token: auth, parameters: new ActivateModel()
            {
                Key = licenseKey,
                ProductId = 18775,
                Sign = true,
                MachineCode = Helpers.GetMachineCodePI(v: 2)
            });

            if (result == null || result.Result == ResultType.Error ||
                !result.LicenseKey.HasValidSignature(RSAPubKey).IsValid())
            {
                resultMessage = "The license does not work.";
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, resultMessage, KeyEnums.JsonMessageType.FAILURE, "/User/Login");
                license = false;
            }
            else
            {
                if (Helpers.IsOnRightMachinePI(result.LicenseKey, v: 2))
                {
                    resultMessage = "On the Correct Machine";
                    _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, resultMessage, KeyEnums.JsonMessageType.SUCCESS, "true");
                    license = true;
                }
                else
                {
                    resultMessage = "Not on the Correct Machine";
                    _jsonMessage = new JsonMessage(false, Resource.lbl_error, resultMessage, KeyEnums.JsonMessageType.FAILURE, "/User/Login");
                    license = false;
                }
            }
            return _jsonMessage;
        }
        public JsonMessage IsLoginValid(string username, string password, string LoginMode = "")
        {
            Users objUserEntity = new Users();
            // string resultMessage = "";
            // bool license = false;
            _jsonMessage = LicenseKeys(_licenseKey, _RSAPubKey, _auth);
            if (_jsonMessage.IsSuccess)
            {
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
        //private void SetEncryptedCookieKey(string key, string value, TimeSpan expires)
        //{
        //    //var httpContext = _httpContextAccessor.HttpContext;
        //    var encription = new AdaniCall.Utility.Common.Encription();
        //    value = encription.Encrypt(value);

        //    if (_httpContextAccessor.HttpContext.Request.Cookies[key] != null)
        //    {
        //        CookieOptions cookieOld = new CookieOptions();
        //        cookieOld.Expires = DateTime.MaxValue;
        //        //cookieOld.Equals() = value;
        //        Response.Cookies.Append(key, value, cookieOld); ;
        //    }
        //    else
        //    {
        //        CookieOptions newCookie = new CookieOptions();
        //        newCookie.Expires = DateTime.MaxValue;
        //        Response.Cookies.Append(key, value, newCookie);
        //    }
        //}
    }
}
