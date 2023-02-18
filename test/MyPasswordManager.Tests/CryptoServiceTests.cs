using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.ApplicationServices;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class CryptoServiceTests
    {
        [TestMethod]
        public void TestEncryptDecrypt()
        {
            const string encryptionKey = "AbcDef123456";
            const string plainText = "Hello World";
            const string saltText = "MySalt";
            var cryptoService = new CryptoService();
            var crypted = cryptoService.Encrypt(plainText, encryptionKey, saltText);
            crypted.Should().Be("9G6tTXhAgdPbtutNdZzBocdx4y4CWjorx3paqvd5k8M=");
            var decrypted = cryptoService.Decrypt(crypted, encryptionKey, saltText);
            decrypted.Should().Be(plainText);
        }
    }
}
