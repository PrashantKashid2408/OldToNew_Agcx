using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaniCall.Business.DataAccess.Constants
{
    public class LocationSettingDBFields
    {

	
        public static string IU_Flag = "IU_Flag"; 


		

        public static string TableNameVal = "LocationSetting";
        public static string  ID = "ID";
        public static string  IconMasterID = "IconMasterID";
        public static string  KioskID = "KioskID";
        public static string  StatusId = "StatusId";
        public static string  CreatedDate = "CreatedDate";
        public static string  UpdatedDate = "UpdatedDate";
        public static string RowNumber = "RowNumber";


    }
    public class LocationSettingStoredProcedures
    {

        #region Object StoredProcedure

	
        public static string LocationSettingSAVE = "LocationSetting_SAVE";
        public static string LocationSettingGetRecordById = "LocationSetting_GetRecordById";
        public static string LocationSetting_GetRecordsByID = "LocationSetting_GetRecordsByID"; 
        public static string GetLocationSettingRecords = "LocationSetting_GetRecords";
        public static string GetLocationSettingRecordsAdmin = "LocationSetting_GetRecords_Admin";
        public static string GetLocationSettingRecordsKiosk = "LocationSetting_GetRecords_Kiosk";
        public static string GetLocationSettingRecordByValue =  "LocationSetting_GetRecordByValue";
        public static string GetLocationSettingRecordByValueArray = "LocationSetting_GetRecordByValueArray";
        public static string LocationSettingDescAdminGetRecords = "LocationSettingDescAdmin_GetRecords";
        public static string LocationSettingDescKioskGetRecords = "LocationSettingDescKiosk_GetRecords";
        public static string LocationSettingIsIconImageExists = "LocationSetting_IsIconImageExists"; //created date 3/feb


        #endregion

        #region Collection StoredProcedure

        public static string LocationSettingSearch = "LocationSetting_Search";
        public static string LocationSettingSearchByValue =  "LocationSetting_SearchByValue";
        public static string LocationSettingSearchByValueArray = "LocationSetting_SearchByValueArray";
        #endregion
 
        public static string IsExist = "";
        public static string GetCollectionForQuery = "";
        public static string SortingString = "SortOrder";


      
    }
}
