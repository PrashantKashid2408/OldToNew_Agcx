using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaniCall.Business.DataAccess.Constants;
using System.Data.SqlClient;
using System.Data;
using AdaniCall.Entity;
using AdaniCall.Entity.Common;
using AdaniCall.Utility.Common;

namespace AdaniCall.Business.DataAccess.Mapper
{
    public class LocationSettingDataMapper
    {
        private static readonly string _module = "AdaniCall.Business.DataAccess.Mapper.LocationSettingDataMapper";
        private LocationSetting objLocationSetting = null;

        public LocationSetting GetDetails(SqlDataReader sqlDataReader)
        {
            try
            {
                objLocationSetting = new LocationSetting();
               
			    if (sqlDataReader.HasColumn(LocationSettingDBFields.ID))
                   objLocationSetting.ID = (sqlDataReader[LocationSettingDBFields.ID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDBFields.ID]) : 0); 
                if (sqlDataReader.HasColumn(LocationSettingDBFields.IconMasterID))
                   objLocationSetting.IconMasterID = (sqlDataReader[LocationSettingDBFields.IconMasterID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDBFields.IconMasterID]) : 0); 
                if (sqlDataReader.HasColumn(LocationSettingDBFields.KioskID))
                   objLocationSetting.KioskID = (sqlDataReader[LocationSettingDBFields.KioskID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDBFields.KioskID]) : 0); 
                if (sqlDataReader.HasColumn(LocationSettingDBFields.StatusId))
                   objLocationSetting.StatusId = (sqlDataReader[LocationSettingDBFields.StatusId] != DBNull.Value ? Convert.ToByte(sqlDataReader[LocationSettingDBFields.StatusId]) : (byte)0); 
                if (sqlDataReader.HasColumn(LocationSettingDBFields.CreatedDate))
                   objLocationSetting.CreatedDate = (sqlDataReader[LocationSettingDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[LocationSettingDBFields.CreatedDate]) : DateTime.Now); 
                if (sqlDataReader.HasColumn(LocationSettingDBFields.UpdatedDate))
                   objLocationSetting.UpdatedDate = (sqlDataReader[LocationSettingDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[LocationSettingDBFields.UpdatedDate]) : DateTime.Now);
                if (sqlDataReader.HasColumn(IconMasterDBFields.IconName))
                    objLocationSetting.IconName = (sqlDataReader[IconMasterDBFields.IconName] != DBNull.Value ? Convert.ToString(sqlDataReader[IconMasterDBFields.IconName]) : string.Empty);
                if (sqlDataReader.HasColumn(IconMasterDBFields.IconImage))
                    objLocationSetting.IconImage = (sqlDataReader[IconMasterDBFields.IconImage] != DBNull.Value ? Convert.ToString(sqlDataReader[IconMasterDBFields.IconImage]) : string.Empty);
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.LocationDescription))
                    objLocationSetting.LocationDescription = (sqlDataReader[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(sqlDataReader[LocationSettingDescDBFields.LocationDescription]) : string.Empty);
                if (sqlDataReader.HasColumn(LocationSettingDBFields.RowNumber))
                    objLocationSetting.RowNumber = (sqlDataReader[LocationSettingDBFields.RowNumber] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDBFields.RowNumber]) : 0);
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.LocationDescription))
                    objLocationSetting.LocationDescription = (sqlDataReader[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(sqlDataReader[LocationSettingDescDBFields.LocationDescription]) : string.Empty);

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return objLocationSetting;
        }
		
		public List<LocationSetting> GetDetailsList(SqlDataReader sqlDataReader)
        {
            List<LocationSetting> list = new List<LocationSetting>();
            try
            {
                while (sqlDataReader.Read())
                {
                    objLocationSetting = GetDetails(sqlDataReader);
                    list.Add(objLocationSetting);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetailsList(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return list;
        }

        public List<LocationSetting> GetDetails(DataSet dataSet)
        {
            List<LocationSetting> LocationSettings = new List<LocationSetting>();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objLocationSetting = new LocationSetting();
                       
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.ID)) 
                        objLocationSetting.ID = (drow[LocationSettingDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.ID]) : 0); 
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.IconMasterID)) 
                        objLocationSetting.IconMasterID = (drow[LocationSettingDBFields.IconMasterID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.IconMasterID]) : 0); 
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.KioskID)) 
                        objLocationSetting.KioskID = (drow[LocationSettingDBFields.KioskID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.KioskID]) : 0); 
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.StatusId)) 
                        objLocationSetting.StatusId = (drow[LocationSettingDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDBFields.StatusId]) : (byte)0); 
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.CreatedDate)) 
                        objLocationSetting.CreatedDate = (drow[LocationSettingDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.CreatedDate]) : DateTime.Now); 
                        if (drow.Table.Columns.Contains(LocationSettingDBFields.UpdatedDate)) 
                        objLocationSetting.UpdatedDate = (drow[LocationSettingDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.UpdatedDate]) : DateTime.Now); 

                        


                        LocationSettings.Add(objLocationSetting);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return LocationSettings;
        }
		
		public LocationSetting GetDetailsobj(DataSet dataSet)
        {
            LocationSetting objLocationSetting = new LocationSetting();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objLocationSetting = new LocationSetting();
                      
						if (drow.Table.Columns.Contains(LocationSettingDBFields.ID)) 
  objLocationSetting.ID = (drow[LocationSettingDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.IconMasterID)) 
  objLocationSetting.IconMasterID = (drow[LocationSettingDBFields.IconMasterID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.IconMasterID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.KioskID)) 
  objLocationSetting.KioskID = (drow[LocationSettingDBFields.KioskID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.KioskID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.StatusId)) 
  objLocationSetting.StatusId = (drow[LocationSettingDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.CreatedDate)) 
  objLocationSetting.CreatedDate = (drow[LocationSettingDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.UpdatedDate)) 
  objLocationSetting.UpdatedDate = (drow[LocationSettingDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return objLocationSetting;
        }
		
		public LocationSetting GetDetails(DataTable dataTable)
        {
            LocationSetting objLocationSetting = new LocationSetting();

            try
            {
                if (dataTable != null &&  dataTable.Rows.Count > 0)
                {
                    foreach (DataRow drow in dataTable.Rows)
                    {
                        objLocationSetting = new LocationSetting();
                      
						if (drow.Table.Columns.Contains(LocationSettingDBFields.ID)) 
  objLocationSetting.ID = (drow[LocationSettingDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.IconMasterID)) 
  objLocationSetting.IconMasterID = (drow[LocationSettingDBFields.IconMasterID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.IconMasterID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.KioskID)) 
  objLocationSetting.KioskID = (drow[LocationSettingDBFields.KioskID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDBFields.KioskID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.StatusId)) 
  objLocationSetting.StatusId = (drow[LocationSettingDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.CreatedDate)) 
  objLocationSetting.CreatedDate = (drow[LocationSettingDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(LocationSettingDBFields.UpdatedDate)) 
  objLocationSetting.UpdatedDate = (drow[LocationSettingDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(DataTable)", ex.Source, ex.Message, ex);
            }

            return objLocationSetting;
        }

    }
}
