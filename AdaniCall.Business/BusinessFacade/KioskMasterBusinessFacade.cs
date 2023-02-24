using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaniCall.Business.DataAccess.Wrapper;
using AdaniCall.Business.DataAccess.Mapper;
using AdaniCall.Business.DataAccess.DataAccessLayer.General;
using AdaniCall.Business.DataAccess.DataAccessLayer;
using AdaniCall.Entity;
using AdaniCall.Business.DataAccess.Constants;
using AdaniCall.Utility;
using AdaniCall.Entity.Common;
using AdaniCall.Utility.Common;
using AdaniCall.Resources;
using AdaniCall.Entity.Enums;

namespace AdaniCall.Business.BusinessFacade
{
    public class KioskMasterBusinessFacade//:UniversalObject
    {
        dynamic objdynamicWrapper; 
		KioskMasterWrapperColletion objKioskMasterWrapperColletion = new KioskMasterWrapperColletion();
        KioskMasterWrapper objKioskMasterWrapper = new KioskMasterWrapper();
		private static readonly string _module = "AdaniCall.Business.BusinessFacade.KioskMasterBusinessFacade";
        JsonMessage _jsonMessage = null;
        public KioskMasterBusinessFacade()
        {
            
        }
        public KioskMasterBusinessFacade(dynamic WrapperType)
        {
            objdynamicWrapper = WrapperType; 
        }
        public dynamic GetRecordsList()
        {
            string[,] Sort = new string[1, 2];
            if (objdynamicWrapper.GetRecords(false, Sort))
            {
                return objdynamicWrapper.Items;
            }
            return null;
        }
        public dynamic GetRecordsListByValue(string Field, String Values)
        {
            string[,] Sort = new string[1, 2];

            if (objdynamicWrapper.GetRecords(false, Sort, true, Field, Values))
            {
                return objdynamicWrapper.Items;
            }
            return null;
        }

        public dynamic GetRecordByValue(string Field, string Values)
        {
            string[,] Sort = new string[1, 2];

            if (objdynamicWrapper.GetRecordByValue(Field, Values))
            {
                return objdynamicWrapper.objUsers;
            }
            return null;
        }

        public dynamic GetRecords(int Id)
        {

            if (objdynamicWrapper.GetRecordById(Id))
            {
                return objdynamicWrapper.objWrapperClass;
            }
            return null;
        } 
        public bool Save(dynamic objEntity)
        {
            try
            {

                objKioskMasterWrapper.objWrapperClass = objEntity;
                DataAccess.DataAccessLayer.DataAccess dalObject = new DataAccess.DataAccessLayer.DataAccess();
                Transaction TransObj = new Transaction(dalObject);
                TransObj.ConnectionString = dbClass.SqlConnectString();
                Dictionary<string, Command> CommandsObj = new Dictionary<string, Command>();
                int commandCounter = 0;

                bool result = objKioskMasterWrapper.Save(ref CommandsObj, ref commandCounter);
                TransObj.AddCommandList(CommandsObj);
                if (TransObj.ExecuteTransaction())
                {
					long ID = 0;
                    if (long.TryParse(TransObj.ReturnID, out ID) && ID > 0)
                    {
                        objEntity.ID = ID;
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { }

        }

        public KioskMaster GetKioskDetails(string TravellerCallerID)
        {
            try
            {
                return objKioskMasterWrapper.GetKioskDetails(TravellerCallerID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetKioskDetails(TravellerCallerID:" + TravellerCallerID + ")", ex.Source, ex.Message, ex);
            }
            return null;
        }

        // PRATIK 19/JAN/2022
        #region LIST FOR KIOSK MASTER CONTROLLER
        public List<KioskMaster> GetListKioskMasterBF(Int64 LoginID, string ListType, string search = "")
        {
            List<KioskMaster> _List = new List<KioskMaster>();
            try
            {
                _List = objKioskMasterWrapperColletion.GetLisKioskMaster(LoginID, ListType, search);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListKioskMasterBF(LoginID:" + LoginID + ",ListType" + ListType + ",search=" + search + ")", ex.Source, ex.Message, ex);
            }

            return _List;       
        }


        #endregion

        // PRATIK 19/JAN/2022
        #region
        public JsonMessage ChangeStatus(Int64 UserID, string StatusID)
        {
            try
            {
                KioskMasterWrapper objWrapper = new KioskMasterWrapper();
                objWrapper.ChangeStatus(UserID, StatusID);
                string strMessage = "";
                if (StatusID == "0")
                    strMessage = Resource.lbl_Disabled;
                else if (StatusID == "1")
                    strMessage = "Enabled";
                else if (StatusID == "2")
                    strMessage = Resource.lbl_accountDeleted;
                _jsonMessage = new JsonMessage(true, Resource.lbl_success, strMessage, KeyEnums.JsonMessageType.SUCCESS, "", "");
            }
            catch (Exception ex)
            {
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_internalServerErrorOccurred, KeyEnums.JsonMessageType.ERROR, "", "0", null);
                Log.WriteLog(_module, "ChangeStatus(ID=" + UserID + ",StatusID=" + StatusID + ")", ex.Source, ex.Message, ex);
            }

            return _jsonMessage;
        }
        #endregion


    }
}
