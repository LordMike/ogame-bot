using System.IO;
using System.Text;

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
    }
}