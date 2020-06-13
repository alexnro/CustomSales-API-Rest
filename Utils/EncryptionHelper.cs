using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace APIRestCustomSales.Utils {
    public static class EncryptionHelper {
    
        public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV) {
            // Check arguments
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            // Create a Rijndael object
            // with the specified key and IV
            using (Rijndael rijndael = Rijndael.Create()) {
                rijndael.Key = Key;
                rijndael.IV = IV;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream()) {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                            // Write all data to the stream
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream
            return encrypted;
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV) {
            // Check arguments
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold the decrypted text
            string plaintext = null;

            // Create an Rijndael object with the specified key and IV
            using (Rijndael rijndael = Rijndael.Create()) {
                rijndael.Key = Key;
                rijndael.IV = IV;

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
                
                // Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            // Read the decrypted bytes from the decrypting stream
                            // and place the in a string
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

    }
}
