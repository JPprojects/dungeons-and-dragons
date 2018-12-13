using Microsoft.AspNetCore.Http;  
using Newtonsoft.Json;  
  
namespace StaticHttpContextAccessor.Helpers  
{  
    public static class HttpSessionHelper  
    {  
        //public static void Set<T>(this ISession session, string key, T value)  
        //{  
        //    session.SetString(key, JsonConvert.SerializeObject(value));  
        //}  

        public static string GetS(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? "null" : value;
        }

        public static int? GetInt(this ISession session, string key)
        {
            var value = session.GetInt32(key);
            return value;
        }

        public static void SetS(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static void SetInt(this ISession session, string key, int value)
        {
            session.SetInt32(key, value);
        }

    }  
}  