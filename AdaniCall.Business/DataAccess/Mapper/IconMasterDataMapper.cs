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
    public class IconMasterDataMapper
    {
        private static readonly string _module = "AdaniCall.Business.DataAccess.Mapper.IconMasterDataMapper";
        private IconMaster objIconMaster = null;

        public IconMaster GetDetails(SqlDataReader sqlDataReader)
        {
            try
            {
                objIconMaster = new IconMaster();
               
			   if (sqlDataReader.HasColumn(IconMasterDBFields.ID))
   objIconMaster.ID = (sqlDataReader[IconMasterDBFields.ID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[IconMasterDBFields.ID]) : 0); 
if (sqlDataReader.HasColumn(IconMasterDBFields.IconName))
   objIconMaster.IconName = (sqlDataReader[IconMasterDBFields.IconName] != DBNull.Value ? Convert.ToString(sqlDataReader[IconMasterDBFields.IconName]) : string.Empty); 
if (sqlDataReader.HasColumn(IconMasterDBFields.IconImage))
   objIconMaster.IconImage = (sqlDataReader[IconMasterDBFields.IconImage] != DBNull.Value ? Convert.ToString(sqlDataReader[IconMasterDBFields.IconImage]) : string.Empty); 
if (sqlDataReader.HasColumn(IconMasterDBFields.StatusId))
   objIconMaster.StatusId = (sqlDataReader[IconMasterDBFields.StatusId] != DBNull.Value ? Convert.ToByte(sqlDataReader[IconMasterDBFields.StatusId]) : (byte)0); 
if (sqlDataReader.HasColumn(IconMasterDBFields.CreatedDate))
   objIconMaster.CreatedDate = (sqlDataReader[IconMasterDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[IconMasterDBFields.CreatedDate]) : DateTime.Now); 
if (sqlDataReader.HasColumn(IconMasterDBFields.UpdatedDate))
   objIconMaster.UpdatedDate = (sqlDataReader[IconMasterDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[IconMasterDBFields.UpdatedDate]) : DateTime.Now); 

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return objIconMaster;
        }
		
		public List<IconMaster> GetDetailsList(SqlDataReader sqlDataReader)
        {
            List<IconMaster> list = new List<IconMaster>();
            try
            {
                while (sqlDataReader.Read())
                {
                    objIconMaster = GetDetails(sqlDataReader);
                    list.Add(objIconMaster);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetailsList(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return list;
        }

        public List<IconMaster> GetDetails(DataSet dataSet)
        {
            List<IconMaster> IconMasters = new List<IconMaster>();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objIconMaster = new IconMaster();
                       
						if (drow.Table.Columns.Contains(IconMasterDBFields.ID)) 
  objIconMaster.ID = (drow[IconMasterDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[IconMasterDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconName)) 
  objIconMaster.IconName = (drow[IconMasterDBFields.IconName] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconName]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconImage)) 
  objIconMaster.IconImage = (drow[IconMasterDBFields.IconImage] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconImage]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.StatusId)) 
  objIconMaster.StatusId = (drow[IconMasterDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[IconMasterDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.CreatedDate)) 
  objIconMaster.CreatedDate = (drow[IconMasterDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(IconMasterDBFields.UpdatedDate)) 
  objIconMaster.UpdatedDate = (drow[IconMasterDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.UpdatedDate]) : DateTime.Now); 


                        IconMasters.Add(objIconMaster);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return IconMasters;
        }
		
		public IconMaster GetDetailsobj(DataSet dataSet)
        {
            IconMaster objIconMaster = new IconMaster();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objIconMaster = new IconMaster();
                      
						if (drow.Table.Columns.Contains(IconMasterDBFields.ID)) 
  objIconMaster.ID = (drow[IconMasterDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[IconMasterDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconName)) 
  objIconMaster.IconName = (drow[IconMasterDBFields.IconName] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconName]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconImage)) 
  objIconMaster.IconImage = (drow[IconMasterDBFields.IconImage] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconImage]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.StatusId)) 
  objIconMaster.StatusId = (drow[IconMasterDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[IconMasterDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.CreatedDate)) 
  objIconMaster.CreatedDate = (drow[IconMasterDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(IconMasterDBFields.UpdatedDate)) 
  objIconMaster.UpdatedDate = (drow[IconMasterDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return objIconMaster;
        }
		
		public IconMaster GetDetails(DataTable dataTable)
        {
            IconMaster objIconMaster = new IconMaster();

            try
            {
                if (dataTable != null &&  dataTable.Rows.Count > 0)
                {
                    foreach (DataRow drow in dataTable.Rows)
                    {
                        objIconMaster = new IconMaster();
                      
						if (drow.Table.Columns.Contains(IconMasterDBFields.ID)) 
  objIconMaster.ID = (drow[IconMasterDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[IconMasterDBFields.ID]) : 0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconName)) 
  objIconMaster.IconName = (drow[IconMasterDBFields.IconName] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconName]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.IconImage)) 
  objIconMaster.IconImage = (drow[IconMasterDBFields.IconImage] != DBNull.Value ? Convert.ToString(drow[IconMasterDBFields.IconImage]) : string.Empty); 
if (drow.Table.Columns.Contains(IconMasterDBFields.StatusId)) 
  objIconMaster.StatusId = (drow[IconMasterDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[IconMasterDBFields.StatusId]) : (byte)0); 
if (drow.Table.Columns.Contains(IconMasterDBFields.CreatedDate)) 
  objIconMaster.CreatedDate = (drow[IconMasterDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.CreatedDate]) : DateTime.Now); 
if (drow.Table.Columns.Contains(IconMasterDBFields.UpdatedDate)) 
  objIconMaster.UpdatedDate = (drow[IconMasterDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[IconMasterDBFields.UpdatedDate]) : DateTime.Now); 

                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(DataTable)", ex.Source, ex.Message, ex);
            }

            return objIconMaster;
        }

    }
}
