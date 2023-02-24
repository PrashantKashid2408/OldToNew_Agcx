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
    public class UserFeedBackDataMapper
    {
        private static readonly string _module = "AdaniCall.Business.DataAccess.Mapper.UserFeedBackDataMapper";
        private UserFeedBack objUserFeedBack = null;

        public UserFeedBack GetDetails(SqlDataReader sqlDataReader)
        {
            try
            {
                objUserFeedBack = new UserFeedBack();

                if (sqlDataReader.HasColumn(UserFeedBackDBFields.ID))
                    objUserFeedBack.ID = (sqlDataReader[UserFeedBackDBFields.ID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[UserFeedBackDBFields.ID]) : 0);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.KioskMasterID))
                    objUserFeedBack.KioskMasterID = (sqlDataReader[UserFeedBackDBFields.KioskMasterID] != DBNull.Value ? Convert.ToInt32(sqlDataReader[UserFeedBackDBFields.KioskMasterID]) : 0);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.PhoneNumber))
                    objUserFeedBack.PhoneNumber = (sqlDataReader[UserFeedBackDBFields.PhoneNumber] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.PhoneNumber]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.FirstName))
                    objUserFeedBack.FirstName = (sqlDataReader[UserFeedBackDBFields.FirstName] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.FirstName]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.LastName))
                    objUserFeedBack.LastName = (sqlDataReader[UserFeedBackDBFields.LastName] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.LastName]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.Email))
                    objUserFeedBack.Email = (sqlDataReader[UserFeedBackDBFields.Email] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.Email]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.Name))
                    objUserFeedBack.Name = (sqlDataReader[UserFeedBackDBFields.Name] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.Name]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.AirportName))
                    objUserFeedBack.AirportName = (sqlDataReader[UserFeedBackDBFields.AirportName] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.AirportName]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.SubCategoryCode))
                    objUserFeedBack.SubCategoryCode = (sqlDataReader[UserFeedBackDBFields.SubCategoryCode] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.SubCategoryCode]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.CaseTypeCode))
                    objUserFeedBack.CaseTypeCode = (sqlDataReader[UserFeedBackDBFields.CaseTypeCode] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.CaseTypeCode]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.RatingText))
                    objUserFeedBack.RatingText = (sqlDataReader[UserFeedBackDBFields.RatingText] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.RatingText]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.RatingNumber))
                    objUserFeedBack.RatingNumber = (sqlDataReader[UserFeedBackDBFields.RatingNumber] != DBNull.Value ? Convert.ToInt32(sqlDataReader[UserFeedBackDBFields.RatingNumber]) : 0);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.FeedbackDesc))
                    objUserFeedBack.FeedbackDesc = (sqlDataReader[UserFeedBackDBFields.FeedbackDesc] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.FeedbackDesc]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.Terminal))
                    objUserFeedBack.Terminal = (sqlDataReader[UserFeedBackDBFields.Terminal] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.Terminal]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.CRMCaseNumber))
                    objUserFeedBack.CRMCaseNumber = (sqlDataReader[UserFeedBackDBFields.CRMCaseNumber] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.CRMCaseNumber]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.CRMResponseMessage))
                    objUserFeedBack.CRMResponseMessage = (sqlDataReader[UserFeedBackDBFields.CRMResponseMessage] != DBNull.Value ? Convert.ToString(sqlDataReader[UserFeedBackDBFields.CRMResponseMessage]) : string.Empty);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.StatusId))
                    objUserFeedBack.StatusId = (sqlDataReader[UserFeedBackDBFields.StatusId] != DBNull.Value ? Convert.ToByte(sqlDataReader[UserFeedBackDBFields.StatusId]) : (byte)0);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.CreatedDate))
                    objUserFeedBack.CreatedDate = (sqlDataReader[UserFeedBackDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[UserFeedBackDBFields.CreatedDate]) : DateTime.Now);
                if (sqlDataReader.HasColumn(UserFeedBackDBFields.UpdatedDate))
                    objUserFeedBack.UpdatedDate = (sqlDataReader[UserFeedBackDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(sqlDataReader[UserFeedBackDBFields.UpdatedDate]) : DateTime.Now);

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return objUserFeedBack;
        }

        public List<UserFeedBack> GetDetailsList(SqlDataReader sqlDataReader)
        {
            List<UserFeedBack> list = new List<UserFeedBack>();
            try
            {
                while (sqlDataReader.Read())
                {
                    objUserFeedBack = GetDetails(sqlDataReader);
                    list.Add(objUserFeedBack);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetailsList(sqlDataReader)", ex.Source, ex.Message, ex);
            }
            return list;
        }

        public List<UserFeedBack> GetDetails(DataSet dataSet)
        {
            List<UserFeedBack> UserFeedBacks = new List<UserFeedBack>();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objUserFeedBack = new UserFeedBack();

                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.ID))
                            objUserFeedBack.ID = (drow[UserFeedBackDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.ID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.KioskMasterID))
                            objUserFeedBack.KioskMasterID = (drow[UserFeedBackDBFields.KioskMasterID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.KioskMasterID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.PhoneNumber))
                            objUserFeedBack.PhoneNumber = (drow[UserFeedBackDBFields.PhoneNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.PhoneNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FirstName))
                            objUserFeedBack.FirstName = (drow[UserFeedBackDBFields.FirstName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FirstName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.LastName))
                            objUserFeedBack.LastName = (drow[UserFeedBackDBFields.LastName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.LastName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Email))
                            objUserFeedBack.Email = (drow[UserFeedBackDBFields.Email] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Email]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingText))
                            objUserFeedBack.RatingText = (drow[UserFeedBackDBFields.RatingText] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.RatingText]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingNumber))
                            objUserFeedBack.RatingNumber = (drow[UserFeedBackDBFields.RatingNumber] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.RatingNumber]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FeedbackDesc))
                            objUserFeedBack.FeedbackDesc = (drow[UserFeedBackDBFields.FeedbackDesc] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FeedbackDesc]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Terminal))
                            objUserFeedBack.Terminal = (drow[UserFeedBackDBFields.Terminal] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Terminal]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMCaseNumber))
                            objUserFeedBack.CRMCaseNumber = (drow[UserFeedBackDBFields.CRMCaseNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMCaseNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMResponseMessage))
                            objUserFeedBack.CRMResponseMessage = (drow[UserFeedBackDBFields.CRMResponseMessage] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMResponseMessage]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.StatusId))
                            objUserFeedBack.StatusId = (drow[UserFeedBackDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[UserFeedBackDBFields.StatusId]) : (byte)0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CreatedDate))
                            objUserFeedBack.CreatedDate = (drow[UserFeedBackDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.CreatedDate]) : DateTime.Now);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.UpdatedDate))
                            objUserFeedBack.UpdatedDate = (drow[UserFeedBackDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.UpdatedDate]) : DateTime.Now);


                        UserFeedBacks.Add(objUserFeedBack);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return UserFeedBacks;
        }

        public UserFeedBack GetDetailsobj(DataSet dataSet)
        {
            UserFeedBack objUserFeedBack = new UserFeedBack();

            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dataSet.Tables[0].Rows)
                    {
                        objUserFeedBack = new UserFeedBack();

                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.ID))
                            objUserFeedBack.ID = (drow[UserFeedBackDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.ID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.KioskMasterID))
                            objUserFeedBack.KioskMasterID = (drow[UserFeedBackDBFields.KioskMasterID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.KioskMasterID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.PhoneNumber))
                            objUserFeedBack.PhoneNumber = (drow[UserFeedBackDBFields.PhoneNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.PhoneNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FirstName))
                            objUserFeedBack.FirstName = (drow[UserFeedBackDBFields.FirstName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FirstName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.LastName))
                            objUserFeedBack.LastName = (drow[UserFeedBackDBFields.LastName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.LastName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Email))
                            objUserFeedBack.Email = (drow[UserFeedBackDBFields.Email] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Email]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingText))
                            objUserFeedBack.RatingText = (drow[UserFeedBackDBFields.RatingText] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.RatingText]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingNumber))
                            objUserFeedBack.RatingNumber = (drow[UserFeedBackDBFields.RatingNumber] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.RatingNumber]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FeedbackDesc))
                            objUserFeedBack.FeedbackDesc = (drow[UserFeedBackDBFields.FeedbackDesc] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FeedbackDesc]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Terminal))
                            objUserFeedBack.Terminal = (drow[UserFeedBackDBFields.Terminal] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Terminal]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMCaseNumber))
                            objUserFeedBack.CRMCaseNumber = (drow[UserFeedBackDBFields.CRMCaseNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMCaseNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMResponseMessage))
                            objUserFeedBack.CRMResponseMessage = (drow[UserFeedBackDBFields.CRMResponseMessage] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMResponseMessage]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.StatusId))
                            objUserFeedBack.StatusId = (drow[UserFeedBackDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[UserFeedBackDBFields.StatusId]) : (byte)0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CreatedDate))
                            objUserFeedBack.CreatedDate = (drow[UserFeedBackDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.CreatedDate]) : DateTime.Now);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.UpdatedDate))
                            objUserFeedBack.UpdatedDate = (drow[UserFeedBackDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.UpdatedDate]) : DateTime.Now);


                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(dataSet)", ex.Source, ex.Message, ex);
            }

            return objUserFeedBack;
        }

        public UserFeedBack GetDetails(DataTable dataTable)
        {
            UserFeedBack objUserFeedBack = new UserFeedBack();

            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow drow in dataTable.Rows)
                    {
                        objUserFeedBack = new UserFeedBack();

                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.ID))
                            objUserFeedBack.ID = (drow[UserFeedBackDBFields.ID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.ID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.KioskMasterID))
                            objUserFeedBack.KioskMasterID = (drow[UserFeedBackDBFields.KioskMasterID] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.KioskMasterID]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.PhoneNumber))
                            objUserFeedBack.PhoneNumber = (drow[UserFeedBackDBFields.PhoneNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.PhoneNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FirstName))
                            objUserFeedBack.FirstName = (drow[UserFeedBackDBFields.FirstName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FirstName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.LastName))
                            objUserFeedBack.LastName = (drow[UserFeedBackDBFields.LastName] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.LastName]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Email))
                            objUserFeedBack.Email = (drow[UserFeedBackDBFields.Email] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Email]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingText))
                            objUserFeedBack.RatingText = (drow[UserFeedBackDBFields.RatingText] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.RatingText]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.RatingNumber))
                            objUserFeedBack.RatingNumber = (drow[UserFeedBackDBFields.RatingNumber] != DBNull.Value ? Convert.ToInt32(drow[UserFeedBackDBFields.RatingNumber]) : 0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.FeedbackDesc))
                            objUserFeedBack.FeedbackDesc = (drow[UserFeedBackDBFields.FeedbackDesc] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.FeedbackDesc]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.Terminal))
                            objUserFeedBack.Terminal = (drow[UserFeedBackDBFields.Terminal] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.Terminal]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMCaseNumber))
                            objUserFeedBack.CRMCaseNumber = (drow[UserFeedBackDBFields.CRMCaseNumber] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMCaseNumber]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CRMResponseMessage))
                            objUserFeedBack.CRMResponseMessage = (drow[UserFeedBackDBFields.CRMResponseMessage] != DBNull.Value ? Convert.ToString(drow[UserFeedBackDBFields.CRMResponseMessage]) : string.Empty);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.StatusId))
                            objUserFeedBack.StatusId = (drow[UserFeedBackDBFields.StatusId] != DBNull.Value ? Convert.ToByte(drow[UserFeedBackDBFields.StatusId]) : (byte)0);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.CreatedDate))
                            objUserFeedBack.CreatedDate = (drow[UserFeedBackDBFields.CreatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.CreatedDate]) : DateTime.Now);
                        if (drow.Table.Columns.Contains(UserFeedBackDBFields.UpdatedDate))
                            objUserFeedBack.UpdatedDate = (drow[UserFeedBackDBFields.UpdatedDate] != DBNull.Value ? Convert.ToDateTime(drow[UserFeedBackDBFields.UpdatedDate]) : DateTime.Now);


                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetDetails(DataTable)", ex.Source, ex.Message, ex);
            }

            return objUserFeedBack;
        }

    }
}
