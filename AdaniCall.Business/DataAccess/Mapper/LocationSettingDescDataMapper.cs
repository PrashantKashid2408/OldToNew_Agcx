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
    public class LocationSettingDescDataMapper
    {
        private static readonly string _module = "AdaniCall.Business.DataAccess.Mapper.LocationSettingDescDataMapper";
        private LocationSettingDesc objLocationSettingDesc = null;

        public LocationSettingDesc GetDetails(SqlDataReader sqlDataReader)
        {
            try
            {
                objLocationSettingDesc = new LocationSettingDesc();
               
			    if (sqlDataReader.HasColumn(LocationSettingDescDBFields.ID))
                   objLocationSettingDesc.ID = (sqlDataReader[LocationSettingDescDBFields.ID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDescDBFields.ID]) : 0); 
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.LocationSettingID))
                   objLocationSettingDesc.LocationSettingID = (sqlDataReader[LocationSettingDescDBFields.LocationSettingID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[LocationSettingDescDBFields.LocationSettingID]) : 0); 
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.LocationDescription))
                   objLocationSettingDesc.LocationDescription = (sqlDataReader[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(sqlDataReader[LocationSettingDescDBFields.LocationDescription]) : string.Empty); 
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.StatusId))
                   objLocationSettingDesc.StatusId = (sqlDataReader[LocationSettingDescDBFields.StatusId] != DBNull.Value ? Convert.ToByte(sqlDataReader[LocationSettingDescDBFields.StatusId]) : (byte)0); 
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.CreatedDate))
                   objLocationSettingDesc.CreatedDate = (sqlDataReader[LocationSettingDescDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[LocationSettingDescDBFields.CreatedDate]) : DateTime.Now); 
                if (sqlDataReader.HasColumn(LocationSettingDescDBFields.UpdatedDate))
                   objLocationSettingDesc.UpdatedDate = (sqlDataReader[LocationSettingDescDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[LocationSettingDescDBFields.UpdatedDate]) : DateTime.Now); 
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return objLocationSettingDesc;
        }
		
		public List<LocationSettingDesc> GetDetailsList(SqlDataReader sqlDataReader)
        {
            List<LocationSettingDesc> list = new List<LocationSettingDesc>();
            try
            {
                while (sqlDataReader.Read())
                {
                    objLocationSettingDesc = GetDetails(sqlDataReader);
                    list.Add(objLocationSettingDesc);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetailsList(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return list;
        }

        public List<LocationSettingDesc> GetDetails(DataSet dataSet)
        {
            List<LocationSettingDesc> LocationSettingDescs = new List<LocationSettingDesc>();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objLocationSettingDesc = new LocationSettingDesc();
                       
						if (drow.Table.Columns.Contains(LocationSettingDescDBFields.ID)) 
  objLocationSettingDesc.ID = (drow[LocationSettingDescDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationSettingID)) 
  objLocationSettingDesc.LocationSettingID = (drow[LocationSettingDescDBFields.LocationSettingID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.LocationSettingID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationDescription)) 
  objLocationSettingDesc.LocationDescription = (drow[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(drow[LocationSettingDescDBFields.LocationDescription]) : string.Empty); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.StatusId)) 
  objLocationSettingDesc.StatusId = (drow[LocationSettingDescDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDescDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.CreatedDate)) 
  objLocationSettingDesc.CreatedDate = (drow[LocationSettingDescDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.UpdatedDate)) 
  objLocationSettingDesc.UpdatedDate = (drow[LocationSettingDescDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.UpdatedDate]) : DateTime.Now); 


                        LocationSettingDescs.Add(objLocationSettingDesc);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return LocationSettingDescs;
        }
		
		public LocationSettingDesc GetDetailsobj(DataSet dataSet)
        {
            LocationSettingDesc objLocationSettingDesc = new LocationSettingDesc();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objLocationSettingDesc = new LocationSettingDesc();
                      
						if (drow.Table.Columns.Contains(LocationSettingDescDBFields.ID)) 
  objLocationSettingDesc.ID = (drow[LocationSettingDescDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationSettingID)) 
  objLocationSettingDesc.LocationSettingID = (drow[LocationSettingDescDBFields.LocationSettingID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.LocationSettingID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationDescription)) 
  objLocationSettingDesc.LocationDescription = (drow[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(drow[LocationSettingDescDBFields.LocationDescription]) : string.Empty); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.StatusId)) 
  objLocationSettingDesc.StatusId = (drow[LocationSettingDescDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDescDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.CreatedDate)) 
  objLocationSettingDesc.CreatedDate = (drow[LocationSettingDescDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.UpdatedDate)) 
  objLocationSettingDesc.UpdatedDate = (drow[LocationSettingDescDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return objLocationSettingDesc;
        }
		
		public LocationSettingDesc GetDetails(DataTable dataTable)
        {
            LocationSettingDesc objLocationSettingDesc = new LocationSettingDesc();

            try
            {
                if (dataTable != null &&  dataTable.Rows.Count > 0)
                {
                    foreach (DataRow drow in dataTable.Rows)
                    {
                        objLocationSettingDesc = new LocationSettingDesc();
                      
						if (drow.Table.Columns.Contains(LocationSettingDescDBFields.ID)) 
  objLocationSettingDesc.ID = (drow[LocationSettingDescDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationSettingID)) 
  objLocationSettingDesc.LocationSettingID = (drow[LocationSettingDescDBFields.LocationSettingID] != DBNull.Value ? Convert.ToInt32(drow[LocationSettingDescDBFields.LocationSettingID]) : 0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.LocationDescription)) 
  objLocationSettingDesc.LocationDescription = (drow[LocationSettingDescDBFields.LocationDescription] != DBNull.Value ? Convert.ToString(drow[LocationSettingDescDBFields.LocationDescription]) : string.Empty); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.StatusId)) 
  objLocationSettingDesc.StatusId = (drow[LocationSettingDescDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[LocationSettingDescDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.CreatedDate)) 
  objLocationSettingDesc.CreatedDate = (drow[LocationSettingDescDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(LocationSettingDescDBFields.UpdatedDate)) 
  objLocationSettingDesc.UpdatedDate = (drow[LocationSettingDescDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[LocationSettingDescDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(DataTable)", ex.Source, ex.Message, ex);
            }

            return objLocationSettingDesc;
        }

    }
}
