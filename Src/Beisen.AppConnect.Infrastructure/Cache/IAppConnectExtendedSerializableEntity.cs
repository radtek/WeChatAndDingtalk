using MySpace.Common;

namespace Beisen.AppConnect.Infrastructure.Cache
{
    public interface IAppConnectExtendedSerializableEntity : IExtendedCacheParameter
    {
        string CacheName { get; }

        bool IsEnable { get; } 
    }
}