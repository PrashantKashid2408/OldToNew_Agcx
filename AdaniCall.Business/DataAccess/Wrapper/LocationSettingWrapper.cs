using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaniCall.Business.DataAccess.Constants;
using AdaniCall.Business.DataAccess.DataAccessLayer;
using AdaniCall.Business.DataAccess.DataAccessLayer.General;
using AdaniCall.Business.DataAccess.Mapper;
using AdaniCall.Entity;
using AdaniCall.Entity.Common;
using AdaniCall.Utility.Common;

namespace AdaniCall.Business.DataAccess.Wrapper
{
    public class LocationSettingWrapper : UniversalObject
    {
		private readonly string _module = "AdaniCall.Business.DataAccess.Wrapper.LocationSetting"; 
        private SqlConnection Connection;

        #region UniversalObject Interface Members
        public bool ObjectChanged { get; set; }


        public LocationSetting objWrapperClass = new LocationSetting();
        private LocationSettingDataMapper objLocationSettingDataMapper = new LocationSettingDataMapper();
        #region GetRecords methods
        public bool GetRecordById(int id)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = LocationSettingStoredProcedures.LocationSettingGetRecordById;
                sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = this.Connection;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, id);
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    objWrapperClass = objLocationSettingDataMapper.GetDetails(sqlDataReader);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetRecordById(" + id + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        public bool GetRecordByValue(string fieldName, string value)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = LocationSettingStoredProcedures.GetLocationSettingRecordByValue;
                sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = this.Connection;
                sqlCommand.Parameters.AddWithValue(fieldName, value);
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    objWrapperClass = objLocationSettingDataMapper.GetDetails(sqlDataReader);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetRecordByValue(" + fieldName + "," + value + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        public bool GetRecordByValue(string[] fieldNames, string[] values)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = LocationSettingStoredProcedures.GetLocationSettingRecordByValueArray;
                sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = this.Connection;
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(fieldNames[i], values[i]);
                }

                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    objWrapperClass = objLocationSettingDataMapper.GetDetails(sqlDataReader);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetRecordByValue(" + string.Join(",", fieldNames) + "," + string.Join(",", values) + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        #endregion
        public bool Save(ref Dictionary<string, Command> commandList, ref int commandCounter)
        {
            try
            {
                if (objWrapperClass.ID > 0)
                {
                    Update(ref commandList, ref commandCounter);
                }
                else
                {
                    Command command = new Command(LocationSettingStoredProcedures.LocationSettingSAVE, CommandType.StoredProcedure);
                    command.AddParameter(LocationSettingDBFields.IU_Flag, "I", DataAccessLayer.DataAccess.DataType.Char, 0, ParameterDirection.Input);
					command.AddParameter(LocationSettingDBFields.ID, objWrapperClass.ID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                    command.AddParameter(LocationSettingDBFields.IconMasterID, objWrapperClass.IconMasterID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                    command.AddParameter(LocationSettingDBFields.KioskID, objWrapperClass.KioskID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                    command.AddParameter("RetID", 0, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Output);
                    //command.AddParameter(LocationSettingDBFields.StatusId, objWrapperClass.StatusId, DataAccessLayer.DataAccess.DataType.Varchar2, 0, ParameterDirection.Input);
                    //command.AddParameter(LocationSettingDBFields.CreatedDate, objWrapperClass.CreatedDate, DataAccessLayer.DataAccess.DataType.DateTime, 0, ParameterDirection.Input);
                    //command.AddParameter(LocationSettingDBFields.UpdatedDate, objWrapperClass.UpdatedDate, DataAccessLayer.DataAccess.DataType.DateTime, 0, ParameterDirection.Input);

                    // PRATIK 27/JAN/2023
                  //  command.AddParameter(LocationSettingDescDBFields.LocationDescription, objWrapperClass.LocationDescription, DataAccessLayer.DataAccess.DataType.NVarChar, 0, ParameterDirection.Input);

                    command.Name = LocationSettingStoredProcedures.LocationSettingSAVE + commandCounter.ToString();
                    commandCounter++;
                    commandList.Add(command.Name, command);
                }
                return true;

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Save", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {


            }
        }
        public bool Update(ref Dictionary<string, Command> commandList, ref int commandCounter)
        {
            try
            {

                Command command = new Command(LocationSettingStoredProcedures.LocationSettingSAVE, CommandType.StoredProcedure);


                command.AddParameter(LocationSettingDBFields.IU_Flag, "U", DataAccessLayer.DataAccess.DataType.Char, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.ID, objWrapperClass.ID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.IconMasterID, objWrapperClass.IconMasterID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.KioskID, objWrapperClass.KioskID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.StatusId, objWrapperClass.StatusId, DataAccessLayer.DataAccess.DataType.Varchar2, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.CreatedDate, objWrapperClass.CreatedDate, DataAccessLayer.DataAccess.DataType.DateTime, 0, ParameterDirection.Input);
                command.AddParameter(LocationSettingDBFields.UpdatedDate, objWrapperClass.UpdatedDate, DataAccessLayer.DataAccess.DataType.DateTime, 0, ParameterDirection.Input);


                command.Name = LocationSettingStoredProcedures.LocationSettingSAVE + commandCounter.ToString();
                commandCounter++;
                commandList.Add(command.Name, command);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Update", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {


            }
        }
        public bool Delete(ref Dictionary<string, Command> commandList, ref int commandCounter)
        {
            try
            {

                Command command = new Command("SP_MASTERS_Delete", CommandType.StoredProcedure);
                command.AddParameter("@TableName", "LocationSetting", DataAccessLayer.DataAccess.DataType.Varchar, 0, ParameterDirection.Input);
                command.AddParameter("@PrimaryKeyColumn", "ID", DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                command.AddParameter("@IDs", objWrapperClass.ID, DataAccessLayer.DataAccess.DataType.Number, 0, ParameterDirection.Input);
                command.Name = "DeleteLocationSetting" + commandCounter.ToString();
                commandCounter++;
                commandList.Add(command.Name, command);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Delete", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
            }
        }

        public bool SaveCopy()
        {
            throw new NotImplementedException();
        }

        public bool Move()
        {
            throw new NotImplementedException();
        }

        public bool Print()
        {
            throw new NotImplementedException();
        }
        #endregion UniversalObject Interface Members

        #region Other Methods

        public LocationSetting IsIconImageExists(string iconname, int kioskid)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                if (!string.IsNullOrEmpty(iconname))
                {
                    if (this.Connection == null)
                        this.Connection = dbClass.GetSqlConnection();

                    SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.LocationSettingIsIconImageExists, this.Connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = Convert.ToInt32(AdaniCallConstants.SQLCommandTimeOut);
                    sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.IconMasterID, iconname);
                    sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.KioskID, kioskid);

                    sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                        objWrapperClass = objLocationSettingDataMapper.GetDetails(sqlDataReader);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "IsIconImageExists(iconname=" + iconname + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return objWrapperClass;
        }


        #endregion Other Methods

    }

    public class LocationSettingWrapperColletion : UniversalCollection
    {
		private readonly string _module = "LocationSetting"; 
        private SqlConnection Connection;
        private List<LocationSetting> _Items = new List<LocationSetting>();
        public List<LocationSetting> Items
        { get { return this._Items; } set { this._Items = value; } }

        private DataSet _DtsDataset = null;
        private string _SortingString = "";

        #region UniversalCollection Interface Members Implementation

        #region GetRecords methods
        public bool GetRecords(bool createDataSet, string[,] sortFields)
        {
            if (createDataSet)
                return GetDataSetForQuery(LocationSettingStoredProcedures.GetLocationSettingRecords);
            else
                return GetCollectionForQuery(LocationSettingStoredProcedures.GetLocationSettingRecords);
        }
        public bool GetRecords(bool createDataSet, string[,] sortFields, bool isParameter)
        {
            if (sortFields != null)
            {
                if (sortFields.Length > 0)
                {
                    _SortingString += "order by ";
                    for (int i = 0; i <= sortFields.GetUpperBound(0); i++)
                    {
                        _SortingString += "" + sortFields[i, 0] + " " + sortFields[i, 1] + ",";
                    }
                    _SortingString = _SortingString.Substring(0, _SortingString.Length - 1);
                }
            }

            SqlParameterCollection sqlParameterCollection = null;
            SqlParameter ObjSqlParameter = new SqlParameter();
            ObjSqlParameter.ParameterName = LocationSettingStoredProcedures.SortingString;
            ObjSqlParameter.Value = _SortingString;
            sqlParameterCollection.Add(ObjSqlParameter);

            if (createDataSet)
                return GetDataSetForQueryParameter(LocationSettingStoredProcedures.GetLocationSettingRecords, sqlParameterCollection);
            else
                return GetDataSetForQueryParameter(LocationSettingStoredProcedures.GetLocationSettingRecords, sqlParameterCollection);
        }
        public bool GetRecords(bool createDataSet, string[,] sortFields, bool isParameter, string fieldName, string fieldValue)
        {
            if (sortFields != null)
            {
                if (sortFields.Length > 0)
                {
                    _SortingString += "order by ";
                    for (int i = 0; i <= sortFields.GetUpperBound(0); i++)
                    {
                        _SortingString += "" + sortFields[i, 0] + " " + sortFields[i, 1] + ",";
                    }
                    _SortingString = _SortingString.Substring(0, _SortingString.Length - 1);
                }
            }

            string[] Fieldsname = new string[1];
            string[] Values = new string[1];
            Fieldsname[0] = fieldName;
            Values[0] = fieldValue;

            return GetCollectionForQueryWithParameters(LocationSettingStoredProcedures.GetLocationSettingRecordByValue, Fieldsname, Values);
        }
		
	private bool GetCollectionForQueryWithParameters(string sqlQuery, string[] fieldNames, string[] values)
        {
            SqlDataReader _Dtr = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = this.Connection;

                if (fieldNames != null)
                {
                    if (fieldNames.Length > 0)
                    {
                        for (int i = 0; i < fieldNames.Length; i++)
                        {
                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = fieldNames[i];
                            sqlParameter.Value = values[i];
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                }

                _Dtr = sqlCommand.ExecuteReader();
                while (_Dtr.Read())
                {
                    LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
                    this.Items.Add(objDataMapper.GetDetails(_Dtr));
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetCollectionForQueryWithParameters(" + sqlQuery + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (_Dtr != null)
                    _Dtr.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
		
        public bool GetRecords(bool createDataSet, string[,] sortFields, bool isParameter, string[] fieldName, string[] fieldValue)
        {
            SqlParameterCollection sqlParameterCollection = null;
            if (fieldName != null)
            {
                if (fieldName.Length > 0)
                {
                    for (int i = 0; i < fieldName.Length; i++)
                    {
                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = fieldName[i];
                        sqlParameter.Value = fieldValue[i];
                        sqlParameterCollection.Add(sqlParameter);
                    }
                }
            }
            if (sortFields != null)
            {
                if (sortFields.Length > 0)
                {
                    _SortingString += "order by ";
                    for (int i = 0; i <= sortFields.GetUpperBound(0); i++)
                    {
                        _SortingString += "" + sortFields[i, 0] + " " + sortFields[i, 1] + ",";
                    }
                    _SortingString = _SortingString.Substring(0, _SortingString.Length - 1);
                }

                SqlParameter ObjSqlParameter = new SqlParameter();
                ObjSqlParameter.ParameterName = LocationSettingStoredProcedures.SortingString;
                ObjSqlParameter.Value = _SortingString;
                sqlParameterCollection.Add(ObjSqlParameter);
            }



            if (createDataSet)
                return GetDataSetForQueryParameter(LocationSettingStoredProcedures.GetLocationSettingRecords, sqlParameterCollection);
            else
                return GetDataSetForQueryParameter(LocationSettingStoredProcedures.GetLocationSettingRecords, sqlParameterCollection);
        }
        #endregion

        #region Seach Method
        public bool Search(string searchString, string[,] sortString)
        {
            throw new NotImplementedException();
        }
        public bool Search(string fieldName, string fieldValue, string[,] sortString)
        {
            try
            {
                SqlParameterCollection sqlParameterCollection = null;
                SqlParameter ObjSqlParameter = new SqlParameter();
                ObjSqlParameter.ParameterName = fieldName;
                ObjSqlParameter.Value = fieldValue;
                sqlParameterCollection.Add(ObjSqlParameter);

                GetCollectionForQueryWithParameter(LocationSettingStoredProcedures.LocationSettingSearch, sqlParameterCollection);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Search(" + fieldName + "," + fieldValue + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        public bool Search(string searchString, bool createDataSet, string[,] sortFields)
        {
            throw new NotImplementedException();
        }
        public bool Search(string fieldName, string fieldValue, bool createDataSet, string[,] sortFields)
        {


            try
            {
                SqlParameterCollection sqlParameterCollection = null;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = fieldName;
                sqlParameter.Value = fieldValue;
                sqlParameterCollection.Add(sqlParameter);

                SqlParameter ObjSqlParameter = new SqlParameter();
                ObjSqlParameter.ParameterName = LocationSettingStoredProcedures.SortingString;
                ObjSqlParameter.Value = _SortingString;
                sqlParameterCollection.Add(ObjSqlParameter);

                if (createDataSet)
                    return GetDataSetForQueryParameter(LocationSettingStoredProcedures.LocationSettingSearchByValue, sqlParameterCollection);
                else
                    return GetDataSetForQueryParameter(LocationSettingStoredProcedures.LocationSettingSearchByValue, sqlParameterCollection);


            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Search(" + fieldName + "," + fieldValue + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        public bool Search(string[] fieldName, string[] fieldValue, bool createDataSet, string[,] sortFields)
        {


            try
            {



                SqlParameterCollection sqlParameterCollection = null;
                if (fieldName != null)
                {
                    if (fieldName.Length > 0)
                    {
                        for (int i = 0; i < fieldName.Length; i++)
                        {
                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = fieldName[i];
                            sqlParameter.Value = fieldValue[i];
                            sqlParameterCollection.Add(sqlParameter);
                        }
                    }
                }
                if (sortFields != null)
                {
                    if (sortFields.Length > 0)
                    {
                        _SortingString += "order by ";
                        for (int i = 0; i <= sortFields.GetUpperBound(0); i++)
                        {
                            _SortingString += "" + sortFields[i, 0] + " " + sortFields[i, 1] + ",";
                        }
                        _SortingString = _SortingString.Substring(0, _SortingString.Length - 1);
                    }

                    SqlParameter ObjSqlParameter = new SqlParameter();
                    ObjSqlParameter.ParameterName = LocationSettingStoredProcedures.SortingString;
                    ObjSqlParameter.Value = _SortingString;
                    sqlParameterCollection.Add(ObjSqlParameter);
                }


                if (createDataSet)
                    return GetDataSetForQueryParameter(LocationSettingStoredProcedures.LocationSettingSearchByValueArray, sqlParameterCollection);
                else
                    return GetDataSetForQueryParameter(LocationSettingStoredProcedures.LocationSettingSearchByValueArray, sqlParameterCollection);


            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Search(" + fieldName + "," + fieldValue + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {

                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        #endregion

        #region ExecuteQuery Methods
        private bool GetDataSetForQuery(string sqlQuery)
        {
            try
            {
                DataSet _DtsUsers = new DataSet("LocationSetting");
                SqlDataAdapter _Adpusers = new SqlDataAdapter(sqlQuery, this.Connection);
                _Adpusers.Fill(_DtsUsers);
                this._DtsDataset = _DtsUsers;
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDataSetForQuery(" + sqlQuery + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally { }
        }
        private bool GetDataSetForQueryParameter(string sqlQuery, SqlParameterCollection ObjSqlParameter)
        {
            try
            {
                DataSet _DtsUsers = new DataSet("LocationSetting");
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, this.Connection);
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.CommandTimeout = 0;
                sqlCommand.Parameters.Add(ObjSqlParameter);
                SqlDataAdapter _Adpusers = new SqlDataAdapter();
                _Adpusers.SelectCommand = sqlCommand;
                _Adpusers.Fill(this._DtsDataset);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDataSetForQuery(" + sqlQuery + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally { }
        }
        private bool GetCollectionForQuery(string sqlQuery)
        {
            SqlDataReader _Dtr = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = this.Connection;

                _Dtr = sqlCommand.ExecuteReader();
                while (_Dtr.Read())
                {
                    LocationSettingDataMapper objLocationSettingDataMapper = new LocationSettingDataMapper();
                    this.Items.Add(objLocationSettingDataMapper.GetDetails(_Dtr));
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetCollectionForQuery(" + sqlQuery + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (_Dtr != null)
                    _Dtr.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        private bool GetCollectionForQueryWithParameter(string sqlQuery, SqlParameterCollection ObjSqlParameter)
        {
            SqlDataReader _Dtr = null;
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.CommandTimeout = 0;
                sqlCommand.Parameters.Add(ObjSqlParameter);
                sqlCommand.Connection = this.Connection;
                _Dtr = sqlCommand.ExecuteReader();
                while (_Dtr.Read())
                {
                    LocationSettingDataMapper objLocationSettingDataMapper = new LocationSettingDataMapper();
                    this.Items.Add(objLocationSettingDataMapper.GetDetails(_Dtr));
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetCollectionForQuery(" + sqlQuery + ")", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
                if (_Dtr != null)
                    _Dtr.Close();
                dbClass.CloseSqlConnection(ref this.Connection);
            }
        }
        #endregion



        public bool Save(ref Dictionary<string, Command> commandList, ref int commandCounter)
        {
            try
            {
                LocationSettingWrapper   objLocationSettingWrapper = new LocationSettingWrapper();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].ObjectChanged == true)
                    {
                        Dictionary<string, Command> subCommands = new Dictionary<string, Command>();
                          objLocationSettingWrapper.Save(ref subCommands, ref commandCounter);
                        foreach (KeyValuePair<string, Command> commandPair in subCommands)
                        {
                            commandList.Add(commandPair.Key, commandPair.Value);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Save", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
            }
        }
        public bool Delete(string ids, ref Dictionary<string, Command> commandList, ref int commandCounter)
        {
            try
            {
                Command command = new Command("SP_MASTERS_DELETE", CommandType.StoredProcedure);
                command.AddParameter("@TableName", LocationSettingDBFields.TableNameVal, DataAccessLayer.DataAccess.DataType.Varchar, 0, ParameterDirection.Input);
                command.AddParameter("@PrimaryKeyColumn", LocationSettingDBFields.ID, DataAccessLayer.DataAccess.DataType.Varchar, 0, ParameterDirection.Input);
                command.AddParameter("@IDs", ids, DataAccessLayer.DataAccess.DataType.Varchar, 0, ParameterDirection.Input);
                command.Name = "Delete" + LocationSettingDBFields.TableNameVal + commandCounter.ToString();
                commandCounter++;
                commandList.Add(command.Name, command);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "Delete", ex.Source, ex.Message, ex);
                return false;
            }
            finally
            {
            }
        }

        object UniversalCollection.GetRecordById(int id)
        {
            throw new NotImplementedException();
        }
        object UniversalCollection.GetRecordByValue(string fieldName, string value)
        {
            throw new NotImplementedException();
        }


        #endregion UniversalCollection Interface Members..


        #region Other Methods


        public List<LocationSetting> GetListIconsAdmin(Int64 KioskID)
        {
            SqlDataReader sqlDataReader = null;
            LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.GetLocationSettingRecordsAdmin, this.Connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, KioskID);

                sqlDataReader = sqlCommand.ExecuteReader();
                _Items = objDataMapper.GetDetailsList(sqlDataReader);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconsAdmin(KioskID: " + KioskID + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return _Items;
        }
        public List<LocationSetting> GetListIconsKiosk(Int64 KioskID)
        {
            SqlDataReader sqlDataReader = null;
            LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.GetLocationSettingRecordsKiosk, this.Connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, KioskID);

                sqlDataReader = sqlCommand.ExecuteReader();
                _Items = objDataMapper.GetDetailsList(sqlDataReader);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconsKiosk(KioskID: " + KioskID + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return _Items;
        }
        public List<LocationSetting> GetListIconDescAdmin(Int64 LocationSettingID)
        {
            SqlDataReader sqlDataReader = null;
            LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.LocationSettingDescAdminGetRecords, this.Connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, LocationSettingID);

                sqlDataReader = sqlCommand.ExecuteReader();
                _Items = objDataMapper.GetDetailsList(sqlDataReader);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconDescAdmin(LocationSettingID: " + LocationSettingID + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return _Items;
        }
        public List<LocationSetting> GetListIconDescKiosk(Int64 LocationSettingID)
        {
            SqlDataReader sqlDataReader = null;
            LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.LocationSettingDescKioskGetRecords, this.Connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, LocationSettingID);

                sqlDataReader = sqlCommand.ExecuteReader();
                _Items = objDataMapper.GetDetailsList(sqlDataReader);
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetListIconDescKiosk(LocationSettingID: " + LocationSettingID + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return _Items;
        }

        public LocationSetting GetLocationDetailsByID(Int64 KioskID)
        {
            LocationSetting objItem = new LocationSetting();

            SqlDataReader sqlDataReader = null;
            LocationSettingDataMapper objDataMapper = new LocationSettingDataMapper();
            try
            {
                if (this.Connection == null)
                    this.Connection = dbClass.GetSqlConnection();

                SqlCommand sqlCommand = new SqlCommand(LocationSettingStoredProcedures.LocationSetting_GetRecordsByID, this.Connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue(LocationSettingDBFields.ID, KioskID);

                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    objItem = objDataMapper.GetDetails(sqlDataReader);

                    if (sqlDataReader.NextResult())
                    {
                        objItem.lstLocationSettingDesc = new List<LocationSettingDesc>();
                        LocationSettingDescDataMapper objLSDM = new LocationSettingDescDataMapper();
                        objItem.lstLocationSettingDesc = objLSDM.GetDetailsList(sqlDataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetLocationDetailsByID(KioskID: " + KioskID + ")", ex.Source, ex.Message, ex);
            }
            finally
            {
                dbClass.CloseSqlConnection(ref this.Connection);
            }

            return objItem;
        }

        #endregion


    }




}
