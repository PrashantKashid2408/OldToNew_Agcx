using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaniCall.Business.DataAccess.Constants
{
    public class LocationSettingDescDBFields
    {

	
        public static string IU_Flag = "IU_Flag"; 


		

        public static string TableNameVal = "LocationSettingDesc";
public static string  ID = "ID";
public static string  LocationSettingID = "LocationSettingID";
public static string  LocationDescription = "LocationDescription";
public static string  StatusId = "StatusId";
public static string  CreatedDate = "CreatedDate";
public static string  UpdatedDate = "UpdatedDate";

      
    }
    public class LocationSettingDescStoredProcedures
    {

        #region Object StoredProcedure

		



		
        public static string LocationSettingDescSAVE = "LocationSettingDesc_SAVE";
        public static string LocationSettingUPDATE = "LocationSetting_UPDATE"; // created by pratik 
        public static string LocationSettingDescGetRecordById = "LocationSettingDesc_GetRecordById";

        public static string GetLocationSettingDescRecords = "LocationSettingDesc_GetRecords";
        public static string GetLocationSettingDescRecordByValue =  "LocationSettingDesc_GetRecordByValue";
        public static string GetLocationSettingDescRecordByValueArray = "LocationSettingDesc_GetRecordByValueArray";
         
        #endregion

        #region Collection StoredProcedure
		 
        public static string LocationSettingDescSearch = "LocationSettingDesc_Search";
        public static string LocationSettingDescSearchByValue =  "LocationSettingDesc_SearchByValue";
        public static string LocationSettingDescSearchByValueArray = "LocationSettingDesc_SearchByValueArray";
        #endregion
 
        public static string IsExist = "";
        public static string GetCollectionForQuery = "";
        public static string SortingString = "SortOrder";


      
    }
}
