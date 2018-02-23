using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Beisen.AppConnect.CloudClient.Helper
{
    public class SerializeHelper
    {
        /// <summary>
        /// 反序列化请求结果
        /// </summary>
        /// <typeparam name="T">继承自Result的实体</typeparam>
        /// <param name="requestContent">返回结果</param>
        /// <returns></returns>
        public static T Deserialize<T>(string requestContent)
        {
            var jsonSerializer = new JsonSerializer();
            JsonReader reader = new JsonTextReader(new StringReader(requestContent));
            return jsonSerializer.Deserialize<T>(reader);
        }

        /// <summary>
        /// 序列化请求的body
        /// </summary>
        /// <typeparam name="T">请求的body的类型</typeparam>
        /// <param name="obj">请求的body</param>
        /// <returns>json序列化后的body字符串</returns>
        public static string Serialize<T>(T obj)
        {
            var jsonSerializer = new JsonSerializer();
            var json = new StringBuilder();
            var write = new StringWriter(json);
            jsonSerializer.Serialize(write, obj);
            return json.ToString();
        }
    }
}
