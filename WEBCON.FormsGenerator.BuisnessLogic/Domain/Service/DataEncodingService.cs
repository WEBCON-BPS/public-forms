using System;
using System.Linq;
using System.Text;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using System.Security.Cryptography;
using System.IO;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Exceptions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Service
{
    public class DataEncodingService : IDataEncoding
    {
        public string AppKey { get; set; }

        public string Encode(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ApplicationArgumentException("Passed secret is empty");
            if (data.Length < 4)
                throw new ApplicationArgumentException("Minimum secret length is 4");

            data = string.Join("", data.Reverse());
            string sub = data.Substring(data.Length - 3, 3);
            string sub2 = sub.Substring(0, 1);

            data = data.Remove(data.Length - 3, 3).Insert(0, sub).Insert(data.Length, sub2);


            var encrypted = Encrypt(data, AppKey);
            return encrypted;
        }
        public string Decode(string data)
        {
            string decryptedText = Decrypt(data, AppKey);
            decryptedText = decryptedText.Remove(decryptedText.Length - 1, 1);
            string sub = decryptedText.Substring(0, 3);
            decryptedText = decryptedText.Remove(0, 3);
            decryptedText = decryptedText.Insert(decryptedText.Length, sub);
            decryptedText = string.Join("", decryptedText.Reverse());
            return decryptedText;

        }
        private string Encrypt(string data, string key)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(data);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    data = Convert.ToBase64String(ms.ToArray());
                }
            }
            return data;
        }            
        private string Decrypt(string data, string key)
        {
            data = data.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(data);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    data = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return data;
        }
    }
}