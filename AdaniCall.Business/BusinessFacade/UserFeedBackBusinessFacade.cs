using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaniCall.Business.DataAccess.Wrapper;
using AdaniCall.Business.DataAccess.Mapper;
using AdaniCall.Business.DataAccess.DataAccessLayer.General;
using AdaniCall.Business.DataAccess.DataAccessLayer;
using AdaniCall.Entity;
using AdaniCall.Business.DataAccess.Constants;
using AdaniCall.Utility;
using AdaniCall.Entity.Common;
using AdaniCall.Utility.Common;

namespace AdaniCall.Business.BusinessFacade
{
    public class UserFeedBackBusinessFacade//:UniversalObject
    {
        dynamic objdynamicWrapper;
        UserFeedBackWrapperColletion objUserFeedBackWrapperColletion = new UserFeedBackWrapperColletion();
        UserFeedBackWrapper objUserFeedBackWrapper = new UserFeedBackWrapper();
        private static readonly string _module = "AdaniCall.Business.BusinessFacade.UserFeedBackBusinessFacade";
        public UserFeedBackBusinessFacade()
        {

        }
        public UserFeedBackBusinessFacade(dynamic WrapperType)
        {
            objdynamicWrapper = WrapperType;
        }
        public dynamic GetRecordsList()
        {
            string[,] Sort = new string[1, 2];
            if (objdynamicWrapper.GetRecords(false, Sort))
            {
                return objdynamicWrapper.Items;
            }
            return null;
        }
        public dynamic GetRecordsListByValue(string Field, String Values)
        {
            string[,] Sort = new string[1, 2];

            if (objdynamicWrapper.GetRecords(false, Sort, true, Field, Values))
            {
                return objdynamicWrapper.Items;
            }
            return null;
        }

        public dynamic GetRecordByValue(string Field, string Values)
        {
            string[,] Sort = new string[1, 2];

            if (objdynamicWrapper.GetRecordByValue(Field, Values))
            {
                return objdynamicWrapper.objUsers;
            }
            return null;
        }

        public dynamic GetRecords(int Id)
        {

            if (objdynamicWrapper.GetRecordById(Id))
            {
                return objdynamicWrapper.objWrapperClass;
            }
            return null;
        }
        public long Save(dynamic objEntity)
        {
            long success = 0;
            try
            {
                objUserFeedBackWrapper.objWrapperClass = objEntity;
                DataAccess.DataAccessLayer.DataAccess dalObject = new DataAccess.DataAccessLayer.DataAccess();
                Transaction TransObj = new Transaction(dalObject);
                TransObj.ConnectionString = dbClass.SqlConnectString();
                Dictionary<string, Command> CommandsObj = new Dictionary<string, Command>();
                int commandCounter = 0;

                bool result = objUserFeedBackWrapper.Save(ref CommandsObj, ref commandCounter);
                TransObj.AddCommandList(CommandsObj);
                if (TransObj.ExecuteTransaction())
                {
                    long ID = 0;
                    if (long.TryParse(TransObj.ReturnID, out ID) && ID > 0)
                    {
                        success = ID;
                    }
                }



                //           if (TransObj.ExecuteTransaction())
                //           {
                //long ID = 0;
                //               if (long.TryParse(TransObj.ReturnID, out ID) && ID > 0)
                //               {
                //                   objEntity.ID = ID;
                //               }
                //               return true;
                //           }
                //           else
                //               return false;
            }
            catch (Exception ex)
            {

            }
            finally { }
            return success;
        }
    }
}
