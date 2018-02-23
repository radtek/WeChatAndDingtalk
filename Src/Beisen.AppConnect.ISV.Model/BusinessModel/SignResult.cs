using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.BusinessModel
{
    public class SignResult
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "errorMsg")]
        public string ErrorMsg { get; set; }

        [DataMember(Name = "ssoSign")]
        public string SsoSign { get; set; }
    }
}
