using Beisen.AppConnectISV.Model.HttpModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.BusinessModel
{
    public class UserInfoResult : ApiResult
    {
        //[JsonProperty("user_list")]
        public List<UserInfoModel> UserList { get; set; }
    }

    public class UserInfoModel
    {
        public int TenantId { get; set; }

        public int StaffId { get; set; }

        public string MappingUserId { get; set; }

        public string BeisenAccount { get; set; }

        public int MasterAccountId { get; set; }

        public string MappingType { get; set; }

        public int Type { get; set; }

        public DateTime Time { get; set; }
    }
}
