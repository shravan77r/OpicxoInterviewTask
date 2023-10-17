using SSXImport.Portal.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SSXImport.Portal.Models;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace SSXImport.Portal.Controllers
{
    /// <summary>
    /// Used for All Controllers provides basic methods like Logs and Type Conversions
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Upload file called via Ajax
        /// </summary>
        /// <returns>Returns FileName</returns>
        public JsonResult UploadFileUsingAjax()
        {
            var fileName = string.Empty;

            if (Request.Form.Files.Count > 0)
            {

                var tempFile = Request.Form.Files[0];
                fileName = new DateTimeOffset(AppCommon.GetDateTime()).ToUnixTimeSeconds().ToString() + Path.GetExtension(tempFile.FileName);
                if (tempFile != null && tempFile.Length > 0)
                {
                    string dir = AppCommon.Environment.WebRootPath + "\\" + AppConstant.TempFilePath + "\\";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    using (Stream stream = new FileStream(Path.Combine(dir + fileName), FileMode.Create))
                        tempFile.CopyTo(stream);
                }
            }
            return Json(fileName);
        }

        /// <summary>
        /// Convert String to Nullable String
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Returns Nullable String</returns>
        public static string ToNullableString(string val, string defaultValue = null)
        {
            return !string.IsNullOrEmpty(val) ? val.Trim() : defaultValue;
        }

    }
}
