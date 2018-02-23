using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.Infrastructure
{
    /// <summary>
    /// 查询专用删除状态
    /// </summary>
    public enum DeletedStatus
    {
        /// <summary>
        /// 未删除
        /// </summary> 
        False = 0,
        /// <summary>
        /// 已删除
        /// </summary> 
        True = 1,
        /// <summary>
        /// 全部数据
        /// </summary>
        Both = 2
    }
}
