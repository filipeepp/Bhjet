using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BHJet_Core.Utilitario
{
    public class CriptografiaUtil
    {
        /// <summary>
        /// Retorna uma string criptografada em Hash, não descriptografavel
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CriptografiaHash(string value)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
        }

        // Essa constante é utilizada para determinar o tamanho da chave do algoritmo de encriptação em bits.
        // Dividimos esse valor por 8 para obter o número equivalente em bits.
        private const int TamanhoChave = 256;

        // Essa constante determina o numero de iterações para a função geradora de bytes de senha.
        private const int Iteracoes = 1000;

        /// <summary>
        /// Encripta uma string baseado na chave.
        /// </summary>
        /// <param name="plainText">Texto a ser criptografado.</param>
        /// <param name="passPhrase">Palavra chave a ser usada na criptografia.</param>
        /// <returns></returns>
        public static string Criptografa(string plainText, string passPhrase)
        {
            // Salt and IV é gerado aleatoriamente a cada vez, mas está preparado para cifra de texto criptografado.
            // Sal e IV são gerados aleatoriamente a cada vez, mas são pré-programados para o texto cifrado criptografado
            // para que os mesmos valores de salt e IV possam ser usados ao decriptografar.
            var saltStringBytes = Gera256BitsAleatorios();
            var ivStringBytes = Gera256BitsAleatorios();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Iteracoes))
            {
                var keyBytes = password.GetBytes(TamanhoChave / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Descriptografa(string cipherText, string passPhrase)
        {
            // Obtenha o fluxo completo de bytes que representam:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Obtenha os saltbytes extraindo os primeiros 32 bytes dos bytes cipherText fornecidos.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(TamanhoChave / 8).ToArray();
            // Obtenha os bytes IV extraindo os próximos 32 bytes dos bytes cipherText fornecidos.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(TamanhoChave / 8).Take(TamanhoChave / 8).ToArray();
            // Obtenha os bytes de texto criptografados reais removendo os primeiros 64 bytes da cadeia de caracteres cipherText.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((TamanhoChave / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((TamanhoChave / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Iteracoes))
            {
                var keyBytes = password.GetBytes(TamanhoChave / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Gera256BitsAleatorios()
        {
            var randomBytes = new byte[32]; // 32 Bytes = 256 bits
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Preencha o array com bytes aleatórios criptograficamente seguros.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
