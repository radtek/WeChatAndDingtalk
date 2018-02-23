using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnectISV.Infrastructure
{
    public class CloudDataConst
    {

        #region 多租赁默认字段
        public const string LogicalDelete = "StdIsDeleted";
        #endregion

        /// <summary>
        /// Filter总个数的限制
        /// </summary>
        public const int FilterMaxLimit = 100;
        /// <summary>
        /// Terms里面的值个数限制
        /// </summary>
        public const int TermsMaxLimit = 100;
        /// <summary>
        /// 多租赁查询的个数限制
        /// </summary>
        public const int MaxQueryRowLimit = 100;
        public const int MaxQueryColumnLimit = 10000;
    }
}
