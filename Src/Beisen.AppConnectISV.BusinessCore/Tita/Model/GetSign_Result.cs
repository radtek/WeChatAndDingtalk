using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Beisen.AppConnectISV.BusinessCore.Tita.Model
{
    [DataContract]
    public class GetSign_Result
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
        [DataMember(Name = "data")]
        public ApiEmptyResultModel Data { get; set; }
    }
    [DataContract]
    public class ApiEmptyResultModel
    {
        [DataMember(Name = "objData")]
        public string ObjData { get; set; }
    }
}
