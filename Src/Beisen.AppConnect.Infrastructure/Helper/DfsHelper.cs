using System;
using System.Threading;
using Beisen.DfsClient;

namespace Beisen.AppConnect.Infrastructure.Helper
{
    public static class DfsHelper
    {
        /// <summary>
        /// 获取DFS内容
        /// </summary>
        /// <param name="dfsPath"></param>
        /// <returns></returns>
        public static byte[] GetDataFromDfsPath(string dfsPath)
        {
            try
            {
                if (string.IsNullOrEmpty(dfsPath))
                {
                    return null;
                }
                var item = Dfs.Get(dfsPath);
                byte[] data = null;
                int i = 0;
                while (i < 5)
                {
                    data = GetDataFromDfsItem(item);
                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        i++;
                        Thread.Sleep(300);
                    }
                }
                return data;
            }
            catch (Exception err)
            {
                AppConnectLogHelper.Error(string.Format("DfsHelper.GetDataFromDfsPath failed, dfsPaht:{0}, exception{1}", dfsPath, err));
                return null;
            }
        }

        public static byte[] GetDataFromDfsItem(DfsItem item)
        {
            byte[] bytes = null;
            if (item != null)
            {
                if (item.IsStream)
                {
                    bytes = new byte[item.Length];
                    int index = 0;
                    while (index < item.Length)
                    {
                        int read = item.FileDataStream.Read(bytes, index, (int)(item.Length - index));
                        index += read;
                    }
                }
                else
                {
                    bytes = item.FileDataBytes;
                }
            }
            return bytes;
        }
    }
}