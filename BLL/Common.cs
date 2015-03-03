using System;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SyncDataTool.BLL
{
    public class Common
    {
        #region 根据URL获取用户名
        public static string GetUserByUrl(string strUrl)
        {
            string strUser = string.Empty;
            strUser = strUrl.Substring(8, 32);
            return strUser;
        }
        #endregion

        #region 根据URL获取密码
        public static string GetPwdByUrl(string strUrl)
        {
            string strUser = string.Empty;
            strUser = strUrl.Substring(41, 32);
            return strUser;
        }
        #endregion

        #region 实体与JSON互转
        /// <summary>
        /// 指定实体转JSON
        /// </summary>
        /// <typeparam name="T">类型约束</typeparam>
        /// <param name="t">类型</param>
        /// <returns>返回JSON字符串</returns>
        public static string EntityToJsonString<T>(T t)
        {
            return JsonConvert.SerializeObject(t).ToLower();
        }

        /// <summary>
        /// JSON转指定实体
        /// </summary>
        /// <typeparam name="T">类型约束</typeparam>
        /// <param name="strJson">JSON字符串</param>
        /// <param name="t">类型</param>
        /// <returns>返回指定类型</returns>
        public static T JsonStringToEntity<T>(string strJson, Type t) where T : class
        {
            return JsonConvert.DeserializeObject(strJson, t) as T;
        }
        #endregion

        #region  写日志
        /// <summary>
        /// Write text to log file.
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLogMessage)
        {
            string savePath = string.Format("{0}\\Log\\{1}.txt", Environment.CurrentDirectory, System.DateTime.Now.ToString("yyyy-MM-dd"));
            string saveFolder = Path.GetDirectoryName(savePath);
            if (!System.IO.Directory.Exists(saveFolder))
            {
                System.IO.Directory.CreateDirectory(saveFolder);
                DirectorySecurity secu = new DirectorySecurity(Path.GetDirectoryName(savePath), AccessControlSections.Access);
                FileSystemAccessRule fsar = new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow);
                secu.SetAccessRule(fsar);
                System.IO.Directory.SetAccessControl(saveFolder, secu);
            }
            using (StreamWriter sw = new StreamWriter(savePath, true))
            {
                sw.WriteLine(string.Format("【{0}】\r\n  {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), strLogMessage));
            }
        }
        #endregion

        #region MD5
        /// <summary>
        /// 取得输入字符串的MD5哈希值
        /// </summary>
        /// <param name="argInput">输入字符串</param>
        /// <returns>MD5哈希值</returns>
        public static string GetMd5Hash(string argInput)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(argInput));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 验证MD5哈希值
        /// </summary>
        /// <param name="argInput">输入字符串</param>
        /// <param name="argHash">哈希值</param>
        /// <returns>相同返回TRUE,不同返回FALSE</returns>
        public static bool VerifyMd5Hash(string argInput, string argHash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(argInput);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, argHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 获取当前程序版本号
        /// <summary>
        /// 获取当前程序版本号
        /// </summary>
        /// <returns>版本号</returns>
        public static string AssemblyFileVersion()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
            }
        }
        #endregion
    }
}