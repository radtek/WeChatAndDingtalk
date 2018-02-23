using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Tool.Model
{
    public class UserInfoModel
    {
        public string AppId { get; set; }

        public string OpenId { get; set; }

        public int TenantId { get; set; }

        public int UserId { get; set; }

        public short State { get; set; }

        public short Type { get; set; }
    }
}
