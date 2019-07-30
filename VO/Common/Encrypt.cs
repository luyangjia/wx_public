using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace WxPay2017.API.VO.Common
{
    /// <summary>
    /// 对称加密算法类
    /// </summary>
    public class Encrypt
    {

        private SymmetricAlgorithm mobjCryptoService;
        private string Key;
        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public Encrypt()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fjNewcap6*h%(44lJ$lhj!y6&(*jkPtrjH7";
        }

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&9500YfjNewcap#er57HBh(u%gtr5($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string Encrypto(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string Decrypto(string Source)
        {

            byte[] bytIn;
            try
            {
                bytIn = Convert.FromBase64String(Source);
            }
            catch
            {
                bytIn = Convert.FromBase64String(Source.Substring(0, Source.Length - 1));
            }
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string TryDecrypto(string Source)
        {

            if (Source.EndsWith("="))
            {
                try
                {
                    byte[] bytIn = Convert.FromBase64String(Source);
                    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                    mobjCryptoService.Key = GetLegalKey();
                    mobjCryptoService.IV = GetLegalIV();
                    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    Source = sr.ReadToEnd();
                }
                catch
                {
                    try
                    {
                        Source = Source.Substring(0, Source.Length - 1);
                        byte[] bytIn = Convert.FromBase64String(Source);
                        MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                        mobjCryptoService.Key = GetLegalKey();
                        mobjCryptoService.IV = GetLegalIV();
                        ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                        StreamReader sr = new StreamReader(cs);
                        Source = sr.ReadToEnd();
                    }
                    catch { }

                }
            }
            return Source;
        }



    }
}