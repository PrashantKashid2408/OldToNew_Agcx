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
using AdaniCall.Entity.Enums;
using AdaniCall.Resources;

namespace AdaniCall.Business.BusinessFacade
{
    public class LocationSettingBusinessFacade//:UniversalObject
    {
        dynamic objdynamicWrapper; 
		LocationSettingWrapperColletion objLocationSettingWrapperColletion = new LocationSettingWrapperColletion();
        LocationSettingWrapper objLocationSettingWrapper = new LocationSettingWrapper();
        LocationSetting objLocationSettingEntity = new LocationSetting();
        JsonMessage _jsonMessage = null;

        private static readonly string _module = "AdaniCall.Business.BusinessFacade.LocationSettingBusinessFacade";
		public LocationSettingBusinessFacade()
        {
            
        }
        public LocationSettingBusinessFacade(dynamic WrapperType)
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
        public Int64 Save(dynamic objEntity)
        {
            try
            {

                objLocationSettingWrapper.objWrapperClass = objEntity;
                DataAccess.DataAccessLayer.DataAccess dalObject = new DataAccess.DataAccessLayer.DataAccess();
                Transaction TransObj = new Transaction(dalObject);
                TransObj.ConnectionString = dbClass.SqlConnectString();
                Dictionary<string, Command> CommandsObj = new Dictionary<string, Command>();
                int commandCounter = 0;

                bool result = objLocationSettingWrapper.Save(ref CommandsObj, ref commandCounter);
                TransObj.AddCommandList(CommandsObj);
                if (TransObj.ExecuteTransaction())
                {
					long ID = 0;
                    if (long.TryParse(TransObj.ReturnID, out ID) && ID > 0)
                    {
                        objEntity.ID = ID;
                    }
                    return ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally { }

        }

     
 
        public List<LocationSetting> GetListIconsAdminBF(Int64 KioskID)
        {
            List<LocationSetting> _List = new List<LocationSetting>();
            try
            {
                _List = objLocationSettingWrapperColletion.GetListIconsAdmin(KioskID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconsBF(KioskID:" + KioskID + ")", ex.Source, ex.Message, ex);
            }

            return _List;
        }
        public List<LocationSetting> GetListIconsKioskBF(Int64 KioskID)
        {
            List<LocationSetting> _List = new List<LocationSetting>();
            try
            {
                _List = objLocationSettingWrapperColletion.GetListIconsKiosk(KioskID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconsKioskBF(KioskID:" + KioskID + ")", ex.Source, ex.Message, ex);
            }

            return _List;
        }


        public LocationSetting GetLocationDetailsByID(Int64 KioskID)
        {
            LocationSetting objEntity = new LocationSetting();
            try
            {
                objEntity = objLocationSettingWrapperColletion.GetLocationDetailsByID(KioskID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetLocationDetailsByID(KioskID:" + KioskID + ")", ex.Source, ex.Message, ex);
            }

            return objEntity;
        }


        public List<LocationSetting> GetListIconDescAdminBF(Int64 LocationSettingID)
        {
            List<LocationSetting> _List = new List<LocationSetting>();
            try
            {
                _List = objLocationSettingWrapperColletion.GetListIconDescAdmin(LocationSettingID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconDescAdminBF(LocationSettingID:" + LocationSettingID + ")", ex.Source, ex.Message, ex);
            }

            return _List;
        }

        public List<LocationSetting> GetListIconDescKioskBF(Int64 LocationSettingID)
        {
            List<LocationSetting> _List = new List<LocationSetting>();
            try
            {
                _List = objLocationSettingWrapperColletion.GetListIconDescKiosk(LocationSettingID);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconDescKioskBF(LocationSettingID:" + LocationSettingID + ")", ex.Source, ex.Message, ex);
            }

            return _List;
        }

        public JsonMessage IsIconImageExists(string iconname, int kioskid)
        {
            bool returnValue = false;
            try
            {
                if (!string.IsNullOrEmpty(iconname))
                {
                    objLocationSettingWrapper = new LocationSettingWrapper();
                    objLocationSettingEntity = objLocationSettingWrapper.IsIconImageExists(iconname, kioskid);

                    if (objLocationSettingEntity != null && objLocationSettingEntity.ID > 0)
                        returnValue = true;

                    if (returnValue)
                        _jsonMessage = new JsonMessage(false, Resource.lbl_error, "Icon Name In Use", KeyEnums.JsonMessageType.ERROR, objLocationSettingEntity);
                    else
                        _jsonMessage = new JsonMessage(true, Resource.lbl_success, Resource.lbl_success, KeyEnums.JsonMessageType.SUCCESS);
                }
            }
            catch (Exception ex)
            {
                _jsonMessage = new JsonMessage(false, Resource.lbl_error, Resource.lbl_internalServerErrorOccurred, KeyEnums.JsonMessageType.ERROR, "", "Exception", ex.Message);
                Log.WriteLog(_module, "IsIconImageExists(iconname=" + iconname + ")", ex.Source, ex.Message, ex);
            }
            return _jsonMessage;
        }


    }


}
