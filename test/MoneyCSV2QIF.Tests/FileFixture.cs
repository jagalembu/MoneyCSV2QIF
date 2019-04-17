using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace MoneyCSV2QIF.Tests {
    public class FileFixture : IDisposable {
        public string TempFolder { get; private set; }

        public FileFixture () {
            TempFolder = $"{Directory.GetCurrentDirectory()}/temp/";
            if (!Directory.Exists (TempFolder)) {
                Directory.CreateDirectory (TempFolder);
            }
        }

        public void AssertFiles (string expectedFile, string actualFile) {

            var expectedHash = GetFileHash (expectedFile);
            var actualHash = GetFileHash (actualFile);

            Assert.Equal (expectedHash, actualHash);
        }

        public void Dispose () {
            if (!string.IsNullOrEmpty (TempFolder)) {
                var files = Directory.GetFiles (TempFolder);
                foreach (var file in files) {
                    File.Delete (file);
                }
            }
        }

        private string GetFileHash (string filename) {
            var hash = new SHA1Managed ();
            var clearBytes = File.ReadAllBytes (filename);
            var hashedBytes = hash.ComputeHash (clearBytes);
            return ConvertBytesToHex (hashedBytes);
        }

        private string ConvertBytesToHex (byte[] bytes) {
            var sb = new StringBuilder ();

            for (var i = 0; i < bytes.Length; i++) {
                sb.Append (bytes[i].ToString ("x"));
            }
            return sb.ToString ();
        }

    }
}