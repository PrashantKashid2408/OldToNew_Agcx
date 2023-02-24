using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaniCall.Business.DataAccess.Constants
{
    public class UserFeedBackDBFields
    {


        public static string IU_Flag = "IU_Flag";




        public static string TableNameVal = "UserFeedBack";
        public static string ID = "ID";
        public static string KioskMasterID = "KioskMasterID";
        public static string PhoneNumber = "PhoneNumber";
        public static string FirstName = "FirstName";
        public static string LastName = "LastName";
        public static string Email = "Email";
        public static string Name = "Name";
        public static string AirportName = "AirportName";
        public static string SubCategoryCode = "SubCategoryCode";
        public static string CaseTypeCode = "CaseTypeCode";
        public static string RatingText = "RatingText";
        public static string RatingNumber = "RatingNumber";
        public static string FeedbackDesc = "FeedbackDesc";
        public static string Terminal = "Terminal";
        public static string CRMCaseNumber = "CRMCaseNumber";
        public static string CRMResponseMessage = "CRMResponseMessage";
        public static string StatusId = "StatusId";
        public static string CreatedDate = "CreatedDate";
        public static string UpdatedDate = "UpdatedDate";



    }
    public class UserFeedBackStoredProcedures
    {

        #region Object StoredProcedure






        public static string UserFeedBackSAVE = "UserFeedBack_SAVE";
        public static string UserFeedBackGetRecordById = "UserFeedBack_GetRecordById";

        public static string GetUserFeedBackRecords = "UserFeedBack_GetRecords";
        public static string GetUserFeedBackRecordByValue = "UserFeedBack_GetRecordByValue";
        public static string GetUserFeedBackRecordByValueArray = "UserFeedBack_GetRecordByValueArray";

        #endregion

        #region Collection StoredProcedure

        public static string UserFeedBackSearch = "UserFeedBack_Search";
        public static string UserFeedBackSearchByValue = "UserFeedBack_SearchByValue";
        public static string UserFeedBackSearchByValueArray = "UserFeedBack_SearchByValueArray";
        #endregion

        public static string IsExist = "";
        public static string GetCollectionForQuery = "";
        public static string SortingString = "SortOrder";



    }
}
