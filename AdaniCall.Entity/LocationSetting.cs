using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  AdaniCall.Entity
{
    public class LocationSetting
    {
        #region Declarations

        private bool _boolObjectChanged;
        private Int64 _intID;
        private Int64 _intIconMasterID;
        private Int64 _intKioskID;
        private byte _bytStatusId;
        private DateTime _datCreatedDate;
        private DateTime _datUpdatedDate;
        private string _locationDescription;


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

         public Int64 IconMasterID
         { 
            get { return this._intIconMasterID; } 
            set { this._intIconMasterID = value; }
         } 

         public Int64 KioskID
         { 
            get { return this._intKioskID; } 
            set { this._intKioskID = value; }
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

        public Int64 RowNumber { get; set; }
        public string LocationDescription
        {
            get { return this._locationDescription; }
            set { this._locationDescription = value; }
        }
        public string IconImage { get; set; }
        public string IconName { get; set; }
        public Int64 LocationSettingID { get; set; }
        public List<LocationSettingDesc> lstLocationSettingDesc { get; set; }
        #endregion Properties
    }
}
