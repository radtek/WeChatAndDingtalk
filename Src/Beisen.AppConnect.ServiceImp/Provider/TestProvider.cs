using Beisen.AppConnect.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp
{
    public class TestProvider: ITestProvider
    {
        #region 单例

        private static readonly ITestProvider _instance = new TestProvider();
        public static ITestProvider Instance
        {
            get { return _instance; }
        }

        private TestProvider()
        {
        }
        #endregion
        public string GetString()
        {
            return "Hello World";
        }

        public string MRestDefService()
        {
            return "ISVPortal  Data  MRestService";
        }
    }
}
