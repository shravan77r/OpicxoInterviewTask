using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    public class APIData : BaseModel
    {
        public int APIId { get; set; }
        public string APIGUID { get; set; }
        public string Name { get; set; }
        public string APIEndPoint { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int AuthorizationType { get; set; }
        public string AuthorizationUsername { get; set; }
        public string AuthorizationPassword { get; set; }
        public int AuthorizationOathAPIId { get; set; }
        public string AuthorizationTokenName { get; set; }
        public string AuthorizationToken { get; set; }
        public int InputParameterType { get; set; }
        public int BodyParameterType { get; set; }
        public string OutPutParameterJson { get; set; }
        public List<APIInputParameter> lstInputParameter { get; set; }
        public List<APIOutputParameter> lstOutputParametere { get; set; }
    }


    public class APIInputParameter : BaseModel
    {
        public int InputParameterId { get; set; }
        public string InputParameterGUID { get; set; }
        public int APIId { get; set; }
        public string APIGUID { get; set; }
        public int InputParameterTypeId { get; set; }
        public string KeyColumn { get; set; }
        public string ValueColumn { get; set; }
        public int BodyType { get; set; }
    }

    public class APIOutputParameter : BaseModel
    {
        public int OutputParameterId { get; set; }
        public string OutputParameterGUID { get; set; }
        public int APIId { get; set; }
        public string APIGUID { get; set; }
        public string KeyColumn { get; set; }
        public int Type { get; set; }
    }

    public class APIExecutionResponse
    {
        public int APIId { get; set; }
        public string Name { get; set; }
        public string APIEndPoint { get; set; }
        public string APIResultData { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
    }

}
