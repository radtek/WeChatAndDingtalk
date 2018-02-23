using Beisen.AppConnect.ServiceInterface.Model.Enum;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class MessageSendResult
    {
        public MessageState State { get; set; }

        public string ErrMsg { get; set; }

        public string MessageId { get; set; }
    }
}
