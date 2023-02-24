using AdaniCall.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using Azure.Core;
using Microsoft.AspNetCore.Routing;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using AdaniCall.Entity.Enums;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using AdaniCall.Business.BusinessFacade;
using AdaniCall.Utility.Common;
using AdaniCall.Entity;
using AdaniCall.Business.DataAccess.Constants;
using AdaniCall.Controllers;

namespace AdaniCall.Controllers
{
    public class HomeController : Controller
    {
        TokenHelper objTokenHelper = new TokenHelper();
        private readonly string _module = "AdaniCall.Controllers.UserController";

        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
         private ISession _session => _httpContextAccessor.HttpContext.Session;
         private IMemoryCache _cache;

        
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor , IMemoryCache cache)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }
        public ActionResult Chatbot()
        {
            return View();
        }
        public ActionResult Emergency()
        {
            return View();
        }
        public ActionResult Call()
        {
            string _cacheKey = "CallToken_" + _session.Id;
            _cache.Remove(_cacheKey);
            Int64 UserID = 0;
            string CallerID = "";
            string KioskID = "";
            if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
            {
                UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                if (_session.GetString(KeyEnums.SessionKeys.CallerID.ToString()) != null)
                    CallerID = _session.GetString(KeyEnums.SessionKeys.CallerID.ToString()).ToString();
                if (_session.GetString(KeyEnums.SessionKeys.KioskID.ToString()) != null)
                    KioskID = _session.GetString(KeyEnums.SessionKeys.KioskID.ToString()).ToString();
            }

            try
            {
                ViewBag.Title = "Traveller";

               // CommonData objCD = new CommonData(_cache);
                AccessToken objAT = new AccessToken();

                if (!string.IsNullOrWhiteSpace(CallerID))
                {
                    DateTime objDTNow = DateTime.Now;
                    if (_cache.Get(_cacheKey) != null)
                    {

                        objAT = (AccessToken)_cache.Get(_cacheKey);
                       
                        if (objDTNow > objAT.ExpiresOn.DateTime)
                        {
                            objAT = objTokenHelper.RefreshTokenAsync(CallerID);
                            //  objCD.AddToCache(_cacheKey, objDTNow, objAT.ExpiresOn.DateTime);
                            _cache.Set(_cacheKey, DateTime.Now, TimeSpan.FromDays(1));
                        }
                    }
                    else
                    {
                        objAT = objTokenHelper.RefreshTokenAsync(CallerID);
                        // objCD.AddToCache(_cacheKey, objDTNow, objAT.ExpiresOn.DateTime);
                        _cache.Set(_cacheKey, DateTime.Now, TimeSpan.FromDays(1));
                    }
                }
                else
                    new UserController(_httpContextAccessor,_cache).Logout();

                ViewBag.CallToken = objAT.Token;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Accept(UserId=" + UserID + ")", ex.Source, ex.Message);
            }

            return View();
        }
        //[CustomAuthorizeAttribute(false, Roles = RoleEnums.Agent)]
        public ActionResult Accept()
        {
            string _cacheKey = "AcceptToken_" + HttpContext.Session.Id;
            _cache.Remove(_cacheKey);
            Int64 UserID = 0;
            string CallerID = "";
         
            if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
            {
                UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                if (_session.GetString(KeyEnums.SessionKeys.CallerID.ToString()) != null)
                    CallerID = _session.GetString(KeyEnums.SessionKeys.CallerID.ToString()).ToString();
                UsersBusinessFacade objUsers = new UsersBusinessFacade();
                objUsers.ChangeAvailabilityStatus(UserID, "1");
            }

            try
            {
                ViewBag.Title = "Help Desk";

                //CommonData objCD = new CommonData(_cache);
                AccessToken objAT = new AccessToken();

                if (!string.IsNullOrWhiteSpace(CallerID))
                {
                    DateTime objDTNow = DateTime.Now;
                    if (_cache.Get(_cacheKey)!= null)
                    {
                       objAT = (AccessToken)_cache.Get(_cacheKey);
                      
                        if (objDTNow > objAT.ExpiresOn.DateTime)
                        {
                            objAT = objTokenHelper.RefreshTokenAsync(CallerID);
                           // objCD.AddToCache(_cacheKey, objDTNow, objAT.ExpiresOn.DateTime);
                            _cache.Set(_cacheKey, DateTime.Now, TimeSpan.FromDays(1));
                        }
                    }
                    else
                    {
                        objAT = objTokenHelper.RefreshTokenAsync(CallerID);
                        //objCD.AddToCache(_cacheKey, objDTNow, objAT.ExpiresOn.DateTime);
                        _cache.Set(_cacheKey, DateTime.Now, TimeSpan.FromDays(1));
                    }
                }
                else
                    new UserController(_httpContextAccessor,_cache).Logout();

                ViewBag.AcceptToken = objAT.Token;
                ViewBag.AgentCallerID = CallerID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(_module, "Accept(UserId=" + UserID + ")", ex.Source, ex.Message);
            }

            return View();
        }
        //public IActionResult Index()
        //{
        //    return View();
           
        //}
        [HttpPost]
        public JsonResult MakeCallTransaction(string UniqueCallID, string IncomingCallID)
        {
            Int64 UserID = 0;
            CallTransactions objCT = new CallTransactions();
            try
            {
                CallTransactionsBusinessFacade objBF = new CallTransactionsBusinessFacade();
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                {
                    UserID = Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()));
                    objCT.AgentUserID = UserID;
                    objCT.TravellerCallID = IncomingCallID;
                    objCT.UniqueCallID = UniqueCallID;
                    objBF.Save(objCT);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "MakeCallTransaction(UniqueCallID:" + UniqueCallID + ",IncomingCallID:" + IncomingCallID + ")", ex.Source, ex.Message);
            }
            return Json("");
        }
        [HttpPost]
        public JsonResult UpdateCallTransactionsEndTime(string UniqueCallID, string CallLanguage = "3")
        {
            CallTransactions objCT = new CallTransactions();
            try
            {
                CallTransactionsBusinessFacade objBF = new CallTransactionsBusinessFacade();
                if (_session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null)
                {
                    int callLanguage;
                    int.TryParse(CallLanguage, out callLanguage);
                    objBF.UpdateCallTransactions(CallTransactionsDBFields.CallEndTime + "=CONVERT(VARCHAR, GETDATE(), 120)," + CallTransactionsDBFields.LanguageId + "=" + callLanguage, CallTransactionsDBFields.UniqueCallID + "='" + UniqueCallID + "'");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "UpdateCallTransactionsEndTime(UniqueCallID:" + UniqueCallID + ",CallLanguage:" + CallLanguage + ")", ex.Source, ex.Message);
            }
            return Json("");
        }
        
        public ActionResult GetFeedback()
        { //*****
          //

            return PartialView("_FeedbackPartial");
        }
      

        }
    }
