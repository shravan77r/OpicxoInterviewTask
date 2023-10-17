using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SSXImport.Data;
using SSXImport.Data.ImplementationManager;

namespace SSXImport.Business.ImplementationManager
{
    /// <summary>
    /// Template Related operations
    /// </summary>
    public class BLEmailTemplateManager
    {
        /// <summary>
        /// Get Template list based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetTemplateList(int sortCol, string sortDir,
            int pageIndex, int pageSize, string keyword)
        {
            try
            {
                return DLEmailTemplateManager.GetTemplateList(sortCol, sortDir,
                      pageIndex, pageSize, keyword);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Template using Template GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplate(string templateID, int updatedBy)
        {
            try
            {
                DLEmailTemplateManager.DeleteTemplate(templateID, updatedBy);
            }
            catch
            {
                throw;
            }
        }


        public static Int32? InsertTemplate(string templateName,string templateFile,  int enteredBy,string objectType, string ddlObject)
        {
            try
            {
                return DLEmailTemplateManager.InsertTemplate(templateName, templateFile, enteredBy, objectType, ddlObject);
            }
            catch
            {
                throw;
            }
        }

        public static Int32? UpdateTemplate(string templateName, string templateFile, int templatID, int updatedBy, string objectType, string ddlObject)
        {
            try
            {
                 return DLEmailTemplateManager.UpdateTemplate(templateName, templateFile, templatID, updatedBy, objectType, ddlObject);
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetEmailTemplateByID(string emailtemplateID)
        {
            try
            {
                return DLEmailTemplateManager.GetEmailTemplateByID(emailtemplateID);
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// Execute Scaler query
        /// </summary>
        /// <param name="query"></param>
        public static void ExecuteScalarQuery(string query)
        {
            try
            {
                DLEmailTemplateManager.ExecuteScalarQuery(query);
            }
            catch
            {
                throw;
            }
        }

        public static List<string> GetTables()
        {
            try
            {
                return DLEmailTemplateManager.GetTables();
            }
            catch
            {
                throw;
            }
        }

        public static List<string> GetViews()
        {
            try
            {
                return DLEmailTemplateManager.GetViews();
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetFieldsByTableName(string tableName)
        {
            try
            {
                return DLEmailTemplateManager.GetFieldsByTableName(tableName);
            }
            catch
            {
                throw;
            }
        }


        public static DataTable GetDataByTableName(string tableName)
        {
            try
            {
                return DLEmailTemplateManager.GetDataByTableName(tableName);
            }
            catch
            {
                throw;
            }
        }
    }
}
