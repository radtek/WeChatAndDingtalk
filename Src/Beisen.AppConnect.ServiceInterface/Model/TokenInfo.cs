using System;
using Beisen.AppConnect.Infrastructure.Cache;
using MySpace.Common.IO;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    /// <summary>
    /// Token
    /// </summary>
    public class TokenInfo: AppConnectExtendedSerializableEntity<TokenInfo>
    {
        /// <summary>
        /// Id
        /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// AppId
        /// </summary>

        public string AppId { get; set; }

        /// <summary>
        /// Token
        /// </summary>

        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>

        public DateTime ExpireTime { get; set; }

        public override void Serialize(IPrimitiveWriter writer)
        {
            writer.Write(IsEmpty);
            //writer.Write(Id);
            writer.Write(AppId);
            writer.Write(AccessToken);
            writer.Write(ExpireTime);
        }

        public override void Deserialize(IPrimitiveReader reader)
        {
            IsEmpty = reader.ReadBoolean();
            //Id = reader.ReadInt32();
            AppId = reader.ReadString();
            AccessToken = reader.ReadString();
            ExpireTime = reader.ReadDateTime();
        }

        public override string ExtendedId
        {
            get
            {
                return AppId.ToLowerInvariant();
            }
            set
            {
                AppId = value.ToLowerInvariant();
            }
        }
    }
}
