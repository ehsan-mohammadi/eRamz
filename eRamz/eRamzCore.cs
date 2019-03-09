using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Cryptography;

namespace eRamz
{
    class eRamzCore
    {
        /// <summary>
        /// Encrypt - Decrypt operations
        /// </summary>
        private static class Operations
        {
            public const string ENCRYPT = "encrypt";
            public const string DECRYPT = "decrypt";
        }

        /// <summary>
        /// Print eRamz get start
        /// </summary>
        public static void PrintERamz()
        {
            Console.WriteLine("NAME:");
            Console.WriteLine("  eRamz - Protect files with DES encryption\n");
            Console.WriteLine("VERSION:");
            Console.WriteLine("  1.0.0\n");
            Console.WriteLine("AUTHOR:");
            Console.WriteLine("  Ehsan Mohammadi - mohammadi.ehsan1994@gmail.com\n");
            Console.WriteLine("LICESNSE:");
            Console.WriteLine("  MIT - (c) 2019 Ehsan Mohammadi\n");
            Console.WriteLine("INSTRUCTION:");
            Console.WriteLine("  eramz [encrypt / decrypt] [8-character key] [file name]\n");
            Console.WriteLine("EXAMPLES:");
            Console.WriteLine("  eramz encrypt ehsan123 \"C:\\Ehsan.jpg\"    # encrypt 'Ehsan.jpg' with key 'ehsan123'");
            Console.WriteLine("  eramz decrypt ehsan123 \"C:\\Ehsan.jpg\"    # decrypt 'Ehsan.jpg' with key 'ehsan123'\n");
        }

        /// <summary>
        /// Print a message from eRamz
        /// </summary>
        /// <param name="message">The input message</param>
        /// <param name="color">message color</param>
        private static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("\n  " + message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Check the key length
        /// </summary>
        /// <param name="key">The input key</param>
        /// <returns>Key set correctly or not</returns>
        private static bool CheckKey(string key)
        {
            if (key.Length == 8)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check the path is exist or not
        /// </summary>
        /// <param name="path">The input path</param>
        /// <returns>Path is exist or not</returns>
        private static bool CheckPath(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }
        
        /// <summary>
        /// Encrypt the file
        /// </summary>
        /// <param name="key">The input key</param>
        /// <param name="path">The input path</param>
        /// <returns>Encrypt successfully or not</returns>
        private static bool Encrypt(string key, string path)
        {
            try
            {
                byte[] plainContent = File.ReadAllBytes(path);
                using (var DES = new DESCryptoServiceProvider())
                {
                    DES.IV = Encoding.UTF8.GetBytes(key);
                    DES.Key = Encoding.UTF8.GetBytes(key);
                    DES.Mode = CipherMode.CBC;
                    DES.Padding = PaddingMode.PKCS7;

                    using (var memoryStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, DES.CreateEncryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(plainContent, 0, plainContent.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(path, memoryStream.ToArray());

                        return true;
                    }
                }
            }
            catch(Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Decrypt the file
        /// </summary>
        /// <param name="key">The input key</param>
        /// <param name="path">The input path</param>
        /// <returns>Decrypt successfully or not</returns>
        private static bool Decrypt(string key, string path)
        {
            try
            {
                byte[] encryptedContent = File.ReadAllBytes(path);
                using (var DES = new DESCryptoServiceProvider())
                {
                    DES.IV = Encoding.UTF8.GetBytes(key);
                    DES.Key = Encoding.UTF8.GetBytes(key);
                    DES.Mode = CipherMode.CBC;
                    DES.Padding = PaddingMode.PKCS7;

                    using (var memoryStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, DES.CreateDecryptor(), CryptoStreamMode.Write);
                        cryptoStream.Write(encryptedContent, 0, encryptedContent.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(path, memoryStream.ToArray());

                        return true;
                    }
                }
            }
            catch(Exception e)
            {
                if (e.Message == "Bad Data.\r\n")
                    PrintMessage("Error: The key is invalid", ConsoleColor.Red);
                else if (e.Message == "Length of the data to decrypt is invalid.")
                    PrintMessage("Error: This file is already decrypted", ConsoleColor.Red);

                return false;
            }
        }

        public static void ERamz(string operation, string key, string path)
        {
            if(operation == Operations.ENCRYPT) // Encrypt operation
            {
                if (CheckKey(key))
                {
                    if(CheckPath(path))
                    {
                        // Encrypt the file
                        if (Encrypt(key, path))
                            PrintMessage("'" + path + "' Encrypted successfully", ConsoleColor.Green);
                        else
                            PrintMessage("'" + path + "' Encrypted not successfully", ConsoleColor.Red);
                    }
                    else
                    {
                        PrintMessage("Error: The input path is not exist", ConsoleColor.Red);
                    }
                }
                else
                {
                    PrintMessage("Error: The length of the key must be 8", ConsoleColor.Red);
                }

            }
            else if(operation == Operations.DECRYPT) // Decrypt operation
            {
                if (CheckKey(key))
                {
                    if (CheckPath(path))
                    {
                        // Decrypt the file
                        if (Decrypt(key, path))
                            PrintMessage("'" + path + "' Decrypted successfully", ConsoleColor.Green);
                        else
                            PrintMessage("'" + path + "' Decrypted not successfully", ConsoleColor.Red);
                    }
                    else
                    {
                        PrintMessage("Error: The input path is not exist", ConsoleColor.Red);
                    }
                }
                else
                {
                    PrintMessage("Error: The length of the key must be 8", ConsoleColor.Red);
                }
            }
            else
            {
                // Operation error
                PrintMessage("Error: Invalid operation. Make sure you type 'encrypt' or 'decrypt' correctly.", ConsoleColor.Red);
            }
        }
    }
}
