using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class RegisterUserInfo
    {
        public int TenantId { get; set; }

        //0: 邮箱 1：手机号
        public int RegisterType { get; set; }

        public int InviteUser { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Mobile { get; set; }

        public string WorkPhone { get; set; }

        public string Address { get; set; }

        public string DepartmentName { get; set; }

        public string Position { get; set; }
    }
}
