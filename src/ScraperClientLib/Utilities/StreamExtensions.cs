using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScraperClientLib.Utilities
{
    public static class StreamExtensions
    {
        public static void WriteString(this Stream stream, string str)
        {
            WriteString(stream, str, Encoding.UTF8);
        }

        public static void WriteString(this Stream stream, string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static async Task<Stream> ReadAsStream2Async(this HttpContent content)
        {
            Stream stream = await content.ReadAsStreamAsync();

            foreach (string encoding in content.Headers.ContentEncoding)
            {
                switch (encoding)
                {
                    case "gzip":
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                        break;
                    default:
                        throw new Exception($"Unsupported encoding: {encoding}");
                }
            }

            return stream;
        }
    }
}