using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
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
        public int InputParameterType { get; set; }
        public int BodyParameterType { get; set; }
        public string OutPutParameterJson { get; set; }
        public List<APIInputParameter> lstInputParameter { get; set; }
        public List<APIOutputParameter> lstOutputParametere { get; set; }
        public APIInputParameter objInputParameter { get; set; }
        public APIOutputParameter objOutputParametere { get; set; }
        public List<MasterItem> APITypeList { get; set; }
        public string APIResultData { get; set; }

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
        public string ValueColumn { get; set; }
        public int Type { get; set; }
        public int BodyType { get; set; }
    }

    public class APIInputParameterList
    {
        public string RowNo { get; set; }
        public int InputParameterId { get; set; }
        public string InputParameterGUID { get; set; }
        public int APIId { get; set; }
        public string APIGUID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class APIOutputParameterList
    {
        public string RowNo { get; set; }
        public int OutputParameterId { get; set; }
        public string OutputParameterGUID { get; set; }
        public int APIId { get; set; }
        public string APIGUID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class APIDataList
    {
        public string RowNo { get; set; }
        public string APIGUID { get; set; }
        public string APIEndPoint { get; set; }
        public string Name { get; set; }
    }

    public enum APIType
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
        HEAD = 4,
        OPTIONS = 5,
        PATCH = 6,
        MERGE = 7,
        COPY = 8
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

