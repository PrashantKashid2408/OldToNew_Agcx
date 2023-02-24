using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdaniCall.Entity
{
    public class UserFeedBack
    {
        #region Declarations

        private bool _boolObjectChanged;
        private Int64 _intID;
        private Int64 _intKioskMasterID;
        private string _strPhoneNumber;
        private string _strFirstName;
        private string _strLastName;
        private string _strEmail;
        private string _name;
        private string _airportName;
        private string _subCategoryCode;
        private string _caseTypeCode;
        private string _strRatingText;
        private Int64 _intRatingNumber;
        private string _strFeedbackDesc;
        private string _terminal;
        private string _cRMCaseNumber;
        private string _cRMResponseMessage;
        private byte _bytStatusId;
        private DateTime _datCreatedDate;
        private DateTime _datUpdatedDate;


        #endregion Declarations

        #region Properties

        public bool ObjectChanged
        {
            get { return this._boolObjectChanged; }
            set { this._boolObjectChanged = value; }
        }

        public Int64 ID
        {
            get { return this._intID; }
            set { this._intID = value; }
        }

        public Int64 KioskMasterID
        {
            get { return this._intKioskMasterID; }
            set { this._intKioskMasterID = value; }
        }

        public string PhoneNumber
        {
            get { return this._strPhoneNumber; }
            set { this._strPhoneNumber = value; }
        }

        public string FirstName
        {
            get { return this._strFirstName; }
            set { this._strFirstName = value; }
        }

        public string LastName
        {
            get { return this._strLastName; }
            set { this._strLastName = value; }
        }

        public string Email
        {
            get { return this._strEmail; }
            set { this._strEmail = value; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public string AirportName
        {
            get { return this._airportName; }
            set { this._airportName = value; }
        }
        public string SubCategoryCode
        {
            get { return this._subCategoryCode; }
            set { this._subCategoryCode = value; }
        }
        public string CaseTypeCode
        {
            get { return this._caseTypeCode; }
            set { this._caseTypeCode = value; }
        }
        public string RatingText
        {
            get { return this._strRatingText; }
            set { this._strRatingText = value; }
        }

        public Int64 RatingNumber
        {
            get { return this._intRatingNumber; }
            set { this._intRatingNumber = value; }
        }

        public string FeedbackDesc
        {
            get { return this._strFeedbackDesc; }
            set { this._strFeedbackDesc = value; }
        }

        public byte StatusId
        {
            get { return this._bytStatusId; }
            set { this._bytStatusId = value; }
        }

        public DateTime CreatedDate
        {
            get { return this._datCreatedDate; }
            set { this._datCreatedDate = value; }
        }

        public DateTime UpdatedDate
        {
            get { return this._datUpdatedDate; }
            set { this._datUpdatedDate = value; }
        }

        public string Terminal
        {
            get { return this._terminal; }
            set { this._terminal = value; }
        }
        public string CRMCaseNumber
        {
            get { return this._cRMCaseNumber; }
            set { this._cRMCaseNumber = value; }
        }

        public string CRMResponseMessage
        {
            get { return this._cRMResponseMessage; }
            set { this._cRMResponseMessage = value; }
        }
        #endregion Properties
    }
    public class FeedbackCRMResponse
    {
        public string CaseNumber { get; set; }
        public String Message { get; set; }
    }
}
