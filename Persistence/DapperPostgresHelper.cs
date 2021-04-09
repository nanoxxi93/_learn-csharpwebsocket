using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZyxMeSocket.Entities;
using ZyxMeSocket.Logs;

namespace ZyxMeSocket.Persistence
{
    public class DapperPostgresHelper
    {
        public static async Task<T> ExecuteSP_Single<T>(AppSettings appSettings, string guidRequest, string spName, dynamic param = null) where T : class
        {
            try
            {
                string spValue = appSettings.Queries.First(x => x.Key == spName).Value;
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spValue, param);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(spValue, param: (object)param, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(Enumerable.FirstOrDefault<T>(temp))}");
                    return await Task.Run(() =>
                    Enumerable.FirstOrDefault<T>(temp));
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_Multiple<T>(AppSettings appSettings, string guidRequest, string spName, dynamic param = null) where T : class
        {
            try
            {
                string spValue = appSettings.Queries.First(x => x.Key == spName).Value;
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spValue, param);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(spValue, param: (object)param, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(temp)}");
                    return temp;
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_SingleQuery<T>(AppSettings appSettings, string guidRequest, string spName, dynamic param = null) where T : class
        {
            try
            {
                string spValue = appSettings.Queries.First(x => x.Key == spName).Value;
                IDictionary<string, object> keyValuePairs = FormatKeyValues(param);
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spValue, keyValuePairs);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(spValue, param: (object)keyValuePairs, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(Enumerable.FirstOrDefault<T>(temp))}");
                    return await Task.Run(() =>
                    Enumerable.FirstOrDefault<T>(temp));
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_MultipleQuery<T>(AppSettings appSettings, string guidRequest, string spName, dynamic param = null) where T : class
        {
            try
            {
                string spValue = appSettings.Queries.First(x => x.Key == spName).Value;
                IDictionary<string, object> keyValuePairs = FormatKeyValues(param);
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spValue, keyValuePairs);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(spValue, param: (object)keyValuePairs, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(temp)}");
                    return temp;
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_SingleQueryText<T>(AppSettings appSettings, string guidRequest, string query, dynamic param = null) where T : class
        {
            try
            {
                IDictionary<string, object> keyValuePairs = FormatKeyValues(param);
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, query, keyValuePairs);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(query, param: (object)keyValuePairs, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(Enumerable.FirstOrDefault<T>(temp))}");
                    return await Task.Run(() =>
                    Enumerable.FirstOrDefault<T>(temp));
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, query, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_MultipleQueryText<T>(AppSettings appSettings, string guidRequest, string query, dynamic param = null) where T : class
        {
            try
            {
                IDictionary<string, object> keyValuePairs = FormatKeyValues(param);
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, query, keyValuePairs);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(query, param: (object)keyValuePairs, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(temp)}");
                    return temp;
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, query, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static async Task<dynamic> ExecuteSP_MultipleQueryFree<T>(AppSettings appSettings, string guidRequest, string spName, dynamic param = null) where T : class
        {
            try
            {
                IDictionary<string, object> keyValuePairs = FormatKeyValues(param);
                Logging(appSettings, LogEnum.DEBUG.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, keyValuePairs);
                using (var connection = new NpgsqlConnection(GetConnectionString(appSettings)))
                {
                    connection.Open();
                    var temp = await connection.QueryAsync<T>(spName, param: (object)keyValuePairs, commandType: System.Data.CommandType.Text);
                    Log.Write(appSettings, LogEnum.INFO.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().DeclaringType.ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"RESULT {guidRequest}: {JsonConvert.SerializeObject(temp)}");
                    return temp;
                }
            }
            catch (Exception ex)
            {
                Logging(appSettings, LogEnum.ERROR.ToString(), guidRequest, MethodBase.GetCurrentMethod().DeclaringType.Name, spName, param);
                Log.Write(appSettings, LogEnum.ERROR.ToString(), $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().DeclaringType.Name, $"SQL ERROR {guidRequest}: {ex.Source + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace}");
                throw ex;
            }
        }

        public static string GetConnectionString(AppSettings appSettings)
        {
            return appSettings.Environments.Find(x => x.Label == appSettings.Environment).ConnectionStrings.PostgresSQL;
        }

        public static void Logging(AppSettings appSettings, string level, string guidRequest, string method, string spName, dynamic param = null)
        {
            if (param != null)
            {
                Log.Write(appSettings, level, $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, method, $"REQUEST {guidRequest}: {spName}, Parametros: {JsonConvert.SerializeObject(param)}");
            }
            else
            {
                Log.Write(appSettings, level, $"02{Assembly.GetExecutingAssembly().GetName().Name}", MethodBase.GetCurrentMethod().ReflectedType.Name, method, $"REQUEST {guidRequest}: {spName}");
            }
        }

        public static IDictionary<string, object> FormatKeyValues(dynamic param)
        {
            IDictionary<string, object> keyValuePairs = new ExpandoObject();
            foreach (KeyValuePair<string, object> item in param)
            {
                DateTime dateTime = new DateTime();
                if (item.Value != null)
                {
                    if (DateTime.TryParse(item.Value.ToString(), out dateTime) && !item.Value.ToString().Contains(','))
                    {
                        keyValuePairs.Add("@" + item.Key, DateTime.Parse(item.Value.ToString()));
                    }
                    else if (item.Value.GetType().Name == "JArray")
                    {
                        keyValuePairs.Add("@" + item.Key, item.Value.ToString().Replace("[\r\n", "").Replace("\r\n]", "").Replace("\r\n", "").Replace("  ", ""));
                    }
                    else
                    {
                        keyValuePairs.Add("@" + item.Key, item.Value);
                    }
                }
                else
                {
                    keyValuePairs.Add("@" + item.Key, new long());
                }
            }
            return keyValuePairs;
        }

        public static Dictionary<string, object> ToDictionary(object value)
        {
            IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

            Dictionary<string, object> expando = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> property in dapperRowProperties)
                expando.Add(property.Key, property.Value);

            return expando as Dictionary<string, object>;
        }

        public static JObject ToJObject(object value)
        {
            IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

            JObject expando = new JObject();

            if (value != null)
            {
                foreach (KeyValuePair<string, object> property in dapperRowProperties)
                    expando.Add(property.Key, new JValue(property.Value));
            }

            return expando as JObject;
        }

        public static List<JObject> ToJObjectList(List<object> value)
        {
            List<JObject> expando = new List<JObject>();

            foreach (object item in value)
            {
                var temp = ToJObject(item);
                expando.Add(temp);
            }

            return expando as List<JObject>;
        }
    }
}
