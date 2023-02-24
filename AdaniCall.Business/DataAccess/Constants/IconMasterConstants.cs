using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaniCall.Business.DataAccess.Constants
{
    public class IconMasterDBFields
    {

	
        public static string IU_Flag = "IU_Flag"; 


		

        public static string TableNameVal = "IconMaster";
public static string  ID = "ID";
public static string  IconName = "IconName";
public static string  IconImage = "IconImage";
public static string  StatusId = "StatusId";
public static string  CreatedDate = "CreatedDate";
public static string  UpdatedDate = "UpdatedDate";

      
    }
    public class IconMasterStoredProcedures
    {

        #region Object StoredProcedure

		



		
        public static string IconMasterSAVE = "IconMaster_SAVE";
        public static string IconMasterGetRecordById = "IconMaster_GetRecordById";

        public static string GetIconMasterRecords = "IconMaster_GetRecords";
        public static string GetIconMasterRecordByValue =  "IconMaster_GetRecordByValue";
        public static string GetIconMasterRecordByValueArray = "IconMaster_GetRecordByValueArray";
         
        #endregion

        #region Collection StoredProcedure
		 
        public static string IconMasterSearch = "IconMaster_Search";
        public static string IconMasterSearchByValue =  "IconMaster_SearchByValue";
        public static string IconMasterSearchByValueArray = "IconMaster_SearchByValueArray";
        #endregion
 
        public static string IsExist = "";
        public static string GetCollectionForQuery = "";
        public static string SortingString = "SortOrder";


      
    }
}
