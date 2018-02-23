using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Model.HttpModel
{
    public class UserInfoArgument
    {
        public List<int> userIds { get; set; }
        public string appAccountId { get; set; }
    }
}
