using GraphLibrary;
using Microsoft.CodeAnalysis;
using System.IO.Compression;
using System.Reflection.Emit;
using System.Text;
using ZstdSharp;
using ZstdSharp.Unsafe;

namespace GraphVisualizer
{
    public class CodeChange
    {
        public static async Task<string> ParameterToCode(string base64)
        {
            var b = Convert.FromBase64String(base64.Replace("~", "+").Replace("_", "/").Replace("-","="));
            using (MemoryStream memoryStream = new MemoryStream(b))
            {
                memoryStream.Position = 0;
                using (var decompressionStream = new DecompressionStream(memoryStream,checkEndOfStream:false))
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        await decompressionStream.CopyToAsync(outStream);
                        outStream.Position = 0;

                        var data = outStream.ToArray();
                        UTF8Encoding uTF8Encoding = new UTF8Encoding();
                        return uTF8Encoding.GetString(data);
                    }
                }


            }

        }
        public static async Task<string> CodeToParameter(string code)
        {
            using (MemoryStream inputstream = new MemoryStream())
            { 
                using(StreamWriter streamWriter = new StreamWriter(inputstream, System.Text.Encoding.UTF8))
                {
                    await streamWriter.WriteAsync(code);
                    await streamWriter.FlushAsync();
                    inputstream.Position = 0;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        using (var compressionStream = new CompressionStream(outputStream, 8))
                        {
                            await inputstream.CopyToAsync(compressionStream);
                            await compressionStream.FlushAsync();
                            outputStream.Position = 0;
                            var array = outputStream.ToArray();
                            return Convert.ToBase64String(array).Replace("+", "~").Replace("/", "_").Replace("=", "-");
                        }

                    }

                }
            }


        }
    }
}
