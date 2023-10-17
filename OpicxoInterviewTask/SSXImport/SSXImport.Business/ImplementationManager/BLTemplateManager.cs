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
    public class BLTemplateManager
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
        public static Tuple<int, DataTable> GetTemplateList(int sortCol, string sortDir, int pageIndex, int pageSize, string keyword, string applicationGuid)
        {
            try
            {
                return DLTemplateManager.GetTemplateList(sortCol, sortDir, pageIndex, pageSize, keyword, applicationGuid);
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
        public static void DeleteTemplate(Guid templateGUID, int updatedBy)
        {
            try
            {
                DLTemplateManager.DeleteTemplate(templateGUID, updatedBy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Template Table List for given Template GUID
        /// </summary>
        /// <param name="templateGUID">Template GUID</param>
        /// <returns>Returns List of Template Tables</returns>
        public static DataTable GetTemplateTableDetailsList(string templateGUID)
        {
            try
            {
                return DLTemplateManager.GetTemplateTableDetailsList(templateGUID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Template Table Detail bases on given Template Table Detail Id
        /// </summary>
        /// <param name="templateTableDetailId"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateTableDetail(int templateTableDetailId, int updatedBy)
        {
            try
            {
                DLTemplateManager.DeleteTemplateTableDetail(templateTableDetailId, updatedBy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Template Column Detail based on Template Table Detail GUID
        /// </summary>
        /// <param name="templateTableDetailGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateColumnDetail(Guid templateTableDetailGUID, int updatedBy)
        {
            try
            {
                DLTemplateManager.DeleteTemplateColumnDetail(templateTableDetailGUID, updatedBy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Template Columns Details based on Template GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateColumnDetailFromTemplateGUID(Guid templateGUID, int updatedBy)
        {
            try
            {
                DLTemplateManager.DeleteTemplateColumnDetailFromTemplateGUID(templateGUID, updatedBy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Template Column Details List based on Template GUID and Template Table Detail GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="templateTableDetailGUID"></param>
        /// <returns></returns>
        public static DataSet GetTemplateColumnDetailList(Guid templateGUID, Guid templateTableDetailGUID)
        {
            try
            {
                return DLTemplateManager.GetTemplateColumnDetailList(templateGUID, templateTableDetailGUID);
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
                DLTemplateManager.ExecuteScalarQuery(query);
            }
            catch
            {
                throw;
            }
        }



    }
}
