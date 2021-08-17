﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MSC.Server.Utils
{
    public class Codec
    {
        /// <summary>
        /// Base64编解码
        /// </summary>
        public static class Base64
        {
            public static string Decode(string str, string type = "utf-8")
                => Encoding.GetEncoding(type).GetString(Convert.FromBase64String(str));

            public static string Encode(string str, string type = "utf-8")
                => Convert.ToBase64String(Encoding.GetEncoding(type).GetBytes(str));

            public static byte[] DecodeToBytes(string str)
                => Convert.FromBase64String(str);
        }

        /// <summary>
        /// 字节数组转换为16进制字符串
        /// </summary>
        /// <param name="bytes">原始字节数组</param>
        /// <param name="useLower">是否使用小写</param>
        /// <returns></returns>
        public static string BytesToHex(byte[] bytes, bool useLower = true)
        {
            string output = BitConverter.ToString(bytes).Replace("-", "");
            return useLower ? output.ToLower() : output.ToUpper();
        }

        /// <summary>
        /// 根据xor进行byte数异或
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="xor">xor密钥</param>
        /// <returns>异或结果</returns>
        public static byte[] Xor(byte[] data, byte[] xor)
        {
            byte[] res = new byte[data.Length];
            for (int i = 0; i < data.Length; ++i)
            {
                res[i] = (byte)(data[i] ^ xor[i % xor.Length]);
            }
            return res;
        }

        /// <summary>
        /// 获取字符串ASCII数组
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        public static List<int> ASCII(string str)
        {
            byte[] buff = Encoding.ASCII.GetBytes(str);
            List<int> res = new List<int>();
            foreach (var item in buff)
                res.Add(item);
            return res;
        }

        /// <summary>
        /// 转换为对应进制
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="tobase">进制支持2,8,10,16</param>
        /// <returns></returns>
        public static List<string> ToBase(List<int> source, int tobase)
            => new List<string>(source.ConvertAll((int a) => Convert.ToString(a, tobase)));

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="s">原字符串</param>
        /// <returns></returns>
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// 获取字符串MD5哈希摘要
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="useBase64">是否使用Base64编码</param>
        /// <returns></returns>
        public static string StrMD5(string str, bool useBase64 = false)
        {
            MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(Encoding.Default.GetBytes(str));
            if (useBase64)
                return Convert.ToBase64String(output);
            else
                return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 获取SHA256哈希摘要
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="useBase64">是否使用Base64编码</param>
        /// <returns></returns>
        public static string StrSHA256(string str, bool useBase64 = false)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] output = sha256.ComputeHash(Encoding.Default.GetBytes(str));
            if (useBase64)
                return Convert.ToBase64String(output);
            else
                return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 获取MD5哈希字节摘要
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static byte[] BytesMD5(string str)
            => MD5.Create().ComputeHash(Encoding.Default.GetBytes(str));

        /// <summary>
        /// 获取SHA256哈希字节摘要
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static byte[] BytesSHA256(string str)
            => SHA256.Create().ComputeHash(Encoding.Default.GetBytes(str));
    }
}