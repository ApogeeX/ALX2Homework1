using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ALX2Homework1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string serverAddress = "http://51.91.120.89/TABLICE/";
            long bytes = 0;
            long totalBytes = 0;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(serverAddress);

                if (response.IsSuccessStatusCode)
                {
                    var imagesList = await response.Content.ReadAsStringAsync();
                    var clearList = Regex.Replace(imagesList, @"\r\n", ",");
                    clearList = clearList.Remove(clearList.Length - 1);

                    var charSeparator = ',';
                    var result = clearList.Split(charSeparator);

                    foreach (var image in result)
                    {
                        Console.WriteLine($"Downloading - {image}");
                        using (var clientImage = new HttpClient())
                        {
                            using (var s = clientImage.GetStreamAsync($"{serverAddress}{image}"))
                            {
                                using (var fs = new FileStream(image, FileMode.OpenOrCreate))
                                {
                                    bytes = fs.Length;
                                    s.Result.CopyTo(fs);
                                }
                            }
                        }
                        Console.WriteLine($"File size: {bytes} bytes");
                        totalBytes+= bytes;
                    }
                    Console.WriteLine($"Total bytes downloaded: {totalBytes} bytes");
                    Console.ReadKey();
                }
            }
        }
    }
}
