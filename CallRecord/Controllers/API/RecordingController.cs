using Azure.Communication.CallingServer;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using AdaniCall.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CallRecord;
using System.Configuration;
using AdaniCall.Utility.Common;
using AdaniCall.Business.BusinessFacade;
using AdaniCall.Business.DataAccess.Constants;
using Microsoft.AspNetCore.Mvc;
using static AdaniCall.Models.CommonData;
using Microsoft.Azure.WebJobs.Logging;
using Azure.Storage.Blobs;
using Azure.Communication.Identity;

namespace AdaniCall.Controllers.API
{
    [Route("api/Recording")]
    public class RecordingController :Controller
    {
        private readonly string blobStorageConnectionString;
        private readonly string callbackUri;
        private readonly string containerName;
        private readonly CallingServerClient callingServerClient;
        private const string CallRecodingActiveErrorCode = "8553";
        private const string CallRecodingActiveError = "Recording is already in progress, one recording can be active at one time.";
        private readonly string _module = "AdaniCall.Controllers.API.RecordingController";
        private string RootPath; 
        static Dictionary<string, string> recordingData = new Dictionary<string, string>();
        public static string recFileFormat;

        public RecordingController(IConfiguration configuration)
        {
            blobStorageConnectionString = configuration["BlobStorageConnectionString"].ToString();
            containerName = configuration["BlobContainerName"].ToString();
            callingServerClient = new CallingServerClient(configuration["ACSResourceConnectionString"].ToString());
            RootPath = configuration["RootPath"].ToString() + configuration["VideoPath"].ToString();
            callbackUri = configuration["CallBackURI"].ToString();
        }

        /// <summary>
        /// Method to start call recording
        /// </summary>
        /// <param name="serverCallId">Conversation id of the call</param>
        [HttpPost]
        [Route("startRecording")]
        public async Task<IActionResult> StartRecordingAsync(RecordParams objRecordParams)
        {
            try
            {
                string serverCallId = objRecordParams.sCallID.ToString();
                string recordingFormat = objRecordParams.recordingFormat.ToString();
                string uniqueCallID = objRecordParams.UniqueCallID.ToString();
                if (!string.IsNullOrEmpty(serverCallId))
                {
                    var uri = new Uri(callbackUri);
                  
                    var startRecordingResponse = await callingServerClient.InitializeServerCall(serverCallId).StartRecordingAsync(uri).ConfigureAwait(false);
                    
                    var recordingId = startRecordingResponse.Value.RecordingId;
                    if (!recordingData.ContainsKey(serverCallId))
                    {
                        recordingData.Add(serverCallId, string.Empty);
                    }
                    recordingData[serverCallId] = recordingId;

                    if (uniqueCallID != "")
                    {
                        CallTransactionsBusinessFacade objBF = new CallTransactionsBusinessFacade();
                        objBF.UpdateCallTransactions(CallTransactionsDBFields.ServerCallID + "='" + serverCallId + "'," + CallTransactionsDBFields.RecordingID + "='" + recordingId + "'", CallTransactionsDBFields.UniqueCallID + "='" + uniqueCallID + "'");
                    }

                    return Json(recordingId);
                }
                else
                {
                    return BadRequest(new { Message = "serverCallId is invalid" });
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "StartRecordingAsync(sCallID=" + objRecordParams.sCallID + ",recordingFormat:" + objRecordParams.recordingFormat + ")", ex.Source, ex.Message);
                if (ex.Message.Contains(CallRecodingActiveErrorCode))
                {
                    Log.WriteLog(_module, "StartRecordingAsync(Message=" + CallRecodingActiveError + ")", ex.Source, ex.Message);                  
                    return BadRequest(new { Message = CallRecodingActiveError });
                }
                return Json(new { Exception = ex });
            }
        }


        public async Task<IActionResult> GetRecordingState(string serverCallId, string recordingId)
        {
            try
            {
                if (!string.IsNullOrEmpty(serverCallId))
                {
                    if (string.IsNullOrEmpty(recordingId))
                    {
                        recordingId = recordingData[serverCallId];
                    }
                    else
                    {
                        if (!recordingData.ContainsKey(serverCallId))
                        {
                            recordingData[serverCallId] = recordingId;
                        }
                    }

                    var recordingState = await callingServerClient.InitializeServerCall(serverCallId).GetRecordingStateAsync(recordingId).ConfigureAwait(false);

                    return Json(recordingState.Value.RecordingState);
                }
                else
                {
                    return Json(recordingId);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Exception = ex });
            }
        }


     
        /// <summary>
        /// Web hook to receive the recording file update status event
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getRecordingFile")]
        public async Task<IActionResult> GetRecordingFile([FromBody] object request)
        {
            try
            {
             
                var httpContent = new BinaryData(request.ToString()).ToStream();
              
                EventGridEvent cloudEvent = EventGridEvent.ParseMany(await BinaryData.FromStreamAsync(httpContent)).FirstOrDefault();
                if (cloudEvent.EventType == SystemEventNames.EventGridSubscriptionValidation)
                {
                    var eventData1 = cloudEvent.Data.ToObjectFromJson<SubscriptionValidationEventData>();
                    var responseData = new SubscriptionValidationResponse
                    {
                        ValidationResponse = eventData1.ValidationCode
                    };

                    if (responseData.ValidationResponse != null)
                    {
                        return Json(responseData);
                    }
                }

                if (cloudEvent.EventType == SystemEventNames.AcsRecordingFileStatusUpdated)
                {
                    var eventData = cloudEvent.Data.ToObjectFromJson<AcsRecordingFileStatusUpdatedEventData>();
                    await ProcessFile(eventData.RecordingStorageInfo.RecordingChunks[0].MetadataLocation,
                        eventData.RecordingStorageInfo.RecordingChunks[0].DocumentId,
                        FileFormat.Json,
                        FileDownloadType.Metadata);

                    await ProcessFile(eventData.RecordingStorageInfo.RecordingChunks[0].ContentLocation,
                        eventData.RecordingStorageInfo.RecordingChunks[0].DocumentId,
                        string.IsNullOrWhiteSpace(recFileFormat) ? FileFormat.Mp4 : recFileFormat,
                        FileDownloadType.Recording);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetRecordingFile()", "Source " + ex.Source + ",Message:" + ex.Message, ex.StackTrace);
                return Json(new { Exception = ex });
            }
        }

        private async Task<bool> ProcessFile(string downloadLocation, string documentId, string fileFormat, string downloadType)
        {
            string filePath = "";
            try
            {
                var recordingDownloadUri = new Uri(downloadLocation);
                var response = callingServerClient.DownloadStreamingAsync(recordingDownloadUri);

                filePath = RootPath + @"\" + documentId + "." + fileFormat;
                //string filePath = "~/AllFiles/Video1" + @"\" + documentId + "." + fileFormat;
                using (Stream streamToReadFrom = response.Result.Value)
                {
                    using (Stream streamToWriteTo = System.IO.File.Open(filePath, FileMode.Create))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        await streamToWriteTo.FlushAsync();
                    }
                }

                string recordedfileName = "";
                string uniqueCallID = "";
                if (string.Equals(downloadType, FileDownloadType.Metadata, StringComparison.InvariantCultureIgnoreCase) && System.IO.File.Exists(filePath))
                {
                    Root deserializedFilePath = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(filePath));
                    recFileFormat = deserializedFilePath.recordingInfo.format;
                    recordedfileName = deserializedFilePath.chunkDocumentId;
                    uniqueCallID = deserializedFilePath.callId;

                   }

               var blobStorageHelperInfo = await BlobStorageHelper.UploadFileAsync(blobStorageConnectionString, containerName, filePath, filePath);
                if (blobStorageHelperInfo.Status)
                {
                 
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    //Logger.LogError($"{downloadType} file was not uploaded,{blobStorageHelperInfo.Message}");
                }

                CallTransactionsBusinessFacade objBF = new CallTransactionsBusinessFacade();
                if (uniqueCallID != "")
                {
                  if (recFileFormat.ToLower() == "mp4" && recordedfileName != "")
                    {
                        objBF.UpdateCallTransactions(CallTransactionsDBFields.VideoFileName + "='" + recordedfileName + "'," + CallTransactionsDBFields.UpdatedDate + "=getDate()", CallTransactionsDBFields.UniqueCallID + "='" + uniqueCallID + "'");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "ProcessFile(blobStorageConnectionString:" + blobStorageConnectionString + ",containerName:" + containerName + ",filePath:" + filePath + ")", ex.Source, ex.Message);
                return false;
            }
        }
    }
}




































