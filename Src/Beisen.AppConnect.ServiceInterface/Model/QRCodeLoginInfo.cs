using Beisen.AppConnect.Infrastructure.Cache;
using Beisen.AppConnect.ServiceInterface.Model.Enum;
using MySpace.Common.IO;

namespace Beisen.AppConnect.ServiceInterface.Model
{
    public class QrCodeLoginInfo: AppConnectExtendedSerializableEntity<QrCodeLoginInfo>
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int TitaAppId { get; set; }
        public int TenantId { get; set; }
        public int UserId { get; set; }
        public QrCodeLoginState State { get; set; }

        public override string ExtendedId
        {
            get
            {
                return Code.ToLowerInvariant();
            }
            set
            {
                Code = value.ToLowerInvariant();
            }
        }

        public override void Deserialize(IPrimitiveReader reader)
        {
            IsEmpty = reader.ReadBoolean();
            Code = reader.ReadString();
            TitaAppId = reader.ReadInt32();
            TenantId = reader.ReadInt32();
            UserId = reader.ReadInt32();
            State = (QrCodeLoginState)reader.ReadInt16();
        }

        public override void Serialize(IPrimitiveWriter writer)
        {
            writer.Write(IsEmpty);
            writer.Write(Code);
            writer.Write(TitaAppId);
            writer.Write(TenantId);
            writer.Write(UserId);
            writer.Write((short)State);
        }

        public QrCodeLoginInfo Clone()
        {
            return MemberwiseClone() as QrCodeLoginInfo;
        }
    }
}