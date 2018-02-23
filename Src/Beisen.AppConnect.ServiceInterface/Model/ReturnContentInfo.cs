using System;
using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class CallbackContentInfo
    {
        public int Id { get; set; }

        public string BatchId { get; set; }

        public int TenantId { get; set; }

        public string AppAccountPublic { get; set; }

        public string AppAccountPrivate { get; set; }

        public string Content { get; set; }

        public CallbackContentState State { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }
    }
}
