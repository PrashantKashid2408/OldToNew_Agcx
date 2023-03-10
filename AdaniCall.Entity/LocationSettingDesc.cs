using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  AdaniCall.Entity
{
    public class LocationSettingDesc
    {
        #region Declarations

         private bool _boolObjectChanged;
private Int64 _intID;
private Int64 _intLocationSettingID;
private string _strLocationDescription;
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

         public Int64 LocationSettingID
         { 
            get { return this._intLocationSettingID; } 
            set { this._intLocationSettingID = value; }
         } 

         public string LocationDescription
         { 
            get { return this._strLocationDescription; } 
            set { this._strLocationDescription = value; }
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

        #endregion Properties
    }
}
