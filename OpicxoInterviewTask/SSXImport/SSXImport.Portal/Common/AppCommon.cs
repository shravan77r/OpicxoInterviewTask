using SSXImport.Portal.Controllers;
using SSXImport.Portal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SSXImport.Portal.Models.APIModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SSXImport.Portal.Common
{
    /// <summary>
    /// Contains Static common methods used throughout the Application
    /// </summary>
    public class AppCommon
    {
        internal static IWebHostEnvironment Environment { get; set; }
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        internal AppCommon(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get Current DateTime
        /// </summary>
        /// <returns>Returns DateTime</returns>
        internal static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Get List of Data Source
        /// </summary>
        /// <param name="IsIncludeExcel">Include Excel in list or not</param>
        /// <returns>List of Data Source</returns>
        internal static SelectList GetDataSourceList(bool IsIncludeExcel = true)
        {
            var selectListItem = new List<SelectListItem>();
            if (IsIncludeExcel)
                selectListItem.Add(new SelectListItem
                {
                    Text = AppConstant.DataSource_Excel_Name,
                    Value = AppConstant.DataSource_Excel.ToString()
                });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.DataSource_MsSQL_Name,
                Value = AppConstant.DataSource_MsSQL.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.DataSource_MySQL_Name,
                Value = AppConstant.DataSource_MySQL.ToString()
            });
            if (IsIncludeExcel)
                selectListItem.Add(new SelectListItem
                {
                    Text = AppConstant.DataSource_API_Name,
                    Value = AppConstant.DataSource_API.ToString(),
                });
            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get Template Type List
        /// </summary>
        /// <returns>Returns List of Template Type</returns>
        internal static SelectList GetTemplateTypeList()
        {
            var selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.TempType_Name_Normal,
                Value = AppConstant.TempType_Value_Normal.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.TempType_Name_OneToMany,
                Value = AppConstant.TempType_Value_OneToMany.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.TempType_Name_ManyToMany,
                Value = AppConstant.TempType_Value_ManyToMany.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get API Type List
        /// </summary>
        /// <returns>Returns List of API Type</returns>
        internal static SelectList GetAPITypeList()
        {

            var selectListItem = new List<SelectListItem>();

            var enumValueArray = Enum.GetValues(typeof(APIType));
            foreach (int enumValue in enumValueArray)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(APIType), enumValue),
                    Value = Convert.ToString(enumValue)
                });
            }

            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get API Authorization Type List
        /// </summary>
        /// <returns>Returns List of API Authorization Type </returns>
        internal static SelectList GetAPIDataAuthorizationTypeList()
        {

            var selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.APIData_Authorization_Name_None,
                Value = AppConstant.APIData_Authorization_Name_None.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.APIData_Authorization_Name_Basic_Auth,
                Value = AppConstant.APIData_Authorization_Value_Basic_Auth.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.APIData_Authorization_Name_Auth2,
                Value = AppConstant.APIData_Authorization_Value_Auth2.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get Input Parameter Type List
        /// </summary>
        /// <returns>Return List of Input Parameter Type</returns>
        internal static SelectList GetInputParameterTypeList()
        {
            var selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Name_Query_String,
                Value = AppConstant.Input_Param_Value_Query_String.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Name_Header,
                Value = AppConstant.Input_Param_Value_Header.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Name_Body,
                Value = AppConstant.Input_Param_Value_Body.ToString()
            });
            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get Input Parameter Body Type List
        /// </summary>
        /// <returns>Returns List of Input Parameter Body Type</returns>
        internal static SelectList GetInputParameterBodyTypeList()
        {
            var selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Body_Name_Form_Data,
                Value = AppConstant.Input_Param_Body_Value_Form_Data.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Body_Name_Urlencoded,
                Value = AppConstant.Input_Param_Body_Value_Urlencoded.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Input_Param_Body_Name_Raw,
                Value = AppConstant.Input_Param_Body_Value_Raw.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get Output Parameter Type List
        /// </summary>
        /// <returns>Returns List of Output Parameter Type</returns>
        internal static SelectList GetOutputParameterTypeList()
        {
            var selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Output_Param_Name_String,
                Value = AppConstant.Output_Param_Value_String.ToString()
            }); ;
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Output_Param_Name_Number,
                Value = AppConstant.Output_Param_Value_Number.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Output_Param_Name_Boolean,
                Value = AppConstant.Output_Param_Value_Boolean.ToString()
            });
            selectListItem.Add(new SelectListItem
            {
                Text = AppConstant.Output_Param_Name_DateTime,
                Value = AppConstant.Output_Param_Value_DateTime.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        /// <summary>
        /// Get Empty Select List
        /// </summary>
        /// <returns>Returns Empty List</returns>
        internal static SelectList BindEmptyList()
        {
            return new SelectList(Enumerable.Empty<SelectListItem>());
        }

        /// <summary>
        /// Get API Template List for showing into Authorization API
        /// </summary>
        /// <returns>Returns List of API Template</returns>
        internal SelectList GetAPITemplateList()
        {
            var list = new SelectList(Enumerable.Empty<SelectListItem>());

            var request = new Request();

            var baseResponse = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                new APIManager(_logger).CallPostMethod("apidata/apitemplate/list", request).Result);

            list = new SelectList(baseResponse.Data.OrderBy(o => o.Name),
                "Id",
                "Name");

            return list;
        }
    }
}
