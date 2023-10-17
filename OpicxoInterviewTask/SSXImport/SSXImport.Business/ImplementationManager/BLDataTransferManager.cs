using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog;
using SSXImport.Data;
using SSXImport.Data.ImplementationManager;

namespace SSXImport.Business.ImplementationManager
{
    /// <summary>
    /// Data Transfer related Operations
    /// </summary>
    public class BLDataTransferManager
    {
        /// <summary>
        /// Get Data Transfer List based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetDataTransferList(int sortCol, string sortDir,
        int pageIndex, int pageSize, string keyword)
        {
            try
            {
                return DLDataTransferManager.GetDataTransferList(sortCol, sortDir,
                      pageIndex, pageSize, keyword);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Column mapping Status for Data Transfer to check if transfer can be start or not
        /// </summary>
        /// <param name="templateGUID">Template GUID</param>
        /// <returns></returns>
        public static DataSet GetColumnMappingForDataTransfer(Guid templateGUID)
        {
            try
            {
                return DLDataTransferManager.GetColumnMappingForDataTransfer(templateGUID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Data Transfer from Template GUID (Replicate Template master data into Data Transfer Tables)
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <returns></returns>
        public static DataSet SaveDataTransferFromTemplateGUID(Guid templateGUID)
        {
            try
            {
                return DLDataTransferManager.SaveDataTransferFromTemplateGUID(templateGUID);
            }
            catch
            {
                throw;
            }
        }
    }
}
