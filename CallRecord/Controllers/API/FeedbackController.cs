using AdaniCall.Business.BusinessFacade;
using AdaniCall.Entity;
using AdaniCall.Entity.Common;
using AdaniCall.Entity.Enums;
using AdaniCall.Resources;
using AdaniCall.Utility.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CallRecord;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CallRecord.Controllers.API
{
    [Route("api/Feedback")]
    public class FeedbackController : Controller
    {
        private readonly string _module = "AdaniCall.Controllers.FeedbackController";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        JsonMessage _jsonMessage = null;
        public FeedbackController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("SaveFeedback")]
        public JsonResult SaveFeedback(IFormCollection objForm)
        {
            UserFeedBack objActivity = new UserFeedBack();
            try
            {
                Int64 KioskUserId = _session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null ? Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()).ToString()) : 0;
                objActivity.KioskMasterID = KioskUserId;
                objActivity.PhoneNumber = !string.IsNullOrWhiteSpace(objForm["mobileNum"]) ? Convert.ToString(objForm["mobileNum"].ToString().Trim()) : "";
                objActivity.RatingText = !string.IsNullOrWhiteSpace(objForm["feedbackVal"]) ? Convert.ToString(objForm["feedbackVal"].ToString().Trim()) : "";
                objActivity.RatingNumber = 0;
                if (objActivity.RatingText == "Excellent")
                    objActivity.RatingNumber = 5;
                if (objActivity.RatingText == "Good")
                    objActivity.RatingNumber = 4;
                if (objActivity.RatingText == "Moderate")
                    objActivity.RatingNumber = 3;
                if (objActivity.RatingText == "Poor")
                    objActivity.RatingNumber = 2;
                if (objActivity.RatingText == "Very Bad")
                    objActivity.RatingNumber = 1;
                objActivity.Terminal = _session.GetString(KeyEnums.SessionKeys.LevelTwoName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.LevelTwoName.ToString()).ToString() : "";
                var revsponse = JsonConvert.SerializeObject(new { objForm });
                Log.WriteInfoLogWithoutMail(_module, "SaveFeedback(SaveFeedback= " + objActivity + ")", revsponse, "Request Recived");
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "SaveFeedback(SaveFeedback= " + objActivity + ")", ex.Source, ex.Message);
            }
            try
            {
                if (objActivity != null)
                {
                    Int64 KioskUserId = _session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null ? Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()).ToString()) : 0;
                    string AirportShortName = _session.GetString(KeyEnums.SessionKeys.AirportShortName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.AirportShortName.ToString()).ToString() : "";
                    objActivity.AirportName = AirportShortName;
                    string DefaultName = _session.GetString(KeyEnums.SessionKeys.DefaultName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultName.ToString()).ToString() : "";
                    objActivity.Name = DefaultName;
                    string DefaultPhone = _session.GetString(KeyEnums.SessionKeys.DefaultPhone.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultPhone.ToString()).ToString() : "";
                    if (!string.IsNullOrWhiteSpace(objActivity.PhoneNumber))
                        DefaultPhone = objActivity.PhoneNumber;
                    string DefaultEmail = _session.GetString(KeyEnums.SessionKeys.DefaultEmail.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultEmail.ToString()).ToString() : "";
                    string Subcategorycode = AdaniCallConstants.SubcategoryCode.ToString();
                    objActivity.SubCategoryCode = Subcategorycode;
                    string Casetypecode = AdaniCallConstants.CaseTypeCode.ToString();
                    objActivity.CaseTypeCode = Casetypecode;
                    string TerminalName = objActivity.Terminal;
                    string Adl_description = AirportShortName + "_" + TerminalName + "_" + KioskUserId + "_" + DefaultName + "/" + objActivity.RatingText;
                    string Csatscore = objActivity.RatingNumber.ToString();
                    string StrRequest = "{\"Name\":\"" + DefaultName + "\", \"adl_mobilenumber\": \"" + DefaultPhone + "\", \"adl_emailaddress\": \"" + DefaultEmail + "\",\"adl_airportname\": \"" + AirportShortName + "\",\"subcategorycode\":\"" + Subcategorycode + "\",\"casetypecode\": " + Convert.ToInt32(Casetypecode) + ", \"adl_description\": \"" + Adl_description + "\", \"terminal\": \"" + TerminalName + "\", \"csatscore\": " + Convert.ToInt32(Csatscore) + "}";
                    bool IsSuccess = false;
                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(AdaniCallConstants.CRMBaseAddress);
                        client1.DefaultRequestHeaders.Add("securitytoken", "crminc0710ms");
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var request = new HttpRequestMessage(HttpMethod.Post, AdaniCallConstants.CRMBaseAddress);
                        request.Content = new StringContent(StrRequest, Encoding.UTF8, "application/json");
                        var r = client1.SendAsync(request).Result;
                        var returnValue = r.Content.ReadAsStringAsync();
                        var apiResponse = returnValue.Result;
                        apiResponse = apiResponse.Replace("Case Number", "CaseNumber");
                        FeedbackCRMResponse feedbackCRMResponse = JsonConvert.DeserializeObject<FeedbackCRMResponse>(apiResponse);
                        objActivity.CRMCaseNumber = feedbackCRMResponse.CaseNumber;
                        objActivity.CRMResponseMessage = feedbackCRMResponse.Message;
                        objActivity.FeedbackDesc = Adl_description;
                        objActivity.FirstName = _session.GetString(KeyEnums.SessionKeys.FirstName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.FirstName.ToString()).ToString() : "";
                        objActivity.LastName = _session.GetString(KeyEnums.SessionKeys.LastName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.LastName.ToString()).ToString() : "";

                        Log.WriteInfoLogWithoutMail(_module, "PutUserInteraction(,r.IsSuccessStatusCode= " + r.IsSuccessStatusCode + ")", "", "Response Received:" + r.Content.ReadAsStringAsync().Result.ToString());
                        if (r.IsSuccessStatusCode)
                        {
                            if (returnValue.Result.ToString().ToLower().Contains("case created successfully"))
                                IsSuccess = r.IsSuccessStatusCode;
                            else
                                IsSuccess = false;
                        }
                    }

                    if (IsSuccess)
                        _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, Resource.lbl_msg_dataSavedSuccessfully, KeyEnums.JsonMessageType.SUCCESS, "", "", objActivity);
                    else
                        _jsonMessage = new JsonMessage(false, Resource.lbl_error, "Something went wrong! Please try after some time.", KeyEnums.JsonMessageType.ERROR, "", "", objActivity);
                    UserFeedBackBusinessFacade objBF = new UserFeedBackBusinessFacade();
                    objBF.Save(objActivity);
                }
            }
            catch (Exception ex)
            {
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_internalServerErrorOccurred, KeyEnums.JsonMessageType.DANGER, "", Resource.lbl_exception, ex.Message);
                Log.WriteLog(_module, "SelfActivityHistory_Save(SelfActivityHistory= " + objActivity + ")", ex.Source, ex.Message);

            }
            return Json(_jsonMessage);
        }

        [HttpPost]
        [Route("SkipFeedback")]
        public JsonResult SkipFeedback(IFormCollection objForm)
        {
            UserFeedBack objActivity = new UserFeedBack();
            try
            {
                Int64 KioskUserId = _session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null ? Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()).ToString()) : 0;
                objActivity.KioskMasterID = KioskUserId;
                objActivity.PhoneNumber = "-1";
                objActivity.RatingText = !string.IsNullOrWhiteSpace(objForm["feedbackVal"]) ? Convert.ToString(objForm["feedbackVal"].ToString().Trim()) : "";
                if (objActivity.RatingText == "Excellent")
                    objActivity.RatingNumber = 5;
                if (objActivity.RatingText == "Good")
                    objActivity.RatingNumber = 4;
                if (objActivity.RatingText == "Moderate")
                    objActivity.RatingNumber = 3;
                if (objActivity.RatingText == "Poor")
                    objActivity.RatingNumber = 2;
                if (objActivity.RatingText == "Very Bad")
                    objActivity.RatingNumber = 1;
                var revsponse = JsonConvert.SerializeObject(new { objForm });                                                                                                                                           // var revsponse = JsonConvert.SerializeObject(new { objActivity });
                Log.WriteInfoLogWithoutMail(_module, "SkipFeedback(SkipFeedback= " + objActivity + ")", revsponse, "Request Recived");

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "SkipFeedback(SkipFeedback= " + objActivity + ")", ex.Source, ex.Message);
            }
            try
            {
                if (objActivity != null)
                {
                    Int64 KioskUserId = _session.GetString(KeyEnums.SessionKeys.UserId.ToString()) != null ? Convert.ToInt64(_session.GetString(KeyEnums.SessionKeys.UserId.ToString()).ToString()) : 0;
                    string AirportShortName = _session.GetString(KeyEnums.SessionKeys.AirportShortName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.AirportShortName.ToString()).ToString() : "";
                    objActivity.AirportName = AirportShortName;
                    string DefaultName = _session.GetString(KeyEnums.SessionKeys.DefaultName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultName.ToString()).ToString() : "";
                    objActivity.Name = DefaultName;
                    string DefaultPhone = _session.GetString(KeyEnums.SessionKeys.DefaultPhone.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultPhone.ToString()).ToString() : "";
                    string DefaultEmail = _session.GetString(KeyEnums.SessionKeys.DefaultEmail.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.DefaultEmail.ToString()).ToString() : "";
                    string Subcategorycode = AdaniCallConstants.SubcategoryCode.ToString();
                    objActivity.SubCategoryCode = Subcategorycode;
                    string Casetypecode = AdaniCallConstants.CaseTypeCode.ToString();
                    objActivity.CaseTypeCode = Casetypecode;
                    string TerminalName = objActivity.Terminal;
                    string Adl_description = AirportShortName + "_" + TerminalName + "_" + KioskUserId + "_" + DefaultName + "/" + objActivity.RatingText;
                    string Csatscore = objActivity.RatingNumber.ToString();
                    string StrRequest = "{\"Name\":\"" + DefaultName + "\", \"adl_mobilenumber\": \"" + DefaultPhone + "\", \"adl_emailaddress\": \"" + DefaultEmail + "\",\"adl_airportname\": \"" + AirportShortName + "\",\"subcategorycode\":\"" + Subcategorycode + "\",\"casetypecode\": " + Convert.ToInt32(Casetypecode) + ", \"adl_description\": \"" + Adl_description + "\", \"terminal\": \"" + TerminalName + "\", \"csatscore\": " + Convert.ToInt32(Csatscore) + "}";
                    bool IsSuccess = false;
                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(AdaniCallConstants.CRMBaseAddress);
                        client1.DefaultRequestHeaders.Add("securitytoken", "crminc0710ms");
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var request = new HttpRequestMessage(HttpMethod.Post, AdaniCallConstants.CRMBaseAddress);
                        request.Content = new StringContent(StrRequest, Encoding.UTF8, "application/json");
                        var r = client1.SendAsync(request).Result;
                        var returnValue = r.Content.ReadAsStringAsync();
                        var apiResponse = returnValue.Result;
                        apiResponse = apiResponse.Replace("Case Number", "CaseNumber");
                        FeedbackCRMResponse feedbackCRMResponse = JsonConvert.DeserializeObject<FeedbackCRMResponse>(apiResponse);
                        objActivity.CRMCaseNumber = feedbackCRMResponse.CaseNumber;
                        objActivity.FeedbackDesc = Adl_description;
                        objActivity.FirstName = _session.GetString(KeyEnums.SessionKeys.FirstName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.FirstName.ToString()).ToString() : "";
                        objActivity.LastName = _session.GetString(KeyEnums.SessionKeys.LastName.ToString()) != null ? _session.GetString(KeyEnums.SessionKeys.LastName.ToString()).ToString() : "";
                        objActivity.CRMResponseMessage = feedbackCRMResponse.Message;
                        Log.WriteInfoLogWithoutMail(_module, "PutUserInteraction(,r.IsSuccessStatusCode= " + r.IsSuccessStatusCode + ")", "", "Response Received:" + r.Content.ReadAsStringAsync().Result.ToString());
                        if (r.IsSuccessStatusCode)
                        {
                            if (returnValue.Result.ToString().ToLower().Contains("case created successfully"))
                                IsSuccess = r.IsSuccessStatusCode;
                            else
                                IsSuccess = false;
                        }
                    }

                    if (IsSuccess)
                        _jsonMessage = new JsonMessage(true, Resource.lbl_Cap_success, Resource.lbl_msg_dataSavedSuccessfully, KeyEnums.JsonMessageType.SUCCESS, "", "", objActivity);
                    else
                        _jsonMessage = new JsonMessage(false, Resource.lbl_error, "Something went wrong! Please try after some time.", KeyEnums.JsonMessageType.ERROR, "", "", objActivity);
                    UserFeedBackBusinessFacade objBF = new UserFeedBackBusinessFacade();
                    objBF.Save(objActivity);
                }
            }
            catch (Exception ex)
            {
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_msg_internalServerErrorOccurred, KeyEnums.JsonMessageType.DANGER, "", Resource.lbl_exception, ex.Message);
                Log.WriteLog(_module, "SelfActivityHistory_Save(SelfActivityHistory= " + objActivity + ")", ex.Source, ex.Message);

            }
            return Json(_jsonMessage);
        }
    }
}



