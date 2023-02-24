using AdaniCall.Business.BusinessFacade;
using AdaniCall.Entity;
using AdaniCall.Utility.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdaniCall.Models
{

    public class CommonData
    {
        private readonly string _module = "AdaniCall.Models.CommonData";

        //public void AddToCache(string CacheKey, DateTime date, DateTime Days) //DateTime dtExp)
        //{
        //   _cache.Set(CacheKey, date, Days);
        //    //  HttpContext.Current.Cache.Insert(CacheKey, OBJ, null, dtExp, TimeSpan.Zero);
        //}

        //public dynamic GetFromCache(string CacheKey)
        //{
        //    return _cache.Get(CacheKey);
        //}

        //public void RemoveFromCache(string CacheKey)
        //{
        //    _cache.Remove(CacheKey);
        //}


        public List<SelectListItem> GetPageSizes()
        {
            List<SelectListItem> pageSizes = new List<SelectListItem>();
            try
            {
                pageSizes = new List<SelectListItem>
                {
                    //new SelectListItem { Text = "5", Value = "5"} ,
                    new SelectListItem { Text = "10", Value = "10"} ,
                    new SelectListItem { Text = "25", Value = "25"} ,
                    new SelectListItem { Text = "50", Value = "50" },
                    new SelectListItem { Text = "100", Value = "100" }
                };
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "GetPageSizes()", ex.Source, ex.Message, ex);
            }
            return pageSizes;
        }
        public class RecordParams
        {
            public string sCallID { get; set; }
            public string recordingFormat { get; set; }
            public string UniqueCallID { get; set; }

        }
        public List<SelectListItem> LanguageList(long SelectedLang = 0)
        {
            List<SelectListItem> _LanguageList = new List<SelectListItem>();
            LanguageMasterBusinessFacade _LanguageMasterBusinessFacade = new LanguageMasterBusinessFacade();
            try
            {
                List<LanguageMaster> _List = _LanguageMasterBusinessFacade.GetRecordsList();

                for (int i = 0; i < _List.Count; i++)
                {
                    SelectListItem _SelectListItem = new SelectListItem();
                    _SelectListItem.Text = _List[i].Language;
                    _SelectListItem.Value = Convert.ToString(_List[i].ID);
                    if (_List[i].ID == SelectedLang && SelectedLang > 0)
                    {
                        _SelectListItem.Selected = true;
                    }
                    _LanguageList.Add(_SelectListItem);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "LanguageList()", ex.Source, ex.Message, ex);
            }
            return _LanguageList;
        }

        public string LanguageNameFromId(string SelectedLang)
        {
            string LanguageName = "";
            try
            {
                List<SelectListItem> ListSelectListItem = LanguageList();

                if (ListSelectListItem.FindAll(X => X.Value == SelectedLang).Count > 0)
                {
                    LanguageName = ListSelectListItem.FindAll(X => X.Value == SelectedLang)[0].Text;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "LanguageNameFromId(SelectedLang:" + SelectedLang + ")", ex.Source, ex.Message, ex);
            }

            return LanguageName.ToLower();
            // return "en";
        }

        public string LanguageIdFromName(string SelectedLangName)
        {
            string LanguageID = "";
            try
            {
                List<SelectListItem> ListSelectListItem = LanguageList();

                if (ListSelectListItem.FindAll(X => X.Text.ToLower() == SelectedLangName.ToLower()).Count > 0)
                {
                    LanguageID = ListSelectListItem.FindAll(X => X.Text.ToLower() == SelectedLangName.ToLower())[0].Value;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(_module, "LanguageNameFromId(SelectedLangName:" + SelectedLangName + ")", ex.Source, ex.Message, ex);
            }

            return LanguageID;
        }
    }
}