using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SSXImport.Data;
using SSXImport.Data.ImplementationManager;

namespace SSXImport.Business.ImplementationManager
{
    /// <summary>
    /// API Data Related operations
    /// </summary>
    public class BLAPIDataManager
    {

        /// <summary>
        /// Get API Template List based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetAPIList(int sortCol, string sortDir,
            int pageIndex, int pageSize, string keyword)
        {
            try
            {
                return DLAPIDataManager.GetAPIList(sortCol, sortDir,
                      pageIndex, pageSize, keyword);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete API Data based on apiGUID
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteAPI(Guid apiGUID, int updatedBy)
        {
            try
            {
                DLAPIDataManager.DeleteAPI(apiGUID, updatedBy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get API Data, Input Parameter Data and Output Parameter data for API Execution.
        /// </summary>
        /// <param name="apiGUID">API GUID to fetch data from</param>
        /// <returns>Returns Multiple Tables Containing main API Data with Authentication API Data</returns>
        public static DataSet GetAPIData(string apiGUID)
        {
            try
            {
                return DLAPIDataManager.GetAPIData(apiGUID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get API Input Parameter List based on API GUID
        /// </summary>
        /// <param name="apiGUID">API GUID</param>
        /// <returns></returns>
        public static DataTable GetAPIInputParameterDetailsList(string apiGUID)
        {
            try
            {
                return DLAPIDataManager.GetAPIInputParameterDetailsList(apiGUID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get API Output Parameter List based on AP GUID
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <returns></returns>
        public static DataTable GetAPIOutputParameterDetailsList(string apiGUID)
        {
            try
            {
                return DLAPIDataManager.GetAPIOutputParameterDetailsList(apiGUID);
            }
            catch
            {
                throw;
            }
        }
    }
}
