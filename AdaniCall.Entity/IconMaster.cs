using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  AdaniCall.Entity
{
    public class IconMaster
    {
        #region Declarations

         private bool _boolObjectChanged;
private Int64 _intID;
private string _strIconName;
private string _strIconImage;
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

 public string IconName
 { 
    get { return this._strIconName; } 
    set { this._strIconName = value; }
 } 

 public string IconImage
 { 
    get { return this._strIconImage; } 
    set { this._strIconImage = value; }
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



        #endregion Properties
    }
}
