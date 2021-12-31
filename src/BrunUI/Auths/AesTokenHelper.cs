using BrunUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Auths
{
    /// <summary>
    /// 简单的对称加密生成token
    /// </summary>
    public class AesTokenHelper
    {
        //32位
        private static string _aesKey = "mvyyybozkaairhpfwmievusfmjndhzcg";
        //16位
        private static string _aesIV = "tbsjoakzndbmfano";
        /// <summary>
        /// 获取权限token
        /// </summary>
        /// <param name="brunUser"></param>
        /// <param name="aesKey">必须是32个字符串，为空使用默认的字符串(不安全)</param>
        /// <returns></returns>
        public static string GetToken(BrunUser brunUser, string aesKey = null)
        {
            if (aesKey == null)
            {
                aesKey = _aesKey;
            }
            //TODO 版本降级，需要安装Json序列化器
            var str = System.Text.Json.JsonSerializer.Serialize(brunUser);
            return AesEncrypt(str, aesKey, _aesIV);
        }
        /// <summary>
        /// 解析出User
        /// </summary>
        /// <param name="token"></param>
        /// <param name="aesKey">必须是32个字符串，为空使用默认的随机字符串</param>
        /// <returns></returns>
        public static BrunUser GetUser(string token, string aesKey = null)
        {
            if (string.IsNullOrEmpty(token) || token == "null")
            {
                return null;
            }
            if (aesKey == null)
            {
                aesKey = _aesKey;
            }
            string str = AesDecrypt(token, aesKey, _aesIV);
            return System.Text.Json.JsonSerializer.Deserialize<BrunUser>(str);
        }
        //AES加密
        static string AesEncrypt(string value, string key, string iv = "")
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("未将对象引用设置到对象的实例。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Encoding.UTF8.GetBytes(value);
            using (var aes = Aes.Create())
            {
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Key = _keyByte;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var cryptoTransform = aes.CreateEncryptor())
                {
                    var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }
        //AES解密
        static string AesDecrypt(string value, string key, string iv = "")
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("未将对象引用设置到对象的实例。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            byte[] _keyByte = Encoding.UTF8.GetBytes(key);
            byte[] _valueByte = Convert.FromBase64String(value);
            using (var aes = Aes.Create())
            {
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Key = _keyByte;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var cryptoTransform = aes.CreateDecryptor())
                {
                    byte[] resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }
    }
}
